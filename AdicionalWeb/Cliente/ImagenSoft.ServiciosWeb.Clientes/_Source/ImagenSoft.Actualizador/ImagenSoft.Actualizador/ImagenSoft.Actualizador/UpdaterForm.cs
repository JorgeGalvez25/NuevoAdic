using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Threading;

namespace ImagenSoft.Actualizador
{
    public partial class UpdaterForm : DevExpress.XtraEditors.XtraForm, IUpdater
    {
        private string aplicacion;

        private int maximo;
        private int actual = 0;
        Presenter _presenter;
        private bool completo;
        private bool error;
        private string ensamblado = string.Empty;
        private string errores;

        private Thread hiloCalculo;
        private Thread hiloAvance;

        public UpdaterForm(string aplicacion)
        {
            InitializeComponent();
            _presenter = new Presenter(this);
            completo = false;
            this.aplicacion = aplicacion;
        }

        #region IUpdater Members
        public int MaxPbar
        {
            set
            {
                maximo = value;
            }
        }

        public bool Finalizado
        {
            set
            {
                this.completo = value;
            }
        }

        public bool Error
        {
            set
            {
                this.error = value;
            }
        }

        public string Errores
        {
            set
            {
                this.errores = value;
            }
        }

        public void actualizar(string valor)
        {
            ensamblado = valor;
            if (actual < maximo)
            {
                actual++;
            }
            else
            {
                completo = true;
            }

        }

        #endregion

        private void Actualizar()
        {
            _presenter.Actualizar();
        }

        private void Mostrar()
        {
            string ensambladoAnt = string.Empty;
            int valorAnt = 0;

            while (!completo)
            {
                this.progressBarControl1.Properties.Maximum = maximo;

                if (ensambladoAnt != ensamblado)
                {
                    int dif = actual - valorAnt;
                    valorAnt = actual;
                    this.labelControl1.Refresh();
                    this.listBoxControl1.Items.Add(ensamblado);
                    ensambladoAnt = ensamblado;

                    this.progressBarControl1.Increment(dif);
                    this.progressBarControl1.Refresh();

                    this.listBoxControl1.SelectedIndex = valorAnt;
                    this.listBoxControl1.Refresh();
                }

            }
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {
        }

        private void UpdaterForm_Shown(object sender, EventArgs e)
        {
            this.progressBarControl1.Properties.Minimum = 0;
            try
            {
                completo = false;
                error = false;
                CheckForIllegalCrossThreadCalls = false;

                this.hiloCalculo =
                    new Thread(
                        new ThreadStart(
                            Actualizar));
                this.hiloCalculo.Start();

                //_presenter.Actualizar();

                this.hiloAvance =
                    new Thread(
                        new ThreadStart(
                            Mostrar));
                this.hiloAvance.Start();

                this.hiloCalculo.Join();
                this.hiloAvance.Join();

                this.hiloCalculo = null;
                this.hiloAvance = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!error)
            {
                bool continuar = true;
                if (errores != null)
                {
                    if (MessageBox.Show(errores + "\n¿Continuar con la Aplicación?", "Lista de Errores", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        continuar = false;
                    }
                }

                if (continuar && !string.IsNullOrEmpty(aplicacion))
                {
                    string rutaEnsamblados = System.Configuration.ConfigurationSettings.AppSettings["RutaEnsamblados"];
                    // Ejecutar shell.exe y cerrar actualizador
                    System.IO.Directory.SetCurrentDirectory(rutaEnsamblados);
                    //string aplicacion = Path.Combine(rutaEnsamblados, "Shell.exe");

                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = aplicacion;
                    p.StartInfo.Arguments = "NoActualizar";
                    p.Start();
                    //p.WaitForExit();
                }
            }
            this.Close();
        }

        private void UpdaterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!completo && !error) { e.Cancel = true; }
        }
    }
}