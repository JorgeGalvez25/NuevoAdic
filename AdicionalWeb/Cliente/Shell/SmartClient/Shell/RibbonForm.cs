using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using EstandarCliente.Infrastructure.Interface.Constants;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.Services;
using DevExpress.XtraBars.Ribbon;
using CABDevExpress.Workspaces;
using System.Collections;
using DevExpress.XtraTab;
using EstandarCliente.Infrastructure.Interface.Services;

namespace EstandarCliente.Infrastructure.Shell
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly WorkItem workItem;
        private IWorkItemTypeCatalogService workItemTypeCatalog;

        [InjectionConstructor]
        public RibbonForm(WorkItem workItem, IWorkItemTypeCatalogService workItemTypeCatalog)
            : this()
        {
            this.workItem = workItem;
            this.workItemTypeCatalog = workItemTypeCatalog;

            this.workItem.RootWorkItem.Items.Add(this.ApplicationMenu, UIExtensionSiteNames.ApplicationMenu);
            this.workItem.RootWorkItem.Items.Add(this.barSubItemNuevo, UIExtensionSiteNames.ApplicationMenuNuevo);
        }

        public RibbonForm()
        {
            InitializeComponent();
            _layoutWorkspace.Name = WorkspaceNames.LayoutWorkspace;
        }

        private void clientPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        internal ApplicationMenu ApplicationMenu
        {
            get { return applicationMenu; }
        }

        internal BarSubItem BarSubItemNuevo
        {
            get { return barSubItemNuevo; }
        }

        private void barButtonItem1_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        private void RibbonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool ImplementaBotonCerrar = false;

            DialogResult salir = MessageBox.Show("¿Desea salir del Sistema?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            Cursor = Cursors.WaitCursor;

            if (salir == DialogResult.Yes)
            {
                // Se recorren todas las Tabs que se muestran

                foreach (Object Tab in workItem.Workspaces[WorkspaceNames.RightWorkspace].SmartParts)
                {
                    Type BotonCerrarTab = Tab.GetType().GetInterface("IServicioBotonCerrarTab");

                    // Se verifica si se implementa la interfaz del botón de cierre en el Tab para mandarla invocar

                    if (BotonCerrarTab != null)
                    {
                        ImplementaBotonCerrar = true;

                        workItem.Workspaces[WorkspaceNames.RightWorkspace].Show(Tab);
                        (Tab as IServicioBotonCerrarTab).BotonCerrarClick();
                        //try { (Tab as IServicioBotonCerrarTab).BotonCerrarClick(); }
                        //catch { }
                    }
                }

                // Revisa si aun hay Tabs abiertos, si hay cancela el proceso de cierre

                if (workItem.Workspaces[WorkspaceNames.RightWorkspace].SmartParts.Count > 0 && ImplementaBotonCerrar)
                {
                    Cursor = Cursors.Default;

                    e.Cancel = true;
                }
            }
            else
            {
                Cursor = Cursors.Default;

                e.Cancel = true;
            }
        }
    }
}