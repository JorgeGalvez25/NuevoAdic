using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using ImagenSoft.ServiciosWeb.Entidades.Monitor;
using ImagenSoft.ServiciosWeb.Entidades;

namespace ImagenSoft.SeriviciosWeb.Monitor.Views.Monitor
{
    public partial class VLInformacion : UserControl
    {
        public DatosSesion DatosInformacion { get; set; }

        public VLInformacion()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Dock = DockStyle.Fill;
            this.ConfigurarDatosCliente();
        }

        private void ConfigurarDatosCliente()
        {
            if (this.DatosInformacion != null)
            {
                this.txtRazonSocial.Text = this.DatosInformacion.RazonSocial;
                this.txtTipoSesion.Text = this.DatosInformacion.Usuario;

                this.lblFechaConexion.Text = this.DatosInformacion.InicioSesion.ToString("dd/MM/yyyy HH:mm:ss");
                this.lblPaginas.Text = this.DatosInformacion.Link;
                this.lblContacto.Text = this.DatosInformacion.Contacto;
                this.lblTelefonos.Text = this.DatosInformacion.Telefono;
                this.lblTipoSesion.Text = this.DatosInformacion.TipoSesion.ToString();

                if (this.DatosInformacion.Conexion != null)
                {
                    try { this.lblIPs.Text = this.DatosInformacion.Conexion.UriCliente.Authority; }
                    catch { }
                }

                this.btnReenviar.Visible = this.DatosInformacion.TipoSesion == TipoSesion.Cliente;
            }
        }
    }
}
