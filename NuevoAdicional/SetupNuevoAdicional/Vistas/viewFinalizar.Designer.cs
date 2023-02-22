namespace SetupNuevoAdicional
{
    partial class viewFinalizar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(viewFinalizar));
            this.chkEjecutar = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chkEjecutar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEjecutar
            // 
            this.chkEjecutar.Location = new System.Drawing.Point(1, 93);
            this.chkEjecutar.Name = "chkEjecutar";
            this.chkEjecutar.Properties.Caption = "Ejecutar Consola Adicional Gasolineras";
            this.chkEjecutar.Size = new System.Drawing.Size(205, 18);
            this.chkEjecutar.TabIndex = 7;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(3, 142);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(335, 13);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Por favor, haga clic en Finalizar para salir del programa de instalación.";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(438, 52);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = resources.GetString("labelControl1.Text");
            // 
            // viewFinalizar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkEjecutar);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "viewFinalizar";
            this.Size = new System.Drawing.Size(477, 310);
            ((System.ComponentModel.ISupportInitialize)(this.chkEjecutar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkEjecutar;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
