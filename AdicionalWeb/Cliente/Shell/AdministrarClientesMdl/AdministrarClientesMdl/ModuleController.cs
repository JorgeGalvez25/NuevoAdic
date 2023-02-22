using EstandarCliente.AdministrarClientesMdl.Services;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.AdministrarClientesMdl
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
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModulos<AdministrarClientes>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IModuloServiciosMtn<AdministrarClientes>>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVLAdministrarClientes>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVMAdministrarClientes>();
            WorkItem.Services.AddOnDemand<ServiciosModulo, IVMAdministrarGrupos>();
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
