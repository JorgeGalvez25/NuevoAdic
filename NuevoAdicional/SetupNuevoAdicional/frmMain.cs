using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraWizard;

namespace SetupNuevoAdicional
{
    public partial class frmMain : XtraForm
    {
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

            this.SuspendLayout();
            this.Controls.Add(WorkItem.Wizard);
            this.ResumeLayout(false);

            WorkItem.Wizard.SelectedPageChanging += new WizardPageChangingEventHandler(wizard_SelectedPageChanging);
            WorkItem.Wizard.PrevClick += new WizardCommandButtonClickEventHandler(wizard_PrevClick);
            WorkItem.Wizard.NextClick += new WizardCommandButtonClickEventHandler(wizard_NextClick);
            WorkItem.Wizard.CancelClick += new CancelEventHandler(wizard_CancelClick);
            WorkItem.Wizard.FinishClick += new CancelEventHandler(wizard_FinishClick);


            this.FormClosing += new FormClosingEventHandler(frmMain_FormClosing);
        }

        #region Delegados

        delegate void cursor(Cursor valor);

        private void cursorCallBack(Cursor valor)
        {
            if (this.InvokeRequired)
            {
                cursor control = new cursor(cursorCallBack);

                this.Invoke(control, new object[] { valor });
            }
            else
            {
                this.Cursor = valor;
                Application.UseWaitCursor = valor == Cursors.WaitCursor;
            }
        }

        #endregion

        #region Eventos

        void wizard_SelectedPageChanging(object sender, WizardPageChangingEventArgs e)
        {
            try
            {
                cursorCallBack(Cursors.WaitCursor);

                var t = e.Page.Controls.OfType<IPage>();
                if (t.Count() > 0)
                {
                    t.First().Load();
                }
            }
            finally
            {
                cursorCallBack(Cursors.Default);
            }
        }

        void wizard_PrevClick(object sender, WizardCommandButtonClickEventArgs e)
        {
            try
            {
                cursorCallBack(Cursors.WaitCursor);

                var t = e.Page.Controls.OfType<IPage>();
                if (t.Count() > 0)
                {
                    t.First().PrevClick(sender, e);
                }
            }
            finally
            {
                cursorCallBack(Cursors.Default);
            }
        }

        void wizard_NextClick(object sender, WizardCommandButtonClickEventArgs e)
        {
            try
            {
                cursorCallBack(Cursors.WaitCursor);

                var t = e.Page.Controls.OfType<IPage>();
                if (t.Count() > 0)
                {
                    t.First().NextClick(sender, e);
                }
            }
            finally
            {
                cursorCallBack(Cursors.Default);
            }
        }

        void wizard_CancelClick(object sender, CancelEventArgs e)
        {
            Close();
        }

        void wizard_FinishClick(object sender, CancelEventArgs e)
        {
            this.FormClosing -= new FormClosingEventHandler(frmMain_FormClosing);
            this.FormClosing += (s, ev) => WorkItem.Vistas.Find(i => i.GetType() == typeof(viewFinalizar)).Closing(s, ev);
            this.Close();
        }

        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cursorCallBack(Cursors.WaitCursor);

                var t = WorkItem.Wizard.SelectedPage.Controls.OfType<IPage>();
                if (t.Count() > 0)
                {
                    t.First().Closing(sender, e);
                }

                if (!e.Cancel)
                {
                    if (!Utils.MensajeConfirmacion(Constantes.Mensajes.DeseaSalir))
                    {
                        e.Cancel = true;
                    }
                }
            }
            finally
            {
                cursorCallBack(Cursors.Default);
            }
        }

        #endregion
    }
}
