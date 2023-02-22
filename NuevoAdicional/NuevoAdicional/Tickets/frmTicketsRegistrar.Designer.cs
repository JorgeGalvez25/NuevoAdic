namespace NuevoAdicional
{
    partial class frmTicketsRegistrar
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
            this.txtFolio = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.luCombustible = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtPrecio = new DevExpress.XtraEditors.TextEdit();
            this.txtVolumen = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtImporte = new DevExpress.XtraEditors.TextEdit();
            this.spnPoscionCarga = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.chkNoAjustar = new DevExpress.XtraEditors.CheckEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.chkFecha = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luCombustible.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrecio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolumen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnPoscionCarga.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNoAjustar.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFecha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFolio
            // 
            this.txtFolio.EditValue = "";
            this.txtFolio.Location = new System.Drawing.Point(12, 31);
            this.txtFolio.Name = "txtFolio";
            this.txtFolio.Properties.ReadOnly = true;
            this.txtFolio.Size = new System.Drawing.Size(73, 20);
            this.txtFolio.TabIndex = 1;
            this.txtFolio.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(22, 13);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "Folio";
            // 
            // luCombustible
            // 
            this.luCombustible.Location = new System.Drawing.Point(118, 76);
            this.luCombustible.Name = "luCombustible";
            this.luCombustible.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luCombustible.Properties.NullText = "";
            this.luCombustible.Size = new System.Drawing.Size(100, 20);
            this.luCombustible.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(118, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(58, 13);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "Combustible";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 102);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(29, 13);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "Precio";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(12, 121);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(100, 20);
            this.txtPrecio.TabIndex = 4;
            this.txtPrecio.TabStop = false;
            // 
            // txtVolumen
            // 
            this.txtVolumen.Location = new System.Drawing.Point(118, 121);
            this.txtVolumen.Name = "txtVolumen";
            this.txtVolumen.Properties.Mask.EditMask = "(\\d{0,3}([,]{1})?\\d{0,3})([.]{1}\\d{0,3})?";
            this.txtVolumen.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtVolumen.Properties.Mask.ShowPlaceHolders = false;
            this.txtVolumen.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVolumen.Size = new System.Drawing.Size(100, 20);
            this.txtVolumen.TabIndex = 5;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(224, 102);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(38, 13);
            this.labelControl5.TabIndex = 15;
            this.labelControl5.Text = "Importe";
            // 
            // txtImporte
            // 
            this.txtImporte.Location = new System.Drawing.Point(224, 121);
            this.txtImporte.Name = "txtImporte";
            this.txtImporte.Properties.Mask.EditMask = "(\\d{0,3}([,]{1})?\\d{0,3}([,]{1})?\\d{0,3})([.]{1}\\d{0,3})?";
            this.txtImporte.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtImporte.Properties.Mask.ShowPlaceHolders = false;
            this.txtImporte.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtImporte.Size = new System.Drawing.Size(100, 20);
            this.txtImporte.TabIndex = 6;
            // 
            // spnPoscionCarga
            // 
            this.spnPoscionCarga.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnPoscionCarga.Location = new System.Drawing.Point(12, 76);
            this.spnPoscionCarga.Name = "spnPoscionCarga";
            this.spnPoscionCarga.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spnPoscionCarga.Properties.IsFloatValue = false;
            this.spnPoscionCarga.Properties.Mask.EditMask = "N00";
            this.spnPoscionCarga.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spnPoscionCarga.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnPoscionCarga.Size = new System.Drawing.Size(100, 20);
            this.spnPoscionCarga.TabIndex = 2;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 57);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(83, 13);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "Posición de carga";
            // 
            // chkNoAjustar
            // 
            this.chkNoAjustar.Location = new System.Drawing.Point(12, 147);
            this.chkNoAjustar.Name = "chkNoAjustar";
            this.chkNoAjustar.Properties.Caption = "No ajustar";
            this.chkNoAjustar.Size = new System.Drawing.Size(75, 19);
            this.chkNoAjustar.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 169);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 46);
            this.panel1.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(246, 11);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Location = new System.Drawing.Point(165, 11);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = null;
            this.dtFecha.Location = new System.Drawing.Point(95, 31);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.DisplayFormat.FormatString = "g";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtFecha.Properties.Mask.EditMask = "g";
            this.dtFecha.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Size = new System.Drawing.Size(145, 20);
            this.dtFecha.TabIndex = 16;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(118, 102);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(40, 13);
            this.labelControl7.TabIndex = 17;
            this.labelControl7.Text = "Volumen";
            // 
            // chkFecha
            // 
            this.chkFecha.Location = new System.Drawing.Point(93, 6);
            this.chkFecha.Name = "chkFecha";
            this.chkFecha.Properties.Caption = "Fecha";
            this.chkFecha.Size = new System.Drawing.Size(75, 19);
            this.chkFecha.TabIndex = 18;
            // 
            // frmTicketsRegistrar
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(333, 215);
            this.Controls.Add(this.chkFecha);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkNoAjustar);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.spnPoscionCarga);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtImporte);
            this.Controls.Add(this.txtVolumen);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.luCombustible);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtFolio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTicketsRegistrar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar Ticket";
            ((System.ComponentModel.ISupportInitialize)(this.txtFolio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luCombustible.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrecio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolumen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImporte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnPoscionCarga.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkNoAjustar.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFecha.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtFolio;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit luCombustible;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtPrecio;
        private DevExpress.XtraEditors.TextEdit txtVolumen;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtImporte;
        private DevExpress.XtraEditors.SpinEdit spnPoscionCarga;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.CheckEdit chkNoAjustar;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private DevExpress.XtraEditors.DateEdit dtFecha;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.CheckEdit chkFecha;
    }
}