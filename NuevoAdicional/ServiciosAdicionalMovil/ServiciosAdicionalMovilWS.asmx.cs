using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Adicional.Entidades;
using System.ServiceModel;
using System.ServiceModel.Description;
using Servicios.Adicional;
using System.Configuration;
using ServiciosCliente;
using Adicional.Entidades.Web;

namespace ServiciosAdicionalMovil
{
    /// <summary>
    /// Summary description for ServiciosAdicionalMovilWS
    /// </summary>
    [WebService(Namespace = "http://adicional.gasolineras.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiciosAdicionalMovilWS : System.Web.Services.WebService
    {
        private NetTcpBinding binding = null;
        private ServiceEndpoint endPointAd = null;
        private ServiceEndpoint endPointCl = null;
        private ChannelFactory<IServiciosAdicional> cfAdicional = null;
        private ChannelFactory<IServiciosCliente> cfCliente = null;
        private string uri = "";

        private IServiciosAdicional srvAdi = null;
        private IServiciosCliente srvCli = null;

        public ServiciosAdicionalMovilWS()
        {
            uri = string.Format("net.tcp://{0}/",
                                 (ConfigurationManager.AppSettings["ipServicios"] != null ? ConfigurationManager.AppSettings["ipServicios"] : "localhost"));

            binding = new NetTcpBinding();
            binding.Name = "NetTcp";
            binding.CloseTimeout = new TimeSpan(0, 5, 0);
            binding.OpenTimeout = new TimeSpan(0, 5, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 5, 0);
            binding.TransactionFlow = false;
            binding.TransferMode = TransferMode.Buffered;
            binding.TransactionProtocol = TransactionProtocol.OleTransactions;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.ListenBacklog = 10;
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxConnections = 10;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReliableSession.Ordered = true;
            binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
            binding.ReliableSession.Enabled = false;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;

            endPointAd = new ServiceEndpoint(ContractDescription.GetContract(typeof(Servicios.Adicional.ServiciosAdicional)),
                                           binding,
                                           new EndpointAddress(string.Concat(uri, "ServiciosAdicional")));

            endPointCl = new ServiceEndpoint(ContractDescription.GetContract(typeof(ServiciosCliente.ServiciosCliente)),
                                           binding,
                                           new EndpointAddress(string.Concat(uri, "ServiciosCliente")));
        }

        private void validarLicencia()
        {
            string mensajeLicencia = string.Empty;

            try
            {
                Licencia lic = srvAdi.LicenciaObtener(Licencia.ClaveMovil);

                if (!srvAdi.LicenciaValida(lic, Licencia.Version))
                    mensajeLicencia = "Submódulo Android no tiene licencia";
            }
            catch (Exception)
            {
                throw new Exception("Canal de comunicación no disponible");
            }

            if (!string.IsNullOrEmpty(mensajeLicencia))
            {
                //Solo si es inválida se cierra el canal, si es válida el canal se cierra en el método que invocó a este submétodo.
                cfAdicional.Close();
                throw new Exception(mensajeLicencia);
            }
        }

        [WebMethod]
        public Respuesta HelloWorld()
        {
            Respuesta result = new Respuesta();
            try
            {
                result.Result = "Hello World";
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.IsFaulted = true;
            }
            return result;
        }

        [WebMethod]
        public Respuesta RegistrarCelular(Moviles sesion)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                try
                {
                    Moviles movilExistente = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = sesion.Telefono });

                    Bitacora bitacora = new Bitacora()
                    {
                        Id_usuario = string.Format("{0} ({1})", sesion.Responsable, sesion.Telefono),
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay
                    };

                    if (movilExistente == null)
                    {
                        bitacora.Suceso = "Registrar Móvil";

                        srvAdi.MovilesInsertar(sesion);
                        srvAdi.BitacoraInsertar(bitacora);
                    }
                    else
                    {
                        if (movilExistente.Telefono != sesion.Telefono || movilExistente.Responsable != sesion.Responsable || movilExistente.Password != sesion.Password)
                        {
                            resultado.IsFaulted = true;
                            resultado.Message = "Móvil inválido";
                        }
                    }

                    resultado.Result = resultado.IsFaulted ? string.Empty : srvAdi.ObtenerNombreEstacion().Split('|')[0];

                    cfAdicional.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta ActualizarCelular(Moviles sesion)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                try
                {
                    Moviles movilExistente = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = sesion.Telefono });

                    Bitacora bitacora = new Bitacora()
                    {
                        Id_usuario = string.Format("{0} ({1})", sesion.Responsable, sesion.Telefono),
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay
                    };

                    if (movilExistente == null)
                    {
                        resultado.IsFaulted = true;
                        resultado.Message = "Móvil no existe.";
                    }
                    else
                    {
                        bitacora.Suceso = "Actualizar Móvil";

                        sesion.Activo = movilExistente.Activo;
                        sesion.Permisos = movilExistente.Permisos;

                        srvAdi.MovilesActualizar(sesion);
                        srvAdi.BitacoraInsertar(bitacora);

                        resultado.Result = srvAdi.ObtenerNombreEstacion().Split('|')[0];
                    }

                    cfAdicional.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta ObtenerCelular(Moviles sesion)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                Moviles movil = new Moviles();

                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                try
                {
                    movil = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = sesion.Telefono });

