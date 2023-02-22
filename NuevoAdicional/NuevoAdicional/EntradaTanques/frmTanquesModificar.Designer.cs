namespace NuevoAdicional.EntradaTanques
{
    partial class frmTanquesModificar
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
            this.txtFolio = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtFecha = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtFechaHora = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtCorte = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtTanque = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtCombustible = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtVolRecepcion = new DevExpress.XtraEditors.TextEdit();
            this.chkGenerado = new DevExpress.XtraEditors.CheckEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHora.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTanque.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombustible.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolRecepcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkGenerado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 244);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 29);
            this.panel1.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(227, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(146, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            // 
            // txtFolio
            // 
            this.txtFolio.Location = new System.Drawing.Point(12, 31);
            this.txtFolio.Name = "txtFolio";
            this.txtFolio.Properties.AllowFocused = false;
            this.txtFolio.Properties.ReadOnly = true;
            this.txtFolio.Size = new System.Drawing.Size(100, 20);
            this.txtFolio.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(22, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Folio";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(29, 13);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "Fecha";
            // 
            // txtFecha
            // 
            this.txtFecha.Location = new System.Drawing.Point(12, 76);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.Properties.AllowFocused = false;
            this.txtFecha.Properties.ReadOnly = true;
            this.txtFecha.Size = new System.Drawing.Size(100, 20);
            this.txtFecha.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(118, 57);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(56, 13);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "Fecha/Hora";
            // 
            // txtFechaHora
            // 
            this.txtFechaHora.EditValue = "";
            this.txtFechaHora.Location = new System.Drawing.Point(118, 76);
            this.txtFechaHora.Name = "txtFechaHora";
            this.txtFechaHora.Properties.AllowFocused = false;
            this.txtFechaHora.Properties.ReadOnly = true;
            this.txtFechaHora.Size = new System.Drawing.Size(141, 20);
            this.txtFechaHora.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 102);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(27, 13);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "Corte";
            // 
            // txtCorte
            // 
            this.txtCorte.Location = new System.Drawing.Point(14, 121);
            this.txtCorte.Name = "txtCorte";
            this.txtCorte.Properties.AllowFocused = false;
            this.txtCorte.Properties.ReadOnly = true;
            this.txtCorte.Size = new System.Drawing.Size(100, 20);
            this.txtCorte.TabIndex = 4;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(123, 102);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 13);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "Tanque";
            // 
            // txtTanque
            // 
            this.txtTanque.Location = new System.Drawing.Point(123, 121);
            this.txtTanque.Name = "txtTanque";
            this.txtTanque.Properties.AllowFocused = false;
            this.txtTanque.Properties.ReadOnly = true;
            this.txtTanque.Size = new System.Drawing.Size(100, 20);
            this.txtTanque.TabIndex = 5;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(14, 147);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(58, 13);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "Combustible";
            // 
            // txtCombustible
            // 
            this.txtCombustible.Location = new System.Drawing.Point(14, 166);
            this.txtCombustible.Name = "txtCombustible";
            this.txtCombustible.Properties.AllowFocused = false;
            this.txtCombustible.Properties.ReadOnly = true;
            this.txtCombustible.Size = new System.Drawing.Size(100, 20);
            this.txtCombustible.TabIndex = 6;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(14, 192);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(92, 13);
            this.labelControl7.TabIndex = 15;
            this.labelControl7.Text = "Volumen Recepción";
            // 
            // txtVolRecepcion
            // 
            this.txtVolRecepcion.Location = new System.Drawing.Point(14, 211);
            this.txtVolRecepcion.Name = "txtVolRecepcion";
            this.txtVolRecepcion.Properties.Mask.EditMask = "(\\d{0,3}([,]{1})?\\d{0,3})([.]{1}\\d{0,3})?";
            this.txtVolRecepcion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtVolRecepcion.Properties.Mask.ShowPlaceHolders = false;
            this.txtVolRecepcion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVolRecepcion.Size = new System.Drawing.Size(100, 20);
            this.txtVolRecepcion.TabIndex = 7;
            // 
            // chkGenerado
            // 
            this.chkGenerado.Location = new System.Drawing.Point(121, 211);
            this.chkGenerado.Name = "chkGenerado";
            this.chkGenerado.Properties.Caption = "Generado";
            this.chkGenerado.Size = new System.Drawing.Size(75, 19);
            this.chkGenerado.TabIndex = 8;
            // 
            // frmTanquesModificar
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(314, 273);
            this.Controls.Add(this.chkGenerado);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.txtVolRecepcion);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtCombustible);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtTanque);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtCorte);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtFechaHora);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtFecha);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtFolio);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTanquesModificar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modificar Entrada";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFolio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaHora.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTanque.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombustible.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolRecepcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkGenerado.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraEditors.TextEdit txtFolio;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtFecha;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtFechaHora;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtCorte;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtTanque;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtCombustible;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtVolRecepcion;
        private DevExpress.XtraEditors.CheckEdit chkGenerado;
    }
}