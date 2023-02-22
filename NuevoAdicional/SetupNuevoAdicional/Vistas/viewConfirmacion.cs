using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SetupNuevoAdicional
{
    public partial class viewConfirmacion : DevExpress.XtraEditors.XtraUserControl, IPage
    {
        public viewConfirmacion()
        {
            InitializeComponent();

            this.Parent = new DevExpress.XtraWizard.WizardPage();
            this.Parent.Text = Constantes.Titulos.Confirmar;
            this.Parent.Controls.Add(this);
        }

        #region IPage Members

        public new DevExpress.XtraWizard.BaseWizardPage Parent { get; set; }

        public new void Load()
        {
        }

        public bool Closing(object sender, FormClosingEventArgs e)
        {
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
