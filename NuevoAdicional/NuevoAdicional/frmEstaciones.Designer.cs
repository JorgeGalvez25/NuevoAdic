namespace NuevoAdicional
{
    partial class frmEstaciones
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tiNuevaEstacion = new System.Windows.Forms.ToolStripButton();
            this.tiModificarEstacion = new System.Windows.Forms.ToolStripButton();
            this.tiEliminarEstacion = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colNombre = new System.Windows.Forms.ColumnHeader();
            this.colIpServicios = new System.Windows.Forms.ColumnHeader();
            this.colDispensario = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiNuevaEstacion,
            this.tiModificarEstacion,
            this.tiEliminarEstacion});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tiNuevaEstacion
            // 
            this.tiNuevaEstacion.Image = global::NuevoAdicional.Properties.Resources.document_new;
            this.tiNuevaEstacion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiNuevaEstacion.Name = "tiNuevaEstacion";
            this.tiNuevaEstacion.Size = new System.Drawing.Size(108, 22);
            this.tiNuevaEstacion.Tag = "1";
            this.tiNuevaEstacion.Text = "Nueva estación";
            this.tiNuevaEstacion.Click += new System.EventHandler(this.tiNuevaEstacion_Click);
            // 
            // tiModificarEstacion
            // 
            this.tiModificarEstacion.Image = global::NuevoAdicional.Properties.Resources.document_edit;
            this.tiModificarEstacion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiModificarEstacion.Name = "tiModificarEstacion";
            this.tiModificarEstacion.Size = new System.Drawing.Size(125, 22);
            this.tiModificarEstacion.Tag = "2";
            this.tiModificarEstacion.Text = "Modificar estación";
            this.tiModificarEstacion.Click += new System.EventHandler(this.tiModificarEstacion_Click);
            // 
            // tiEliminarEstacion
            // 
            this.tiEliminarEstacion.Image = global::NuevoAdicional.Properties.Resources.document_delete;
            this.tiEliminarEstacion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiEliminarEstacion.Name = "tiEliminarEstacion";
            this.tiEliminarEstacion.Size = new System.Drawing.Size(117, 22);
            this.tiEliminarEstacion.Tag = "3";
            this.tiEliminarEstacion.Text = "Eliminar Estación";
            this.tiEliminarEstacion.Click += new System.EventHandler(this.tiEliminarEstacion_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colNombre,
            this.colIpServicios,
            this.colDispensario});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(784, 359);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // colId
            // 
            this.colId.Text = "Id";
            this.colId.Width = 0;
            // 
            // colNombre
            // 
            this.colNombre.Text = "Nombre";
            this.colNombre.Width = 300;
            // 
            // colIpServicios
            // 
            this.colIpServicios.Text = "Ip Servicios";
            this.colIpServicios.Width = 300;
            // 
            // colDispensario
            // 
            this.colDispensario.Text = "Dispensario";
            this.colDispensario.Width = 150;
            // 
            // frmEstaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 384);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEstaciones";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estaciones";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colNombre;
        private System.Windows.Forms.ColumnHeader colIpServicios;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ToolStripButton tiNuevaEstacion;
        private System.Windows.Forms.ToolStripButton tiModificarEstacion;
        private System.Windows.Forms.ToolStripButton tiEliminarEstacion;
        private System.Windows.Forms.ColumnHeader colDispensario;
    }
}