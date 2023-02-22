using System;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.Interfaces;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VLAdministrarDistribuidoresPresenter : Presenter<IVLAdministrarDistribuidores>,
                                                                IVLAdministrarDistribuidores
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

        internal void EjecutarServiciosMantenimientoPaleta(AdministrarDistribuidores t, string modo, string invoker)
        {
            IModuloServiciosMtn<AdministrarDistribuidores> servicio =
                WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL].Services.Get<IModuloServiciosMtn<AdministrarDistribuidores>>();

            if (servicio == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL));
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

        #region IVLAdministrarDistribuidores Members

        private void FnOperacionHost(Action<IVLAdministrarDistribuidores> fn)
        {
            IVLAdministrarDistribuidores servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL].Services.Get<IVLAdministrarDistribuidores>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL));
            }

            fn(servicios);
        }

        public bool Eliminar(FiltroAdministrarDistribuidores filtro)
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

        public ListaAdministrarDistribuidores ObtenerTodosFiltro(FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();

            this.FnOperacionHost((servicios) =>
                {
                    try
                    {
                        resultado = servicios.ObtenerTodosFiltro(filtro);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                    }
                });

            return resultado;
        }

        #endregion
    }
}

