namespace NuevoAdicional
{
    partial class frmLectTanques
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
            this.chkPorTurno = new DevExpress.XtraEditors.CheckEdit();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            this.spnTurno = new DevExpress.XtraEditors.SpinEdit();
            this.txtLectura = new DevExpress.XtraEditors.TextEdit();
            this.cmbTanques = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chkPorTurno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnTurno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLectura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTanques.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPorTurno
            // 
            this.chkPorTurno.Location = new System.Drawing.Point(309, 15);
            this.chkPorTurno.Name = "chkPorTurno";
            this.chkPorTurno.Properties.Caption = "Turno";
            this.chkPorTurno.Size = new System.Drawing.Size(75, 19);
            this.chkPorTurno.TabIndex = 28;
            this.chkPorTurno.CheckedChanged += new System.EventHandler(this.chkPorTurno_CheckedChanged);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = null;
            this.dtFecha.Location = new System.Drawing.Point(192, 37);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.DisplayFormat.FormatString = "g";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtFecha.Properties.Mask.EditMask = "g";
            this.dtFecha.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Size = new System.Drawing.Size(80, 20);
            this.dtFecha.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 46);
            this.panel1.TabIndex = 19;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(298, 11);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(217, 11);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // spnTurno
            // 
            this.spnTurno.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnTurno.Enabled = false;
            this.spnTurno.Location = new System.Drawing.Point(311, 37);
            this.spnTurno.Name = "spnTurno";
            this.spnTurno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spnTurno.Properties.IsFloatValue = false;
            this.spnTurno.Properties.Mask.EditMask = "N00";
            this.spnTurno.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spnTurno.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnTurno.Size = new System.Drawing.Size(57, 20);
            this.spnTurno.TabIndex = 21;
            // 
            // txtLectura
            // 
            this.txtLectura.Location = new System.Drawing.Point(12, 97);
            this.txtLectura.Name = "txtLectura";
            this.txtLectura.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtLectura.Size = new System.Drawing.Size(100, 20);
            this.txtLectura.TabIndex = 23;
            this.txtLectura.TabStop = false;
            // 
            // cmbTanques
            // 
            this.cmbTanques.Location = new System.Drawing.Point(12, 37);
            this.cmbTanques.Name = "cmbTanques";
            this.cmbTanques.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTanques.Size = new System.Drawing.Size(149, 20);
            this.cmbTanques.TabIndex = 29;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 13);
            this.labelControl1.TabIndex = 30;
            this.labelControl1.Text = "Tanque";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(192, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(29, 13);
            this.labelControl2.TabIndex = 31;
            this.labelControl2.Text = "Fecha";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 78);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 13);
            this.labelControl3.TabIndex = 32;
            this.labelControl3.Text = "Lectura";
            // 
            // frmLectTanques
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 187);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cmbTanques);
            this.Controls.Add(this.chkPorTurno);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.spnTurno);
            this.Controls.Add(this.txtLectura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLectTanques";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar lectura de tanques";
            this.Load += new System.EventHandler(this.frmLectTanques_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkPorTurno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spnTurno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLectura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTanques.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkPorTurno;
        private DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraEditors.SpinEdit spnTurno;
        private DevExpress.XtraEditors.TextEdit txtLectura;
        private DevExpress.XtraEditors.ComboBoxEdit cmbTanques;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}