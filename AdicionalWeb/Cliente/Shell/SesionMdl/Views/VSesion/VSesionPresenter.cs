using System;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.SesionMdl
{
    public partial class VSesionPresenter : Presenter<IVSesion>,
                                            IVSesion
    {
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
        }

        /// <summary>
        /// Close the view
        /// </summary>
        public void OnCloseView()
        {
            base.CloseView();
        }

        #region LookUps

        internal void luUsuario(LookUpEdit t)
        {
            IVSesion servicios =
                WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IVSesion>();

            if (servicios != null)
            {
                t.Properties.HeaderClickMode = DevExpress.XtraEditors.Controls.HeaderClickMode.AutoSearch;
                t.Properties.Columns.Clear();
                t.Properties.PopupFormMinSize = new System.Drawing.Size(140, 100);

                LookUpColumnInfo columna = new LookUpColumnInfo();
                columna.FieldName = "Clave";
                columna.Caption = "Clave";
                columna.FormatType = FormatType.Custom;
                columna.FormatString = "000";
                columna.Width = 7;
                columna.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                t.Properties.Columns.Add(columna);

                columna = new LookUpColumnInfo();
                columna.FieldName = "Nombre";
                columna.Caption = "Nombre";
                columna.FormatType = FormatType.Custom;
                t.Properties.Columns.Add(columna);

                t.Properties.DisplayMember = "Nombre";
                t.Properties.ValueMember = "Clave";

                ListaSesiones lista = servicios.ObtenerTodosFiltro(new FiltroSesionModuloWeb());

                if (lista.Count <= 0)
                {
                    t.Properties.NullText = "No existen usuarios.";
                    t.Properties.DataSource = null;
                    t.EditValue = null;
                }
                else
                {
                    lista.RemoveAll(p => p.Usuario.Activo.Equals("No", StringComparison.OrdinalIgnoreCase));
                    t.Properties.DataSource = lista;
                    t.EditValue = lista[0].Clave;
                }
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.SESIONES_MDL));
            }
        }

        #endregion

        #region IServiciosSesionFACELEI

        internal bool CambiarContraseña(SesionModuloWeb sesion)
        {
            VContrasenaPresenter presenter = new VContrasenaPresenter();
            presenter.WorkItem = WorkItem;

            VContrasena vista = new VContrasena(presenter, ref sesion);

            presenter.View = vista;
            vista.Presenter = presenter;

            vista.ShowDialog();
            return vista.Realizado;
        }

        #endregion

        #region IVSesion Members

        public bool Ping()
        {
            try
            {
                IVSesion servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IVSesion>();

                if (servicios == null)
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.SESIONES_MDL));
                }
                else
                {
                    try
                    {
                        return servicios.Ping();
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "validar contraseña"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListaSesiones ObtenerTodosFiltro(FiltroSesionModuloWeb f)
        {
            try
            {
                IVSesion servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IVSesion>();

                if (servicios == null)
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.SESIONES_MDL));
                }
                else
                {
                    try
                    {
                        return servicios.ObtenerTodosFiltro(f);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener la lista de sesiones"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarContrasena(string cUsuario, string cEncriptada)
        {
            try
            {
                IVSesion servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.SESIONES_MDL].Services.Get<IVSesion>();

                if (servicios == null)
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.SESIONES_MDL));
                }
                else
                {
                    try
                    {
                        return servicios.ValidarContrasena(cUsuario, cEncriptada);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "validar contraseña"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

