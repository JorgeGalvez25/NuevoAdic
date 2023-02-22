using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public partial class frmMoviles : Form
    {
        int estacion = 0;

        public frmMoviles(int estacion)
        {
            InitializeComponent();

            this.estacion = estacion;
            this.llenaLista();
        }

        private void llenaLista()
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            listView1.Items.Clear();

            ListaMoviles moviles = null;

            try
            {
                moviles = servicioAdicional.MovilesObtenerTodos();

                foreach (Moviles item in moviles)
                {
                    ListViewItem lvItem = new ListViewItem();

                    lvItem.Text = item.Telefono;
                    lvItem.SubItems.Add(item.Responsable);
                    lvItem.SubItems.Add(item.Activo.Equals("S") ? "Si" : "No");
                    lvItem.SubItems.Add(item.Permisos.SubirBajar ? "Si" : "No");

                    listView1.Items.Add(lvItem);
                }

                listView1_ItemSelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
            }
        }

        private string obtenerTelefonoSeleccionado()
        {
            if (listView1.FocusedItem != null)
            {
                return listView1.FocusedItem.Text;
            }
            else
            {
                MessageBox.Show("Debe Seleccionar un Registro Primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return string.Empty;
            }
        }

        private void tiNuevoUsuario_Click(object sender, EventArgs e)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            try
            {
                Moviles movil = new Moviles();
                frmMovilesMto formaMto = new frmMovilesMto(movil, false);

                if (formaMto.ShowDialog() == DialogResult.OK)
                {
                    movil = formaMto.Movil;
                    Moviles movilBusqueda = servicioAdicional.MovilesObtener(new FiltroMoviles() { Telefono = movil.Telefono });

                    if (movilBusqueda == null)
                    {
                        servicioAdicional.MovilesInsertar(movil);
                        this.llenaLista();
                    }
                    else
                    {
                        MessageBox.Show("Ya existe un movil registrado con el número " + movil.Telefono + " con el responsable " + movil.Responsable, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void itDesactivarUsuario_Click(object sender, EventArgs e)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            try
            {
                string numero = obtenerTelefonoSeleccionado();

                if (string.IsNullOrEmpty(numero))
                    return;

                Moviles movil = servicioAdicional.MovilesObtener(new FiltroMoviles() { Telefono = numero, Responsable = listView1.FocusedItem.SubItems[1].Text });
                movil.Activo = "N";

                servicioAdicional.MovilesActualizar(movil);
                this.llenaLista();
            }
            catch (Exception ex)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void itActivarUsuario_Click(object sender, EventArgs e)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            try
            {
                string numero = obtenerTelefonoSeleccionado();

                if (string.IsNullOrEmpty(numero))
                    return;

                Moviles movil = servicioAdicional.MovilesObtener(new FiltroMoviles() { Telefono = numero, Responsable = listView1.FocusedItem.SubItems[1].Text });
                movil.Activo = "S";

                servicioAdicional.MovilesActualizar(movil);
                this.llenaLista();
            }
            catch (Exception ex)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tiEliminarUsuario_Click(object sender, EventArgs e)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            try
            {
                string numero = obtenerTelefonoSeleccionado();

                servicioAdicional.MovilesEliminar(numero);
                this.llenaLista();
            }
            catch (Exception ex)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void itPermisoSubirBajar_Click(object sender, EventArgs e)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];

            try
            {
                string numero = obtenerTelefonoSeleccionado();

                if (string.IsNullOrEmpty(numero))
                    return;

                bool permiso = false;
                switch (listView1.FocusedItem.SubItems[3].Text.ToLower())
                {
                    case "si":
                        permiso = false;
                        break;
                    case "no":
                        permiso = true;
                        break;
                }

                Moviles movil = servicioAdicional.MovilesObtener(new FiltroMoviles() { Telefono = numero, Responsable = listView1.FocusedItem.SubItems[1].Text });
                movil.Permisos.SubirBajar = permiso;

                servicioAdicional.MovilesActualizar(movil);
                this.llenaLista();
            }
            catch (Exception ex)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                if (listView1.FocusedItem.SubItems[2].Text.Equals("Si"))
                {
                    itDesactivarUsuario.Enabled = true;
                    itActivarUsuario.Enabled = false;
                }
                else
                {
                    itDesactivarUsuario.Enabled = false;
                    itActivarUsuario.Enabled = true;
                }
                itPermisoSubirBajar.Enabled = true;
                tiEliminarUsuario.Enabled = true;
            }
            else
            {
                itPermisoSubirBajar.Enabled = false;
                itDesactivarUsuario.Enabled = false;
                itActivarUsuario.Enabled = false;
                tiEliminarUsuario.Enabled = false;
            }
        }
    }
}
