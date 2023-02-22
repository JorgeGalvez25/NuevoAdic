using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.CargadorVistas.Constants;

namespace EstandarCliente.AdministrarUsuariosMdl
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

            ControlledWorkItem<ModuleController> workItem = _rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>(ConstantesModulo.MODULOS.ADMINISTRAR_USUARIOS_MDL);
            workItem.Controller.Run();
        }
    }
}
