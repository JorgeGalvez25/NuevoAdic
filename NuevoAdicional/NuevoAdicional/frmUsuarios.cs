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
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
            LlenaLista();
        }

        private void LlenaLista()
        {
            listView1.Items.Clear();

            ListaUsuario pListaUsuarios = new UsuarioPersistencia().ObtenerLista();

            foreach (var pUsuario in pListaUsuarios)
            {
                ListViewItem pItem = new ListViewItem();
                pItem.Text = pUsuario.Id.ToString();
                pItem.SubItems.Add(pUsuario.Nombre);
                pItem.SubItems.Add(pUsuario.Activo);
                pItem.SubItems.Add(pUsuario.CadenaEstaciones());

                listView1.Items.Add(pItem);
            }
        }

        private int ObtenerClaveSeleccionada()
        {
            if (listView1.FocusedItem == null)//listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un usuario primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            else
            {
                return int.Parse(listView1.FocusedItem.Text);//listView1.SelectedItems[0].Text);
            }
        }

        private void tiNuevoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var pUsuario = new Usuario();
                var pForma = new frmUsuarioMto(pUsuario, false);
                pForma.Text = "Nuevo usuario";

                if (pForma.ShowDialog() == DialogResult.OK)
                {
                    UsuarioPersistencia pUsuarioPersistencia = new UsuarioPersistencia();
                    pUsuarioPersistencia.UsuarioInsertar(pForma._Usuario);
                    LlenaLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itModificarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int pIdUsuario = ObtenerClaveSeleccionada();

                if (pIdUsuario >= 0)
                {
                    UsuarioPersistencia pUsuarioPersistencia = new UsuarioPersistencia();
                    Usuario pUsuario = pUsuarioPersistencia.UsuarioObtener(pIdUsuario);
                    frmUsuarioMto pForma = new frmUsuarioMto(pUsuario, true);
                    pForma.Text = "Modificar contraseña " + listView1.FocusedItem.SubItems[1].Text;

                    if (pForma.ShowDialog() == DialogResult.OK)
                    {
                        pUsuarioPersistencia.UsuarioActualizar(pForma._Usuario);
                        LlenaLista();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itDesactivarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int pIdUsuario = ObtenerClaveSeleccionada();

                if (pIdUsuario >= 0)
                {
                    UsuarioPersistencia pUsuarioPersistencia = new UsuarioPersistencia();
                    Usuario pUsuario = pUsuarioPersistencia.UsuarioObtener(pIdUsuario);

                    if (pUsuario.Activo == "No")
                    {
                        MessageBox.Show("Usuario ya esta inactivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MessageBox.Show("¿Desactivar usuario?", "usuarios", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    pUsuario.Activo = "No";
                    pUsuarioPersistencia.UsuarioActualizar(pUsuario);
                    LlenaLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itActivarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int pIdUsuario = ObtenerClaveSeleccionada();

                if (pIdUsuario >= 0)
                {
                    UsuarioPersistencia pUsuarioPersistencia = new UsuarioPersistencia();
                    Usuario pUsuario = pUsuarioPersistencia.UsuarioObtener(pIdUsuario);

                    if (pUsuario.Activo == "Si")
                    {
                        MessageBox.Show("Usuario ya esta activo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MessageBox.Show("¿Activar usuario?", "usuarios", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    pUsuario.Activo = "Si";
                    pUsuarioPersistencia.UsuarioActualizar(pUsuario);
                    LlenaLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tiDerechos_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int pIdUsuario = ObtenerClaveSeleccionada();

                if (pIdUsuario >= 0)
                {
                    frmDerechos pForma = new frmDerechos(pIdUsuario);
                    pForma.Text = string.Concat("Derechos usuario ", listView1.FocusedItem.SubItems[1].Text);//listView1.SelectedItems[0].SubItems[1].Text;
                    pForma.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tiEliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int pIdUsuario = ObtenerClaveSeleccionada();

                if (pIdUsuario >= 0)
                {
                    if (MessageBox.Show("¿Eliminar el usuario seleccionado?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        new UsuarioPersistencia().UsuarioEliminar(pIdUsuario);
                        LlenaLista();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itEstaciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(12))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idUsuario = ObtenerClaveSeleccionada();

                if (idUsuario >= 0)
                {
                    frmEstacionesPorUsuario forma = new frmEstacionesPorUsuario(idUsuario);
                    forma.Text = string.Concat("Estaciones Por Usuario: ", listView1.FocusedItem.SubItems[1].Text);
                    if (forma.ShowDialog() == DialogResult.OK)
                    {
                        LlenaLista();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void itVariables_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Configuraciones.ListaDerechos.ContainsKey(12))
                {
                    MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idUsuario = ObtenerClaveSeleccionada();

                if (idUsuario >= 0)
                {
                    Usuario usuario = new UsuarioPersistencia().UsuarioObtener(idUsuario);
                    frmVariables forma = new frmVariables(usuario);

                    if (forma.ShowDialog() == DialogResult.OK)
                    {
                        // Guardar las variables...
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
