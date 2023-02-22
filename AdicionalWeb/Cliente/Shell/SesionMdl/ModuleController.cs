using System;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.SesionMdl.Services;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using ImagenSoft.ModuloWeb.Proveedor.Publicador;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.SesionMdl
{
    public class ModuleController : WorkItemController
    {
        public override void Run()
        {
            AddServices();
            ExtendMenu();
            ExtendToolStrip();
            AddViews();
        }

        private void AddServices()
        {
            var monitor = new ServiciosModuloWebProveedor(new SesionModuloWeb() { Nombre = "MonitorSW", Sistema = "SW" }, TipoConexionUsuario.Monitor);
            WorkItem.RootWorkItem.Services.Add<IModuloWebProveedor>(monitor);
            WorkItem.RootWorkItem.Services.Add<IModuloWebPerform>(monitor);

            WorkItem.Services.AddOnDemand<ServiciosModulo, IVSesion>();
            WorkItem.Services.AddOnDemand<ServiciosModuloWebProveedor, IModuloWebProveedor>();
            WorkItem.Services.AddOnDemand<ServiciosModuloWebProveedor, IModuloWebPerform>();
        }

        private void ExtendMenu()
        {
            //TODO: add menu items here, normally by calling the "Add" method on
            //		on the WorkItem.UIExtensionSites collection. For an example 
            //		See: ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-04-340-Showing_UIElements.htm
        }

        private void ExtendToolStrip()
        {
            //TODO: add new items to the ToolStrip in the Shell. See the UIExtensionSites collection in the WorkItem. 
            //		See: ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-04-340-Showing_UIElements.htm
        }

        private void AddViews()
        {
            if (ProbarConexion())
            {
                var empresa = new ImagenSoft.ModuloWeb.Entidades.Base.DatosEmpresa();// new ImagenSoft.Framework.Entidades.Empresa();
                empresa.Id = 1;

                SesionModuloWeb temp = new SesionModuloWeb();
                temp.Empresa = empresa;

                WorkItem.RootWorkItem.Items.Add(temp, ConstantesModulo.SESION_SISTEMA);

                SesionModuloWeb sesion = mostrarVistaSesion();

                if (sesion != null)
                {
                    WorkItem.RootWorkItem.Items.Remove(temp);
                    WorkItem.RootWorkItem.Items.Add(sesion, ConstantesModulo.SESION_SISTEMA);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Mensaje.MensajeError(ListadoMensajes.Error_Conexion_Host);
                EstandarCliente.Infrastructure.Shell.FormaSplash.splash.EfectoCerrar();
                Environment.Exit(0);
            }
        }

        private SesionModuloWeb mostrarVistaSesion()
        {
            try
            {
                VSesionPresenter presenter = new VSesionPresenter();
                presenter.WorkItem = WorkItem;

                VSesion vista = new VSesion(presenter);

                presenter.View = vista;
                vista.Presenter = presenter;

                EstandarCliente.Infrastructure.Library.UI.WindowSmartPartInfo info = new EstandarCliente.Infrastructure.Library.UI.WindowSmartPartInfo();
                info.Keys.Add(EstandarCliente.Infrastructure.Library.UI.WindowWorkspaceSetting.FormStartPosition, FormStartPosition.CenterScreen);
                info.Keys.Add(EstandarCliente.Infrastructure.Library.UI.WindowWorkspaceSetting.FormBorderStyle, FormBorderStyle.None);
                info.ControlBox = true;
                info.Modal = true;
                info.Width = 500;
                info.Height = 300;

                WorkItem.Workspaces[WorkspaceNames.ModalWindows].Show(vista, info);

                ((ServiciosModuloWebProveedor)WorkItem.Services.Get<IModuloWebProveedor>()).Sesion = vista.Sesion;
                return vista.Sesion;
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
                return null;
            }
        }

        private bool ProbarConexion()
        {
            try
            {
                IVSesion servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IVSesion>();

                if (servicios == null)
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.SESIONES_MDL));
                }
                else
                {
                    return servicios.Ping();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
