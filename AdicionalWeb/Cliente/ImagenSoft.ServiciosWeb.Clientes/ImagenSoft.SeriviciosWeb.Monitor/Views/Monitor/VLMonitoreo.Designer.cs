namespace ImagenSoft.SeriviciosWeb.Monitor.Views.Monitor
{
    partial class VLMonitoreo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ttlTitulo = new ControlesEstandar.Titulo();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lstClientes = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.verStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reenviarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblOcultarInfo = new System.Windows.Forms.LinkLabel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlOperaciones = new System.Windows.Forms.Panel();
            this.grpDetalleCliente = new DevExpress.XtraEditors.GroupControl();
            this.btnReenviar = new System.Windows.Forms.Button();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblIPs = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoSesion = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaConexion = new DevExpress.XtraEditors.LabelControl();
            this.grpInfoCliente = new DevExpress.XtraEditors.GroupControl();
            this.lblPaginas = new System.Windows.Forms.LinkLabel();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.lblContacto = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lblTelefonos = new DevExpress.XtraEditors.LabelControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtTipoSesion = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtRazonSocial = new System.Windows.Forms.TextBox();
            this.picSucursal = new DevExpress.XtraEditors.PictureEdit();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnGuardarCliente = new DevExpress.XtraEditors.PictureEdit();
            this.btnEditarCliente = new DevExpress.XtraEditors.PictureEdit();
            this.ToolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.defaultToolTipController1 = new DevExpress.Utils.DefaultToolTipController(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstClientes)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlOperaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDetalleCliente)).BeginInit();
            this.grpDetalleCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfoCliente)).BeginInit();
            this.grpInfoCliente.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSucursal.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnGuardarCliente.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditarCliente.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ttlTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(525, 49);
            this.ToolTipController.SetSuperTip(this.panel1, null);
            this.panel1.TabIndex = 0;
            // 
            // ttlTitulo
            // 
            this.ttlTitulo.BackColor1 = System.Drawing.Color.LightSkyBlue;
            this.ttlTitulo.BackColor2 = System.Drawing.Color.RoyalBlue;
            this.ttlTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ttlTitulo.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.ttlTitulo.Location = new System.Drawing.Point(0, 0);
            this.ttlTitulo.Name = "ttlTitulo";
            this.ttlTitulo.Size = new System.Drawing.Size(525, 49);
            this.ToolTipController.SetSuperTip(this.ttlTitulo, null);
            this.ttlTitulo.TabIndex = 0;
            this.ttlTitulo.TituloMenu = "Monitor";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lstClientes);
            this.groupControl1.Controls.Add(this.panel2);
            this.groupControl1.Controls.Add(this.pnlFooter);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 49);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(525, 651);
            this.ToolTipController.SetSuperTip(this.groupControl1, null);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Lista de clientes";
            // 
            // lstClientes
            // 
            this.lstClientes.ContextMenuStrip = this.contextMenuStrip1;
            this.lstClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstClientes.EmbeddedNavigator.Name = "";
            this.lstClientes.EmbeddedNavigator.ToolTipController = this.defaultToolTipController1.DefaultController;
            this.lstClientes.Location = new System.Drawing.Point(2, 20);
            this.lstClientes.MainView = this.gridView1;
            this.lstClientes.Name = "lstClientes";
            this.lstClientes.Size = new System.Drawing.Size(521, 343);
            this.lstClientes.TabIndex = 0;
            this.lstClientes.ToolTipController = this.ToolTipController;
            this.lstClientes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verStatusToolStripMenuItem,
            this.reenviarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 48);
            this.ToolTipController.SetSuperTip(this.contextMenuStrip1, null);
            // 
            // verStatusToolStripMenuItem
            // 
            this.verStatusToolStripMenuItem.Name = "verStatusToolStripMenuItem";
            this.verStatusToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.verStatusToolStripMenuItem.Text = "Ver status";
            // 
            // reenviarToolStripMenuItem
            // 
            this.reenviarToolStripMenuItem.Name = "reenviarToolStripMenuItem";
            this.reenviarToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.reenviarToolStripMenuItem.Text = "Reenviar";
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.lstClientes;
            this.gridView1.Name = "gridView1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblOcultarInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(2, 363);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(521, 21);
            this.ToolTipController.SetSuperTip(this.panel2, null);
            this.panel2.TabIndex = 9;
            // 
            // lblOcultarInfo
            // 
            this.lblOcultarInfo.AutoSize = true;
            this.lblOcultarInfo.Location = new System.Drawing.Point(416, 5);
            this.lblOcultarInfo.Name = "lblOcultarInfo";
            this.lblOcultarInfo.Size = new System.Drawing.Size(100, 13);
            this.ToolTipController.SetSuperTip(this.lblOcultarInfo, null);
            this.lblOcultarInfo.TabIndex = 0;
            this.lblOcultarInfo.TabStop = true;
            this.lblOcultarInfo.Text = "Mostrar Información";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.pnlOperaciones);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(2, 384);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(521, 265);
            this.ToolTipController.SetSuperTip(this.pnlFooter, null);
            this.pnlFooter.TabIndex = 4;
            // 
            // pnlOperaciones
            // 
            this.pnlOperaciones.Controls.Add(this.grpDetalleCliente);
            this.pnlOperaciones.Controls.Add(this.grpInfoCliente);
            this.pnlOperaciones.Controls.Add(this.panel3);
            this.pnlOperaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOperaciones.Location = new System.Drawing.Point(0, 0);
            this.pnlOperaciones.Name = "pnlOperaciones";
            this.pnlOperaciones.Size = new System.Drawing.Size(521, 265);
            this.ToolTipController.SetSuperTip(this.pnlOperaciones, null);
            this.pnlOperaciones.TabIndex = 2;
            // 
            // grpDetalleCliente
            // 
            this.grpDetalleCliente.Controls.Add(this.btnReenviar);
            this.grpDetalleCliente.Controls.Add(this.labelControl3);
            this.grpDetalleCliente.Controls.Add(this.lblIPs);
            this.grpDetalleCliente.Controls.Add(this.labelControl2);
            this.grpDetalleCliente.Controls.Add(this.lblTipoSesion);
            this.grpDetalleCliente.Controls.Add(this.labelControl1);
            this.grpDetalleCliente.Controls.Add(this.lblFechaConexion);
            this.grpDetalleCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalleCliente.Location = new System.Drawing.Point(0, 58);
            this.grpDetalleCliente.Name = "grpDetalleCliente";
            this.grpDetalleCliente.Size = new System.Drawing.Size(521, 109);
            this.ToolTipController.SetSuperTip(this.grpDetalleCliente, null);
            this.grpDetalleCliente.TabIndex = 1;
            this.grpDetalleCliente.Text = "Detalles";
            // 
            // btnReenviar
            // 
            this.btnReenviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReenviar.Location = new System.Drawing.Point(441, 23);
            this.btnReenviar.Name = "btnReenviar";
            this.btnReenviar.Size = new System.Drawing.Size(75, 23);
            this.ToolTipController.SetSuperTip(this.btnReenviar, null);
            this.btnReenviar.TabIndex = 6;
            this.btnReenviar.Text = "Reenviar";
            this.btnReenviar.UseVisualStyleBackColor = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(6, 83);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(54, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "IP origen:";
            // 
            // lblIPs
            // 
            this.lblIPs.Location = new System.Drawing.Point(66, 83);
            this.lblIPs.Name = "lblIPs";
            this.lblIPs.Size = new System.Drawing.Size(4, 13);
            this.lblIPs.TabIndex = 4;
            this.lblIPs.Text = "-";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(6, 54);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Tipo de Sesión:";
            // 
            // lblTipoSesion
            // 
            this.lblTipoSesion.Location = new System.Drawing.Point(96, 54);
            this.lblTipoSesion.Name = "lblTipoSesion";
            this.lblTipoSesion.Size = new System.Drawing.Size(4, 13);
            this.lblTipoSesion.TabIndex = 2;
            this.lblTipoSesion.Text = "-";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(6, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Sesión desde:";
            // 
            // lblFechaConexion
            // 
            this.lblFechaConexion.Location = new System.Drawing.Point(89, 23);
            this.lblFechaConexion.Name = "lblFechaConexion";
            this.lblFechaConexion.Size = new System.Drawing.Size(103, 13);
            this.lblFechaConexion.TabIndex = 0;
            this.lblFechaConexion.Text = "01/01/0001 00:00:00";
            // 
            // grpInfoCliente
            // 
            this.grpInfoCliente.Controls.Add(this.lblPaginas);
            this.grpInfoCliente.Controls.Add(this.labelControl8);
            this.grpInfoCliente.Controls.Add(this.labelControl6);
            this.grpInfoCliente.Controls.Add(this.lblContacto);
            this.grpInfoCliente.Controls.Add(this.labelControl4);
            this.grpInfoCliente.Controls.Add(this.lblTelefonos);
            this.grpInfoCliente.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpInfoCliente.Location = new System.Drawing.Point(0, 167);
            this.grpInfoCliente.Name = "grpInfoCliente";
            this.grpInfoCliente.Size = new System.Drawing.Size(521, 98);
            this.ToolTipController.SetSuperTip(this.grpInfoCliente, null);
            this.grpInfoCliente.TabIndex = 2;
            this.grpInfoCliente.Text = "Información del cliente";
            // 
            // lblPaginas
            // 
            this.lblPaginas.AutoSize = true;
            this.lblPaginas.Location = new System.Drawing.Point(299, 23);
            this.lblPaginas.Name = "lblPaginas";
            this.lblPaginas.Size = new System.Drawing.Size(10, 13);
            this.ToolTipController.SetSuperTip(this.lblPaginas, null);
            this.lblPaginas.TabIndex = 9;
            this.lblPaginas.TabStop = true;
            this.lblPaginas.Text = "-";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(246, 23);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(47, 13);
            this.labelControl8.TabIndex = 8;
            this.labelControl8.Text = "Páginas:";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(6, 61);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(54, 13);
            this.labelControl6.TabIndex = 7;
            this.labelControl6.Text = "Contacto:";
            // 
            // lblContacto
            // 
            this.lblContacto.Location = new System.Drawing.Point(66, 61);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(4, 13);
            this.lblContacto.TabIndex = 6;
            this.lblContacto.Text = "-";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(6, 23);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(58, 13);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "Teléfonos:";
            // 
            // lblTelefonos
            // 
            this.lblTelefonos.Location = new System.Drawing.Point(70, 23);
            this.lblTelefonos.Name = "lblTelefonos";
            this.lblTelefonos.Size = new System.Drawing.Size(4, 13);
            this.lblTelefonos.TabIndex = 4;
            this.lblTelefonos.Text = "-";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.txtTipoSesion);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.txtRazonSocial);
            this.panel3.Controls.Add(this.picSucursal);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(521, 58);
            this.ToolTipController.SetSuperTip(this.panel3, null);
            this.panel3.TabIndex = 0;
            // 
            // txtTipoSesion
            // 
            this.txtTipoSesion.BackColor = System.Drawing.SystemColors.Control;
            this.txtTipoSesion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTipoSesion.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTipoSesion.ForeColor = System.Drawing.Color.Gray;
            this.txtTipoSesion.Location = new System.Drawing.Point(83, 28);
            this.txtTipoSesion.Name = "txtTipoSesion";
            this.txtTipoSesion.Size = new System.Drawing.Size(423, 13);
            this.ToolTipController.SetSuperTip(this.txtTipoSesion, null);
            this.txtTipoSesion.TabIndex = 6;
            this.txtTipoSesion.Text = "Sesion";
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(60, 28);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(23, 30);
            this.ToolTipController.SetSuperTip(this.panel5, null);
            this.panel5.TabIndex = 8;
            // 
            // txtRazonSocial
            // 
            this.txtRazonSocial.BackColor = System.Drawing.SystemColors.Control;
            this.txtRazonSocial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRazonSocial.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRazonSocial.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRazonSocial.Location = new System.Drawing.Point(60, 0);
            this.txtRazonSocial.Name = "txtRazonSocial";
            this.txtRazonSocial.Size = new System.Drawing.Size(446, 28);
            this.ToolTipController.SetSuperTip(this.txtRazonSocial, null);
            this.txtRazonSocial.TabIndex = 5;
            this.txtRazonSocial.Text = "Empresa";
            // 
            // picSucursal
            // 
            this.picSucursal.Dock = System.Windows.Forms.DockStyle.Left;
            this.picSucursal.EditValue = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.descarga;
            this.picSucursal.Location = new System.Drawing.Point(0, 0);
            this.picSucursal.Name = "picSucursal";
            this.picSucursal.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picSucursal.Properties.ReadOnly = true;
            this.picSucursal.Properties.ShowMenu = false;
            this.picSucursal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picSucursal.Size = new System.Drawing.Size(60, 58);
            this.picSucursal.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnGuardarCliente);
            this.panel4.Controls.Add(this.btnEditarCliente);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(506, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(15, 58);
            this.ToolTipController.SetSuperTip(this.panel4, null);
            this.panel4.TabIndex = 7;
            // 
            // btnGuardarCliente
            // 
            this.btnGuardarCliente.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGuardarCliente.EditValue = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.Disk_floppy_save_512x512;
            this.btnGuardarCliente.Location = new System.Drawing.Point(0, 15);
            this.btnGuardarCliente.Name = "btnGuardarCliente";
            this.btnGuardarCliente.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnGuardarCliente.Size = new System.Drawing.Size(15, 15);
            this.btnGuardarCliente.TabIndex = 5;
            this.btnGuardarCliente.Visible = false;
            // 
            // btnEditarCliente
            // 
            this.btnEditarCliente.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEditarCliente.EditValue = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.pen_paper_2_512;
            this.btnEditarCliente.Location = new System.Drawing.Point(0, 0);
            this.btnEditarCliente.Name = "btnEditarCliente";
            this.btnEditarCliente.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnEditarCliente.Size = new System.Drawing.Size(15, 15);
            this.btnEditarCliente.TabIndex = 4;
            // 
            // defaultToolTipController1
            // 
            // 
            // 
            // 
            this.defaultToolTipController1.DefaultController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // VLMonitoreo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panel1);
            this.Name = "VLMonitoreo";
            this.Size = new System.Drawing.Size(525, 700);
            this.ToolTipController.SetSuperTip(this, null);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstClientes)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlOperaciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpDetalleCliente)).EndInit();
            this.grpDetalleCliente.ResumeLayout(false);
            this.grpDetalleCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpInfoCliente)).EndInit();
            this.grpInfoCliente.ResumeLayout(false);
            this.grpInfoCliente.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSucursal.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnGuardarCliente.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditarCliente.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem verStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reenviarToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl lstClientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Panel pnlOperaciones;
        private System.Windows.Forms.Panel panel3;
        private ControlesEstandar.Titulo ttlTitulo;
        private DevExpress.XtraEditors.GroupControl grpDetalleCliente;
        private DevExpress.XtraEditors.GroupControl grpInfoCliente;
        private DevExpress.XtraEditors.PictureEdit picSucursal;
        private DevExpress.XtraEditors.PictureEdit btnEditarCliente;
        private DevExpress.XtraEditors.LabelControl lblFechaConexion;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblTipoSesion;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblIPs;
        private System.Windows.Forms.TextBox txtTipoSesion;
        private System.Windows.Forms.TextBox txtRazonSocial;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl lblContacto;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lblTelefonos;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.LinkLabel lblPaginas;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.PictureEdit btnGuardarCliente;
        private System.Windows.Forms.Button btnReenviar;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lblOcultarInfo;
        private DevExpress.Utils.ToolTipController ToolTipController;
        private DevExpress.Utils.DefaultToolTipController defaultToolTipController1;
    }
}
