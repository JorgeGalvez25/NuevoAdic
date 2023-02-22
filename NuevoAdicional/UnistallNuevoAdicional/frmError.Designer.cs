namespace UnistallNuevoAdicional
{
    partial class frmError
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
            this.txtError = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtError.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtError
            // 
            this.txtError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtError.Location = new System.Drawing.Point(0, 0);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(460, 189);
            this.txtError.TabIndex = 1;
            // 
            // frmError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 189);
            this.Controls.Add(this.txtError);
            this.Name = "frmError";
            this.Text = "Detalles";
            ((System.ComponentModel.ISupportInitialize)(this.txtError.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit txtError;
    }
}