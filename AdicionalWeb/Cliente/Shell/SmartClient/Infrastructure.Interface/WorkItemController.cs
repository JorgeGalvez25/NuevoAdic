//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The WorkItemController is an abstract base class that contains a WorkItem. 
// This class contains logic that would otherwise exist in the WorkItem. 
// You can use this class to partition your code between a class that derives from WorkItemController and a WorkItem.
// 
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-010-How_to_Create_Smart_Client_Solutions.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using EstandarCliente.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.Infrastructure.Interface
{
    /// <summary>
    /// Base class for a WorkItem controller.
    /// </summary>
    public abstract class WorkItemController : IWorkItemController
    {
        private WorkItem _workItem;

        /// <summary>
        /// Gets or sets the work item.
        /// </summary>
        /// <value>The work item.</value>
        [ServiceDependency]
        public WorkItem WorkItem
        {
            get { return _workItem; }
            set { _workItem = value; }
        }

        public IActionCatalogService ActionCatalogService
        {
            get { return _workItem.Services.Get<IActionCatalogService>(); }
        }

        public virtual void Run()
        {
        }

        /// <summary>
        /// Creates and shows a smart part on the specified workspace.
        /// </summary>
        /// <typeparam name="TView">The type of the smart part to create and show.</typeparam>
        /// <param name="workspaceName">The name of the workspace in which to show the smart part.</param>
        /// <returns>The new smart part instance.</returns>
        protected virtual TView ShowViewInWorkspace<TView>(string workspaceName)
        {
            TView view = WorkItem.SmartParts.AddNew<TView>();
            WorkItem.Workspaces[workspaceName].Show(view);
            return view;
        }

        /// <summary>
        /// Shows a specific smart part in the workspace. If a smart part with the specified id
        /// is not found in the <see cref="WorkItem.SmartParts"/> collection, a new instance
        /// will be created; otherwise, the existing instance will be re used.
        /// </summary>
        /// <typeparam name="TView">The type of the smart part to show.</typeparam>
        /// <param name="viewId">The id of the smart part in the <see cref="WorkItem.SmartParts"/> collection.</param>
        /// <param name="workspaceName">The name of the workspace in which to show the smart part.</param>
        /// <returns>The smart part instance.</returns>
        protected virtual TView ShowViewInWorkspace<TView>(string viewId, string workspaceName)
        {
            TView view = default(TView);
            if (WorkItem.SmartParts.Contains(viewId))
            {
                view = WorkItem.SmartParts.Get<TView>(viewId);
            }
            else
            {
                view = WorkItem.SmartParts.AddNew<TView>();
            }

            WorkItem.Workspaces[workspaceName].Show(view);

            return view;
        }

        protected void RegisterLaunchPoint(string extName, string text, System.Drawing.Image icon, string commandName)
        {
            RegisterLaunchPoint(extName, text, string.Empty, icon, commandName);
        }

        protected void RegisterLaunchPoint(string extName, string text, string descripcion, System.Drawing.Image icon, string commandName)
        {
            DevExpress.XtraBars.BarButtonItem barButtonItemx = new DevExpress.XtraBars.BarButtonItem();
            barButtonItemx.Caption = text;
            //barButtonItemx.Hint = text;

            DevExpress.Utils.SuperToolTip tip = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem title = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem tooltipitem = new DevExpress.Utils.ToolTipItem();

            title.Text = text;
            tooltipitem.Text = descripcion;

            tip.Items.Add(title);
            tip.Items.Add(tooltipitem);

            barButtonItemx.SuperTip = tip;


            barButtonItemx.LargeGlyph = icon;
            WorkItem.Commands[commandName].AddInvoker(barButtonItemx, "ItemClick");
            WorkItem.UIExtensionSites[extName].Add(barButtonItemx);
        }

        protected void RegisterStatusStrip(string statusName, string text)
        {
            DevExpress.XtraBars.BarStaticItem item = new DevExpress.XtraBars.BarStaticItem();
            item.Caption = text;
            item.Name = statusName;

            WorkItem.UIExtensionSites["MainStatus"].Add(item);
        }

        protected void ChangeStatusStrip(string statusName, string text)
        {
            UIExtensionSite site = WorkItem.Parent.UIExtensionSites["MainStatus"];

            site.Clear();




            //DevExpress.XtraBars.BarStaticItem item = new DevExpress.XtraBars.BarStaticItem();
            //item.Caption = text;
            //item.Name = statusName;

            //WorkItem.UIExtensionSites["MainStatus"].Add(item);
        }
    }
}
