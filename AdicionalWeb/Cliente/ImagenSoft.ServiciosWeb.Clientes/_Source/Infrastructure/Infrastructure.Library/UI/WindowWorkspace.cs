//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The WindowWorkspace class provides an extension to the builtin CAB WindowWorkspace 
// by allowing better customization of the form: Accept and Cancel button and FormBorderStyle
// 
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-010-How_to_Create_Smart_Client_Solutions.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EstandarCliente.Infrastructure.Library.UI
{
    public class WindowWorkspace : Microsoft.Practices.CompositeUI.WinForms.WindowWorkspace
    {
        IWin32Window _owner;

        public WindowWorkspace()
        {
        }

        public WindowWorkspace(IWin32Window owner)
            : base(owner)
        {
            _owner = owner;
        }

        protected override void OnShow(Control smartPart, Microsoft.Practices.CompositeUI.WinForms.WindowSmartPartInfo smartPartInfo)
        {
            try
            {
                GetOrCreateForm(smartPart);
                OnApplySmartPartInfo(smartPart, smartPartInfo);
                base.OnShow(smartPart, smartPartInfo);
                try
                {
                    Form form = Windows[smartPart];

                    if (form.StartPosition == FormStartPosition.CenterScreen)
                    {
                        int x = (Screen.PrimaryScreen.Bounds.Width - form.Width) / 2;
                        int y = (Screen.PrimaryScreen.Bounds.Height - form.Height) / 2;
                        form.Location = new Point(x, y);
                    }
                }
                catch
                {


                }
            }
            catch //(Exception e)
            {

            }
        }

        protected override void OnClose(Control smartPart)
        {
            Form host = Windows[smartPart];
            //host.Hide();
            base.OnClose(smartPart);
        }

        protected override void OnApplySmartPartInfo(Control smartPart, Microsoft.Practices.CompositeUI.WinForms.WindowSmartPartInfo smartPartInfo)
        {
            base.OnApplySmartPartInfo(smartPart, smartPartInfo);
            if (smartPartInfo is WindowSmartPartInfo)
            {
                WindowSmartPartInfo spi = (WindowSmartPartInfo)smartPartInfo;
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.AcceptButton))
                    Windows[smartPart].AcceptButton = (IButtonControl)spi.Keys[WindowWorkspaceSetting.AcceptButton];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.CancelButton))
                    Windows[smartPart].CancelButton = (IButtonControl)spi.Keys[WindowWorkspaceSetting.CancelButton];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.FormBorderStyle))
                    Windows[smartPart].FormBorderStyle = (FormBorderStyle)spi.Keys[WindowWorkspaceSetting.FormBorderStyle];
                if (spi.Keys.ContainsKey(WindowWorkspaceSetting.FormStartPosition))
                    Windows[smartPart].StartPosition = FormStartPosition.CenterScreen;
            }
        }

        protected new Form GetOrCreateForm(Control control)
        {
            bool resizeRequired = !Windows.ContainsKey(control);
            Form form = base.GetOrCreateForm(control);
            form.ShowInTaskbar = (_owner == null);
            if (resizeRequired)
                form.ClientSize = control.Size;
            return form;
        }
    }
}
