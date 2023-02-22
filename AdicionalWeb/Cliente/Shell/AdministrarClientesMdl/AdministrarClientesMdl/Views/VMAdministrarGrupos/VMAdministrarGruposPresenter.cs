using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarGruposPresenter : Presenter<IVMAdministrarGrupos>, IVMAdministrarGrupos
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

        private void FnOperacionHost(Action<IVMAdministrarGrupos> fn)
        {
            IVMAdministrarGrupos servicios =
                    WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL].Services.Get<IVMAdministrarGrupos>();

            if (servicios == null)
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.ADMINISTRAR_CLIENTES_MDL));
            }

            fn(servicios);
        }

        public bool UsuariosClienteInsertar(AdministrarUsuariosClientes entidad)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.UsuariosClienteInsertar(entidad);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "insertar"));
                }
            });

            return resultado;
        }

        public bool UsuariosClienteModificar(AdministrarUsuariosClientes entidad)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.UsuariosClienteModificar(entidad);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "modificar"));
                }
            });

            return resultado;
        }

        public bool UsuariosClienteEliminar(FiltroAdministrarUsuariosClientes filtro)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.UsuariosClienteEliminar(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "eliminar"));
                }
            });

            return resultado;
        }

        public bool UsuariosClienteNuevaContrasenia(FiltroAdministrarUsuariosClientes filtro)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.UsuariosClienteNuevaContrasenia(filtro);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "nueva contraseña"));
                }
            });

            return resultado;
        }

        public bool UsuariosClienteInsertarModificar(ListaAdministrarUsuariosClientes lista)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.UsuariosClienteInsertarModificar(lista);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "modificar el listado"));
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
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                }
            });

            return resultado;
        }

        public bool Modificar(AdministrarClientes entidad)
        {
            bool resultado = false;

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    resultado = servicios.Modificar(entidad);
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                }
            });

            return resultado;
        }

        public AdministrarClientes Obtener(FiltroAdministrarClientes filtro)
        {
            AdministrarClientes resultado = null;

            this.FnOperacionHost((servicios) =>
                {
                    try { resultado = servicios.Obtener(filtro); }
                    catch { throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro")); }
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

        public ListaAdministrarUsuariosClientes UsuariosClienteObtenerTodos(FiltroAdministrarUsuariosClientes filtro)
        {
            ListaAdministrarUsuariosClientes resultado = new ListaAdministrarUsuariosClientes();

            this.FnOperacionHost((servicios) =>
            {
                try
                {
                    var aux = servicios.UsuariosClienteObtenerTodos(filtro);
                    if (aux != null && aux.Count > 0)
                    {
                        resultado.Clear();
                        resultado.AddRange(aux);
                    }
                }
                catch
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "obtener todos filtro"));
                }
            });

            return resultado;
        }

        internal void DisparaEvento()
        {
            EventTopic evt = this.WorkItem.EventTopics[ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.EVENT_HANDLER];
            EventArgs evento = new EventArgs();
            evt.Fire(this, evento, this.WorkItem, PublicationScope.Global);
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
    }
}

