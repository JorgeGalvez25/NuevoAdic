using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.Interfaces;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VLAdministrarClientesPresenter : Presenter<IVLAdministrarClientes>,
                                                          IVLAdministrarClientes
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

        public void InicializarLookUp(LookUpEdit t)
        {
            LookUpColumnInfo columna = new LookUpColumnInfo();
            columna.FieldName = "Display";
            columna.Caption = "Display";
            t.Properties.Columns.Add(columna);

            columna = new LookUpColumnInfo();
            columna.FieldName = "Value";
            columna.Caption = "Value";
            columna.Visible = false;
            t.Properties.Columns.Add(columna);

            t.Properties.DisplayMember = "Display";
            t.Properties.ValueMember = "Value";
            t.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            t.Properties.PopupSizeable = false;
            t.Properties.ShowFooter = false;
            t.Properties.ShowHeader = false;
        }

        #region IVLAdministrarClientes Members

        private void FnOperacionHost(Action<IVLAdministrarClientes> fn)
        {
            IVLAdministrarClientes servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL].Services.Get<IVLAdministrarClientes>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL));
            }

            fn(servicios);
        }

        public int Consecutivo()
        {
            int resultado = 0;

            this.FnOperacionHost((servicios) =>
                {
                    try
                    {
                        resultado = servicios.Consecutivo();
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "consecutivo"));
                    }
                });

            return resultado;
        }

        public bool Insertar(AdministrarClientes entidad)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
                {
                    try
                    {
                        resultado = servicios.Insertar(entidad);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "insertar"));
                    }
                });

            return resultado;
        }

        public bool Eliminar(FiltroAdministrarClientes filtro)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
                {
                    try
                    {
                        resultado = servicios.Eliminar(filtro);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "eliminar"));
                    }
                });

            return resultado;
        }

        public ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();

            this.FnOperacionHost((servicios) =>
                {
                    try
                    {
                        resultado = servicios.ObtenerTodosFiltro(filtro);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                    }
                });

            return resultado;
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.ObtenerDistribuidores(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener distribuidores"));
                }
            });

            return resultado;
        }

        #endregion

        public void EjecutarServiciosMantenimientoPaleta(AdministrarClientes t, string modo, string invoker)
        {
            IModuloServiciosMtn<AdministrarClientes> servicio =
                WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL].Services.Get<IModuloServiciosMtn<AdministrarClientes>>();

            if (servicio == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL));
            }

            try
            {
                servicio.EjecutarServiciosMantenimientoPaleta(t, modo, invoker);
            }
            catch
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, modo == ConstantesModulo.OPCIONES.PROPIEDADES ? "mostrar propiedades" : modo.ToLower()));
            }
        }
    }
}

