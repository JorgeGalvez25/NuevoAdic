namespace NuevoAdicional
{
    partial class frmReporteAjusteParametros
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.pnlReporte01 = new System.Windows.Forms.Panel();
            this.rad6a6 = new System.Windows.Forms.RadioButton();
            this.rad12a12 = new System.Windows.Forms.RadioButton();
            this.deFechaFin = new System.Windows.Forms.DateTimePicker();
            this.deFechaIni = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlReporte02 = new System.Windows.Forms.Panel();
            this.deFechaIni02 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.rad6a62 = new System.Windows.Forms.RadioButton();
            this.rad12a122 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.pnlReporte01.SuspendLayout();
            this.pnlReporte02.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 33);
            this.panel1.TabIndex = 2;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(187, 6);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(95, 6);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // pnlReporte01
            // 
            this.pnlReporte01.Controls.Add(this.rad6a6);
            this.pnlReporte01.Controls.Add(this.rad12a12);
            this.pnlReporte01.Controls.Add(this.deFechaFin);
            this.pnlReporte01.Controls.Add(this.deFechaIni);
            this.pnlReporte01.Controls.Add(this.label2);
            this.pnlReporte01.Controls.Add(this.label1);
            this.pnlReporte01.Location = new System.Drawing.Point(62, 25);
            this.pnlReporte01.Name = "pnlReporte01";
            this.pnlReporte01.Size = new System.Drawing.Size(286, 118);
            this.pnlReporte01.TabIndex = 3;
            this.pnlReporte01.Tag = "1";
            this.pnlReporte01.Visible = false;
            // 
            // rad6a6
            // 
            this.rad6a6.AutoSize = true;
            this.rad6a6.Location = new System.Drawing.Point(184, 73);
            this.rad6a6.Name = "rad6a6";
            this.rad6a6.Size = new System.Drawing.Size(64, 17);
            this.rad6a6.TabIndex = 8;
            this.rad6a6.Text = "de 6 a 6";
            this.rad6a6.UseVisualStyleBackColor = true;
            this.rad6a6.CheckedChanged += new System.EventHandler(this.rad6a6_CheckedChanged);
            // 
            // rad12a12
            // 
            this.rad12a12.AutoSize = true;
            this.rad12a12.Checked = true;
            this.rad12a12.Location = new System.Drawing.Point(184, 50);
            this.rad12a12.Name = "rad12a12";
            this.rad12a12.Size = new System.Drawing.Size(76, 17);
            this.rad12a12.TabIndex = 7;
            this.rad12a12.TabStop = true;
            this.rad12a12.Text = "de 12 a 12";
            this.rad12a12.UseVisualStyleBackColor = true;
            // 
            // deFechaFin
            // 
            this.deFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deFechaFin.Location = new System.Drawing.Point(184, 24);
            this.deFechaFin.Name = "deFechaFin";
            this.deFechaFin.Size = new System.Drawing.Size(91, 20);
            this.deFechaFin.TabIndex = 3;
            // 
            // deFechaIni
            // 
            this.deFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deFechaIni.Location = new System.Drawing.Point(17, 24);
            this.deFechaIni.Name = "deFechaIni";
            this.deFechaIni.Size = new System.Drawing.Size(91, 20);
            this.deFechaIni.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha Final";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha Inicial";
            // 
            // pnlReporte02
            // 
            this.pnlReporte02.Controls.Add(this.rad6a62);
            this.pnlReporte02.Controls.Add(this.rad12a122);
            this.pnlReporte02.Controls.Add(this.deFechaIni02);
            this.pnlReporte02.Controls.Add(this.label3);
            this.pnlReporte02.Location = new System.Drawing.Point(0, -1);
            this.pnlReporte02.Name = "pnlReporte02";
            this.pnlReporte02.Size = new System.Drawing.Size(244, 104);
            this.pnlReporte02.TabIndex = 4;
            this.pnlReporte02.Tag = "2";
            this.pnlReporte02.Visible = false;
            // 
            // deFechaIni02
            // 
            this.deFechaIni02.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deFechaIni02.Location = new System.Drawing.Point(15, 26);
            this.deFechaIni02.Name = "deFechaIni02";
            this.deFechaIni02.Size = new System.Drawing.Size(91, 20);
            this.deFechaIni02.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Fecha";
            // 
            // rad6a62
            // 
            this.rad6a62.AutoSize = true;
            this.rad6a62.Location = new System.Drawing.Point(155, 74);
            this.rad6a62.Name = "rad6a62";
            this.rad6a62.Size = new System.Drawing.Size(64, 17);
            this.rad6a62.TabIndex = 10;
            this.rad6a62.Text = "de 6 a 6";
            this.rad6a62.UseVisualStyleBackColor = true;
            // 
            // rad12a122
            // 
            this.rad12a122.AutoSize = true;
            this.rad12a122.Checked = true;
            this.rad12a122.Location = new System.Drawing.Point(155, 51);
            this.rad12a122.Name = "rad12a122";
            this.rad12a122.Size = new System.Drawing.Size(76, 17);
            this.rad12a122.TabIndex = 9;
            this.rad12a122.TabStop = true;
            this.rad12a122.Text = "de 12 a 12";
            this.rad12a122.UseVisualStyleBackColor = true;
            // 
            // frmReporteAjusteParametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 132);
            this.Controls.Add(this.pnlReporte02);
            this.Controls.Add(this.pnlReporte01);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReporteAjusteParametros";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmReporteAjuste";
            this.panel1.ResumeLayout(false);
            this.pnlReporte01.ResumeLayout(false);
            this.pnlReporte01.PerformLayout();
            this.pnlReporte02.ResumeLayout(false);
            this.pnlReporte02.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Panel pnlReporte01;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlReporte02;
        private System.Windows.Forms.DateTimePicker deFechaFin;
        private System.Windows.Forms.DateTimePicker deFechaIni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rad6a6;
        private System.Windows.Forms.RadioButton rad12a12;
        private System.Windows.Forms.DateTimePicker deFechaIni02;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rad6a62;
        private System.Windows.Forms.RadioButton rad12a122;
    }
}