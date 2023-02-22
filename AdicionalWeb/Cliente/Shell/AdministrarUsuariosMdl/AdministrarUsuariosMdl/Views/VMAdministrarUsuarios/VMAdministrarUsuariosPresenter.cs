using System;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VMAdministrarUsuariosPresenter : Presenter<IVMAdministrarUsuarios>,
                                                          IVMAdministrarUsuarios
    {
        [EventPublication(ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.EVENT_HANDLER, PublicationScope.Global)]
        public event EventHandler<EventArgs> evtAdministrarUsuarios;

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

        #region IVMAdministrarUsuarios Members

        private void FnOperacionHost(Action<IVMAdministrarUsuarios> fn)
        {
            IVMAdministrarUsuarios servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL].Services.Get<IVMAdministrarUsuarios>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL));
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
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener fecha hora del servidor"));
                }
            });

            return resultado;
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

        public bool Insertar(AdministrarUsuarios entidad)
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

        public bool Modificar(AdministrarUsuarios entidad)
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

        public AdministrarUsuarios Obtener(FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuarios resultado = null;

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
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener distribuidores"));
                }
            });

            return resultado;
        }

        #endregion

        internal void DisparaEvento()
        {
            EventTopic evt = this.WorkItem.EventTopics[ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.EVENT_HANDLER];
            EventArgs evento = new EventArgs();
            evt.Fire(this, evento, this.WorkItem, PublicationScope.Global);
        }

        protected virtual void OnEvtAdministrarClientes(EventArgs eventArgs)
        {
            if (this.evtAdministrarUsuarios != null)
            {
                this.evtAdministrarUsuarios(this, eventArgs);
            }
        }
    }
}

