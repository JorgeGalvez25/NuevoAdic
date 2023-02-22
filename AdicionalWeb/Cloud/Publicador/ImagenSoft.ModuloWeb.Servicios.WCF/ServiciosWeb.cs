using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Fachada;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using System;
using System.ServiceModel;

namespace ImagenSoft.ModuloWeb.Servicios.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true,
                     UseSynchronizationContext = false)]
    public class ServicioModuloWeb : IModuloWeb
    {
        public byte[] EnviarPeticion(byte[] solicitud)
        {
            ServiciosFachada servicio = new ServiciosFachada();
            UtileriasWCF utilerias = new UtileriasWCF();
            try
            {
                return utilerias.Serializar(servicio.EnviarPeticion(utilerias.Deserializar<SolicitudHostWeb>(solicitud))).Result;
            }
            catch (System.Exception e)
            {
                try
                {
                    MensajesRegistros.Error("ServiciosAdicionalWeb", e);
                    RespuestaHostWeb respuesta = new RespuestaHostWeb();
                    respuesta.EsValido = false;
                    respuesta.Mensaje = e.Message;
                    respuesta.Resultado = new object();

                    return utilerias.Serializar(respuesta).Result;
                }
                catch (System.Exception ex)
                {
                    MensajesRegistros.Error("ServiciosAdicionalWeb", ex);
                    return new byte[0];
                }
            }
        }

        public bool IniciarServiciosPing()
        {
            ServiciosFachada servicio = new ServiciosFachada();
            var resultado = servicio.IniciarServicioPing();
            if (!resultado.EsValido)
            {
                throw new Exception(resultado.Mensaje);
            }

            return (bool)resultado.Resultado;
        }
    }
}
