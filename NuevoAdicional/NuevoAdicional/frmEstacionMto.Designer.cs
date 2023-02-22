namespace NuevoAdicional
{
    partial class frmEstacionMto
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIpServicios = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTipoDispensario = new System.Windows.Forms.TextBox();
            this.lblTryConect = new System.Windows.Forms.Label();
            this.pnlTryConnect = new System.Windows.Forms.Panel();
            this.pnlTryConnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.SystemColors.Window;
            this.txtNombre.Location = new System.Drawing.Point(84, 41);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(248, 20);
            this.txtNombre.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ip Servicios:";
            // 
            // txtIpServicios
            // 
            this.txtIpServicios.Location = new System.Drawing.Point(139, 12);
            this.txtIpServicios.MaxLength = 80;
            this.txtIpServicios.Name = "txtIpServicios";
            this.txtIpServicios.Size = new System.Drawing.Size(156, 20);
            this.txtIpServicios.TabIndex = 2;
            this.txtIpServicios.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtIpServicios.TextChanged += new System.EventHandler(this.txtIpServicios_TextChanged);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Enabled = false;
            this.btnAceptar.Location = new System.Drawing.Point(147, 99);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 10;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(257, 99);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(84, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "net.tcp://";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Enabled = false;
            this.btnBuscar.Image = global::NuevoAdicional.Properties.Resources.find;
            this.btnBuscar.Location = new System.Drawing.Point(301, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(31, 20);
            this.btnBuscar.TabIndex = 7;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tipo de Dispensario:";
            // 
            // txtTipoDispensario
            // 
            this.txtTipoDispensario.BackColor = System.Drawing.SystemColors.Window;
            this.txtTipoDispensario.Location = new System.Drawing.Point(123, 70);
            this.txtTipoDispensario.Name = "txtTipoDispensario";
            this.txtTipoDispensario.ReadOnly = true;
            this.txtTipoDispensario.Size = new System.Drawing.Size(209, 20);
            this.txtTipoDispensario.TabIndex = 13;
            // 
            // lblTryConect
            // 
            this.lblTryConect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTryConect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTryConect.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTryConect.Location = new System.Drawing.Point(0, 0);
            this.lblTryConect.Name = "lblTryConect";
            this.lblTryConect.Size = new System.Drawing.Size(348, 129);
            this.lblTryConect.TabIndex = 0;
            this.lblTryConect.Text = "Intentando conexión.\r\nEspere por favor";
            this.lblTryConect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTryConnect
            // 
            this.pnlTryConnect.Controls.Add(this.lblTryConect);
            this.pnlTryConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTryConnect.Location = new System.Drawing.Point(0, 0);
            this.pnlTryConnect.Name = "pnlTryConnect";
            this.pnlTryConnect.Size = new System.Drawing.Size(348, 129);
            this.pnlTryConnect.TabIndex = 12;
            this.pnlTryConnect.Visible = false;
            // 
            // frmEstacionMto
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(348, 129);
            this.Controls.Add(this.txtTipoDispensario);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtIpServicios);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlTryConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEstacionMto";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "--";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmEstacionMto_HelpButtonClicked);
            this.Shown += new System.EventHandler(this.frmEstacionMto_Shown);
            this.pnlTryConnect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtIpServicios;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtTipoDispensario;
        private System.Windows.Forms.Label lblTryConect;
        private System.Windows.Forms.Panel pnlTryConnect;
    }
}