using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Fachada;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using System;

namespace ImagenSoft.ModuloWeb.Servicios.WCF
{
    public class ServiciosModuloWebSocket : IModuloWeb
    {
        public byte[] EnviarPeticion(byte[] solicitud)
        {
            RespuestaHostWeb _result = null;
            try
            {
                ServicioFachadaSocket servicio = new ServicioFachadaSocket();
                var _solicitud = SerializadorModuloWeb.DeserializarXML<SolicitudHostWeb>(solicitud);
                var _r = servicio.EnviarPeticion(_solicitud);

                _result = new RespuestaHostWeb()
                {
                    EsValido = true,
                    Resultado = _r
                };
            }
            catch (System.Exception e)
            {
                try
                {
                    MensajesRegistros.Excepcion("ServiciosModuloWebSocket", e);

                    _result = new RespuestaHostWeb()
                    {
                        EsValido = false,
                        Mensaje = e.Message,
                        Resultado = new object()
                    };
                }
                catch (Exception ex)
                {
                    MensajesRegistros.Excepcion("ServiciosModuloWebSocket", ex);
                    return new byte[0];
                }
            }

            return SerializadorModuloWeb.SerializarXML(_result ?? new RespuestaHostWeb()
                {
                    EsValido = false,
                    Mensaje = "Error no administrado."
                });
        }
    }
}
