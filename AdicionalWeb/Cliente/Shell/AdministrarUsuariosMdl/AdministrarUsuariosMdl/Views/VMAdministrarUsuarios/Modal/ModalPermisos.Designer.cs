namespace EstandarCliente.AdministrarUsuariosMdl.Views.VMAdministrarUsuarios.Modal
{
    partial class ModalPermisos
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
            this.titulo1 = new ControlesEstandar.Titulo();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpModificar = new DevExpress.XtraEditors.GroupControl();
            this.chkCambiarDesface = new System.Windows.Forms.CheckBox();
            this.chkCambiarPassword = new System.Windows.Forms.CheckBox();
            this.chkCambiarActivo = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkEliminar = new System.Windows.Forms.CheckBox();
            this.chkModificar = new System.Windows.Forms.CheckBox();
            this.chkRegistrar = new System.Windows.Forms.CheckBox();
            this.chkMostrar = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpRegistrar = new DevExpress.XtraEditors.GroupControl();
            this.chkCambiarDesfaceRegistrar = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpModificar)).BeginInit();
            this.grpModificar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRegistrar)).BeginInit();
            this.grpRegistrar.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.titulo1.Size = new System.Drawing.Size(565, 50);
            this.titulo1.TabIndex = 0;
            this.titulo1.TituloMenu = "Administrar Permisos";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gridControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(197, 144);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Modulos";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Name = "";
            this.gridControl1.Location = new System.Drawing.Point(2, 20);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(193, 122);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowRowSizing = true;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsDetail.AllowZoomDetail = false;
            this.gridView1.OptionsDetail.EnableMasterViewMode = false;
            this.gridView1.OptionsDetail.ShowDetailTabs = false;
            this.gridView1.OptionsDetail.SmartDetailExpand = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowColumnHeaders = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorzLines = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowPreviewLines = false;
            this.gridView1.OptionsView.ShowVertLines = false;
            // 
            // grpModificar
            // 
            this.grpModificar.Controls.Add(this.chkCambiarDesface);
            this.grpModificar.Controls.Add(this.chkCambiarPassword);
            this.grpModificar.Controls.Add(this.chkCambiarActivo);
            this.grpModificar.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpModificar.Location = new System.Drawing.Point(428, 0);
            this.grpModificar.Name = "grpModificar";
            this.grpModificar.Size = new System.Drawing.Size(136, 144);
            this.grpModificar.TabIndex = 2;
            this.grpModificar.Text = "Modificar";
            // 
            // chkCambiarDesface
            // 
            this.chkCambiarDesface.AutoSize = true;
            this.chkCambiarDesface.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCambiarDesface.Location = new System.Drawing.Point(2, 62);
            this.chkCambiarDesface.Name = "chkCambiarDesface";
            this.chkCambiarDesface.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.chkCambiarDesface.Size = new System.Drawing.Size(132, 21);
            this.chkCambiarDesface.TabIndex = 2;
            this.chkCambiarDesface.Text = "Cambiar Desface";
            this.chkCambiarDesface.UseVisualStyleBackColor = true;
            // 
            // chkCambiarPassword
            // 
            this.chkCambiarPassword.AutoSize = true;
            this.chkCambiarPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCambiarPassword.Location = new System.Drawing.Point(2, 41);
            this.chkCambiarPassword.Name = "chkCambiarPassword";
            this.chkCambiarPassword.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.chkCambiarPassword.Size = new System.Drawing.Size(132, 21);
            this.chkCambiarPassword.TabIndex = 1;
            this.chkCambiarPassword.Text = "Cambiar Contraseña";
            this.chkCambiarPassword.UseVisualStyleBackColor = true;
            // 
            // chkCambiarActivo
            // 
            this.chkCambiarActivo.AutoSize = true;
            this.chkCambiarActivo.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCambiarActivo.Location = new System.Drawing.Point(2, 20);
            this.chkCambiarActivo.Name = "chkCambiarActivo";
            this.chkCambiarActivo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.chkCambiarActivo.Size = new System.Drawing.Size(132, 21);
            this.chkCambiarActivo.TabIndex = 0;
            this.chkCambiarActivo.Text = "Cambiar Activo";
            this.chkCambiarActivo.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkEliminar);
            this.panel1.Controls.Add(this.chkModificar);
            this.panel1.Controls.Add(this.chkRegistrar);
            this.panel1.Controls.Add(this.chkMostrar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(197, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(95, 144);
            this.panel1.TabIndex = 1;
            // 
            // chkEliminar
            // 
            this.chkEliminar.AutoSize = true;
            this.chkEliminar.Location = new System.Drawing.Point(6, 89);
            this.chkEliminar.Name = "chkEliminar";
            this.chkEliminar.Size = new System.Drawing.Size(62, 17);
            this.chkEliminar.TabIndex = 3;
            this.chkEliminar.Text = "Eliminar";
            this.chkEliminar.UseVisualStyleBackColor = true;
            // 
            // chkModificar
            // 
            this.chkModificar.AutoSize = true;
            this.chkModificar.Location = new System.Drawing.Point(6, 66);
            this.chkModificar.Name = "chkModificar";
            this.chkModificar.Size = new System.Drawing.Size(69, 17);
            this.chkModificar.TabIndex = 2;
            this.chkModificar.Text = "Modificar";
            this.chkModificar.UseVisualStyleBackColor = true;
            // 
            // chkRegistrar
            // 
            this.chkRegistrar.AutoSize = true;
            this.chkRegistrar.Location = new System.Drawing.Point(6, 43);
            this.chkRegistrar.Name = "chkRegistrar";
            this.chkRegistrar.Size = new System.Drawing.Size(70, 17);
            this.chkRegistrar.TabIndex = 1;
            this.chkRegistrar.Text = "Registrar";
            this.chkRegistrar.UseVisualStyleBackColor = true;
            // 
            // chkMostrar
            // 
            this.chkMostrar.AutoSize = true;
            this.chkMostrar.Location = new System.Drawing.Point(6, 20);
            this.chkMostrar.Name = "chkMostrar";
            this.chkMostrar.Size = new System.Drawing.Size(63, 17);
            this.chkMostrar.TabIndex = 0;
            this.chkMostrar.Text = "Mostrar";
            this.chkMostrar.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grpModificar);
            this.panel2.Controls.Add(this.grpRegistrar);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.groupControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(565, 144);
            this.panel2.TabIndex = 1;
            // 
            // grpRegistrar
            // 
            this.grpRegistrar.Controls.Add(this.chkCambiarDesfaceRegistrar);
            this.grpRegistrar.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpRegistrar.Location = new System.Drawing.Point(292, 0);
            this.grpRegistrar.Name = "grpRegistrar";
            this.grpRegistrar.Size = new System.Drawing.Size(136, 144);
            this.grpRegistrar.TabIndex = 3;
            this.grpRegistrar.Text = "Registrar";
            // 
            // chkCambiarDesfaceRegistrar
            // 
            this.chkCambiarDesfaceRegistrar.AutoSize = true;
            this.chkCambiarDesfaceRegistrar.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCambiarDesfaceRegistrar.Location = new System.Drawing.Point(2, 20);
            this.chkCambiarDesfaceRegistrar.Name = "chkCambiarDesfaceRegistrar";
            this.chkCambiarDesfaceRegistrar.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.chkCambiarDesfaceRegistrar.Size = new System.Drawing.Size(132, 21);
            this.chkCambiarDesfaceRegistrar.TabIndex = 3;
            this.chkCambiarDesfaceRegistrar.Text = "Cambiar Desface";
            this.chkCambiarDesfaceRegistrar.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.btnCancelar);
            this.panel3.Controls.Add(this.btnAceptar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 194);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(565, 47);
            this.panel3.TabIndex = 2;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(487, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(406, 12);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            // 
            // ModalPermisos
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(565, 241);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.titulo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalPermisos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Permisos";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpModificar)).EndInit();
            this.grpModificar.ResumeLayout(false);
            this.grpModificar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRegistrar)).EndInit();
            this.grpRegistrar.ResumeLayout(false);
            this.grpRegistrar.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ControlesEstandar.Titulo titulo1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl grpModificar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkEliminar;
        private System.Windows.Forms.CheckBox chkModificar;
        private System.Windows.Forms.CheckBox chkRegistrar;
        private System.Windows.Forms.CheckBox chkMostrar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkCambiarPassword;
        private System.Windows.Forms.CheckBox chkCambiarActivo;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.CheckBox chkCambiarDesface;
        private DevExpress.XtraEditors.GroupControl grpRegistrar;
        private System.Windows.Forms.CheckBox chkCambiarDesfaceRegistrar;
    }
}