                    cfAdicional.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

                if (movil == null) { throw new Exception("Móvil inválido."); }

                resultado.Result = movil;
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta SubirBajarFlujo(string std, string numCelular)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                string result = string.Empty;
                string error = string.Empty;

                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                try
                {
                    cfCliente = new ChannelFactory<IServiciosCliente>(endPointCl);
                    srvCli = cfCliente.CreateChannel();

                    Bitacora bitacora;
                    Moviles movil = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = numCelular });

                    if (movil != null && movil.Activo.Equals("S"))
                    {

                        if (movil.Permisos.SubirBajar)
                        {
                            bitacora = new Bitacora()
                            {
                                Id_usuario = string.Format("{0} ({1})", movil.Responsable, movil.Telefono),
                                Fecha = DateTime.Now.Date,
                                Hora = DateTime.Now.TimeOfDay,
                                Suceso = std.Equals("Estandar", StringComparison.OrdinalIgnoreCase) ? "Subir Flujo" : "Bajar Flujo"
                            };

                            if (string.IsNullOrEmpty(std))
                            {
                                error = "variable \"std\" es nula";
                            }

                            resultado.Result = srvCli.SubirBajarFlujo(std.Equals("Estandar", StringComparison.OrdinalIgnoreCase));
                            if (resultado.Result.ToString().Equals("Ok", StringComparison.OrdinalIgnoreCase))
                            {
                                resultado.Result = std;

                                srvAdi.ConfiguracionCambiarEstado(std);
                                srvAdi.BitacoraInsertar(bitacora);
                            }
                            else
                            {
                                resultado.Result = "No se aplicó el flujo";
                            }
                        }
                        else
                        {
                            error = "No tiene permitido cambiar el flujo.";
                        }
                    }
                    else
                    {
                        error = "Permiso Denegado";
                    }

                    cfAdicional.Close();
                    cfCliente.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta ObtenerEstadoFlujo(string numCelular)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                string error = string.Empty;

                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                try
                {
                    Moviles movil = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = numCelular });

                    if (movil == null || !movil.Activo.Equals("S"))
                    {
                        error = "Permiso Denegado";
                    }

                    Bitacora bitacora = new Bitacora()
                    {
                        Id_usuario = (movil == null) ? string.Format("Desconocido ({0})", numCelular) : string.Format("{0} ({1})", movil.Responsable, movil.Telefono),
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay,
                        Suceso = "Obtener Estado Flujo"
                    };

                    Configuracion cfg = srvAdi.ConfiguracionObtener(1);
                    resultado.Result = cfg.Estado;
                    srvAdi.BitacoraInsertar(bitacora);

                    cfAdicional.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta ValidarCelularSesion(Moviles sesion)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                Moviles movilExistente = null;
                try
                {
                    movilExistente = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = sesion.Telefono });
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

                if (movilExistente == null)
                {
                    throw new Exception("Móvil inválido.");
                }
                else if (!sesion.Responsable.Equals(movilExistente.Responsable, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("Móvil inválido.");
                }
                else if (!movilExistente.Password.Equals(sesion.Password))
                {
                    throw new Exception("Password inválido.");
                }

                resultado.Result = srvAdi.ObtenerNombreEstacion().Split('|')[0];

                cfAdicional.Close();
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        [WebMethod]
        public Respuesta ValidarCelular(string telefono)
        {
            Respuesta resultado = new Respuesta();
            try
            {
                cfAdicional = new ChannelFactory<IServiciosAdicional>(endPointAd);
                srvAdi = cfAdicional.CreateChannel();

                validarLicencia();

                Moviles movilExistente = null;
                try
                {
                    movilExistente = srvAdi.MovilesObtener(new FiltroMoviles() { Telefono = telefono });

                    cfAdicional.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Canal de comunicación no disponible");
                }

                if (movilExistente == null) { throw new Exception("Móvil inválido."); }
                if (!movilExistente.Activo.Equals("S")) { throw new Exception("Móvil inválido."); }

                resultado.Result = (!movilExistente.Password.Equals("") ? "Si" : "No");
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }
    }
}
