namespace EstandarCliente.Infrastructure.Layout
{
    partial class ShellLayoutView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnPnlLateral = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this._statusLabel = new DevExpress.XtraBars.BarStaticItem();
            this._turnoLabel = new DevExpress.XtraBars.BarStaticItem();
            this.t = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbnSimples = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbnComplejos = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpDocumentos = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbnDocumentos = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpConsultas = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbnConsultas = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpVer = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbnOpciones = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpOpciones = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbnOpcion = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this._mainStatusStrip = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this._leftWorkspace = new CABDevExpress.Workspaces.XtraNavBarWorkspace();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this._rightWorkspace = new CABDevExpress.Workspaces.XtraTabWorkspace();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._leftWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._rightWorkspace)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "navBarGroup1";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ApplicationButtonKeyTip = "";
            this.ribbonControl1.ApplicationIcon = null;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnPnlLateral,
            this.barButtonGroup1,
            this._statusLabel,
            this._turnoLabel});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.t,
            this.rbpDocumentos,
            this.rbpConsultas,
            this.rbpVer,
            this.rbpOpciones});
            this.ribbonControl1.SelectedPage = this.rbpOpciones;
            this.ribbonControl1.Size = new System.Drawing.Size(612, 116);
            this.ribbonControl1.StatusBar = this._mainStatusStrip;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // btnPnlLateral
            // 
            this.btnPnlLateral.Caption = "Ocultar Panel";
            this.btnPnlLateral.Id = 0;
            this.btnPnlLateral.LargeGlyph = global::EstandarCliente.Infrastructure.Layout.Properties.Resources.window_sidebar;
            this.btnPnlLateral.Name = "btnPnlLateral";
            this.btnPnlLateral.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPnlLateral_ItemClick);
            this.btnPnlLateral.DownChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPnlLateral_DownChanged);
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.Id = 2;
            this.barButtonGroup1.ItemLinks.Add(this._statusLabel);
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // _statusLabel
            // 
            this._statusLabel.Caption = "barStaticItem1";
            this._statusLabel.Id = 3;
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // _turnoLabel
            // 
            this._turnoLabel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this._turnoLabel.Id = 4;
            this._turnoLabel.Name = "_turnoLabel";
            this._turnoLabel.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // t
            // 
            this.t.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbnSimples,
            this.rbnComplejos});
            this.t.KeyTip = "";
            this.t.Name = "t";
            this.t.Text = "Catálogos";
            // 
            // rbnSimples
            // 
            this.rbnSimples.KeyTip = "";
            this.rbnSimples.Name = "rbnSimples";
            this.rbnSimples.Text = "Simples";
            // 
            // rbnComplejos
            // 
            this.rbnComplejos.KeyTip = "";
            this.rbnComplejos.Name = "rbnComplejos";
            this.rbnComplejos.Text = "Complejos";
            // 
            // rbpDocumentos
            // 
            this.rbpDocumentos.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbnDocumentos});
            this.rbpDocumentos.KeyTip = "";
            this.rbpDocumentos.Name = "rbpDocumentos";
            this.rbpDocumentos.Text = "Documentos";
            // 
            // rbnDocumentos
            // 
            this.rbnDocumentos.KeyTip = "";
            this.rbnDocumentos.Name = "rbnDocumentos";
            this.rbnDocumentos.Text = "Documentos";
            // 
            // rbpConsultas
            // 
            this.rbpConsultas.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbnConsultas});
            this.rbpConsultas.KeyTip = "";
            this.rbpConsultas.Name = "rbpConsultas";
            this.rbpConsultas.Text = "Consultas";
            // 
            // rbnConsultas
            // 
            this.rbnConsultas.KeyTip = "";
            this.rbnConsultas.Name = "rbnConsultas";
            this.rbnConsultas.Text = "Consultas";
            // 
            // rbpVer
            // 
            this.rbpVer.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbnOpciones});
            this.rbpVer.KeyTip = "";
            this.rbpVer.Name = "rbpVer";
            this.rbpVer.Text = "Ver";
            // 
            // rbnOpciones
            // 
            this.rbnOpciones.ItemLinks.Add(this.btnPnlLateral);
            this.rbnOpciones.KeyTip = "";
            this.rbnOpciones.Name = "rbnOpciones";
            this.rbnOpciones.ShowCaptionButton = false;
            this.rbnOpciones.Text = "Opciones";
            // 
            // rbpOpciones
            // 
            this.rbpOpciones.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbnOpcion});
            this.rbpOpciones.KeyTip = "";
            this.rbpOpciones.Name = "rbpOpciones";
            this.rbpOpciones.Text = "Menú";
            this.rbpOpciones.Visible = false;
            // 
            // rbnOpcion
            // 
            this.rbnOpcion.KeyTip = "";
            this.rbnOpcion.Name = "rbnOpcion";
            this.rbnOpcion.Text = "Opciones";
            // 
            // _mainStatusStrip
            // 
            this._mainStatusStrip.ItemLinks.Add(this.barButtonGroup1);
            this._mainStatusStrip.ItemLinks.Add(this._turnoLabel);
            this._mainStatusStrip.Location = new System.Drawing.Point(0, 514);
            this._mainStatusStrip.Name = "_mainStatusStrip";
            this._mainStatusStrip.Ribbon = this.ribbonControl1;
            this._mainStatusStrip.Size = new System.Drawing.Size(612, 23);
            this._mainStatusStrip.Click += new System.EventHandler(this._mainStatusStrip_Click);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.KeyTip = "";
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.KeyTip = "";
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // _leftWorkspace
            // 
            this._leftWorkspace.ActiveGroup = this.navBarGroup1;
            this._leftWorkspace.ContentButtonHint = null;
            this._leftWorkspace.Dock = System.Windows.Forms.DockStyle.Left;
            this._leftWorkspace.Location = new System.Drawing.Point(0, 116);
            this._leftWorkspace.MaximumSize = new System.Drawing.Size(181, 0);
            this._leftWorkspace.Name = "_leftWorkspace";
            this._leftWorkspace.OptionsNavPane.ExpandedWidth = 204;
            this._leftWorkspace.Size = new System.Drawing.Size(181, 398);
            this._leftWorkspace.TabIndex = 1;
            this._leftWorkspace.Text = "xtraNavBarWorkspace1";
            this._leftWorkspace.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(181, 116);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 398);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // _rightWorkspace
            // 
            this._rightWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightWorkspace.Location = new System.Drawing.Point(189, 116);
            this._rightWorkspace.Name = "_rightWorkspace";
            this._rightWorkspace.Size = new System.Drawing.Size(423, 398);
            this._rightWorkspace.TabIndex = 8;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // ShellLayoutView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._rightWorkspace);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this._leftWorkspace);
            this.Controls.Add(this._mainStatusStrip);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "ShellLayoutView";
            this.Size = new System.Drawing.Size(612, 537);
            ((System.ComponentModel.ISupportInitialize)(this._leftWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._rightWorkspace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage t;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar _mainStatusStrip;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private CABDevExpress.Workspaces.XtraNavBarWorkspace _leftWorkspace;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnSimples;
        private System.Windows.Forms.Splitter splitter1;
        private DevExpress.XtraBars.BarButtonItem btnPnlLateral;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpVer;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnOpciones;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnComplejos;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpDocumentos;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnDocumentos;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpConsultas;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnConsultas;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarStaticItem _statusLabel;
        private DevExpress.XtraBars.BarStaticItem _turnoLabel;
        public DevExpress.XtraBars.Ribbon.RibbonPageGroup rbnOpcion;
        public CABDevExpress.Workspaces.XtraTabWorkspace _rightWorkspace;
        public DevExpress.XtraBars.Ribbon.RibbonPage rbpOpciones;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}

