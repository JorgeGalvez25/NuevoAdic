using Microsoft.Practices.CompositeUI.EventBroker;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.Infrastructure.Interface.Constants;

namespace EstandarCliente.Infrastructure.Layout
{
    public class ShellLayoutViewPresenter : Presenter<ShellLayoutView>
    {
        protected override void OnViewSet()
        {
            //WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.MainMenu, View.MainMenuStrip);
            WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.MainStatus, View.MainStatusStrip);
           // WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.MainToolbar, View.MainToolbarStrip);
            WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.Complejos, View.Complejos);
            WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.Simples, View.Simples);
            WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.Documentos, View.Documentos);
            WorkItem.UIExtensionSites.RegisterSite(UIExtensionSiteNames.Consultas, View.Consultas);
            WorkItem.Items.Add(View.Ribbon,UIExtensionSiteNames.Ribbon);
            WorkItem.Items.Add(View.rbnOpcion,UIExtensionSiteNames.MenuCatalogo);
            WorkItem.Items.Add(View.rbpOpciones, UIExtensionSiteNames.PaginaMenu);
            WorkItem.Items.Add(View._rightWorkspace, UIExtensionSiteNames.RightWorkspace);
            WorkItem.Items.Add(View.LeftWorkspace, UIExtensionSiteNames.LeftWorkspace);
            WorkItem.Items.Add(View.MainStatusStrip, UIExtensionSiteNames.MainStatus);
        }

        /// <summary>
        /// Status update handler. Updates the status strip on the main form.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        [EventSubscription(EventTopicNames.StatusUpdate, ThreadOption.UserInterface)]
        public void StatusUpdateHandler(object sender, EventArgs<string> e)
        {
            View.SetStatusLabel(e.Data);
        }

        /// <summary>
        /// Called when the user asks to exit the application.
        /// </summary>
        public void OnFileExit()
        {
            View.ParentForm.Close();
        }
    }
}
