namespace SetupNuevoAdicional
{
    partial class viewBienvenida
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(viewBienvenida));
            this.lbMensaje = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // lbMensaje
            // 
            this.lbMensaje.Location = new System.Drawing.Point(3, 3);
            this.lbMensaje.Name = "lbMensaje";
            this.lbMensaje.Size = new System.Drawing.Size(404, 91);
            this.lbMensaje.TabIndex = 1;
            this.lbMensaje.Text = resources.GetString("lbMensaje.Text");
            // 
            // viewBienvenida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbMensaje);
            this.Name = "viewBienvenida";
            this.Size = new System.Drawing.Size(503, 139);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbMensaje;

    }
}
