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
    public partial class viewRuta : DevExpress.XtraEditors.XtraUserControl, IPage
    {
        public viewRuta()
        {
            InitializeComponent();

            this.txtCarpeta.Text = Path.Combine(Constantes.TargetDir, Constantes.ApplicationFolder);

            this.Parent = new DevExpress.XtraWizard.WizardPage();
            this.Parent.Text = Constantes.Titulos.Ruta;
            this.Parent.Controls.Add(this);
        }

        #region Eventos
        private void txtCarpeta_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.dlgFolder.SelectedPath = txtCarpeta.Text;
            if (this.dlgFolder.ShowDialog() == DialogResult.OK)
            {
                txtCarpeta.Text = this.dlgFolder.SelectedPath;
            }
        }

        private void txtCarpeta_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        #endregion

        #region IPage Members

        public new DevExpress.XtraWizard.BaseWizardPage Parent { get; set; }

        public new void Load()
        {
            this.BeginInvoke(new MethodInvoker(() => txtCarpeta.Focus()));
        }

        public bool Closing(object sender, FormClosingEventArgs e)
        {
            return e.Cancel;
        }

        public void NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(txtCarpeta.Text));
                WorkItem.Objetos<DirectoryInfo>.Add("ruta ejecutable", dir);
            }
            catch (Exception)
            {
                Utils.MensajeInfo(Constantes.Mensajes.CarpetaSeleccionadaEsInvalida);
                e.Handled = true;
            }
        }

        public void PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
        }

        #endregion
    }
}
