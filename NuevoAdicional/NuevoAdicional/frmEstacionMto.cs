using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persistencia;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public partial class frmEstacionMto : Form
    {
        private MarcaDispensario marca = MarcaDispensario.Ninguno;

        public Estacion _Estacion { get; set; }

        public frmEstacionMto(Estacion AEstacion)
        {
            InitializeComponent();
            this._Estacion = AEstacion;
            DespliegaEntidad();
        }

        private void DespliegaEntidad()
        {
            this.txtNombre.Text      = this._Estacion.Nombre;
            this.txtIpServicios.Text = this._Estacion.IpServicios;
        }

        private void ObtenerEntidad()
        {
            this._Estacion.Nombre = this.txtNombre.Text;
            this._Estacion.IpServicios = this.txtIpServicios.Text;
            this._Estacion.TipoDispensario = this.marca;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pMensajeError = string.Empty;
            if (DatosCorrectos(out pMensajeError))
            {
                ObtenerEntidad();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(pMensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool DatosCorrectos(out string AMensajeError)
        {
            AMensajeError = string.Empty;
            if (txtNombre.Text.Trim().Length == 0)
            {
                AMensajeError = "Necesita capturar el nombre de la estación.";
                txtNombre.Focus();
                return false;
            }
            else if (txtIpServicios.Text.Trim().Length == 0)
            {
                AMensajeError = "Necesita capturar la dirección IP de los servicios.";
                txtIpServicios.Focus();
                return false;
            }

            return true;
        }

        private void frmEstacionMto_Shown(object sender, EventArgs e)
        {
            txtIpServicios.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string[] partesUri = txtIpServicios.Text.Split(':');

            if (partesUri.Length == 2)
            {
                pnlTryConnect.Visible = true;
                pnlTryConnect.BringToFront();
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    int puerto = int.Parse(partesUri[1]);
                    System.Net.IPAddress[] direcciones = System.Net.Dns.GetHostAddresses(partesUri[0]);
                    System.Net.IPEndPoint ip = new System.Net.IPEndPoint(direcciones[0], puerto);

                    System.Net.IPEndPoint ipep = new System.Net.IPEndPoint(ip.Address.Address, ip.Port);
                    System.Net.Sockets.Socket soc = new System.Net.Sockets.Socket(ip.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                    soc.Connect(ipep);

                    marca = MarcaDispensario.Ninguno;
                    txtNombre.Text = obtenerNombreEstacion(string.Concat("net.tcp://", txtIpServicios.Text, "/ServiciosAdicional"), out marca);
                    txtTipoDispensario.Text = marca.ToString();
                    btnAceptar.Enabled = true;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    MessageBox.Show("No ha sido posible establecer comunicación con ningún servicio en la dirección y puerto especificados.",
                                    "No se encuentra servicio", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (System.ServiceModel.ProtocolException)
                {
                    MessageBox.Show("No se ha encontrado un servicios compatible con el sistema en la dirección y puerto especificados.",
                                    "Servicio no compatible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    MessageBox.Show("No se ha encontrado un servicios compatible con el sistema en la dirección y puerto especificados.",
                                    "Punto de entrada no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (System.ServiceModel.ActionNotSupportedException)
                {
                    MessageBox.Show("No se ha encontrado un servicios compatible con el sistema en la dirección y puerto especificados.",
                                    "Método no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (Exception ex)
                {
                    string nombreArchivo = string.Format("ERROR_Estaciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                    MessageBox.Show("Se ha detectado un error no clasificado al intentar conectar con el servicio." +
                                    " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                    "Para más información verifique el archivo: " + nombreArchivo + ".",
                                    "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);
                }
                finally
                {
                    pnlTryConnect.Visible = false;
                    pnlTryConnect.SendToBack();
                    Application.DoEvents();
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("Debe teclear una dirección y puerto válidos. Ejemplo 192.168.0.1:8055 .",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtIpServicios.Focus();
                txtIpServicios.SelectAll();
            }
        }

        private void txtIpServicios_TextChanged(object sender, EventArgs e)
        {
            btnBuscar.Enabled = !string.IsNullOrEmpty(txtIpServicios.Text);
            txtNombre.Text = string.Empty;
            txtTipoDispensario.Text = string.Empty;
            btnAceptar.Enabled = false;
        }

        private void frmEstacionMto_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            string mensaje = "teclear la una dirección válida con un puerto, ejemplo 192.168.0.1:8055\nclick en el botón buscar para obtener los servicios\nclick en aceptar para guardar.";
            MessageBox.Show(mensaje, "Ayuda", MessageBoxButtons.OK);
        }

        private string obtenerNombreEstacion(string uri, out MarcaDispensario marcaDispensarios)
        {
            string nombre = string.Empty;
            string numMarca = string.Empty;
            string[] partes = null;

            System.ServiceModel.ChannelFactory<Servicios.Adicional.IServiciosAdicional> factory = new System.ServiceModel.ChannelFactory<Servicios.Adicional.IServiciosAdicional>("epAdicional");
            Servicios.Adicional.IServiciosAdicional canal = factory.CreateChannel(new System.ServiceModel.EndpointAddress(uri));

            nombre = canal.ObtenerNombreEstacion();
            partes = nombre.Split('|');
            numMarca = partes.Length >= 2 ? partes[1] : "0";
            nombre = partes.Length >= 1 && !string.IsNullOrEmpty(partes[0]) ? partes[0] : "Sin Nombre";

            marcaDispensarios = (MarcaDispensario)Convert.ToInt32(numMarca);

            ((System.ServiceModel.IClientChannel)canal).Close();
            ((System.ServiceModel.IClientChannel)canal).Dispose();
            factory.Close();

            return nombre;
        }
    }
}
