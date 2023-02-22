namespace ClientTest.View
{
    partial class FrmCliente
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCorrecto = new System.Windows.Forms.Button();
            this.btnIncorrecto = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnIniciarSesion = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtInfoLog = new System.Windows.Forms.TextBox();
            this.btnSolitarActualizacion = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Location = new System.Drawing.Point(93, 90);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(75, 23);
            this.btnCerrarSesion.TabIndex = 5;
            this.btnCerrarSesion.Text = "Cerrar";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSolitarActualizacion);
            this.panel1.Controls.Add(this.btnCorrecto);
            this.panel1.Controls.Add(this.btnIncorrecto);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(716, 128);
            this.panel1.TabIndex = 3;
            // 
            // btnCorrecto
            // 
            this.btnCorrecto.Location = new System.Drawing.Point(192, 64);
            this.btnCorrecto.Name = "btnCorrecto";
            this.btnCorrecto.Size = new System.Drawing.Size(75, 23);
            this.btnCorrecto.TabIndex = 2;
            this.btnCorrecto.Text = "Correcto";
            this.btnCorrecto.UseVisualStyleBackColor = true;
            // 
            // btnIncorrecto
            // 
            this.btnIncorrecto.Location = new System.Drawing.Point(192, 12);
            this.btnIncorrecto.Name = "btnIncorrecto";
            this.btnIncorrecto.Size = new System.Drawing.Size(75, 23);
            this.btnIncorrecto.TabIndex = 1;
            this.btnIncorrecto.Text = "Fallo";
            this.btnIncorrecto.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCerrarSesion);
            this.panel2.Controls.Add(this.btnIniciarSesion);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtUsuario);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(186, 128);
            this.panel2.TabIndex = 0;
            // 
            // btnIniciarSesion
            // 
            this.btnIniciarSesion.Location = new System.Drawing.Point(12, 90);
            this.btnIniciarSesion.Name = "btnIniciarSesion";
            this.btnIniciarSesion.Size = new System.Drawing.Size(75, 23);
            this.btnIniciarSesion.TabIndex = 4;
            this.btnIniciarSesion.Text = "Iniciar";
            this.btnIniciarSesion.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 64);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Usuario";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(12, 25);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 0;
            // 
            // txtInfoLog
            // 
            this.txtInfoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoLog.Location = new System.Drawing.Point(0, 128);
            this.txtInfoLog.Multiline = true;
            this.txtInfoLog.Name = "txtInfoLog";
            this.txtInfoLog.Size = new System.Drawing.Size(716, 351);
            this.txtInfoLog.TabIndex = 2;
            // 
            // btnSolitarActualizacion
            // 
            this.btnSolitarActualizacion.Location = new System.Drawing.Point(316, 12);
            this.btnSolitarActualizacion.Name = "btnSolitarActualizacion";
            this.btnSolitarActualizacion.Size = new System.Drawing.Size(128, 23);
            this.btnSolitarActualizacion.TabIndex = 3;
            this.btnSolitarActualizacion.Text = "Solicitar Actualizacion";
            this.btnSolitarActualizacion.UseVisualStyleBackColor = true;
            // 
            // FrmCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInfoLog);
            this.Controls.Add(this.panel1);
            this.Name = "FrmCliente";
            this.Size = new System.Drawing.Size(716, 479);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCorrecto;
        private System.Windows.Forms.Button btnIncorrecto;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnIniciarSesion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtInfoLog;
        private System.Windows.Forms.Button btnSolitarActualizacion;
    }
}
