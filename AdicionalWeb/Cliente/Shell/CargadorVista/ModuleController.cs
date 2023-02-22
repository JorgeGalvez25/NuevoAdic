using EstandarCliente.CargadorVistas.Services;
using EstandarCliente.Infrastructure.Interface;

namespace EstandarCliente.CargadorVistas
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
            //WorkItem.Services.AddOnDemand<ServicioEmpresa, IServiciosModulo<ListaEmpresaFACELEI, EmpresaFACELEI, FiltroEmpresaFACELEI, FiltroEmpresaFACELEI>>();
            //WorkItem.Services.AddOnDemand<ServicioFechaHora, IServiciosFechaHoraServidor>();
            //WorkItem.Services.AddOnDemand<ServicioLookUps, ILookUpsFACELEI<LookUpEdit, Object>>();
            //WorkItem.Services.AddOnDemand<ServicioLookUps, ILookUpsFACELEI<RepositoryItemLookUpEdit, Object>>();
        }

        private void ExtendMenu()
        {
        }

        private void ExtendToolStrip()
        {
        }

        private void AddViews()
        {
            ServicioInicializa inicializar = new ServicioInicializa(WorkItem);
            inicializar.InicializaSistema();
            ServiciosCargador servicio = new ServiciosCargador(WorkItem);
            servicio.ShowView();
        }
    }
}
