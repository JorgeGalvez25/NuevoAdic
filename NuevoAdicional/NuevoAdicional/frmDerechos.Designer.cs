namespace NuevoAdicional
{
    partial class frmDerechos
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
            this.tiAplicarDerechos = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tiMarcarTodos = new System.Windows.Forms.ToolStripButton();
            this.tiDesmarcarTodos = new System.Windows.Forms.ToolStripButton();
            this.chklbDerechos = new System.Windows.Forms.CheckedListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiAplicarDerechos,
            this.toolStripSeparator1,
            this.tiMarcarTodos,
            this.tiDesmarcarTodos});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(372, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tiAplicarDerechos
            // 
            this.tiAplicarDerechos.Image = global::NuevoAdicional.Properties.Resources.check;
            this.tiAplicarDerechos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiAplicarDerechos.Name = "tiAplicarDerechos";
            this.tiAplicarDerechos.Size = new System.Drawing.Size(115, 22);
            this.tiAplicarDerechos.Text = "Aplicar derechos";
            this.tiAplicarDerechos.Click += new System.EventHandler(this.tiAplicarDerechos_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tiMarcarTodos
            // 
            this.tiMarcarTodos.Image = global::NuevoAdicional.Properties.Resources.checks;
            this.tiMarcarTodos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiMarcarTodos.Name = "tiMarcarTodos";
            this.tiMarcarTodos.Size = new System.Drawing.Size(97, 22);
            this.tiMarcarTodos.Text = "Marcar todos";
            this.tiMarcarTodos.Click += new System.EventHandler(this.tiMarcarTodos_Click);
            // 
            // tiDesmarcarTodos
            // 
            this.tiDesmarcarTodos.Image = global::NuevoAdicional.Properties.Resources.undo;
            this.tiDesmarcarTodos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiDesmarcarTodos.Name = "tiDesmarcarTodos";
            this.tiDesmarcarTodos.Size = new System.Drawing.Size(116, 22);
            this.tiDesmarcarTodos.Text = "Desmarcar todos";
            this.tiDesmarcarTodos.Click += new System.EventHandler(this.tiDesmarcarTodos_Click);
            // 
            // chklbDerechos
            // 
            this.chklbDerechos.CheckOnClick = true;
            this.chklbDerechos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklbDerechos.FormattingEnabled = true;
            this.chklbDerechos.Items.AddRange(new object[] {
            "Ver estaciones",
            "Nueva estacion",
            "Modificar estación",
            "Eliminar estación",
            "Ver flujos",
            "Cambiar porcentaje",
            "Subir",
            "Bajar",
            "Ver configuraciones",
            "Modificar configuraciones",
            "Ver usuarios",
            "Nuevo usuario",
            "Modificar usuario",
            "Desactivar usuario",
            "Activar usuario",
            "Ver derechos de usuario",
            "Bitácora",
            "Eliminar usuario",
            "Reporte",
            "Regenerar archivos volumétricos",
            "Protecciones",
            "Sincronizar",
            "Móviles",
            "Cerrar Aplicación",
            "Registrar Ticket",
            "Modificar Ticket",
            "Tanques",
            "Salir",
            "Protecciones",
            "Lecturas de tanques"});
            this.chklbDerechos.Location = new System.Drawing.Point(0, 25);
            this.chklbDerechos.Name = "chklbDerechos";
            this.chklbDerechos.Size = new System.Drawing.Size(372, 409);
            this.chklbDerechos.TabIndex = 2;
            // 
            // frmDerechos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 442);
            this.Controls.Add(this.chklbDerechos);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDerechos";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDerechos";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.CheckedListBox chklbDerechos;
        private System.Windows.Forms.ToolStripButton tiAplicarDerechos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tiMarcarTodos;
        private System.Windows.Forms.ToolStripButton tiDesmarcarTodos;
    }
}