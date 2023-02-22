using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Persistencia;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public partial class frmLogin : Form
    {
        private void LlenaUsuarios()
        {
            ListaUsuario pListaUsuarios = new UsuarioPersistencia().ObtenerListaActivos();
                //Configuraciones.servicioAdicional.ObtenerUsuariosActivos();//new UsuarioPersistencia().ObtenerListaActivos();

            foreach (Usuario usuario in pListaUsuarios)
            {
                txtUsuario.Items.Add(usuario);
            }

            if (pListaUsuarios.Count > 0)
            {
                txtUsuario.SelectedIndex = 0;
            }
        }
        
        public frmLogin()
        {
            InitializeComponent();
            // Revisar la columna variables
            if (new UsuarioPersistencia().RevisarAgregarColumnaVariables())
            {
                LlenaUsuarios();
                if (ConfigurationManager.AppSettings["InicioAuto"] == "Si")
                {
                    Configuraciones.IdUsuario = ((Usuario)txtUsuario.Items[0]).Id;
                    Configuraciones.NombreUsuario = ((Usuario)txtUsuario.Items[0]).Nombre;

                    Configuraciones.InicializarDerechos(((Usuario)txtUsuario.Items[0]).Variables);
                }
            }
            else
            {
                MessageBox.Show("La base de datos no pudo ser actualizada con las variables de usuario.\nFavor de comunicarse con su encargado de sistemas.", 
                                "Error de Actualización",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                Application.Exit();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Usuario pUsuario = txtUsuario.SelectedItem as Usuario;

            if (pUsuario.Clave == txtContraseña.Text)
            {
                Cursor.Current = Cursors.WaitCursor;

                Configuraciones.IdUsuario     = pUsuario.Id;
                Configuraciones.NombreUsuario = pUsuario.Nombre;

                Configuraciones.InicializarDerechos(pUsuario.Variables);

                Cursor.Current = Cursors.Default;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.SelectAll();
                txtContraseña.Focus();
            }
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            txtContraseña.Focus();
        }

        private void txtUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtContraseña.Focus();
            btnChPass.Enabled = txtUsuario.Text.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }

        private void btnChPass_Click(object sender, EventArgs e)
        {
            Usuario pUsuario = txtUsuario.SelectedItem as Usuario;

            if (pUsuario.Clave == txtContraseña.Text)
            {
                frmUsuarioMto forma = new frmUsuarioMto(pUsuario, true);
                forma.Text = "Modificar contraseña " + pUsuario.Nombre;

                if (forma.ShowDialog() == DialogResult.OK)
                {
                    new UsuarioPersistencia().UsuarioActualizar(forma._Usuario);
                    pUsuario.Clave = forma._Usuario.Clave;
                }
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.SelectAll();
                txtContraseña.Focus();
            }
        }
    }
}
