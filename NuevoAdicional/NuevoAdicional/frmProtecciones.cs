using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Adicional.Entidades;
using ServiciosCliente;

namespace NuevoAdicional
{
    public partial class frmProtecciones : Form
    {
        private int idEstacion;
        private Estacion estacion;
        private ListaProteccion protecciones;
        private bool cambios = false;
        private bool guardado = false;
        private ListaBitacora acciones = new ListaBitacora();
        Servicios.Adicional.IServiciosAdicional servicioAdicional;

        public frmProtecciones(Estacion estacion)
        {
            InitializeComponent();

            this.idEstacion = estacion.Id;
            this.estacion = estacion;

            servicioAdicional = Configuraciones.ListaCanalesAdicional[idEstacion];
            try
            {
                protecciones = servicioAdicional.ProteccionObtenerLista(this.idEstacion);

                gcProtecciones.DataSource = protecciones;

                gvProtecciones.Columns["Estacion"].Visible = false;

                gvProtecciones.Columns["Litros"].Width = 60;
                gvProtecciones.Columns["Litros"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gvProtecciones.Columns["Litros"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gvProtecciones.Columns["Litros"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                gvProtecciones.Columns["Litros"].DisplayFormat.FormatString = "00";
                gvProtecciones.Columns["Litros"].OptionsColumn.ReadOnly = true;

                gvProtecciones.Columns["Activa"].Width = 50;
                gvProtecciones.Columns["Activa"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gvProtecciones.Columns["Activa"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gvProtecciones.Columns["Activa"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                gvProtecciones.Columns["Activa"].ColumnEdit = repositoryItemCheckEdit1;

                gcProtecciones.UseEmbeddedNavigator = false;
                gvProtecciones.OptionsCustomization.AllowFilter = false;
                gvProtecciones.OptionsCustomization.AllowSort = false;
                gvProtecciones.OptionsMenu.EnableColumnMenu = false;
                gvProtecciones.OptionsMenu.EnableFooterMenu = false;
                gvProtecciones.OptionsMenu.EnableGroupPanelMenu = false;
                gvProtecciones.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                gvProtecciones.OptionsView.ShowGroupPanel = false;
                gvProtecciones.OptionsView.ShowFooter = false;
                gvProtecciones.OptionsView.ColumnAutoWidth = false;
                gvProtecciones.OptionsCustomization.AllowColumnMoving = false;
                gvProtecciones.OptionsCustomization.AllowColumnResizing = false;
                gvProtecciones.OptionsCustomization.AllowRowSizing = false;
            }
            catch (Exception)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
            }

            llenarLista();
        }

        private void itGuardar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool adicionalCorrecto = false;
            gvProtecciones.PostEditor();

            try
            {
                string comandostr = string.Empty;
                foreach (Proteccion prot in protecciones)
                {
                    if (prot.Activa == "Si")
                        comandostr += prot.Litros.ToString() + ";";
                }
                if (comandostr != string.Empty)
                    comandostr = comandostr.Substring(0, comandostr.Length - 1).Trim();

                ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[idEstacion];

                pServiciosCliente.AplicarProtecciones(comandostr);

                protecciones.Insert(0, new Proteccion() { Estacion = idEstacion, Litros = 0 });
                servicioAdicional.ProteccionInsertarActualizar(protecciones);
                servicioAdicional.ConfiguracionActivarProtecciones(idEstacion, estacion.ProteccionesActivas);
                servicioAdicional.ConfiguracionActualizarUltimoMovimiento(DateTime.Now);
                adicionalCorrecto = true;

                if (estacion.ProteccionesActivas)
                {
                    string mensaje = string.Empty;
                    //Configuraciones.ListaCanales[idEstacion].ProteccionInsertar((from p in protecciones
                    //                                                             select p.Litros).ToList(), out mensaje);

                    if (!string.IsNullOrEmpty(mensaje)) throw new Exception(mensaje);
                }
                else
                {
                    //Configuraciones.ListaCanales[idEstacion].ProteccionEliminar();
                }

                acciones.Add(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Guardando Protecciones" });
                guardado = true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("No ha sido posible guardar las protecciones\nMensaje original: {0}\nPerderá los cambios, ¿Desea salir?", ex.Message),
                    "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                {
                    return;
                }
                cambios = false;

                if (!adicionalCorrecto)
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                }
                else
                {
                    Configuraciones.AbrirCanalCliente(idEstacion);
                }
            }

            Cursor.Current = Cursors.Default;

            this.Close();
        }

        private void llenarLista()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                gvProtecciones.RefreshData();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAddLitros forma = new frmAddLitros();

            if (forma.ShowDialog() == DialogResult.OK)
            {
                if (protecciones.FindIndex(p => { return p.Litros == forma.Litros; }) < 0)
                {
                    protecciones.Add(new Proteccion() { Estacion = idEstacion, Litros = forma.Litros, Activa = "Si" });

                    llenarLista();
                    cambios = true;

                    acciones.Add(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Agregar Proteccion por " + forma.Litros.ToString() + " litros" });
                }
            }

            forma.Dispose();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gvProtecciones.GetFocusedRow() != null)
            {
                if (MessageBox.Show(string.Format("¿Realmente desea eliminar la protección por {0} litros?", (gvProtecciones.GetFocusedRow() as Proteccion).Litros.ToString()),
                                                  "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int litros = (gvProtecciones.GetFocusedRow() as Proteccion).Litros;
                    int indice = protecciones.FindIndex(p => { return p.Litros == litros; });

                    protecciones.RemoveAt(indice);
                    llenarLista();
                    cambios = true;

                    acciones.Add(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Eliminar Proteccion por " + litros.ToString() + " litros" });
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una protección a eliminar.", "Selección", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void frmProtecciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!guardado)
            {
                if (cambios)
                {
                    if (MessageBox.Show("Perderá los cambios, ¿Desea Salir?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                foreach (Bitacora item in acciones)
                {
                    try
                    {
                        servicioAdicional.BitacoraInsertar(item);
                    }
                    catch (Exception)
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                    }
                }
            }
        }
    }
}