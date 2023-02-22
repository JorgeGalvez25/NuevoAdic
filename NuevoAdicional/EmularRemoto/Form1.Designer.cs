namespace EmularRemoto
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gbRemotos = new System.Windows.Forms.GroupBox();
            this.chkRemoto4 = new System.Windows.Forms.CheckBox();
            this.chkRemoto3 = new System.Windows.Forms.CheckBox();
            this.chkRemoto2 = new System.Windows.Forms.CheckBox();
            this.chkRemoto1 = new System.Windows.Forms.CheckBox();
            this.pbVisual = new System.Windows.Forms.PictureBox();
            this.txtComandosEnviados = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtComandosRecibidos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bwRemoto1 = new System.ComponentModel.BackgroundWorker();
            this.bwRemoto2 = new System.ComponentModel.BackgroundWorker();
            this.bwRemoto3 = new System.ComponentModel.BackgroundWorker();
            this.bwRemoto4 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblEstadoSocket = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbRemotos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVisual)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRemotos
            // 
            this.gbRemotos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRemotos.Controls.Add(this.chkRemoto4);
            this.gbRemotos.Controls.Add(this.chkRemoto3);
            this.gbRemotos.Controls.Add(this.chkRemoto2);
            this.gbRemotos.Controls.Add(this.chkRemoto1);
            this.gbRemotos.Location = new System.Drawing.Point(12, 12);
            this.gbRemotos.Name = "gbRemotos";
            this.gbRemotos.Size = new System.Drawing.Size(299, 117);
            this.gbRemotos.TabIndex = 0;
            this.gbRemotos.TabStop = false;
            this.gbRemotos.Text = "Remotos";
            // 
            // chkRemoto4
            // 
            this.chkRemoto4.AutoSize = true;
            this.chkRemoto4.Location = new System.Drawing.Point(6, 88);
            this.chkRemoto4.Name = "chkRemoto4";
            this.chkRemoto4.Size = new System.Drawing.Size(267, 17);
            this.chkRemoto4.TabIndex = 3;
            this.chkRemoto4.Tag = "3";
            this.chkRemoto4.Text = "00 16 D2 00 40 91 C0 01 FF FE 02 01 00 01 00 00";
            this.chkRemoto4.UseVisualStyleBackColor = true;
            // 
            // chkRemoto3
            // 
            this.chkRemoto3.AutoSize = true;
            this.chkRemoto3.Location = new System.Drawing.Point(6, 65);
            this.chkRemoto3.Name = "chkRemoto3";
            this.chkRemoto3.Size = new System.Drawing.Size(266, 17);
            this.chkRemoto3.TabIndex = 2;
            this.chkRemoto3.Tag = "2";
            this.chkRemoto3.Text = "00 15 C2 00 40 91 C0 01 FF FE 02 01 00 01 00 00";
            this.chkRemoto3.UseVisualStyleBackColor = true;
            // 
            // chkRemoto2
            // 
            this.chkRemoto2.AutoSize = true;
            this.chkRemoto2.Location = new System.Drawing.Point(6, 42);
            this.chkRemoto2.Name = "chkRemoto2";
            this.chkRemoto2.Size = new System.Drawing.Size(266, 17);
            this.chkRemoto2.TabIndex = 1;
            this.chkRemoto2.Tag = "1";
            this.chkRemoto2.Text = "00 14 B2 00 40 91 C0 01 FF FE 02 01 00 01 00 00";
            this.chkRemoto2.UseVisualStyleBackColor = true;
            // 
            // chkRemoto1
            // 
            this.chkRemoto1.AutoSize = true;
            this.chkRemoto1.Location = new System.Drawing.Point(6, 19);
            this.chkRemoto1.Name = "chkRemoto1";
            this.chkRemoto1.Size = new System.Drawing.Size(266, 17);
            this.chkRemoto1.TabIndex = 0;
            this.chkRemoto1.Tag = "0";
            this.chkRemoto1.Text = "00 13 A2 00 40 91 C0 01 FF FE 02 01 00 01 00 00";
            this.chkRemoto1.UseVisualStyleBackColor = true;
            this.chkRemoto1.CheckedChanged += new System.EventHandler(this.chkRemoto_CheckedChanged);
            // 
            // pbVisual
            // 
            this.pbVisual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVisual.Image = global::EmularRemoto.Properties.Resources.LedOFF;
            this.pbVisual.Location = new System.Drawing.Point(201, 318);
            this.pbVisual.Name = "pbVisual";
            this.pbVisual.Size = new System.Drawing.Size(110, 112);
            this.pbVisual.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbVisual.TabIndex = 2;
            this.pbVisual.TabStop = false;
            // 
            // txtComandosEnviados
            // 
            this.txtComandosEnviados.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComandosEnviados.BackColor = System.Drawing.SystemColors.Window;
            this.txtComandosEnviados.Location = new System.Drawing.Point(12, 148);
            this.txtComandosEnviados.Multiline = true;
            this.txtComandosEnviados.Name = "txtComandosEnviados";
            this.txtComandosEnviados.ReadOnly = true;
            this.txtComandosEnviados.Size = new System.Drawing.Size(297, 66);
            this.txtComandosEnviados.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Comandos Enviados";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Comandos Recibidos";
            // 
            // txtComandosRecibidos
            // 
            this.txtComandosRecibidos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComandosRecibidos.BackColor = System.Drawing.SystemColors.Window;
            this.txtComandosRecibidos.Location = new System.Drawing.Point(12, 233);
            this.txtComandosRecibidos.Multiline = true;
            this.txtComandosRecibidos.Name = "txtComandosRecibidos";
            this.txtComandosRecibidos.ReadOnly = true;
            this.txtComandosRecibidos.Size = new System.Drawing.Size(297, 66);
            this.txtComandosRecibidos.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Aviso Visual";
            // 
            // bwRemoto1
            // 
            this.bwRemoto1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRemoto1_DoWork);
            // 
            // bwRemoto2
            // 
            this.bwRemoto2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRemoto2_DoWork);
            // 
            // bwRemoto3
            // 
            this.bwRemoto3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRemoto3_DoWork);
            // 
            // bwRemoto4
            // 
            this.bwRemoto4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRemoto4_DoWork);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblEstadoSocket});
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(323, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblEstadoSocket
            // 
            this.lblEstadoSocket.Name = "lblEstadoSocket";
            this.lblEstadoSocket.Size = new System.Drawing.Size(80, 17);
            this.lblEstadoSocket.Text = "Estado Socket";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 452);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtComandosRecibidos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComandosEnviados);
            this.Controls.Add(this.gbRemotos);
            this.Controls.Add(this.pbVisual);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(339, 491);
            this.MinimumSize = new System.Drawing.Size(339, 491);
            this.Name = "Form1";
            this.Text = "Emulación de Remoto";
            this.gbRemotos.ResumeLayout(false);
            this.gbRemotos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVisual)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVisual;
        private System.Windows.Forms.GroupBox gbRemotos;
        private System.Windows.Forms.CheckBox chkRemoto1;
        private System.Windows.Forms.CheckBox chkRemoto4;
        private System.Windows.Forms.CheckBox chkRemoto3;
        private System.Windows.Forms.CheckBox chkRemoto2;
        private System.Windows.Forms.TextBox txtComandosEnviados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtComandosRecibidos;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker bwRemoto1;
        private System.ComponentModel.BackgroundWorker bwRemoto2;
        private System.ComponentModel.BackgroundWorker bwRemoto3;
        private System.ComponentModel.BackgroundWorker bwRemoto4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblEstadoSocket;
    }
}

