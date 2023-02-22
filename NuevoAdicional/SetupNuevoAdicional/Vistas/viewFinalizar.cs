using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace SetupNuevoAdicional
{
    public partial class viewFinalizar : DevExpress.XtraEditors.XtraUserControl, IPage
    {
        public viewFinalizar()
        {
            InitializeComponent();

            chkEjecutar.Checked = true;

            this.Parent = new DevExpress.XtraWizard.CompletionWizardPage();
            this.Parent.Text = Constantes.Titulos.Finalizar;
            this.Parent.AllowCancel = false;
            this.Parent.AllowBack = false;
            this.Parent.Controls.Add(this);
        }

        #region IPage Members

        public new DevExpress.XtraWizard.BaseWizardPage Parent { get; set; }

        public new void Load()
        {
        }

        public bool Closing(object sender, FormClosingEventArgs e)
        {
            if (chkEjecutar.Checked)
            {
                DirectoryInfo path = WorkItem.Objetos<DirectoryInfo>.Get("ruta ejecutable");

                System.Diagnostics.Process shell = new System.Diagnostics.Process();
                shell.StartInfo.FileName = Path.Combine(path.FullName, Constantes.FileName);
                shell.StartInfo.WorkingDirectory = Path.Combine(path.FullName, Constantes.WorkingDirectory);
                shell.Start();
            }

            return e.Cancel;
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
