namespace Configurador
{
    partial class Form1
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
            this.pnlCliente = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlHost = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtAjustador = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numSincroniz = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBuscarAdicional = new System.Windows.Forms.Button();
            this.txtAdicional = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBuscarConsola = new System.Windows.Forms.Button();
            this.txtBDConsola = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dlgOpenBaseDatos = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.pnlCliente.SuspendLayout();
            this.pnlHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSincroniz)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 162);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 46);
            this.panel1.TabIndex = 2;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(627, 11);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "&Guardar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // pnlCliente
            // 
            this.pnlCliente.Controls.Add(this.label6);
            this.pnlCliente.Controls.Add(this.button1);
            this.pnlCliente.Controls.Add(this.textBox1);
            this.pnlCliente.Controls.Add(this.label7);
            this.pnlCliente.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCliente.Location = new System.Drawing.Point(0, 0);
            this.pnlCliente.Name = "pnlCliente";
            this.pnlCliente.Size = new System.Drawing.Size(353, 162);
            this.pnlCliente.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Adicional";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = global::Configurador.Properties.Resources.data_find;
            this.button1.Location = new System.Drawing.Point(311, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(80, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(225, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Bases de Datos";
            // 
            // pnlHost
            // 
            this.pnlHost.Controls.Add(this.label8);
            this.pnlHost.Controls.Add(this.button2);
            this.pnlHost.Controls.Add(this.txtAjustador);
            this.pnlHost.Controls.Add(this.label5);
            this.pnlHost.Controls.Add(this.numSincroniz);
            this.pnlHost.Controls.Add(this.label4);
            this.pnlHost.Controls.Add(this.label3);
            this.pnlHost.Controls.Add(this.btnBuscarAdicional);
            this.pnlHost.Controls.Add(this.txtAdicional);
            this.pnlHost.Controls.Add(this.label2);
            this.pnlHost.Controls.Add(this.btnBuscarConsola);
            this.pnlHost.Controls.Add(this.txtBDConsola);
            this.pnlHost.Controls.Add(this.label1);
            this.pnlHost.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlHost.Location = new System.Drawing.Point(355, 0);
            this.pnlHost.Name = "pnlHost";
            this.pnlHost.Size = new System.Drawing.Size(359, 162);
            this.pnlHost.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Ajustador";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Image = global::Configurador.Properties.Resources.data_find;
            this.button2.Location = new System.Drawing.Point(311, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtAjustador
            // 
            this.txtAjustador.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAjustador.Location = new System.Drawing.Point(80, 88);
            this.txtAjustador.Name = "txtAjustador";
            this.txtAjustador.Size = new System.Drawing.Size(225, 20);
            this.txtAjustador.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(312, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Horas";
            // 
            // numSincroniz
            // 
            this.numSincroniz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numSincroniz.Location = new System.Drawing.Point(80, 123);
            this.numSincroniz.Name = "numSincroniz";
            this.numSincroniz.Size = new System.Drawing.Size(225, 20);
            this.numSincroniz.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Sincronización";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Adicional";
            // 
            // btnBuscarAdicional
            // 
            this.btnBuscarAdicional.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarAdicional.Image = global::Configurador.Properties.Resources.data_find;
            this.btnBuscarAdicional.Location = new System.Drawing.Point(311, 56);
            this.btnBuscarAdicional.Name = "btnBuscarAdicional";
            this.btnBuscarAdicional.Size = new System.Drawing.Size(36, 23);
            this.btnBuscarAdicional.TabIndex = 6;
            this.btnBuscarAdicional.Text = "...";
            this.btnBuscarAdicional.UseVisualStyleBackColor = true;
            // 
            // txtAdicional
            // 
            this.txtAdicional.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdicional.Location = new System.Drawing.Point(80, 57);
            this.txtAdicional.Name = "txtAdicional";
            this.txtAdicional.Size = new System.Drawing.Size(225, 20);
            this.txtAdicional.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Consola";
            // 
            // btnBuscarConsola
            // 
            this.btnBuscarConsola.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarConsola.Image = global::Configurador.Properties.Resources.data_find;
            this.btnBuscarConsola.Location = new System.Drawing.Point(311, 25);
            this.btnBuscarConsola.Name = "btnBuscarConsola";
            this.btnBuscarConsola.Size = new System.Drawing.Size(36, 23);
            this.btnBuscarConsola.TabIndex = 3;
            this.btnBuscarConsola.Text = "...";
            this.btnBuscarConsola.UseVisualStyleBackColor = true;
            // 
            // txtBDConsola
            // 
            this.txtBDConsola.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBDConsola.Location = new System.Drawing.Point(80, 26);
            this.txtBDConsola.Name = "txtBDConsola";
            this.txtBDConsola.Size = new System.Drawing.Size(225, 20);
            this.txtBDConsola.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bases de Datos";
            // 
            // dlgOpenBaseDatos
            // 
            this.dlgOpenBaseDatos.FileName = "*.fdb";
            this.dlgOpenBaseDatos.Filter = "Base Datos|*.fdb";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 208);
            this.Controls.Add(this.pnlHost);
            this.Controls.Add(this.pnlCliente);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(365, 167);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.pnlCliente.ResumeLayout(false);
            this.pnlCliente.PerformLayout();
            this.pnlHost.ResumeLayout(false);
            this.pnlHost.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSincroniz)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlCliente;
        private System.Windows.Forms.Panel pnlHost;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnBuscarConsola;
        private System.Windows.Forms.TextBox txtBDConsola;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBuscarAdicional;
        private System.Windows.Forms.TextBox txtAdicional;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog dlgOpenBaseDatos;
        private System.Windows.Forms.NumericUpDown numSincroniz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtAjustador;
    }
}

