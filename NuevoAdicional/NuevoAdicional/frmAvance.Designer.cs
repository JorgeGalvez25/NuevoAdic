namespace NuevoAdicional
{
    partial class frmAvance
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblAvance = new System.Windows.Forms.Label();
            this.pbAvance = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(35, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Título";
            // 
            // lblAvance
            // 
            this.lblAvance.BackColor = System.Drawing.Color.Transparent;
            this.lblAvance.Location = new System.Drawing.Point(12, 37);
            this.lblAvance.Name = "lblAvance";
            this.lblAvance.Size = new System.Drawing.Size(225, 14);
            this.lblAvance.TabIndex = 1;
            this.lblAvance.Text = "Avance";
            this.lblAvance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbAvance
            // 
            this.pbAvance.Location = new System.Drawing.Point(12, 54);
            this.pbAvance.Name = "pbAvance";
            this.pbAvance.Size = new System.Drawing.Size(225, 23);
            this.pbAvance.TabIndex = 2;
            this.pbAvance.Value = 50;
            // 
            // frmAvance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(249, 95);
            this.ControlBox = false;
            this.Controls.Add(this.lblAvance);
            this.Controls.Add(this.pbAvance);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAvance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAvance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblAvance;
        private System.Windows.Forms.ProgressBar pbAvance;
    }
}