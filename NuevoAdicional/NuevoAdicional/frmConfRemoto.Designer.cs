namespace NuevoAdicional
{
    partial class frmConfRemoto
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
            this.btnAceptar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nuMinutos = new System.Windows.Forms.NumericUpDown();
            this.txtSeg4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeg3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSeg2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSeg1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorSegmentos = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nuMinutos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorSegmentos)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Image = global::NuevoAdicional.Properties.Resources.check;
            this.btnAceptar.Location = new System.Drawing.Point(100, 75);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAceptar.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dirección IP";
            // 
            // nuMinutos
            // 
            this.nuMinutos.Location = new System.Drawing.Point(173, 41);
            this.nuMinutos.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nuMinutos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuMinutos.Name = "nuMinutos";
            this.nuMinutos.Size = new System.Drawing.Size(47, 20);
            this.nuMinutos.TabIndex = 3;
            this.nuMinutos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nuMinutos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtSeg4
            // 
            this.txtSeg4.Location = new System.Drawing.Point(83, 5);
            this.txtSeg4.MaxLength = 3;
            this.txtSeg4.Name = "txtSeg4";
            this.txtSeg4.Size = new System.Drawing.Size(31, 20);
            this.txtSeg4.TabIndex = 4;
            this.txtSeg4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeg4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSegmentos_KeyPress);
            this.txtSeg4.Validating += new System.ComponentModel.CancelEventHandler(this.txtSegmentos_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = ".";
            // 
            // txtSeg3
            // 
            this.txtSeg3.Location = new System.Drawing.Point(136, 5);
            this.txtSeg3.MaxLength = 3;
            this.txtSeg3.Name = "txtSeg3";
            this.txtSeg3.Size = new System.Drawing.Size(31, 20);
            this.txtSeg3.TabIndex = 6;
            this.txtSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeg3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSegmentos_KeyPress);
            this.txtSeg3.Validating += new System.ComponentModel.CancelEventHandler(this.txtSegmentos_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = ".";
            // 
            // txtSeg2
            // 
            this.txtSeg2.Location = new System.Drawing.Point(189, 5);
            this.txtSeg2.MaxLength = 3;
            this.txtSeg2.Name = "txtSeg2";
            this.txtSeg2.Size = new System.Drawing.Size(31, 20);
            this.txtSeg2.TabIndex = 8;
            this.txtSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeg2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSegmentos_KeyPress);
            this.txtSeg2.Validating += new System.ComponentModel.CancelEventHandler(this.txtSegmentos_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = ".";
            // 
            // txtSeg1
            // 
            this.txtSeg1.Location = new System.Drawing.Point(242, 5);
            this.txtSeg1.MaxLength = 3;
            this.txtSeg1.Name = "txtSeg1";
            this.txtSeg1.Size = new System.Drawing.Size(31, 20);
            this.txtSeg1.TabIndex = 10;
            this.txtSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeg1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSegmentos_KeyPress);
            this.txtSeg1.Validating += new System.ComponentModel.CancelEventHandler(this.txtSegmentos_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(279, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = ":";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(295, 6);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(42, 20);
            this.txtPort.TabIndex = 12;
            this.txtPort.Text = "0";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Revsar estado del remoto cada";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(226, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Minutos";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::NuevoAdicional.Properties.Resources.delete;
            this.btnCancel.Location = new System.Drawing.Point(181, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::NuevoAdicional.Properties.Resources.Salir;
            this.btnClose.Location = new System.Drawing.Point(262, 75);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "C&errar";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // errorSegmentos
            // 
            this.errorSegmentos.ContainerControl = this;
            // 
            // frmConfRemoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 105);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSeg1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSeg2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeg3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSeg4);
            this.Controls.Add(this.nuMinutos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfRemoto";
            this.Text = "Configuraciones del Remoto";
            ((System.ComponentModel.ISupportInitialize)(this.nuMinutos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorSegmentos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nuMinutos;
        private System.Windows.Forms.TextBox txtSeg4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSeg3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSeg2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSeg1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorSegmentos;
    }
}