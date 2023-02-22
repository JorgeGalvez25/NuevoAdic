using System;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VLAdministrarUsuariosPresenter : Presenter<IVLAdministrarUsuarios>,
                                                          IVLAdministrarUsuarios
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

        #region IVLAdministrarUsuarios Members

        private void FnOperacionHost(Action<IVLAdministrarUsuarios> fn)
        {
            IVLAdministrarUsuarios servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL].Services.Get<IVLAdministrarUsuarios>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL));
            }

            fn(servicios);
        }

        public int Consecutivo()
        {
            int resultado = 0;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Consecutivo();
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "consecutivo"));
                }
            });

            return resultado;
        }

        public bool Eliminar(FiltroAdministrarUsuarios filtro)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Eliminar(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "eliminar"));
                }
            });

            return resultado;
        }

        public ListaAdministrarUsuarios ObtenerTodos(FiltroAdministrarUsuarios filtro)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.ObtenerTodos(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                }
            });

            return resultado;
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.ObtenerDistribuidores(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                }
            });

            return resultado;
        }

        #endregion

        internal void EjecutarServiciosMantenimientoPaleta(AdministrarUsuarios t, string modo, string invoker)
        {
            IModuloServiciosMtn<AdministrarUsuarios> servicio =
                WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL].Services.Get<IModuloServiciosMtn<AdministrarUsuarios>>();

            if (servicio == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL));
            }

            try
            {
                servicio.EjecutarServiciosMantenimientoPaleta(t, modo, invoker);
            }
            catch
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, modo == ConstantesModulo.OPCIONES.PROPIEDADES ? "mostrar propiedades" : modo.ToLower()));
            }
        }
    }
}

