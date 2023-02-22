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
using System.Text.RegularExpressions;

namespace NuevoAdicional
{
    public partial class frmBitacora : Form
    {
        private int idEstacion;
        private Regex rgxFiltro = new Regex(@"^\w+\s+\([\d]{10}\)$", RegexOptions.Compiled);

        private void LlenaLista()
        {
            try
            {
                Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[idEstacion];
                ListaBitacora pListaBitacora = null;

                try
                {
                    pListaBitacora = servicioAdicional.BitacoraObtenerListaPorFecha(deFechaInicial.Value.Date, deFechaFinal.Value.Date);
                }
                catch (Exception)
                {
                    MessageBox.Show("Se produjo un error al obtener la bitácora. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                }

                listView1.BeginUpdate();
                listView1.Items.Clear();

                if (pListaBitacora != null && pListaBitacora.Count > 0)
                {
                    pListaBitacora = this.Filtro(pListaBitacora);
                    foreach (var bitacora in pListaBitacora)
                    {
                        ListViewItem pItem = new ListViewItem();
                        pItem.Text = string.Empty;

                        pItem.SubItems.Add(bitacora.Fecha.ToString("dd/MM/yyyy"));
                        pItem.SubItems.Add(bitacora.Hora.ToString());
                        pItem.SubItems.Add(bitacora.Id_usuario);
                        pItem.SubItems.Add(bitacora.Suceso);

                        if (bitacora.Suceso.Contains("Bajar"))
                        {
                            pItem.BackColor = Color.Red;
                            pItem.ForeColor = Color.White;
                        }
                        else if (bitacora.Suceso.Contains("Subir"))
                        {
                            pItem.BackColor = Color.Green;
                            pItem.ForeColor = Color.White;
                        }
                        else if (bitacora.Suceso.Contains("Sincroniza"))
                        {
                            pItem.BackColor = Color.Yellow;
                        }
                        else
                        {
                            pItem.BackColor = Color.White;
                        }

                        listView1.Items.Add(pItem);
                    }
                }
            }
            finally
            {
                listView1.EndUpdate();
            }
        }

        public frmBitacora(string nombreEstacion, int idEstacion)
        {
            this.InitializeComponent();

            this.Text = string.Concat(this.Text, " Estación ", nombreEstacion);
            this.idEstacion = idEstacion;

            this.deFechaFinal.Value = deFechaInicial.Value = DateTime.Today.Date;
            this.cmbFiltro.SelectedIndex = 0;

            this.deFechaFinal.ValueChanged += this.dateTimePicker1_ValueChanged;
            this.deFechaInicial.ValueChanged += this.dateTimePicker1_ValueChanged;
            this.cmbFiltro.SelectedIndexChanged += this.cmbFiltro_SelectedIndexChanged;

            LlenaLista();
        }

        internal ListaBitacora Filtro(ListaBitacora lst)
        {
            string value = this.cmbFiltro.SelectedItem.ToString();
            Bitacora[] aux = new Bitacora[0];

            switch (value)
            {
                case "Moviles":
                    aux = lst.Where(p => rgxFiltro.IsMatch(p.Id_usuario)).ToArray();
                    break;
                case "Estandares":
                    aux = lst.Where(p => !rgxFiltro.IsMatch(p.Id_usuario)).ToArray();
                    break;
                case "Todos":
                default:
                    return lst;
            }
            ListaBitacora resultado = new ListaBitacora();
            resultado.AddRange(aux);
            return resultado;
        }

        internal void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LlenaLista();
            Cursor.Current = Cursors.Default;
        }

        internal void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LlenaLista();
            Cursor.Current = Cursors.Default;
        }
    }
}
