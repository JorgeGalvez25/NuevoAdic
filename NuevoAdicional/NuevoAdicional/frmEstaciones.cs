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
    public partial class frmEstaciones : Form
    {
        private bool cambios;

        private void LlenaLista()
        {
            listView1.Items.Clear();

            ListaEstacion pListaEstacion = Configuraciones.Estaciones;

            foreach (var estacion in pListaEstacion)
            {
                ListViewItem item = new ListViewItem();
                item.Text = estacion.Id.ToString();
                item.SubItems.Add(estacion.Nombre);
                item.SubItems.Add(estacion.IpServicios);
                item.SubItems.Add(estacion.TipoDispensario.ToString());

                listView1.Items.Add(item);
            }
            toolStrip1.Focus();
            listView1_SelectedIndexChanged(null, EventArgs.Empty);
        }
        
        public frmEstaciones()
        {
            InitializeComponent();
            LlenaLista();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (cambios)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private int ObtenerIdSeleccionado()
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                return Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            }

            MessageBox.Show("Seleccione una estación primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return -1;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tiEliminarEstacion.Enabled = listView1.SelectedItems.Count > 0 && Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32(tiEliminarEstacion.Tag));
            tiModificarEstacion.Enabled = listView1.SelectedItems.Count > 0 && Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32(tiModificarEstacion.Tag));
        }

        private void tiNuevaEstacion_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var estacion = new Estacion();

            var pForma = new frmEstacionMto(estacion);
            pForma.Text = "Nueva estación";

            if (pForma.ShowDialog() == DialogResult.OK)
            {
                new EstacionPersistencia().EstacionInsertar(estacion);

                Configuraciones.ActualizarEstaciones();
                cambios = true;
                LlenaLista();
            }
        }

        private void tiModificarEstacion_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pIdEstacion = ObtenerIdSeleccionado();

            if (pIdEstacion > 0)
            {
                Estacion pEstacion = Configuraciones.Estaciones.Find(p => { return p.Id == pIdEstacion; });

                frmEstacionMto pForma = new frmEstacionMto(pEstacion);
                pForma.Text = "Modificar estación";

                if (pForma.ShowDialog() == DialogResult.OK)
                {
                    Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pIdEstacion];

                    new EstacionPersistencia().EstacionActualizar(pEstacion);

                    Configuraciones.AbrirCanalCliente(pEstacion.Id);
                    Configuraciones.AbrirCanalAdicional(pEstacion.Id);
                    cambios = true;
                    LlenaLista();
                }
            }
        }

        private void tiEliminarEstacion_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pId = ObtenerIdSeleccionado();

            if (pId > 0)
            {
                EstacionPersistencia estPers = new EstacionPersistencia();
                List<string> usuariosAsignados = estPers.ObtenerUsuariosAsignadosEstacion(pId);
                if (usuariosAsignados == null || usuariosAsignados.Count == 0)
                {
                    if (MessageBox.Show("¿Eliminar la estación seleccionada?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        estPers.EstacionEliminar(pId);

                        Configuraciones.EliminarEstacion(pId);
                        cambios = true;
                        LlenaLista();
                    }
                }
                else
                {
                    string cadenaUsuarios = usuariosAsignados.Aggregate((f, s) => string.Concat(f, ", ", s));
                    MessageBox.Show("La estación que intenta eliminar está asignada a los siguientes usuarios:\n"+cadenaUsuarios+"\nDebe quitar la asignación antes de eliminarla.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}
