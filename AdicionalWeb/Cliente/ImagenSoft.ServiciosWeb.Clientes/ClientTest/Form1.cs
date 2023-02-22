using System;
using System.Windows.Forms;
using System.Windows.Threading;
using ClientTest.View;
using ImagenSoft.ServiciosWeb.Entidades;

namespace ClientTest
{
    public partial class Form1 : Form
    {
        private Sesion Sesion;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    this.Controls.Add(new FrmCliente() { Dock = DockStyle.Fill });
                }));
        }
    }
}
