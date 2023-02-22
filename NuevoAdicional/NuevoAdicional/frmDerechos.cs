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
    public partial class frmDerechos : Form
    {
        public int IdUsuario { get; set; }
        
        private void MuestraDerechos()
        {
            ListaDerecho pListaDerechos = new DerechoPersistencia().ObtenerListaPorUsuario(IdUsuario);

            foreach (Derecho derecho in pListaDerechos)
            {
                chklbDerechos.SetItemChecked(derecho.Id_Derecho, true);
            }
        }
        
        public frmDerechos(int AIdUsuario)
        {
            InitializeComponent();
            this.IdUsuario = AIdUsuario;
            MuestraDerechos();
        }

        private void tiMarcarTodos_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= chklbDerechos.Items.Count - 1; i++)
            { 
                chklbDerechos.SetItemChecked(i, true);
            }
        }

        private void tiDesmarcarTodos_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= chklbDerechos.Items.Count - 1; i++)
            {
                chklbDerechos.SetItemChecked(i, false);
            }
        }

        private void AplicarDerechos()
        {
            DerechoPersistencia pDerechoPersistencia = new DerechoPersistencia();

            pDerechoPersistencia.EliminarDerechosPorUsuario(IdUsuario);

            foreach (int indice in chklbDerechos.CheckedIndices)
            {
                var pDerecho = new Derecho();
                pDerecho.Id_Derecho = indice;
                pDerecho.Id_Usuario = IdUsuario;
                pDerecho.Nombre = chklbDerechos.Items[indice].ToString();

                pDerechoPersistencia.DerechoInsertar(pDerecho);
            }
        }

        private void tiAplicarDerechos_Click(object sender, EventArgs e)
        {
            AplicarDerechos();

            if (this.IdUsuario == Configuraciones.IdUsuario)
            {
                Configuraciones.InicializarDerechos();
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
