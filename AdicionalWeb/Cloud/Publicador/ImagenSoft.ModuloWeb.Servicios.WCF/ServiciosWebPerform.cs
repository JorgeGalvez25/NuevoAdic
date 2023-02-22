using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Servicios;
using ImagenSoft.ModuloWeb.Fachada;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace ImagenSoft.ModuloWeb.Servicios.WCF
{
    [ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.PerCall,
                     ConcurrencyMode = System.ServiceModel.ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class ServiciosWebPerform : IModuloWebPerform
    {
        public bool Ping(SesionModuloWeb sesion)
        {
            bool result = false;
            if ("Si".Equals(ConfigurationManager.AppSettings["RegistrarPing"] ?? "No", StringComparison.OrdinalIgnoreCase))
            {
                try
                {

                    RemoteEndpointMessageProperty cliente = ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]);
                    MensajesRegistros.Object("HostModuloWeb-Clientes", string.Format("IP - Log _ {0} - {1}:{2}", sesion.NoCliente, cliente.Address, cliente.Port), cliente);
                }
                catch { }
            }

            try
            {
                ServiciosAdministrarClientesFachada servicio = new ServiciosAdministrarClientesFachada();
                servicio.ModificarFechaHoraCliente(sesion, new FiltroAdministrarClientes()
                    {
                        FechaUltimaConexion = DateTime.Now,
                        NoEstacion = sesion.NoCliente,
                        FechaHoraCliente = sesion.FechaHoraCliente,
                        Version = "4.1"
                    });

                //ServiciosPreciosGasolinerasFachada srvPrecios = new ServiciosPreciosGasolinerasFachada();
                //result = srvPrecios.ClienteValidoCambioPrecios(sesion);

                //ServiciosMonitorAplicaciones srvAplicaciones = new ServiciosMonitorAplicaciones();
                //srvAplicaciones.ModificarAplicaciones(sesion);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Ping: {0}", sesion.NoCliente))
                  .AppendLine(MensajesRegistros.GetFullMessage(e));

                MensajesRegistros.Error("Host Modulo Web", string.Format(sb.ToString().Trim()));
            }

            return result;
        }

        public bool Ping()
        {
            return true;
        }

        public byte[] GetConfig(byte[] request)
        {
            UtileriasWCF utilerias = new UtileriasWCF();
            string id = utilerias.Deserializar<string>(request);
            ZonasCambioPrecio zona = ZonasCambioPrecio.None;
            int horasCorte = 0;

            ServiciosFachada servicio = new ServiciosFachada();
            RespuestaHostWeb respuesta = servicio.AdministrarClientesObtener(null, new FiltroAdministrarClientes() { NoEstacion = id });

            if (respuesta.Resultado != null && respuesta.EsValido)
            {
                zona = (respuesta.Resultado as AdministrarClientes).Zona;
                horasCorte = (respuesta.Resultado as AdministrarClientes).HorasCorte;
            }

            int dias = int.Parse(ConfigurationManager.AppSettings["DiasConsulta"] ?? "7");
            ConfigCliente result = new ConfigCliente(zona, dias, horasCorte);
            return utilerias.Serializar(result).Result;
        }
    }
}
