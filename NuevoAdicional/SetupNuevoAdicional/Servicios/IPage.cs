using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraWizard;

namespace SetupNuevoAdicional
{
    public interface IPage
    {
        BaseWizardPage Parent { get; set; }
        string Name { get; set; }

        void Load();
        bool Closing(object sender, System.Windows.Forms.FormClosingEventArgs e);
        void NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e);
        void PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e);
    }
}
