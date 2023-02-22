using System;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.Infrastructure.Interface.Services;
using EstandarCliente.Infrastructure.Interface.Constants;

namespace EstandarCliente.Infrastructure.Module
{
    public class ModuleController : WorkItemController
    {
        public override void Run()
        {
            AddServices();
            ExtendMenu();
            ExtendToolStrip();
            AddViews();
            AgregaRightWorkspace();
        }

        private void AddServices()
        {
            WorkItem.RootWorkItem.Services.AddNew<ServiciosMenuAplicacion, IServiciosMenuAplicacion>();
        }

        private void ExtendMenu()
        {
        }

        private void ExtendToolStrip()
        {
        }

        private void AddViews()
        {
        }

        DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = null;

        DevExpress.XtraBars.Ribbon.RibbonControl ribbon = null;

        CABDevExpress.Workspaces.XtraTabWorkspace tabWorkSpace = null;

        object _smartPart_ = null;

        private bool HabilitaPaginaMenu(bool activar)
        {
            ribbonPage.Visible = activar;
            //if (activar)
            //{
            //    ribbon.SelectedPage = ribbonPage;
            //}

            return activar;
        }

        private void AgregaRightWorkspace()
        {
            tabWorkSpace = WorkItem.RootWorkItem.Items[UIExtensionSiteNames.RightWorkspace] as CABDevExpress.Workspaces.XtraTabWorkspace;

            ribbonPage = WorkItem.RootWorkItem.Items[UIExtensionSiteNames.PaginaMenu] as DevExpress.XtraBars.Ribbon.RibbonPage;

            ribbon = WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;

            tabWorkSpace.SmartPartActivated += new EventHandler<Microsoft.Practices.CompositeUI.SmartParts.WorkspaceEventArgs>(tabWorkSpace_SmartPartActivated);
            tabWorkSpace.Deselecting += new DevExpress.XtraTab.TabPageCancelEventHandler(tabWorkSpace_Deselecting);
            tabWorkSpace.SmartPartClosing += new EventHandler<Microsoft.Practices.CompositeUI.SmartParts.WorkspaceCancelEventArgs>(tabWorkSpace_SmartPartClosing);
            ribbon.MouseLeave += new EventHandler(ribbon_MouseLeave);
            tabWorkSpace.CloseButtonClick += new EventHandler(tabWorkSpace_CloseButtonClick);
        }

        void tabWorkSpace_CloseButtonClick(object sender, EventArgs e)
        {
            Application.DoEvents();

            try
            {
                EstandarCliente.Infrastructure.Interface.Services.IServicioBotonCerrarTab servicio =
                    (EstandarCliente.Infrastructure.Interface.Services.IServicioBotonCerrarTab)_smartPart_;

                servicio.BotonCerrarClick();
            }
            catch (Exception ex)
            {
                //ImagenSoft.Librerias.ExcepcionLogs.Excepcion("tabWorkSpace_CloseButtonClick", ex);
                MessageBox.Show(ex.Message);
            }

        }

        void ribbon_MouseLeave(object sender, EventArgs e)
        {
            //if (tabWorkSpace.SmartParts.Count > 0)
            //{
            //    if (ribbon.SelectedPage != ribbonPage)
            //    {
            //        ribbon.SelectedPage = ribbonPage;
            //    }
            //}
        }

        void tabWorkSpace_SmartPartClosing(object sender, Microsoft.Practices.CompositeUI.SmartParts.WorkspaceCancelEventArgs e)
        {
            Application.DoEvents();

            if (!HabilitaPaginaMenu((tabWorkSpace.SmartParts.Count > 1)))
            {
                EstandarCliente.Infrastructure.Interface.Services.IServiciosMenuAplicacion servicio =
                    WorkItem.RootWorkItem.Services.Get<EstandarCliente.Infrastructure.Interface.Services.IServiciosMenuAplicacion>();

                servicio.LimpiaMenuCatalogo();
            }

            UserControl uc = (UserControl)tabWorkSpace.SmartParts[0];
            uc.Visible = true;
        }

        void tabWorkSpace_Deselecting(object sender, DevExpress.XtraTab.TabPageCancelEventArgs e)
        {
            Application.DoEvents();

            if (!HabilitaPaginaMenu((tabWorkSpace.SmartParts.Count > 1)))
            {
                EstandarCliente.Infrastructure.Interface.Services.IServiciosMenuAplicacion servicio =
                    WorkItem.RootWorkItem.Services.Get<EstandarCliente.Infrastructure.Interface.Services.IServiciosMenuAplicacion>();

                servicio.LimpiaMenuCatalogo();
            }

            if (tabWorkSpace.SmartParts.Count > 1)
            {
                UserControl uc = (UserControl)tabWorkSpace.SmartParts[e.PageIndex];
                uc.Enabled = false;
            }
        }

        void tabWorkSpace_SmartPartActivated(object sender, Microsoft.Practices.CompositeUI.SmartParts.WorkspaceEventArgs e)
        {
            Application.DoEvents();

            HabilitaPaginaMenu((tabWorkSpace.SmartParts.Count >= 1));

            UserControl uc = (UserControl)e.SmartPart;
            if (uc.Visible)
            {
                uc.Enabled = true;
            }
            uc.Visible = true;

            uc.Focus();

            _smartPart_ = e.SmartPart;
            try
            {
                EstandarCliente.Infrastructure.Interface.Services.IConfiguraMenu sevicio =
                    (EstandarCliente.Infrastructure.Interface.Services.IConfiguraMenu)e.SmartPart;

                if (sevicio != null)
                {
                    sevicio.CreaMenu();
                    ribbon.SelectedPage = ribbonPage;
                }
            }
            catch (Exception ex)
            {
                //ImagenSoft.Librerias.ExcepcionLogs.Excepcion("tabWorkSpace_SmartPartActivated", ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
