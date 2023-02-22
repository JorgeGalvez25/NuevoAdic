using System;
using EstandarCliente.CargadorVistas.Constants;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.SesionMdl.Services
{
    public class ServiciosModulo : IVSesion
    {
        public WorkItem _WorkItem;

        public ServiciosModulo(WorkItem workItem)
        {
            this._WorkItem = workItem;
        }

        #region Privadas

        private SesionModuloWeb ObtenerSesion()
        {
            SesionModuloWeb sesion = this._WorkItem.RootWorkItem.Items[ConstantesModulo.SESION_SISTEMA] as SesionModuloWeb;
            if (sesion == null)
            {
                throw new Exception("No existe sesión de sistema");
            }

            return sesion;
        }

        #endregion

        //public Sesion Obtener(FiltroSesion filtro)
        //{
        //    ListaSesiones lista = this.ObtenerTodosFiltro(filtro);// ServiciosGenerales.SesionFACELEIObtenerTodosFiltro(ObtenerSesion(), filtro);
        //    return lista.Count > 0 ? lista[0] : null;
        //}

        //public string EncriptarContraseña(string cUsuario)
        //{
        //    try
        //    {
        //        return Utilerias.GetMD5(cUsuario);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public bool CambiarContrasena(int usuario, string contrasenaAnterior, string nuevaContrasena)
        {
            return true;// ServiciosGenerales.SesionFACELEICambiarContrasena(ObtenerSesion(), usuario, contrasenaAnterior, nuevaContrasena);
        }

        #region IVSesion Members

        public bool Ping()
        {
            IModuloWebPerform servicio = this._WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IModuloWebPerform>();
            return servicio.Ping();
            //return true;
        }

        public bool ValidarContrasena(string cUsuario, string cEncriptada)
        {
            //var servicio = _WorkItem.RootWorkItem.Services.Get<ServiciosGeneralesServiciosWeb>();
            return Utilerias.ValidarEncriptado(cUsuario, cEncriptada);
        }

        public ListaSesiones ObtenerTodosFiltro(FiltroSesionModuloWeb filtro)
        {
            var servicio = this._WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.SesionObtenerTodosFiltro(ObtenerSesion(), filtro);
        }

        #endregion
    }
}
