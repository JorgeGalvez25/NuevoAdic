using System;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VMAdministrarDistribuidoresPresenter : Presenter<IVMAdministrarDistribuidores>,
                                                                IVMAdministrarDistribuidores
    {
        [EventPublication(ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.EVENT_HANDLER, PublicationScope.Global)]
        public event EventHandler<EventArgs> evtAdministrarDistribuidores;

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

        #region IVMAdministrarDistribuidores Members

        private void FnOperacionHost(Action<IVMAdministrarDistribuidores> fn)
        {
            IVMAdministrarDistribuidores servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL].Services.Get<IVMAdministrarDistribuidores>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL));
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
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener consecutivo"));
                    }
                });

            return resultado;
        }

        public AdministrarDistribuidores Obtener(FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidores resultado = null;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Obtener(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener"));
                }
            });

            return resultado;
        }

        public AdministrarDistribuidores Insertar(AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores resultado = null;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Insertar(entidad);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "insertar"));
                }
            });

            return resultado;
        }

        public AdministrarDistribuidores Modificar(AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores resultado = null;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Modificar(entidad);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "modificar"));
                }
            });

            return resultado;
        }

        #endregion

        internal void DisparaEvento()
        {
            EventTopic evt = this.WorkItem.EventTopics[ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.EVENT_HANDLER];
            EventArgs evento = new EventArgs();
            evt.Fire(this, evento, this.WorkItem, PublicationScope.Global);
        }

        protected virtual void OnEvtAdministrarClientes(EventArgs eventArgs)
        {
            if (this.evtAdministrarDistribuidores != null)
            {
                this.evtAdministrarDistribuidores(this, eventArgs);
            }
        }
    }
}

