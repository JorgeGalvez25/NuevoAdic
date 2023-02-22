namespace NuevoAdicional
{
    partial class frmProtecciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProtecciones));
            this.ilProtecciones = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.itGuardar = new System.Windows.Forms.ToolStripButton();
            this.pnlProtecciones = new System.Windows.Forms.Panel();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.gcProtecciones = new DevExpress.XtraGrid.GridControl();
            this.gvProtecciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.toolStrip1.SuspendLayout();
            this.pnlProtecciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProtecciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProtecciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ilProtecciones
            // 
            this.ilProtecciones.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilProtecciones.ImageStream")));
            this.ilProtecciones.TransparentColor = System.Drawing.Color.Transparent;
            this.ilProtecciones.Images.SetKeyName(0, "lock.png");
            this.ilProtecciones.Images.SetKeyName(1, "lock_open.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itGuardar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(212, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // itGuardar
            // 
            this.itGuardar.Image = global::NuevoAdicional.Properties.Resources.disk_green;
            this.itGuardar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itGuardar.Name = "itGuardar";
            this.itGuardar.Size = new System.Drawing.Size(69, 22);
            this.itGuardar.Text = "Guardar";
            this.itGuardar.Click += new System.EventHandler(this.itGuardar_Click);
            // 
            // pnlProtecciones
            // 
            this.pnlProtecciones.Controls.Add(this.btnEliminar);
            this.pnlProtecciones.Controls.Add(this.btnAgregar);
            this.pnlProtecciones.Location = new System.Drawing.Point(26, 51);
            this.pnlProtecciones.Name = "pnlProtecciones";
            this.pnlProtecciones.Size = new System.Drawing.Size(162, 196);
            this.pnlProtecciones.TabIndex = 2;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.Image = global::NuevoAdicional.Properties.Resources.delete2;
            this.btnEliminar.Location = new System.Drawing.Point(88, 170);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "&Eliminar";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgregar.Image = global::NuevoAdicional.Properties.Resources.add2;
            this.btnAgregar.Location = new System.Drawing.Point(7, 170);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 1;
            this.btnAgregar.Text = "&Agregar";
            this.btnAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // gcProtecciones
            // 
            this.gcProtecciones.EmbeddedNavigator.Name = "";
            this.gcProtecciones.Location = new System.Drawing.Point(26, 51);
            this.gcProtecciones.MainView = this.gvProtecciones;
            this.gcProtecciones.Name = "gcProtecciones";
            this.gcProtecciones.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcProtecciones.Size = new System.Drawing.Size(159, 164);
            this.gcProtecciones.TabIndex = 6;
            this.gcProtecciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvProtecciones});
            // 
            // gvProtecciones
            // 
            this.gvProtecciones.GridControl = this.gcProtecciones;
            this.gvProtecciones.Name = "gvProtecciones";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueChecked = "Si";
            this.repositoryItemCheckEdit1.ValueUnchecked = "No";
            // 
            // frmProtecciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 274);
            this.Controls.Add(this.gcProtecciones);
            this.Controls.Add(this.pnlProtecciones);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProtecciones";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Protecciones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProtecciones_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlProtecciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProtecciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProtecciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ImageList ilProtecciones;
        private System.Windows.Forms.Panel pnlProtecciones;
        private System.Windows.Forms.ToolStripButton itGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnAgregar;
        private DevExpress.XtraGrid.GridControl gcProtecciones;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProtecciones;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}