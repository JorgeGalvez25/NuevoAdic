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
    public class ServiciosModuloWebAdicional : IModuloWebAdicional
    {
        #region IModuloWebAdicional Members

        public byte[] EnviarPeticion(byte[] solicitud)
        {
            ServiciosFachada servicio = new ServiciosFachada();
            UtileriasWCF utilerias = new UtileriasWCF();
            try
            {
                return utilerias.SerializarXML(servicio.EnviarPeticion(utilerias.DeserializarXML<SolicitudHostWeb>(solicitud)));
            }
            catch (System.Exception e)
            {
                try
                {
                    MensajesRegistros.Excepcion("ServiciosModuloWebAdicional", e);

                    return utilerias.SerializarXML(new RespuestaHostWeb()
                        {
                            EsValido = false,
                            Mensaje = e.Message,
                            Resultado = new object()
                        });
                }
                catch (Exception ex)
                {
                    MensajesRegistros.Excepcion("ServiciosModuloWebAdicional", ex);
                    return new byte[0];
                }
            }
        }

        #endregion
    }
}
