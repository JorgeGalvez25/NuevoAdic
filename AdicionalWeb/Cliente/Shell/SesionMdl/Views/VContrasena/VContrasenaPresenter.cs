using System;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.SesionMdl.Services;
using ImagenSoft.Librerias;
using EstandarCliente.CargadorVistas.Properties;

namespace EstandarCliente.SesionMdl
{
    public partial class VContrasenaPresenter : Presenter<IVContrasena>
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public void OnCloseView()
        {
            base.CloseView();
        }

        internal bool CambiarContrasena(int usuario, string contrasenaAnterior, string nuevaContrasena)
        {
            ServiciosModulo servicios = new ServiciosModulo(WorkItem);

            try
            {
                bool resultado = false;
                resultado = servicios.CambiarContrasena(usuario, contrasenaAnterior, nuevaContrasena);
                if (resultado)
                {
                    return resultado;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                Mensaje.MensajeError(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "cambiar contraseña"));
                return false;
            }
        }
    }
}

