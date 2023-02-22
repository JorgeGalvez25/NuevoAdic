using EstandarCliente.AdministrarDistribuidoresMdl.Services;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.AdministrarDistribuidoresMdl
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
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModulos<AdministrarDistribuidores>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModuloServiciosMtn<AdministrarDistribuidores>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVLAdministrarDistribuidores>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVMAdministrarDistribuidores>();
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
    }
}
