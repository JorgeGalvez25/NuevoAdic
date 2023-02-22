namespace NuevoAdicional
{
    partial class frmPosicionesEstacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPosicionesEstacion));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deshabilitarPosiciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.habilitarPosiciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itCambiarPorcentaje = new System.Windows.Forms.ToolStripButton();
            this.tiPorcentajeGlobal = new System.Windows.Forms.ToolStripButton();
            this.itActualizarPosiciones = new System.Windows.Forms.ToolStripButton();
            this.tiPorcentajeCalibracion = new System.Windows.Forms.ToolStripButton();
            this.tiAplicarCambios = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itCambiarPorcentaje,
            this.tiPorcentajeGlobal,
            this.toolStripSeparator2,
            this.itActualizarPosiciones,
            this.tiPorcentajeCalibracion,
            this.tiAplicarCambios});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(702, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(702, 391);
            this.treeView1.TabIndex = 1;
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "handheld_device.png");
            this.imageList1.Images.SetKeyName(1, "bullet_ball_glass_green.png");
            this.imageList1.Images.SetKeyName(2, "bullet_ball_glass_red.png");
            this.imageList1.Images.SetKeyName(3, "bullet_ball_glass_grey.png");
            this.imageList1.Images.SetKeyName(4, "1396504101_processor.png");
            this.imageList1.Images.SetKeyName(5, "delete2.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deshabilitarPosiciónToolStripMenuItem,
            this.habilitarPosiciónToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deshabilitarPosiciónToolStripMenuItem
            // 
            this.deshabilitarPosiciónToolStripMenuItem.Name = "deshabilitarPosiciónToolStripMenuItem";
            this.deshabilitarPosiciónToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.deshabilitarPosiciónToolStripMenuItem.Text = "Deshabilitar Posición";
            this.deshabilitarPosiciónToolStripMenuItem.Click += new System.EventHandler(this.deshabilitarPosiciónToolStripMenuItem_Click);
            // 
            // habilitarPosiciónToolStripMenuItem
            // 
            this.habilitarPosiciónToolStripMenuItem.Name = "habilitarPosiciónToolStripMenuItem";
            this.habilitarPosiciónToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.habilitarPosiciónToolStripMenuItem.Text = "Habilitar Posición";
            this.habilitarPosiciónToolStripMenuItem.Click += new System.EventHandler(this.habilitarPosiciónToolStripMenuItem_Click);
            // 
            // itCambiarPorcentaje
            // 
            this.itCambiarPorcentaje.Image = global::NuevoAdicional.Properties.Resources.percent;
            this.itCambiarPorcentaje.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itCambiarPorcentaje.Name = "itCambiarPorcentaje";
            this.itCambiarPorcentaje.Size = new System.Drawing.Size(131, 22);
            this.itCambiarPorcentaje.Tag = "5";
            this.itCambiarPorcentaje.Text = "Cambiar porcentaje";
            this.itCambiarPorcentaje.Click += new System.EventHandler(this.itCambiarPorcentaje_Click);
            // 
            // tiPorcentajeGlobal
            // 
            this.tiPorcentajeGlobal.Image = global::NuevoAdicional.Properties.Resources.percent;
            this.tiPorcentajeGlobal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiPorcentajeGlobal.Name = "tiPorcentajeGlobal";
            this.tiPorcentajeGlobal.Size = new System.Drawing.Size(119, 22);
            this.tiPorcentajeGlobal.Tag = "5";
            this.tiPorcentajeGlobal.Text = "Porcentaje global";
            this.tiPorcentajeGlobal.Click += new System.EventHandler(this.tiPorcentajeGlobal_Click);
            // 
            // itActualizarPosiciones
            // 
            this.itActualizarPosiciones.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.itActualizarPosiciones.Image = global::NuevoAdicional.Properties.Resources.refresh;
            this.itActualizarPosiciones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itActualizarPosiciones.Name = "itActualizarPosiciones";
            this.itActualizarPosiciones.Size = new System.Drawing.Size(138, 22);
            this.itActualizarPosiciones.Text = "Actualizar Posiciones";
            this.itActualizarPosiciones.ToolTipText = "Actualizar Posiciones";
            this.itActualizarPosiciones.Click += new System.EventHandler(this.itActualizarPosiciones_Click);
            // 
            // tiPorcentajeCalibracion
            // 
            this.tiPorcentajeCalibracion.Image = global::NuevoAdicional.Properties.Resources.percent;
            this.tiPorcentajeCalibracion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiPorcentajeCalibracion.Name = "tiPorcentajeCalibracion";
            this.tiPorcentajeCalibracion.Size = new System.Drawing.Size(144, 22);
            this.tiPorcentajeCalibracion.Tag = "5";
            this.tiPorcentajeCalibracion.Text = "Porcentaje calibración";
            this.tiPorcentajeCalibracion.Click += new System.EventHandler(this.tiPorcentajeCalibracion_Click);
            // 
            // tiAplicarCambios
            // 
            this.tiAplicarCambios.Image = global::NuevoAdicional.Properties.Resources.check;
            this.tiAplicarCambios.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiAplicarCambios.Name = "tiAplicarCambios";
            this.tiAplicarCambios.Size = new System.Drawing.Size(112, 22);
            this.tiAplicarCambios.Tag = "5";
            this.tiAplicarCambios.Text = "Aplicar cambios";
            this.tiAplicarCambios.Click += new System.EventHandler(this.tiAplicarCambios_Click);
            // 
            // frmPosicionesEstacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 416);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPosicionesEstacion";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPosicionesEstacion";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton itCambiarPorcentaje;
        private System.Windows.Forms.ToolStripButton tiPorcentajeGlobal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tiAplicarCambios;
        private System.Windows.Forms.ToolStripButton itActualizarPosiciones;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deshabilitarPosiciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem habilitarPosiciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tiPorcentajeCalibracion;
    }
}