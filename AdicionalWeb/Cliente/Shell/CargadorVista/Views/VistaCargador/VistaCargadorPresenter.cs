using System;
using System.Collections.Generic;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.CargadorVistas.CargadorVistasMdl
{
    public partial class VistaCargadorPresenter : Presenter<IVistaCargador>
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

        internal List<ImagenSoft.Framework.Entidades.OpcionMenu> ObtenerMenu()
        {
            return (List<ImagenSoft.Framework.Entidades.OpcionMenu>)WorkItem.Parent.Items.Get(ConstantesModulo.MENUS.MENU);
        }

        #region Servicios

        internal void CargarMonitorTransmisiones()
        {
            IModulos<MonitorTransaccion> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.MONITOR_TRANSMISIONES_MDL].Services.Get<IModulos<MonitorTransaccion>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.MONITOR_TRANSMISIONES_MDL));
            }
        }

        internal void CargarMonitorCambioPrecios()
        {
            IModulos<MonitorCambioPrecio> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.MONITOR_CAMBIO_PRECIO_MDL].Services.Get<IModulos<MonitorCambioPrecio>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.MONITOR_CAMBIO_PRECIO_MDL));
            }
        }

        internal void CargarMonitorConexiones()
        {
            IModulos<MonitorConexiones> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.MONITOR_CONEXIONES_MDL].Services.Get<IModulos<MonitorConexiones>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.MONITOR_CONEXIONES_MDL));
            }
        }

        internal void CargarMonitorAplicaciones()
        {
            IModulos<MonitorAplicaciones> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.MONITOR_APLICACIONES_MDL].Services.Get<IModulos<MonitorAplicaciones>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.MONITOR_APLICACIONES_MDL));
            }
        }

        #endregion

        #region Catalogos

        internal void CargarClientes()
        {
            IModulos<AdministrarClientes> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL].Services.Get<IModulos<AdministrarClientes>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL));
            }
        }

        internal void CargarUsuarios()
        {
            IModulos<AdministrarUsuarios> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL].Services.Get<IModulos<AdministrarUsuarios>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL));
            }
        }

        internal void CargarCambioPrecios()
        {
            IModulos<PreciosGasolinas> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.CAMBIO_PRECIOS].Services.Get<IModulos<PreciosGasolinas>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.CAMBIO_PRECIOS));
            }
        }

        internal void CargarDistribuidores()
        {
            IModulos<AdministrarDistribuidores> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL].Services.Get<IModulos<AdministrarDistribuidores>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL));
            }
        }

        #endregion

        #region Pruebas

        internal void CargarPruebasTransmisiones()
        {
            IModulos<Object> servicios = WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.PRUEBAS_MDL].Services.Get<IModulos<Object>>();

            if (servicios != null)
            {
                servicios.MostrarVista(null);
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.PRUEBAS_MDL));
            }
        }

        #endregion
    }
}