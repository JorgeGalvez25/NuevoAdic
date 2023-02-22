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
    public partial class frmVariables : Form
    {
        public Usuario usuario { get; private set; }

        private DataTable variables;

        public frmVariables(Usuario usuario)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.None;

            this.usuario = usuario;
            this.Text = string.Concat("Variables del usuario ", this.usuario.Nombre);
            this.variables = new DataTable("tblVariables");

            this.variables.Columns.Add("Variable", typeof(string));
            this.variables.Columns.Add("Valor", typeof(string));
            this.variables.Columns["Variable"].ReadOnly = true;

            Array.ForEach(Configuraciones.ListaVariables, item =>
                {
                    DataRow row = this.variables.NewRow();

                    row["Variable"] = item;
                    row["Valor"] = this.usuario.GetValorVariable(item);
                    this.variables.Rows.Add(row);
                });

            gvVariables.DataSource = this.variables;
            gvVariables.Columns["Valor"].Width = 150;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            StringBuilder sbVars = new StringBuilder();

            foreach (DataRow item in this.variables.Rows)
            {
                sbVars.AppendLine(string.Concat(item["Variable"], "=", item["Valor"]));
            }

            this.usuario.Variables = sbVars.ToString();
            new UsuarioPersistencia().UsuarioActualizar(this.usuario);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
