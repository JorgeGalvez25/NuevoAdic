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
        #region IModuloWeb Members

        public byte[] EnviarPeticion(byte[] solicitud)
        {
            ServiciosFachada servicio = new ServiciosFachada();
            RespuestaHostWeb result = null;
            UtileriasWCF utilerias = new UtileriasWCF();
            try
            {
                SolicitudHostWeb _solicitud = utilerias.DeserializarXML<SolicitudHostWeb>(solicitud);
                //lock (UtileriasWCF.Lock_Serializer) { _solicitud = UtileriasWCF.DeserializarGZip<SolicitudHostWeb>(solicitud).Result; }
                result = servicio.EnviarPeticion(_solicitud);
            }
            catch (System.Exception e)
            {
                MensajesRegistros.Excepcion("ServicioModuloWeb", e);
                result = new RespuestaHostWeb()
                    {
                        EsValido = false,
                        Mensaje = e.Message,
                        Resultado = new object()
                    };
            }

            lock (UtileriasWCF.Lock_Serializer)
            {
                //return result == null ? new byte[0] : UtileriasWCF.SerializarGZip(result).Result;
                return result == null ? new byte[0] : utilerias.SerializarXML(result);
            }
        }

        #endregion

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
