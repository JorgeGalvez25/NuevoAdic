using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace SetupNuevoAdicional.Vistas
{
    public partial class viewProcesando : XtraUserControl, IPage
    {
        private BackgroundWorker hilo = null;

        public viewProcesando()
        {
            InitializeComponent();

            hilo = new BackgroundWorker();
            hilo.DoWork += new DoWorkEventHandler(hilo_DoWork);
            hilo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hilo_RunWorkerCompleted);

            this.Parent = new DevExpress.XtraWizard.WizardPage();
            this.Parent.Text = Constantes.Titulos.Procesando;
            this.Parent.AllowBack = false;
            this.Parent.AllowNext = false;
            this.Parent.AllowCancel = false;
            this.Parent.Controls.Add(this);
        }

        #region Delegados

        delegate void estatus(string valor);

        private void estatusCallBack(string valor)
        {
            if (this.InvokeRequired)
            {
                estatus control = new estatus(estatusCallBack);
                this.Invoke(control, new object[] { valor });
            }
            else
            {
                lbEstatus.Text = valor;
            }
        }

        #endregion

        #region Eventos

        void hilo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                WorkItem.Objetos<Exception>.Add("error", e.Error);
                this.Parent.Owner.SelectedPage = WorkItem.Vistas.Find(i => i.GetType().Equals(typeof(viewError))).Parent;
            }
            else
            {
                this.Parent.Owner.SelectedPage = WorkItem.Vistas.Find(i => i.GetType().Equals(typeof(viewFinalizar))).Parent;
            }
        }

        void hilo_DoWork(object sender, DoWorkEventArgs e)
        {
            Proceso p = new Proceso();
            p.Estatus += new Proceso.EstatusEventHandler(proceso_Estatus);
            p.Ejecutar();
        }

        void proceso_Estatus(string e)
        {
            estatusCallBack(e);
        }

        #endregion

        #region IPage Members

        public new DevExpress.XtraWizard.BaseWizardPage Parent { get; set; }

        public new void Load()
        {
            hilo.RunWorkerAsync();
        }

        public bool Closing(object sender, FormClosingEventArgs e)
        {
            return e.Cancel = true;
        }

        public void NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
        }

        public void PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
        }

        #endregion
    }
}
