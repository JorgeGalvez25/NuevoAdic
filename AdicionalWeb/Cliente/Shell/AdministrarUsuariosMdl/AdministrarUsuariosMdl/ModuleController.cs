using System;
using System.Windows.Forms;
using EstandarCliente.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using EstandarCliente.AdministrarUsuariosMdl.Services;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl
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
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModulos<AdministrarUsuarios>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModuloServiciosMtn<AdministrarUsuarios>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVLAdministrarUsuarios>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVMAdministrarUsuarios>();
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
            //TODO: create the Module views, add them to the WorkItem and show them in 
            //		a Workspace. See: ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/03-01-040-How_to_Add_a_View_with_a_Presenter.htm

            // To create and add a view you can customize the following sentence
            // SampleView view = ShowViewInWorkspace<SampleView>(WorkspaceNames.SampleWorkspace);

        }

        //TODO: Add CommandHandlers and/or Event Subscriptions
        //		See: ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-04-350-Registering_Commands.htm
        //		See: ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-04-320-Publishing_and_Subscribing_to_Events.htm
    }
}
