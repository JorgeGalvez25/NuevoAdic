using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Configurador
{
    public partial class Form1 : Form
    {
        Configuration cfg;

        public Form1(string[] args)
        {
            InitializeComponent();

            if (args != null && args.Length > 0)
            {
                this.Text = string.Concat("Configurador ", args[0]);
                if (args[0].Equals("HOST", StringComparison.OrdinalIgnoreCase))
                {
                    configurarHost(args[1]);
                }
                else if (args[0].Equals("CLIENTE", StringComparison.OrdinalIgnoreCase))
                {
                    configurarCliente(args[1]);
                }
            }
            else
            {
                MessageBox.Show("Se debe especificar que módulo del adicional se requiere configurar", "?", MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        private void configurarCliente(string ruta)
        {
            this.pnlHost.Visible = false;
            this.pnlCliente.Dock = DockStyle.Fill;
        }

        private void configurarHost(string ruta)
        {
            this.pnlCliente.Visible = false;
            this.pnlHost.Dock = DockStyle.Fill;

            cfg = ConfigurationManager.OpenExeConfiguration(ruta);
            decimal horasSincro = 0;
            string rutaBDConsola = cfg.ConnectionStrings.ConnectionStrings["GasConsola"].ConnectionString;
            string rutaAdicional = cfg.ConnectionStrings.ConnectionStrings["Adicional"].ConnectionString;
            string rutaAjustador = cfg.ConnectionStrings.ConnectionStrings["Ajusta"].ConnectionString;

            rutaBDConsola = rutaBDConsola.Substring(rutaBDConsola.IndexOf("Database=") + 9);
            rutaBDConsola = rutaBDConsola.Substring(0, rutaBDConsola.IndexOf(";DataSource="));

            rutaAdicional = rutaAdicional.Substring(rutaAdicional.IndexOf("Database=") + 9);
            rutaAdicional = rutaAdicional.Substring(0, rutaAdicional.IndexOf(";DataSource="));

            rutaAjustador = rutaAjustador.Substring(rutaAjustador.IndexOf("Database=") + 9);
            rutaAjustador = rutaAjustador.Substring(0, rutaAjustador.IndexOf(";DataSource="));

            txtBDConsola.Text = rutaBDConsola;
            txtAdicional.Text = rutaAdicional;
            txtAjustador.Text = rutaAjustador;
            string sincro = cfg.AppSettings.Settings["horas Sicronización"].Value;
            decimal.TryParse(sincro, out horasSincro);
            numSincroniz.Value = horasSincro;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            cfg.ConnectionStrings.ConnectionStrings["GasConsola"].ConnectionString = string.Concat("User=SYSDBA;Password=masterkey;Database=", txtBDConsola.Text, ";DataSource=localhost; Port=3050;Dialect=1; Charset=NONE;Role=;Connection lifetime=15;Pooling=true; MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;");
            cfg.ConnectionStrings.ConnectionStrings["Adicional"].ConnectionString = string.Concat("User=SYSDBA;Password=masterkey;Database=", txtAdicional.Text, ";DataSource=localhost; Port=3050;Dialect=1; Charset=NONE;Role=;Connection lifetime=15;Pooling=true; MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;");
            cfg.ConnectionStrings.ConnectionStrings["Ajusta"].ConnectionString = string.Concat("User=SYSDBA;Password=masterkey;Database=", txtAjustador.Text, ";DataSource=localhost; Port=3050;Dialect=1; Charset=NONE;Role=;Connection lifetime=15;Pooling=true; MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;");
            cfg.AppSettings.Settings["horas Sicronización"].Value = numSincroniz.Value.ToString();

            cfg.Save(ConfigurationSaveMode.Modified);
        }
    }
}
