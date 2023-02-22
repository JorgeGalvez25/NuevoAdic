namespace EstandarCliente.Infrastructure.Shell
{
    partial class RibbonForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonForm));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.applicationMenu = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
            this.barSubItemNuevo = new DevExpress.XtraBars.BarSubItem();
            this.itmSalir = new DevExpress.XtraBars.BarButtonItem();
            this.popupControlContainer1 = new DevExpress.XtraBars.PopupControlContainer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonOtro = new DevExpress.XtraBars.BarButtonItem();
            this.clientPanel = new DevExpress.XtraEditors.PanelControl();
            this._layoutWorkspace = new Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).BeginInit();
            this.popupControlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientPanel)).BeginInit();
            this.clientPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonDropDownControl = this.applicationMenu;
            this.ribbon.ApplicationButtonKeyTip = "";
            this.ribbon.ApplicationIcon = ((System.Drawing.Bitmap)(resources.GetObject("ribbon.ApplicationIcon")));
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.itmSalir,
            this.barButtonItem2,
            this.barSubItemNuevo,
            this.barButtonOtro});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 4;
            this.ribbon.Name = "ribbon";
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1016, 51);
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            // 
            // applicationMenu
            // 
            this.applicationMenu.BottomPaneControlContainer = null;
            this.applicationMenu.ItemLinks.Add(this.barSubItemNuevo);
            this.applicationMenu.ItemLinks.Add(this.itmSalir);
            this.applicationMenu.MenuDrawMode = DevExpress.XtraBars.MenuDrawMode.SmallImagesText;
            this.applicationMenu.Name = "applicationMenu";
            this.applicationMenu.Ribbon = this.ribbon;
            this.applicationMenu.RightPaneControlContainer = this.popupControlContainer1;
            this.applicationMenu.RightPaneWidth = 240;
            this.applicationMenu.ShowRightPane = true;
            // 
            // barSubItemNuevo
            // 
            this.barSubItemNuevo.Caption = "Nuevo";
            this.barSubItemNuevo.Glyph = global::EstandarCliente.Infrastructure.Shell.Properties.Resources.document_new;
            this.barSubItemNuevo.Id = 2;
            this.barSubItemNuevo.MenuDrawMode = DevExpress.XtraBars.MenuDrawMode.SmallImagesText;
            this.barSubItemNuevo.Name = "barSubItemNuevo";
            // 
            // itmSalir
            // 
            this.itmSalir.Caption = "Salir";
            this.itmSalir.Glyph = global::EstandarCliente.Infrastructure.Shell.Properties.Resources.window_close;
            this.itmSalir.Id = 0;
            this.itmSalir.Name = "itmSalir";
            this.itmSalir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // popupControlContainer1
            // 
            this.popupControlContainer1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainer1.Controls.Add(this.flowLayoutPanel1);
            this.popupControlContainer1.Controls.Add(this.labelControl1);
            this.popupControlContainer1.Location = new System.Drawing.Point(459, 113);
            this.popupControlContainer1.Name = "popupControlContainer1";
            this.popupControlContainer1.Ribbon = this.ribbon;
            this.popupControlContainer1.Size = new System.Drawing.Size(214, 199);
            this.popupControlContainer1.TabIndex = 2;
            this.popupControlContainer1.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 23);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(208, 162);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(115, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Reportes recientes";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonOtro
            // 
            this.barButtonOtro.Caption = "otro";
            this.barButtonOtro.Id = 3;
            this.barButtonOtro.Name = "barButtonOtro";
            this.barButtonOtro.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick_1);
            // 
            // clientPanel
            // 
            this.clientPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.clientPanel.Controls.Add(this.popupControlContainer1);
            this.clientPanel.Controls.Add(this._layoutWorkspace);
            this.clientPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientPanel.Location = new System.Drawing.Point(0, 51);
            this.clientPanel.Name = "clientPanel";
            this.clientPanel.Size = new System.Drawing.Size(1016, 674);
            this.clientPanel.TabIndex = 2;
            this.clientPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.clientPanel_Paint);
            // 
            // _layoutWorkspace
            // 
            this._layoutWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutWorkspace.Location = new System.Drawing.Point(0, 0);
            this._layoutWorkspace.Name = "_layoutWorkspace";
            this._layoutWorkspace.Size = new System.Drawing.Size(1016, 674);
            this._layoutWorkspace.TabIndex = 1;
            this._layoutWorkspace.Text = "_layoutWorkspace";
            // 
            // RibbonForm
            // 
            this.Appearance.Image = global::EstandarCliente.Infrastructure.Shell.Properties.Resources.window_close;
            this.Appearance.Options.UseImage = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 725);
            this.Controls.Add(this.clientPanel);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 722);
            this.Name = "RibbonForm";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modulo Web (Cliente)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RibbonForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).EndInit();
            this.popupControlContainer1.ResumeLayout(false);
            this.popupControlContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientPanel)).EndInit();
            this.clientPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraEditors.PanelControl clientPanel;
        private Microsoft.Practices.CompositeUI.WinForms.DeckWorkspace _layoutWorkspace;
        private DevExpress.XtraBars.BarButtonItem itmSalir;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarSubItem barSubItemNuevo;
        private DevExpress.XtraBars.BarButtonItem barButtonOtro;
        private DevExpress.XtraBars.PopupControlContainer popupControlContainer1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu;
    }
}