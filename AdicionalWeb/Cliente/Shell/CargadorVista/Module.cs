using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace EstandarCliente.CargadorVistas
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
            ControlledWorkItem<ModuleController> workItem = _rootWorkItem.WorkItems.AddNew<ControlledWorkItem<ModuleController>>(ConstantesModulo.MODULOS.CARGADOR_VISTAS);
            (workItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl).Minimized = true;
            workItem.Controller.Run();
        }
    }
}
