using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using AdicionalWeb.Code;
using AdicionalWeb.Entidades;
using AdicionalWeb.Extesiones;
using ImagenSoft.ModuloWeb.Entidades.Web.Adicional;

namespace AdicionalWeb
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://adicianal.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        private static Regex ValidarNoEstacion = new Regex(@"^E(\d){5}", RegexOptions.Compiled);
        private static readonly object _lock = new object();
        private static readonly object _lockDispensarios = new object();

        private string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // *** Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1).Replace("//", "/");
                else
                    // *** Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // *** Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }
        private string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }
        private string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            // *** Is it already an absolute Url?
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            // *** Start by fixing up the Url an Application relative Url
            string newUrl = ResolveUrl(serverUrl);

            Uri originalUri = HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                     "://" + (originalUri.Authority + newUrl).ReplaceEx("//", "/");

            return newUrl;
        }

        #region Estaciones
        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> VerficarEstaciones(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
            ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

            response.Result = adicional.AdicionalWebVerificarEstaciones(sesionCloud);
            response.IsFaulted = false;

            return AdicionalUtils.Compress(response.ToJSON());
        }

        private ImagenSoft.ModuloWeb.Entidades.Web.Estacion GetCurrentEstacion(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesionCloud, string noEstacion)
        {
            ImagenSoft.ModuloWeb.Entidades.Web.Estacion estActual = sesionCloud.Estaciones.Find(p => p.NoEstacion == noEstacion);
            return estActual == null ? sesionCloud.EstacionActual : estActual;
        }
        #endregion

        #region Porcentajes

        private const string MANGUERAS_KEY_FORMAT = "{0}_mangueras";
        private static bool isQueryDispensarios = false;

        private ListaDispensarios consultarMangueras(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesionCloud)
        {
            ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);
            ImagenSoft.ModuloWeb.Entidades.Web.Adicional.ListaDispensarios mangueras = adicional.AdicionalWebObtenerMangueras(sesionCloud, new Adicional.Entidades.Web.FiltroMangueras()
            {
                Estacion = new Adicional.Entidades.Estacion()
                {
                    TipoDispensario = sesionCloud.EstacionActual.Dispensario
                }
            });

            //if (posicion >= 0)
            //{
            //    mangueras.RemoveAll(p => p.posicion != posicion);
            //}

            mangueras.ForEach(p => p.noEstacion = sesionCloud.EstacionActual.NoEstacion);

            return mangueras;
        }

        private ImagenSoft.ModuloWeb.Entidades.Web.Adicional.ListaDispensarios ObtenerMangueras(string estacion, int posicion)
        {
            var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
            sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, estacion);

            string cacheKey = string.Format(MANGUERAS_KEY_FORMAT, estacion);
        again:
            ImagenSoft.ModuloWeb.Entidades.Web.Adicional.ListaDispensarios cacheItem = null;
            lock (_lock) { cacheItem = HttpContext.Current.Cache[cacheKey] as ImagenSoft.ModuloWeb.Entidades.Web.Adicional.ListaDispensarios; }
            if (cacheItem == null)
            {
                cacheItem = consultarMangueras(sesionCloud);
                lock (_lock)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, cacheItem, null, DateTime.Now.AddSeconds(31), TimeSpan.Zero);
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(obj =>
                    {
                        lock (_lockDispensarios)
                        {
                            if (WebService.isQueryDispensarios) return;
                            WebService.isQueryDispensarios = true;
                        }
                        try
                        {
                            object[] paramArr = obj as object[];
                            var _sesionCloud = paramArr[0] as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb;
                            var _mangueras = consultarMangueras(_sesionCloud);

                            HttpContext _ctx = paramArr[1] as HttpContext;
                            lock (_lock)
                            {
                                _ctx.Cache.Remove(cacheKey);
                                _ctx.Cache.Insert(cacheKey, _mangueras, null, DateTime.Now.AddSeconds(31), TimeSpan.Zero);
                            }
                        }
                        catch
                        { }
                        finally
                        {
                            lock (_lockDispensarios)
                            {
                                WebService.isQueryDispensarios = false;
                            }
                        }
                    }, new object[] { sesionCloud, HttpContext.Current });
            }
            var result = new ImagenSoft.ModuloWeb.Entidades.Web.Adicional.ListaDispensarios();
            if (cacheItem != null)
            {
                result.AddRange(cacheItem.OrderBy(p => p.posicion)
                                         .Where(p => p.posicion == posicion || posicion < 0));
            }
            return result;
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> GetMangueras(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();
            int posicion = 0;
            int.TryParse(data["pos"].ToString(), out posicion);
            string estacion = ValidarNoEstacion.Match((data["noEst"] ?? string.Empty).ToString()).Value;

            response.Result = this.ObtenerMangueras(estacion, posicion);
            response.IsFaulted = false;

            return AdicionalUtils.Compress(response.ToJSON()); ;
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> SetPorcentaje(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();

            int posicion = 0;
            int manguera = 0;
            double porcentaje = 0D;

            int.TryParse(data["pos"].ToString(), out posicion);
            int.TryParse(data["mngra"].ToString(), out manguera);
            double.TryParse(data["val"].ToString(), out porcentaje);
            string estacion = ValidarNoEstacion.Match((data["noEst"] ?? string.Empty).ToString()).Value;

            var posiciones = this.ObtenerMangueras(estacion, posicion);

            Adicional.Entidades.Historial historial = null;
            DateTime fecha = DateTime.Now;
            bool flgResp = false;

            try
            {
                Adicional.Entidades.ListaHistorial listaHistorial = new Adicional.Entidades.ListaHistorial();
                posiciones.Where(p => p.posicion == posicion)
                          .ToList().ForEach(p =>
                                {
                                    if (p.id == manguera)
                                    {
                                        p.valor = porcentaje;
                                    }

                                    p.fecha = fecha;

                                    historial = new Adicional.Entidades.Historial();
                                    historial.Combustible = (short)p.combustible;
                                    historial.Id_Estacion = 1;
                                    historial.Fecha = p.fecha;
                                    historial.Hora = p.fecha.TimeOfDay;
                                    historial.Manguera = p.id;
                                    historial.Porcentaje = (decimal)p.valor;
                                    historial.Posicion = p.posicion;
                                    listaHistorial.Add(historial);
                                });

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, estacion);
                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

                flgResp = adicional.AdicionalWebEstablecerDispensario(sesionCloud, new Adicional.Entidades.Web.FiltroMangueras()
                    {
                        Historial = listaHistorial
                    });

                if (flgResp)
                {
                    ThreadPool.QueueUserWorkItem(sesion => { try { this.consultarMangueras(sesion as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb); } catch { } }, sesionCloud);
                }
            }
            catch (Exception e)
            {
                response.Result = new object();
                response.Message = "Ocurrio un error al aplicar, por favor inicie el cliente adicional y realice una aplicación manual";
                response.ExceptionMessage = e.Message;
                response.IsFaulted = true;
            }

            if (!flgResp)
            {
                response.IsFaulted = true;
                response.Message = "No fue posible registrar el porcentaje.";
            }
            else
            {
                response.Result = posiciones;
                response.IsFaulted = false;
            }

            return AdicionalUtils.Compress(response.ToJSON());
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> SetGlobal(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();

            double porcentaje = 0D;
            double.TryParse(data["val"].ToString(), out porcentaje);
            string estacion = ValidarNoEstacion.Match((data["noEst"] ?? string.Empty).ToString()).Value;

            var posiciones = this.ObtenerMangueras(estacion, -1);

            //Adicional.Entidades.Historial historial = null;
            bool flgResp = false;

            try
            {
                DateTime fecha = DateTime.Now;
                Adicional.Entidades.ListaHistorial listaHistorial = new Adicional.Entidades.ListaHistorial();
                listaHistorial.AddRange(from i in posiciones
                                        select new Adicional.Entidades.Historial()
                                        {
                                            Combustible = (short)i.combustible,
                                            Id_Estacion = 1,
                                            Fecha = fecha,// i.fecha,
                                            Hora = i.fecha.TimeOfDay,
                                            Manguera = i.id,
                                            Porcentaje = (decimal)porcentaje, //i.valor,
                                            Posicion = i.posicion,
                                        });
                //posiciones.ForEach(p =>
                //    {
                //        p.valor = porcentaje;
                //        p.fecha = fecha;

                //        historial = new Adicional.Entidades.Historial();
                //        historial.Combustible = (short)p.combustible;
                //        historial.Id_Estacion = 1;
                //        historial.Fecha = p.fecha;
                //        historial.Hora = p.fecha.TimeOfDay;
                //        historial.Manguera = p.id;
                //        historial.Porcentaje = (decimal)p.valor;
                //        historial.Posicion = p.posicion;
                //        listaHistorial.Add(historial);
                //    });

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, estacion);

                var adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);
                flgResp = adicional.AdicionalWebEstablecerDispensarioGlobal(sesionCloud, new Adicional.Entidades.Web.FiltroMangueras()
                    {
                        Historial = listaHistorial
                    });
                if (flgResp)
                {
                    ThreadPool.QueueUserWorkItem(sesion => { try { this.consultarMangueras(sesion as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb); } catch { } }, sesionCloud);
                }
            }
            catch (Exception e)
            {
                response.Result = new object();
                response.Message = "Ocurrio un error al aplicar.";
                response.ExceptionMessage = e.Message;
                response.IsFaulted = true;
            }

            if (!flgResp)
            {
                response.IsFaulted = true;
                response.Message = "No fue posible registrar el porcentaje Global.";
            }
            else
            {
                response.Result = new Dispensarios() { valor = porcentaje };
                response.IsFaulted = false;
            }

            return AdicionalUtils.Compress(response.ToJSON()); ;
        }

        #endregion

        #region Cambio de Flujo

        private string consultarEstadoFlujo(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesionCloud)
        {
            var adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

            return adicional.AdicionalWebEstadoFlujo(sesionCloud, new Adicional.Entidades.Web.FiltroCambiarFlujo()
            {
                NoEstacion = sesionCloud.EstacionActual.NoEstacion
            });
        }

        private string obtenterEstadoFlujo(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesionCloud, string estacion)
        {
            string cacheKey = string.Format("{0}_flujos", estacion);

            string cacheItem = string.Empty;
            lock (_lock) { cacheItem = HttpContext.Current.Cache[cacheKey] as string; }
            if (cacheItem == null)
            {
                cacheItem = consultarEstadoFlujo(sesionCloud);
                lock (_lock)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, cacheItem, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    object[] paramArr = obj as object[];
                    var _sesionCloud = paramArr[0] as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb;
                    var _flujo = consultarEstadoFlujo(_sesionCloud);

                    HttpContext _ctx = paramArr[1] as HttpContext;
                    lock (_lock)
                    {
                        _ctx.Cache.Remove(cacheKey);
                        _ctx.Cache.Insert(cacheKey, _flujo, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
                    }
                }, new object[] { sesionCloud, HttpContext.Current });
            }

            return cacheItem;
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> GetEstatus(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            try
            {
                var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();
                string estacion = ValidarNoEstacion.Match((data["noEst"] ?? string.Empty).ToString()).Value;

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, estacion);

                response.Result = obtenterEstadoFlujo(sesionCloud, estacion);
                response.IsFaulted = false;
            }
            catch (Exception e)
            {
                response.IsFaulted = true;
                response.Message = e.Message;
            }
            return AdicionalUtils.Compress(response.ToJSON()); ;
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> SetEstatus(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            try
            {
                var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();
                string estacion = ValidarNoEstacion.Match((data["noEst"] ?? string.Empty).ToString()).Value;

                bool flgResult = ((bool)data["flujo"]);

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, estacion);

                var usuario = (ImagenSoft.ModuloWeb.Entidades.Web.UsuarioWeb)HttpContext.Current.Session[AdminSession.ID];
                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

                response.Result = adicional.AdicionalWebCambiarFlujo(sesionCloud, new Adicional.Entidades.Web.FiltroCambiarFlujo()
                    {
                        NoEstacion = sesionCloud.EstacionActual.NoEstacion,
                        Estandar = flgResult,// ? "Estandar" : "Mínimo"
                        Usuario = new Adicional.Entidades.Web.UsuarioCloud()
                            {
                                Correo = usuario.Correo,
                                NoEstacion = usuario.NoEstacion,
                                Password = usuario.Password,
                                //Privilegios = usuario.Privilegios,
                                Usuario = usuario.Usuario
                            }
                    });
                ThreadPool.QueueUserWorkItem(sesion => { try { this.obtenterEstadoFlujo(sesion as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb, (sesion as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb).EstacionActual.NoEstacion); } catch { } });
                response.IsFaulted = false;
            }
            catch (Exception e)
            {
                response.IsFaulted = true;
                response.Message = e.Message;
            }
            return AdicionalUtils.Compress(response.ToJSON());
        }

        #endregion

        #region Reportes

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> GetPDF(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            try
            {
                var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();
                string estacion = ValidarNoEstacion.Match((data["noEstacion"] ?? string.Empty).ToString()).Value;

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                var estacionActual = this.GetCurrentEstacion(sesionCloud, estacion);

                TemplateConfiguration config = new TemplateConfiguration();

                config.NoEstacion = estacion;
                config.ComercialName = estacionActual.Nombre.ToTitle();
                config.Id = data["id"].ToString();
                var tipo = (Adicional.Entidades.TipoVentasCombustible)Convert.ToInt32(data["tipo"]);
                switch (tipo)
                {
                    case Adicional.Entidades.TipoVentasCombustible.AjustePorCombustible:
                        config.ReportName = "Ajuste por Combustible";
                        break;
                    case Adicional.Entidades.TipoVentasCombustible.VentasAjustadas:
                        config.ReportName = "Ventas Ajustadas";
                        break;
                    case Adicional.Entidades.TipoVentasCombustible.VentasReales:
                        config.ReportName = "Ventas Reales";
                        break;
                    case Adicional.Entidades.TipoVentasCombustible.None:
                    default:
                        config.ReportName = string.Empty;
                        break;
                }

                ServicioReportes srvReportes = new ServicioReportes();

                srvReportes.InicializarConfiguraciones(ref config, data);

                string path = AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\files");

                string message = string.Empty;
                if (!srvReportes.ValidarRuta(path, out message))
                {
                    response.IsFaulted = true;
                    response.Message = message;
                    return AdicionalUtils.Compress(response.ToJSON());
                }

                string name = srvReportes.GenerarPDF(config);
                response.Result = ResolveServerUrl("~/Files/Temp/" + name);
            }
            catch (Exception ex)
            {
                //LogExceptionManager.LogException("GetReport", ex);
                response.IsFaulted = true;
                response.ExceptionMessage = ex.Message;
                response.Message = "Ocurri&oacute; un fallo al intentar descargar el archivo.";
            }

            return AdicionalUtils.Compress(response.ToJSON());
        }

        [WebMethod(BufferResponse = true, EnableSession = true)]
        public List<int> GetReportes(List<int> d)
        {
            AdicionalResponse response = new AdicionalResponse();

            if (HttpContext.Current.Session[AdminSession.ID] == null)
            {
                response.IsFaulted = true;
                response.Message = "No cuenta con sesión activa";
                response.Result = ResolveServerUrl("~/");
                return AdicionalUtils.Compress(response.ToJSON());
            }

            try
            {
                var data = AdicionalUtils.Decompress(d).FromJSON<Dictionary<string, object>>();

                var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];
                sesionCloud.EstacionActual = this.GetCurrentEstacion(sesionCloud, data["noEstacion"].ToString());

                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

                var lst = adicional.ObtenerReporteVentasCombustible(sesionCloud, new Adicional.Entidades.FiltroReporteVentasCombustible()
                {
                    NoEstacion = sesionCloud.EstacionActual.NoEstacion,
                    Fecha = DateTime.Parse(data["fecha"].ToString()),
                    Tipo = (Adicional.Entidades.TipoVentasCombustible)Convert.ToInt32(data["tipo"])
                });
                response.Result = lst.ToJSON();
                response.IsFaulted = false;
            }
            catch (Exception e)
            {
                response.IsFaulted = true;
                response.ExceptionMessage = e.Message;
                response.Message = "No fue posible realizar la consulta.";
            }
            return AdicionalUtils.Compress(response.ToJSON());
        }

        #endregion
    }
}
