using System;
using System.Windows.Forms;
using Microsoft.Practices.ObjectBuilder;
using EstandarCliente.Infrastructure.Interface.Constants;
using DevExpress.XtraEditors;

namespace EstandarCliente.Infrastructure.Layout
{
    public partial class ShellLayoutView : XtraUserControl
    {
        private ShellLayoutViewPresenter _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ShellLayoutView"/> class.
        /// </summary>
        public ShellLayoutView()
        {
            InitializeComponent();
            _leftWorkspace.Name = WorkspaceNames.LeftWorkspace;
            _rightWorkspace.Name = WorkspaceNames.RightWorkspace;

            _rightWorkspace.Deselecting += new DevExpress.XtraTab.TabPageCancelEventHandler(_rightWorkspace_Deselecting);

            _rightWorkspace.SmartPartActivated += new EventHandler<Microsoft.Practices.CompositeUI.SmartParts.WorkspaceEventArgs>(_rightWorkspace_SmartPartActivated); 
        }

        void _rightWorkspace_SmartPartActivated(object sender, Microsoft.Practices.CompositeUI.SmartParts.WorkspaceEventArgs e)
        {
            /*UserControl uc = (UserControl)e.SmartPart;
            if (uc.Visible)
            {
                uc.Enabled = true;
            }
            uc.Visible = true;*/
        }

        void _rightWorkspace_Deselecting(object sender, DevExpress.XtraTab.TabPageCancelEventArgs e)
        {
            /*if (_rightWorkspace.SmartParts.Count > 1)
            {
                UserControl uc = (UserControl)_rightWorkspace.SmartParts[e.PageIndex];
                uc.Enabled = false;
            }*/
        }

        /// <summary>
        /// Sets the presenter.
        /// </summary>
        /// <value>The presenter.</value>
        [CreateNew]
        public ShellLayoutViewPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        /// <summary>
        /// Gets the main status strip.
        /// </summary>
        /// <value>The main status strip.</value>
        internal  DevExpress.XtraBars.Ribbon.RibbonStatusBar MainStatusStrip
        {
            get { return _mainStatusStrip; }
        }

        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup Complejos
        {
            get {return rbnComplejos;}
        }

        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup Simples
        {
            get { return rbnSimples; }
        }

        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup Documentos
        {
            get { return rbnDocumentos; }
        }

        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup Consultas
        {
            get { return rbnConsultas; }
        }
        
        internal DevExpress.XtraBars.Ribbon.RibbonControl Ribbon
        {
            get { return ribbonControl1; }
        }
        
        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup PageGroupOpciones
        {
            get { return rbnOpcion; }
        }

        internal CABDevExpress.Workspaces.XtraNavBarWorkspace LeftWorkspace
        {
            get { return _leftWorkspace; }
        }

        /// <summary>
        /// Close the application.
        /// </summary>
        private void OnFileExit(object sender, EventArgs e)
        {
            _presenter.OnFileExit();
        }

        /// <summary>
        /// Sets the status label.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetStatusLabel(string text)
        {
            _statusLabel.Caption = text;
        }
        
        public void SetTurnoLabel(string text)
        {
            _turnoLabel.Caption = text;
        }

        private void _leftWorkspace_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPnlLateral_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnPnlLateral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _leftWorkspace.Visible = !_leftWorkspace.Visible;
            
            if (_leftWorkspace.Visible)
            {
                btnPnlLateral.Caption = "Ocultar panel";
            }
            else
            {
                btnPnlLateral.Caption = "Mostrar panel";
            }
        }

        private void _mainStatusStrip_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
