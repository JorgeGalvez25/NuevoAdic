namespace SetupNuevoAdicional
{
    partial class viewBD
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtCarpeta = new DevExpress.XtraEditors.ButtonEdit();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.txtCarpeta.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(411, 39);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "El programa instalar� la base de datos de usuarios y permisos en la siguiente car" +
                "peta.\r\n\r\nSi desea instalar en otra carpeta, haga clic en el bot�n de b�squeda.";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(3, 58);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 13);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "Carpeta";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 114);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(230, 13);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "Por favor, haga clic en Siguente para continuar.";
            // 
            // txtCarpeta
            // 
            this.txtCarpeta.Location = new System.Drawing.Point(3, 77);
            this.txtCarpeta.Name = "txtCarpeta";
            this.txtCarpeta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, global::SetupNuevoAdicional.Properties.Resources.find, null)});
            this.txtCarpeta.Properties.Mask.EditMask = "[A-Z]{1}\\:\\\\[a-zA-Z0-9\\\\\\_\\-]{0,250}";
            this.txtCarpeta.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtCarpeta.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtCarpeta_Properties_ButtonClick);
            this.txtCarpeta.Size = new System.Drawing.Size(445, 22);
            this.txtCarpeta.TabIndex = 8;
            this.txtCarpeta.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(this.txtCarpeta_InvalidValue);
            // 
            // viewBD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtCarpeta);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "viewBD";
            this.Size = new System.Drawing.Size(489, 228);
            ((System.ComponentModel.ISupportInitialize)(this.txtCarpeta.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ButtonEdit txtCarpeta;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
    }
}
