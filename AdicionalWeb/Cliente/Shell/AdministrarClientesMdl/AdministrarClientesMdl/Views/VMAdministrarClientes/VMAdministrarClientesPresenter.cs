using System;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarClientesPresenter : Presenter<IVMAdministrarClientes>,
                                                          IVMAdministrarClientes
    {
        [EventPublication(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.EVENT_HANDLER, PublicationScope.Global)]
        public event EventHandler<EventArgs> evtAdministrarClientes;

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

        #region IVMAdministrarClientes Members

        private void FnOperacionHost(Action<IVMAdministrarClientes> fn)
        {
            IVMAdministrarClientes servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL].Services.Get<IVMAdministrarClientes>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL));
            }

            fn(servicios);
        }

        public DateTime ObtenerFechaHoraServidor()
        {
            DateTime resultado = DateTime.MinValue;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.ObtenerFechaHoraServidor();
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "Obtener Fecha y Hora del Servidor"));
                }
            });

            return resultado;
        }

        public bool Insertar(ImagenSoft.ModuloWeb.Entidades.AdministrarClientes entidad)
        {
            bool resultado = false;

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

        public bool Modificar(ImagenSoft.ModuloWeb.Entidades.AdministrarClientes entidad)
        {
            bool resultado = false;

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

        public bool Eliminar(ImagenSoft.ModuloWeb.Entidades.FiltroAdministrarClientes filtro)
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

        public ImagenSoft.ModuloWeb.Entidades.AdministrarClientes Obtener(ImagenSoft.ModuloWeb.Entidades.FiltroAdministrarClientes filtro)
        {
            ImagenSoft.ModuloWeb.Entidades.AdministrarClientes resultado = null;

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

        public ImagenSoft.ModuloWeb.Entidades.ListaAdministrarDistribuidores ObtenerDistribuidores(ImagenSoft.ModuloWeb.Entidades.FiltroAdministrarDistribuidores filtro)
        {
            ImagenSoft.ModuloWeb.Entidades.ListaAdministrarDistribuidores resultado = new ImagenSoft.ModuloWeb.Entidades.ListaAdministrarDistribuidores();

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.ObtenerDistribuidores(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener distribuidores"));
                }
            });

            return resultado;
        }

        #endregion

        internal void DisparaEvento()
        {
            EventTopic evt = this.WorkItem.EventTopics[ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.EVENT_HANDLER];
            EventArgs evento = new EventArgs();
            evt.Fire(this, evento, this.WorkItem, PublicationScope.Global);
        }

        protected virtual void OnEvtAdministrarClientes(EventArgs eventArgs)
        {
            if (this.evtAdministrarClientes != null)
            {
                this.evtAdministrarClientes(this, eventArgs);
            }
        }
    }
}

