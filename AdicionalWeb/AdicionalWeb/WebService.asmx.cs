using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using AdicionalWeb.Code;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia;
using AdicionalWeb.Persistencia.Enlaces;

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
        #region Porcentajes

        private ManguerasPersistencia srvMangueras = null;
        private ListaDispensarios ObtenerMangueras(int posicion)
        {
            if (this.srvMangueras == null) { srvMangueras = new ManguerasPersistencia((Usuario)Session["usuario"]); }
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            ListaDispensarios mangueras = srvMangueras.ObtenerPosiciones((Adicional.Entidades.Estacion)HttpContext.Current.Session[AdminSession.ESTACION]);
            if (posicion >= 0)
            {
                mangueras.RemoveAll(p => p.posicion != posicion);
            }

            return mangueras;
        }

        private bool ValidarLicencia(int estacion)
        {
            LicenciaPersistencia servicio = new LicenciaPersistencia();
            return servicio.LicenciaValidaHost(Adicional.Entidades.Licencia.ClabeAutor, estacion);
        }

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

            var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(AdicionalUtils.Decompress(d));
            int posicion = 0;
            int.TryParse(data["pos"].ToString(), out posicion);

            response.Result = this.ObtenerMangueras(posicion);
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

            var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(AdicionalUtils.Decompress(d));

            int posicion = 0;
            int manguera = 0;
            double porcentaje = 0D;
            int estacion = 0;

            int.TryParse(data["est"].ToString(), out estacion);
            int.TryParse(data["pos"].ToString(), out posicion);
            int.TryParse(data["mngra"].ToString(), out manguera);
            double.TryParse(data["val"].ToString(), out porcentaje);

            FlujosPersistencia srvAdicional = new FlujosPersistencia();
            srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Aplicar cambio de porcentaje" }, estacion);

            if (!ValidarLicencia(estacion))
            {
                srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Error al aplicar flujo, Licencia inválida" }, estacion);
                response.IsFaulted = true;
                response.Message = "No fue posible registrar el porcentaje, por favor realice una aplicación manual.";
                response.Result = new object();
                return AdicionalUtils.Compress(response.ToJSON());
            }

            var posiciones = this.ObtenerMangueras(posicion);
            //var dtCurrent = DateTime.Now;
            //posiciones.ForEach(p =>
            //    {
            //        if (p.id == manguera)
            //        {
            //            p.valor = porcentaje;
            //        }
            //        p.fecha = dtCurrent;
            //    });

            //if (this.srvMangueras == null) { this.srvMangueras = new ManguerasPersistencia((Usuario)Session["usuario"]); }
            //var resp = this.srvMangueras.ManguerasInsertar(posiciones, estacion);

            Adicional.Entidades.Historial historial = null;
            DateTime fecha = DateTime.Now;
            bool flgResp = false;

            try
            {
                posiciones.ForEach(p =>
                    {
                        if (p.id == manguera)
                        {
                            p.valor = porcentaje;
                        }

                        p.fecha = fecha;

                        historial = new Adicional.Entidades.Historial();
                        historial.Combustible = (short)p.combustible;
                        historial.Id_Estacion = estacion;
                        historial.Fecha = p.fecha.Date;
                        historial.Hora = p.fecha.TimeOfDay;
                        historial.Manguera = p.id;
                        historial.Porcentaje = (decimal)p.valor;
                        historial.Posicion = p.posicion;
                        if ((srvAdicional.HistorialInsertar(historial, estacion) == null) && (p.id == manguera))
                        {
                            flgResp = true;
                        }
                    });

                FlujosPersistencia servicio = new FlujosPersistencia();
                response = servicio.ObtenerEstadoFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), estacion);

                if (response.Result.ToString().Equals("Estandar", StringComparison.OrdinalIgnoreCase))
                {
                    var currentEstacion = EstacionesAdicionalPersistencia.ListaEstaciones[estacion];

                    response.IsFaulted = false;
                    var rsp = servicio.SubirBajarFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), "Estandar", estacion);
                    if (rsp.Result == null) rsp.Result = string.Empty;

                    if (!rsp.IsFaulted && rsp.Result.ToString().Length > 0)
                    {
                        currentEstacion.Estado = "Estandar";
                        srvAdicional.ConfiguracionActualizarUltimoMovimiento(fecha, estacion);
                        srvAdicional.EstacionActualizar(currentEstacion, estacion);
                        srvAdicional.ConfiguracionCambiarEstado("Estandar", estacion);
                        srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Modificar flujo" }, estacion);
                    }
                    else
                    {
                        flgResp = false;
                        srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Error al aplicar flujo" }, estacion);
                    }
                }
            }
            catch (Exception e)
            {
                response.Result = new object();
                response.Message = "Ocurrio un error al aplicar, por favor inicie el cliente adicional y realice una aplicación manual";
                response.ExceptionMessage = e.Message;
                response.IsFaulted = true;
            }

            //if (resp == null)
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

            var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(AdicionalUtils.Decompress(d));

            double porcentaje = 0D;
            int estacion = 0;

            double.TryParse(data["val"].ToString(), out porcentaje);
            int.TryParse(data["est"].ToString(), out estacion);

            FlujosPersistencia srvAdicional = new FlujosPersistencia();
            srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Aplicar cambio de porcentaje global" }, estacion);
            if (!ValidarLicencia(estacion))
            {
                srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Error al aplicar, Licencia inválida" }, estacion);
                response.IsFaulted = true;
                response.Message = "No fue posible registrar el porcentaje, por favor realice una aplicación manual.";
                response.Result = new object();
                return AdicionalUtils.Compress(response.ToJSON());
            }

            if (this.srvMangueras == null) { srvMangueras = new ManguerasPersistencia((Usuario)Session["usuario"]); }

            var posiciones = this.ObtenerMangueras(-1);
            //var dtCurrent = DateTime.Now;
            //lst.ForEach(p =>
            //    {
            //        p.valor = porcentaje;
            //        p.fecha = dtCurrent;
            //    });

            //var item = this.srvMangueras.ManguerasInsertar(lst, estacion);

            Adicional.Entidades.Historial historial = null;
            DateTime fecha = DateTime.Now;
            bool flgResp = false;

            try
            {
                posiciones.ForEach(p =>
                    {
                        p.valor = porcentaje;
                        p.fecha = fecha;

                        historial = new Adicional.Entidades.Historial();
                        historial.Combustible = (short)p.combustible;
                        historial.Id_Estacion = estacion;
                        historial.Fecha = p.fecha.Date;
                        historial.Hora = p.fecha.TimeOfDay;
                        historial.Manguera = p.id;
                        historial.Porcentaje = (decimal)p.valor;
                        historial.Posicion = p.posicion;
                        if (srvAdicional.HistorialInsertar(historial, estacion) == null)
                        {
                            flgResp = true;
                        }
                    });

                FlujosPersistencia servicio = new FlujosPersistencia();
                response = servicio.ObtenerEstadoFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), estacion);

                if (response.Result.ToString().Equals("Estandar", StringComparison.OrdinalIgnoreCase))
                {
                    var currentEstacion = EstacionesAdicionalPersistencia.ListaEstaciones[estacion];

                    response.IsFaulted = false;
                    var rsp = servicio.SubirBajarFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), "Estandar", estacion);

                    if (rsp.Result.ToString().Length > 0)
                    {
                        currentEstacion.Estado = "Estandar";
                        srvAdicional.ConfiguracionActualizarUltimoMovimiento(fecha, estacion);
                        srvAdicional.EstacionActualizar(currentEstacion, estacion);
                        srvAdicional.ConfiguracionCambiarEstado("Estandar", estacion);
                        srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Modificar flujo" }, estacion);
                    }
                    else
                    {
                        srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Error al aplicar flujo" }, estacion);
                    }
                }

            }
            catch (Exception e)
            {
                response.Result = new object();
                response.Message = "Ocurrio un error al aplicar.";
                response.ExceptionMessage = e.Message;
                response.IsFaulted = true;
            }

            //if (item == null)
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
                var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(AdicionalUtils.Decompress(d));
                int estacion = 1;
                if (!int.TryParse(data["est"].ToString(), out estacion))
                {
                    estacion = 1;
                }

                FlujosPersistencia servicio = new FlujosPersistencia();
                response = servicio.ObtenerEstadoFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), estacion);
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
                var data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(AdicionalUtils.Decompress(d));

                bool flgResult = ((bool)data["flujo"]);
                int estacion = 1;
                if (!int.TryParse(data["est"].ToString(), out estacion))
                {
                    estacion = 1;
                }

                FlujosPersistencia srvAdicional = new FlujosPersistencia();
                srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Aplicar cambio de flujo" }, estacion);
                if (!ValidarLicencia(estacion))
                {
                    srvAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = ((Usuario)Session["usuario"]).Nombre, Suceso = "Error al aplicar el cambio de flujo, Licencia inválida" }, estacion);
                    response.IsFaulted = true;
                    response.Message = "No fue posible hacer el cambio de flujo, por favor realice una aplicación manual.";
                    response.Result = new object();
                    return AdicionalUtils.Compress(response.ToJSON());
                }

                FlujosPersistencia servicio = new FlujosPersistencia();
                response.IsFaulted = false;
                response = servicio.SubirBajarFlujo(((Usuario)HttpContext.Current.Session[AdminSession.ID]), flgResult ? "Estandar" : "Mínimo", estacion);
            }
            catch (Exception e)
            {
                response.IsFaulted = true;
                response.Message = e.Message;
            }
            return AdicionalUtils.Compress(response.ToJSON()); ;
        }

        #endregion
    }
}
