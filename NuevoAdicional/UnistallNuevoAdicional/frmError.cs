using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace UnistallNuevoAdicional
{
    public partial class frmError : DevExpress.XtraEditors.XtraForm
    {
        public frmError(Exception ex)
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.TopLevel = true;

            txtError.Properties.ReadOnly = true;

            StringBuilder m = new StringBuilder();
            m.AppendLine("[Exception.Message]");
            m.AppendLine(ex.Message);
            m.AppendLine();
            m.AppendLine("[Exception.StackTrace]");
            m.AppendLine(ex.StackTrace);
            m.AppendLine();
            m.AppendLine("[Exception.Source]");
            m.AppendLine(ex.Source);
            txtError.Text = m.ToString();

            txtError.Select(0, 0);
        }
    }
}