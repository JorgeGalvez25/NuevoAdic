namespace NuevoAdicional
{
    partial class frmLicencias
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLicenciaBoton = new System.Windows.Forms.TextBox();
            this.txtLicenciaAndroid = new System.Windows.Forms.TextBox();
            this.pbBoton = new System.Windows.Forms.PictureBox();
            this.pbAndroid = new System.Windows.Forms.PictureBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAndroid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Botón de Emergencia:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Android:";
            // 
            // txtLicenciaBoton
            // 
            this.txtLicenciaBoton.Location = new System.Drawing.Point(130, 6);
            this.txtLicenciaBoton.Name = "txtLicenciaBoton";
            this.txtLicenciaBoton.Size = new System.Drawing.Size(136, 20);
            this.txtLicenciaBoton.TabIndex = 2;
            // 
            // txtLicenciaAndroid
            // 
            this.txtLicenciaAndroid.Location = new System.Drawing.Point(130, 44);
            this.txtLicenciaAndroid.Name = "txtLicenciaAndroid";
            this.txtLicenciaAndroid.Size = new System.Drawing.Size(136, 20);
            this.txtLicenciaAndroid.TabIndex = 3;
            // 
            // pbBoton
            // 
            this.pbBoton.Location = new System.Drawing.Point(272, 6);
            this.pbBoton.Name = "pbBoton";
            this.pbBoton.Size = new System.Drawing.Size(20, 20);
            this.pbBoton.TabIndex = 4;
            this.pbBoton.TabStop = false;
            // 
            // pbAndroid
            // 
            this.pbAndroid.Location = new System.Drawing.Point(272, 47);
            this.pbAndroid.Name = "pbAndroid";
            this.pbAndroid.Size = new System.Drawing.Size(20, 20);
            this.pbAndroid.TabIndex = 5;
            this.pbAndroid.TabStop = false;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(130, 85);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(217, 85);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // frmLicencias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(309, 127);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.pbAndroid);
            this.Controls.Add(this.pbBoton);
            this.Controls.Add(this.txtLicenciaAndroid);
            this.Controls.Add(this.txtLicenciaBoton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmLicencias";
            this.Text = "Administración de Licencias";
            ((System.ComponentModel.ISupportInitialize)(this.pbBoton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAndroid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLicenciaBoton;
        private System.Windows.Forms.TextBox txtLicenciaAndroid;
        private System.Windows.Forms.PictureBox pbBoton;
        private System.Windows.Forms.PictureBox pbAndroid;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}