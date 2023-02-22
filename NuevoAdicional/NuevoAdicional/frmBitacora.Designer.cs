namespace NuevoAdicional
{
    partial class frmBitacora
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
            this.label3 = new System.Windows.Forms.Label();
            this.cmbFiltro = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.deFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.deFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colVacia = new System.Windows.Forms.ColumnHeader();
            this.colFecha = new System.Windows.Forms.ColumnHeader();
            this.colHora = new System.Windows.Forms.ColumnHeader();
            this.colUsuario = new System.Windows.Forms.ColumnHeader();
            this.colSuceso = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbFiltro);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.deFechaFinal);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.deFechaInicial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 52);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Filtrar Por:";
            // 
            // cmbFiltro
            // 
            this.cmbFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltro.Items.AddRange(new object[] {
            "Todos",
            "Moviles",
            "Estandares"});
            this.cmbFiltro.Location = new System.Drawing.Point(304, 24);
            this.cmbFiltro.Name = "cmbFiltro";
            this.cmbFiltro.Size = new System.Drawing.Size(121, 21);
            this.cmbFiltro.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fecha Final:";
            // 
            // deFechaFinal
            // 
            this.deFechaFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deFechaFinal.Location = new System.Drawing.Point(158, 25);
            this.deFechaFinal.Name = "deFechaFinal";
            this.deFechaFinal.Size = new System.Drawing.Size(140, 20);
            this.deFechaFinal.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha Inicial:";
            // 
            // deFechaInicial
            // 
            this.deFechaInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deFechaInicial.Location = new System.Drawing.Point(12, 25);
            this.deFechaInicial.Name = "deFechaInicial";
            this.deFechaInicial.Size = new System.Drawing.Size(140, 20);
            this.deFechaInicial.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colVacia,
            this.colFecha,
            this.colHora,
            this.colUsuario,
            this.colSuceso});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 52);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(611, 392);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // colVacia
            // 
            this.colVacia.Text = "";
            this.colVacia.Width = 0;
            // 
            // colFecha
            // 
            this.colFecha.Text = "Fecha";
            this.colFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colFecha.Width = 80;
            // 
            // colHora
            // 
            this.colHora.Text = "Hora";
            this.colHora.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colUsuario
            // 
            this.colUsuario.Text = "Usuario";
            this.colUsuario.Width = 100;
            // 
            // colSuceso
            // 
            this.colSuceso.Text = "Suceso";
            this.colSuceso.Width = 350;
            // 
            // frmBitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 444);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBitacora";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitacora";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker deFechaInicial;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colUsuario;
        private System.Windows.Forms.ColumnHeader colSuceso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker deFechaFinal;
        private System.Windows.Forms.ColumnHeader colFecha;
        private System.Windows.Forms.ColumnHeader colHora;
        private System.Windows.Forms.ColumnHeader colVacia;
        private System.Windows.Forms.ComboBox cmbFiltro;
        private System.Windows.Forms.Label label3;

    }
}