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
    public partial class frmEstacionesPorUsuario : Form
    {
        private ListaEstacion estaciones;
        private Dictionary<int, Estacion> estacionesPorUsuario;
        private int idUsuario;

        public frmEstacionesPorUsuario(int idUsuario)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            estaciones = new EstacionPersistencia().ObtenerLista();
            this.idUsuario = idUsuario;
            estacionesPorUsuario = new Dictionary<int, Estacion>();

            ListaEstacion ests = new EstacionPersistencia().EstacionObtenerPorUsuario(this.idUsuario);
            estacionesPorUsuario = ests.ToDictionary(s => s.Id, s => s);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            int indice = 0;

            estaciones.ForEach(s =>
            {
                ListViewItem itm = new ListViewItem(s.Nombre);
                itm.Checked = estacionesPorUsuario.ContainsKey(s.Id);
                itm.SubItems.Add(indice.ToString());

                lvEstaciones.Items.Add(itm);
                indice++;
            });
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Usuario user = new Usuario() { Id = this.idUsuario, };

            foreach (ListViewItem item in lvEstaciones.Items)
            {
                if (item.Checked)
                {
                    user.Estaciones.Add(estaciones[Convert.ToInt32(item.SubItems[1].Text)]);
                }
            }

            new UsuarioPersistencia().UsuarioActualizarEstaciones(user);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
