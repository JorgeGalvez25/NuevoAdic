namespace NuevoAdicional
{
    partial class frmMoviles
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
            this.tiNuevoUsuario = new System.Windows.Forms.ToolStripButton();
            this.itModificarUsuario = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itDesactivarUsuario = new System.Windows.Forms.ToolStripButton();
            this.itActivarUsuario = new System.Windows.Forms.ToolStripButton();
            this.tiEliminarUsuario = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colTelefono = new System.Windows.Forms.ColumnHeader();
            this.colNombre = new System.Windows.Forms.ColumnHeader();
            this.colActivo = new System.Windows.Forms.ColumnHeader();
            this.colSubirBajar = new System.Windows.Forms.ColumnHeader();
            this.itPermisoSubirBajar = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiNuevoUsuario,
            this.itModificarUsuario,
            this.toolStripSeparator1,
            this.itDesactivarUsuario,
            this.itActivarUsuario,
            this.tiEliminarUsuario,
            this.itPermisoSubirBajar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(712, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tiNuevoUsuario
            // 
            this.tiNuevoUsuario.Image = global::NuevoAdicional.Properties.Resources.document_new;
            this.tiNuevoUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiNuevoUsuario.Name = "tiNuevoUsuario";
            this.tiNuevoUsuario.Size = new System.Drawing.Size(104, 22);
            this.tiNuevoUsuario.Tag = "11";
            this.tiNuevoUsuario.Text = "Nuevo usuario";
            this.tiNuevoUsuario.Click += new System.EventHandler(this.tiNuevoUsuario_Click);
            // 
            // itModificarUsuario
            // 
            this.itModificarUsuario.Image = global::NuevoAdicional.Properties.Resources.document_edit;
            this.itModificarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itModificarUsuario.Name = "itModificarUsuario";
            this.itModificarUsuario.Size = new System.Drawing.Size(139, 22);
            this.itModificarUsuario.Tag = "12";
            this.itModificarUsuario.Text = "Modificar contraseña";
            this.itModificarUsuario.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // itDesactivarUsuario
            // 
            this.itDesactivarUsuario.Enabled = false;
            this.itDesactivarUsuario.Image = global::NuevoAdicional.Properties.Resources.document_lock;
            this.itDesactivarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itDesactivarUsuario.Name = "itDesactivarUsuario";
            this.itDesactivarUsuario.Size = new System.Drawing.Size(123, 22);
            this.itDesactivarUsuario.Tag = "13";
            this.itDesactivarUsuario.Text = "Desactivar usuario";
            this.itDesactivarUsuario.Click += new System.EventHandler(this.itDesactivarUsuario_Click);
            // 
            // itActivarUsuario
            // 
            this.itActivarUsuario.Enabled = false;
            this.itActivarUsuario.Image = global::NuevoAdicional.Properties.Resources.document_refresh;
            this.itActivarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itActivarUsuario.Name = "itActivarUsuario";
            this.itActivarUsuario.Size = new System.Drawing.Size(106, 22);
            this.itActivarUsuario.Tag = "14";
            this.itActivarUsuario.Text = "Activar usuario";
            this.itActivarUsuario.Click += new System.EventHandler(this.itActivarUsuario_Click);
            // 
            // tiEliminarUsuario
            // 
            this.tiEliminarUsuario.Enabled = false;
            this.tiEliminarUsuario.Image = global::NuevoAdicional.Properties.Resources.document_delete;
            this.tiEliminarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiEliminarUsuario.Name = "tiEliminarUsuario";
            this.tiEliminarUsuario.Size = new System.Drawing.Size(112, 22);
            this.tiEliminarUsuario.Tag = "17";
            this.tiEliminarUsuario.Text = "Eliminar usuario";
            this.tiEliminarUsuario.Click += new System.EventHandler(this.tiEliminarUsuario_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTelefono,
            this.colNombre,
            this.colActivo,
            this.colSubirBajar});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(712, 236);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // colTelefono
            // 
            this.colTelefono.Text = "Telefono";
            this.colTelefono.Width = 150;
            // 
            // colNombre
            // 
            this.colNombre.Text = "Nombre";
            this.colNombre.Width = 300;
            // 
            // colActivo
            // 
            this.colActivo.Text = "Activo";
            this.colActivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colActivo.Width = 100;
            // 
            // colSubirBajar
            // 
            this.colSubirBajar.Text = "Subir/Bajar";
            this.colSubirBajar.Width = 66;
            // 
            // itPermisoSubirBajar
            // 
            this.itPermisoSubirBajar.Enabled = false;
            this.itPermisoSubirBajar.Image = global::NuevoAdicional.Properties.Resources.up_down;
            this.itPermisoSubirBajar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itPermisoSubirBajar.Name = "itPermisoSubirBajar";
            this.itPermisoSubirBajar.Size = new System.Drawing.Size(85, 22);
            this.itPermisoSubirBajar.Text = "Subir/Bajar";
            this.itPermisoSubirBajar.Click += new System.EventHandler(this.itPermisoSubirBajar_Click);
            // 
            // frmMoviles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 261);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmMoviles";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Lista de Móviles";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tiNuevoUsuario;
        private System.Windows.Forms.ToolStripButton itModificarUsuario;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton itDesactivarUsuario;
        private System.Windows.Forms.ToolStripButton itActivarUsuario;
        private System.Windows.Forms.ToolStripButton tiEliminarUsuario;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colTelefono;
        private System.Windows.Forms.ColumnHeader colNombre;
        private System.Windows.Forms.ColumnHeader colActivo;
        private System.Windows.Forms.ColumnHeader colSubirBajar;
        private System.Windows.Forms.ToolStripButton itPermisoSubirBajar;

    }
}