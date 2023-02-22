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
    public partial class viewError : DevExpress.XtraEditors.XtraUserControl, IPage
    {
        public viewError()
        {
            InitializeComponent();

            this.Parent = new DevExpress.XtraWizard.WizardPage();
            this.Parent.Text = Constantes.Titulos.Error;
            this.Parent.AllowNext = false;
            this.Parent.Controls.Add(this);
        }

        #region IPage Members

        public new DevExpress.XtraWizard.BaseWizardPage Parent { get; set; }

        public new void Load()
        {
            Exception ex = WorkItem.Objetos<Exception>.Get("error");
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[Exception.Message]");
            sb.AppendLine(ex.Message);
            sb.AppendLine();
            sb.AppendLine("[Exception.StackTrace]");
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine();
            sb.AppendLine("[Exception.Source]");
            sb.AppendLine(ex.Source);
            txtError.Text = sb.ToString();
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
            this.Parent.Owner.SelectedPage = WorkItem.Vistas.Find(p => p.GetType() == typeof(viewConfirmacion)).Parent;
            e.Handled = true;
        }

        #endregion
    }
}
