namespace NuevoAdicional
{
    partial class frmEstacionesPorUsuario
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lvEstaciones = new System.Windows.Forms.ListView();
            this.colEstacion = new System.Windows.Forms.ColumnHeader();
            this.colIndice = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 33);
            this.panel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Image = global::NuevoAdicional.Properties.Resources.check;
            this.btnAceptar.Location = new System.Drawing.Point(126, 6);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Image = global::NuevoAdicional.Properties.Resources.delete2;
            this.btnCancelar.Location = new System.Drawing.Point(207, 6);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lvEstaciones
            // 
            this.lvEstaciones.CheckBoxes = true;
            this.lvEstaciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEstacion,
            this.colIndice});
            this.lvEstaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEstaciones.Location = new System.Drawing.Point(0, 0);
            this.lvEstaciones.MultiSelect = false;
            this.lvEstaciones.Name = "lvEstaciones";
            this.lvEstaciones.Size = new System.Drawing.Size(285, 229);
            this.lvEstaciones.TabIndex = 0;
            this.lvEstaciones.UseCompatibleStateImageBehavior = false;
            this.lvEstaciones.View = System.Windows.Forms.View.Details;
            // 
            // colEstacion
            // 
            this.colEstacion.Text = "Estación";
            this.colEstacion.Width = 250;
            // 
            // colIndice
            // 
            this.colIndice.Text = "Indice";
            this.colIndice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colIndice.Width = 0;
            // 
            // frmEstacionesPorUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 262);
            this.Controls.Add(this.lvEstaciones);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEstacionesPorUsuario";
            this.Text = "Estaciones Por Usuario:";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvEstaciones;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.ColumnHeader colEstacion;
        private System.Windows.Forms.ColumnHeader colIndice;
    }
}