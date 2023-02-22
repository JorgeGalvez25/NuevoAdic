namespace EstandarCliente.AdministrarUsuariosMdl.Views.VMAdministrarUsuarios.Modal
{
    partial class ModalPassword
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
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            this.titulo1 = new ControlesEstandar.Titulo();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.txtConfirmacion = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 192);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 47);
            this.panel1.TabIndex = 5;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(224, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(143, 12);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            // 
            // titulo1
            // 
            this.titulo1.BackColor1 = System.Drawing.Color.LightSkyBlue;
            this.titulo1.BackColor2 = System.Drawing.Color.RoyalBlue;
            this.titulo1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titulo1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.titulo1.Location = new System.Drawing.Point(0, 0);
            this.titulo1.Name = "titulo1";
            this.titulo1.Size = new System.Drawing.Size(302, 50);
            this.titulo1.TabIndex = 0;
            this.titulo1.TituloMenu = "Cambiar Contraseña";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 74);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Contraseña";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 119);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(62, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Confirmación";
            // 
            // txtContrasena
            // 
            this.txtContrasena.Font = new System.Drawing.Font("Wingdings", 9.75F);
            this.txtContrasena.Location = new System.Drawing.Point(12, 93);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.PasswordChar = 'l';
            this.txtContrasena.Size = new System.Drawing.Size(270, 22);
            this.txtContrasena.TabIndex = 1;
            // 
            // txtConfirmacion
            // 
            this.txtConfirmacion.Font = new System.Drawing.Font("Wingdings", 9.75F);
            this.txtConfirmacion.Location = new System.Drawing.Point(12, 138);
            this.txtConfirmacion.Name = "txtConfirmacion";
            this.txtConfirmacion.PasswordChar = 'l';
            this.txtConfirmacion.Size = new System.Drawing.Size(270, 22);
            this.txtConfirmacion.TabIndex = 2;
            // 
            // ModalPassword
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(302, 239);
            this.Controls.Add(this.txtConfirmacion);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.titulo1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(308, 268);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(308, 268);
            this.Name = "ModalPassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contraseña";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ControlesEstandar.Titulo titulo1;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.TextBox txtConfirmacion;

    }
}