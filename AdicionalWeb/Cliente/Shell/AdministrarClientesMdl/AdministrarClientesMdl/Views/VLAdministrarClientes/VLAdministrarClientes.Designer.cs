
//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add View" recipe.
//
// For more information see: 
// ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-09-010-ModelViewPresenter_MVP.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

namespace EstandarCliente.AdministrarClientesMdl
{
    partial class VLAdministrarClientes
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private EstandarCliente.AdministrarClientesMdl.VLAdministrarClientesPresenter _presenter = null;

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
            if (disposing)
            {
                if (_presenter != null)
                    _presenter.Dispose();

                if (components != null)
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
            this.titulo1 = new ControlesEstandar.Titulo();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.luDistribuidor = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.luActivo = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtNombreComercial = new DevExpress.XtraEditors.TextEdit();
            this.pnlData = new DevExpress.XtraEditors.PanelControl();
            this.grdClientes = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkEnGrupos = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luDistribuidor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luActivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombreComercial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlData)).BeginInit();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // titulo1
            // 
            this.titulo1.BackColor1 = System.Drawing.Color.LightSkyBlue;
            this.titulo1.BackColor2 = System.Drawing.Color.RoyalBlue;
            this.titulo1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titulo1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.titulo1.Location = new System.Drawing.Point(0, 0);
            this.titulo1.Name = "titulo1";
            this.titulo1.Size = new System.Drawing.Size(768, 50);
            this.titulo1.TabIndex = 0;
            this.titulo1.TituloMenu = null;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.chkEnGrupos);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.luDistribuidor);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.luActivo);
            this.panel1.Controls.Add(this.txtNombreComercial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(768, 53);
            this.panel1.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(373, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(54, 13);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "Distribuidor";
            // 
            // luDistribuidor
            // 
            this.luDistribuidor.Location = new System.Drawing.Point(373, 25);
            this.luDistribuidor.Name = "luDistribuidor";
            this.luDistribuidor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luDistribuidor.Properties.NullText = "";
            this.luDistribuidor.Size = new System.Drawing.Size(146, 20);
            this.luDistribuidor.TabIndex = 11;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(267, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Activo";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(3, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(86, 13);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "Nombre Comercial";
            // 
            // luActivo
            // 
            this.luActivo.Location = new System.Drawing.Point(267, 25);
            this.luActivo.Name = "luActivo";
            this.luActivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luActivo.Properties.NullText = "";
            this.luActivo.Properties.View = this.gridLookUpEdit1View;
            this.luActivo.Size = new System.Drawing.Size(100, 20);
            this.luActivo.TabIndex = 7;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txtNombreComercial
            // 
            this.txtNombreComercial.Location = new System.Drawing.Point(3, 25);
            this.txtNombreComercial.Name = "txtNombreComercial";
            this.txtNombreComercial.Size = new System.Drawing.Size(258, 20);
            this.txtNombreComercial.TabIndex = 0;
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.grdClientes);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 103);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(768, 392);
            this.pnlData.TabIndex = 2;
            // 
            // grdClientes
            // 
            this.grdClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdClientes.EmbeddedNavigator.Name = "";
            this.grdClientes.Location = new System.Drawing.Point(2, 2);
            this.grdClientes.MainView = this.gridView1;
            this.grdClientes.Name = "grdClientes";
            this.grdClientes.Size = new System.Drawing.Size(764, 388);
            this.grdClientes.TabIndex = 0;
            this.grdClientes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grdClientes;
            this.gridView1.Name = "gridView1";
            // 
            // chkEnGrupos
            // 
            this.chkEnGrupos.AutoSize = true;
            this.chkEnGrupos.ForeColor = System.Drawing.Color.White;
            this.chkEnGrupos.Location = new System.Drawing.Point(525, 27);
            this.chkEnGrupos.Name = "chkEnGrupos";
            this.chkEnGrupos.Size = new System.Drawing.Size(111, 17);
            this.chkEnGrupos.TabIndex = 13;
            this.chkEnGrupos.Text = "Mostrar en grupos";
            this.chkEnGrupos.UseVisualStyleBackColor = true;
            // 
            // VLAdministrarClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.titulo1);
            this.Name = "VLAdministrarClientes";
            this.Size = new System.Drawing.Size(768, 495);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luDistribuidor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luActivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombreComercial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlData)).EndInit();
            this.pnlData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlesEstandar.Titulo titulo1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PanelControl pnlData;
        private DevExpress.XtraEditors.TextEdit txtNombreComercial;
        private DevExpress.XtraGrid.GridControl grdClientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GridLookUpEdit luActivo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit luDistribuidor;
        private System.Windows.Forms.CheckBox chkEnGrupos;
    }
}
