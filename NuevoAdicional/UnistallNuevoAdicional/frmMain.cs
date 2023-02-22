using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace UnistallNuevoAdicional
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        private DirectoryInfo path = null;
        private BackgroundWorker hilo = null;
        private Exception ex = null;


        public frmMain()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.TopLevel = true;

            barra.Visible = false;
            btnDetalles.Visible = false;

            pic.Image = global::UnistallNuevoAdicional.Properties.Resources.Unistall72;
            pic.Properties.AllowFocused = false;
            pic.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pic.BackColor = Color.Transparent;
            pic.Properties.ShowMenu = false;

            if (obtenerPath())
            {
                this.FormClosing += new FormClosingEventHandler(FMain_FormClosing);

                btnDetalles.Click += new EventHandler(btnDetalles_Click);
                btnSiguiente.Click += new EventHandler(btnSiguiente_Click);

                hilo = new BackgroundWorker();
                hilo.DoWork += new DoWorkEventHandler(hilo_DoWork);
                hilo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hilo_RunWorkerCompleted);
            }
            else
            {
                StringBuilder m = new StringBuilder();
                m.AppendLine("Los registros de instalación de Nuevo Adicional son inválidos, no es");
                m.AppendLine("posible iniciar el programa de desinstalación.");
                m.AppendLine();
                m.AppendLine("Haga clic en Cerrar para salir del programa.");
                lbMensaje.Text = m.ToString();

                btnSiguiente.Visible = false;
                btnAccion.Text = "Cerrar";
            }

            btnAccion.Click += new EventHandler(btnCancelar_Click);
        }

        #region Privadas

        private bool obtenerPath()
        {
            RegistryKey r = null;

            try
            {
                r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\NuevoAdicional");
                string uninstallPath = r.GetValue("InstallLocation").ToString();
                r.Close();
                r = null;

                path = new DirectoryInfo(uninstallPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Delegados

        delegate void barraVisible(bool visible);

        private void barraVisibleCallBack(bool visible)
        {
            if (barra.InvokeRequired)
            {
                barraVisible control = new barraVisible(barraVisibleCallBack);
                this.Invoke(control, new object[] { visible });
            }
            else
            {
                barra.Visible = visible;
            }
        }

        #endregion

        #region Eventos

        void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hilo.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            if (MessageBox.Show("¿Desea salir del programa de desinstalación?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        void hilo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StringBuilder m = new StringBuilder();

            if (e.Error != null)
            {
                m.AppendLine("Ocurrió un error durante el proceso de desinstalación, no es posible");
                m.AppendLine("desinstalar Consola de Nuevo Adicional de su equipo.");
                m.AppendLine();
                m.AppendLine("Haga clic en Detalles para mas información.");
                lbMensaje.Text = m.ToString();

                btnDetalles.Visible = true;
                btnAccion.Visible = true;

                ex = e.Error;
            }
            else
            {
                m.AppendLine("El programa completó la desinstalación de consola de Nuevo Adicional");
                m.AppendLine("de su equipo.");
                m.AppendLine();
                m.AppendLine("Haga clic en Finalizar para salir del programa.");
                lbMensaje.Text = m.ToString();

                btnAccion.Click -= new EventHandler(btnCancelar_Click);
                btnAccion.Text = "Finalizar";
                btnAccion.Click += new EventHandler(btnFinalizar_Click);
                btnAccion.Visible = true;
            }

            barraVisibleCallBack(false);
        }

        void btnFinalizar_Click(object sender, EventArgs e)
        {
            this.FormClosing -= new FormClosingEventHandler(FMain_FormClosing);
            Close();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = string.Format("/C choice /C Y /N /D Y /T 3 & Del \"{0}\"", Application.ExecutablePath);
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        void hilo_DoWork(object sender, DoWorkEventArgs e)
        {
            Proceso p = new Proceso();
            p.Ejecutar((DirectoryInfo)e.Argument);
        }

        void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnSiguiente_Click(object sender, EventArgs e)
        {
            StringBuilder m = new StringBuilder();
            m.AppendLine("Por favor, espere mientras se desinstala consola Nuevo Adicional de");
            m.AppendLine("su equipo.");
            m.AppendLine();
            m.AppendLine("Desinstalando...");
            lbMensaje.Text = m.ToString();

            barraVisibleCallBack(true);
            btnSiguiente.Visible = false;
            btnAccion.Visible = false;

            hilo.RunWorkerAsync(path);
        }

        void btnDetalles_Click(object sender, EventArgs e)
        {
            frmError f = new frmError(ex);
            f.ShowDialog(this);
        }

        #endregion
    }
}
