namespace UnistallNuevoAdicional
{
    partial class frmMain
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
            this.pic = new DevExpress.XtraEditors.PictureEdit();
            this.btnDetalles = new DevExpress.XtraEditors.SimpleButton();
            this.barra = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.btnSiguiente = new DevExpress.XtraEditors.SimpleButton();
            this.btnAccion = new DevExpress.XtraEditors.SimpleButton();
            this.lbMensaje = new DevExpress.XtraEditors.LabelControl();
            this.lfLook = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pic.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barra.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pic
            // 
            this.pic.Location = new System.Drawing.Point(343, 3);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(74, 74);
            this.pic.TabIndex = 11;
            // 
            // btnDetalles
            // 
            this.btnDetalles.Location = new System.Drawing.Point(12, 104);
            this.btnDetalles.Name = "btnDetalles";
            this.btnDetalles.Size = new System.Drawing.Size(75, 23);
            this.btnDetalles.TabIndex = 8;
            this.btnDetalles.Text = "Detalles";
            // 
            // barra
            // 
            this.barra.EditValue = 0;
            this.barra.Location = new System.Drawing.Point(12, 70);
            this.barra.Name = "barra";
            this.barra.Size = new System.Drawing.Size(315, 18);
            this.barra.TabIndex = 7;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Location = new System.Drawing.Point(252, 104);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(75, 23);
            this.btnSiguiente.TabIndex = 9;
            this.btnSiguiente.Text = "Siguiente";
            // 
            // btnAccion
            // 
            this.btnAccion.Location = new System.Drawing.Point(333, 104);
            this.btnAccion.Name = "btnAccion";
            this.btnAccion.Size = new System.Drawing.Size(75, 23);
            this.btnAccion.TabIndex = 10;
            this.btnAccion.Text = "Cancelar";
            // 
            // lbMensaje
            // 
            this.lbMensaje.Location = new System.Drawing.Point(12, 12);
            this.lbMensaje.Name = "lbMensaje";
            this.lbMensaje.Size = new System.Drawing.Size(281, 39);
            this.lbMensaje.TabIndex = 6;
            this.lbMensaje.Text = "Este programa desinstalará cliente FACELE-I de su equipo.\r\n\r\nPor favor, haga clic" +
                " en Siguiente para continuar.";
            // 
            // lfLook
            // 
            this.lfLook.LookAndFeel.SkinName = "iMaginary";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 143);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.btnDetalles);
            this.Controls.Add(this.barra);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAccion);
            this.Controls.Add(this.lbMensaje);
            this.Name = "frmMain";
            this.Text = "Desinstalador Consola Nuevo Adicional";
            ((System.ComponentModel.ISupportInitialize)(this.pic.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barra.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pic;
        private DevExpress.XtraEditors.SimpleButton btnDetalles;
        private DevExpress.XtraEditors.MarqueeProgressBarControl barra;
        private DevExpress.XtraEditors.SimpleButton btnSiguiente;
        private DevExpress.XtraEditors.SimpleButton btnAccion;
        private DevExpress.XtraEditors.LabelControl lbMensaje;
        private DevExpress.LookAndFeel.DefaultLookAndFeel lfLook;
    }
}

