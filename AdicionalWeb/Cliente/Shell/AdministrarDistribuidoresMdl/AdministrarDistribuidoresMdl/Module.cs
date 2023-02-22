using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public class Module : ModuleInit
    {
        private WorkItem _rootWorkItem;

        [InjectionConstructor]
        public Module([ServiceDependency] WorkItem rootWorkItem)
        {
            _rootWorkItem = rootWorkItem;
        }

        public override void Load()
        {
            base.Load();

            ControlledWorkItem<ModuleController> workItem = _rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>(ConstantesModulo.MODULOS.ADMINISTRAR_DISTRIBUIDORES_MDL);
            workItem.Controller.Run();
        }
    }
}
