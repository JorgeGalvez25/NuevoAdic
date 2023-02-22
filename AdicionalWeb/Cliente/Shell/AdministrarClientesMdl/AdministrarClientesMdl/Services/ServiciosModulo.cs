using System;
using System.Collections.Generic;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Interfaces;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;

namespace EstandarCliente.AdministrarClientesMdl.Services
{
    public class ServiciosModulo : IModulos<AdministrarClientes>,
                                   IVLAdministrarClientes,
                                   IVMAdministrarClientes,
                                   IVMAdministrarGrupos,
                                   IModuloServiciosMtn<AdministrarClientes>
    {
        private WorkItem _WorkItem;

        public ServiciosModulo(WorkItem workItem)
        {
            this._WorkItem = workItem;
        }

        private SesionModuloWeb ObtenerSesion()
        {
            SesionModuloWeb sesion = _WorkItem.RootWorkItem.Items[ConstantesModulo.SESION_SISTEMA] as SesionModuloWeb;
            if (sesion == null) { throw new Exception(ListadoMensajes.Error_Sesion_No_Existe); }

            return sesion;
        }

        #region IModulos<AdministrarClientes> Members

        public AdministrarClientes Consultar()
        {
            throw new NotImplementedException();
        }

        public void ImprimirVista(List<AdministrarClientes> t, string filtro, string invoker)
        {
            throw new NotImplementedException();
        }

        public void MostrarVista(AdministrarClientes t)
        {
            try
            {
                SesionModuloWeb sesion = ObtenerSesion();

                VLAdministrarClientes vista = (VLAdministrarClientes)_WorkItem.RootWorkItem.SmartParts[ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO];

                if (vista != null)
                {
                    _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista);
                    return;
                }
                VLAdministrarClientesPresenter presenter = new VLAdministrarClientesPresenter()
                {
                    WorkItem = _WorkItem
                };

                vista = new VLAdministrarClientes(sesion, presenter)
                {
                    Titulo = ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA_LISTADO
                };

                presenter.View = vista;

                _WorkItem.RootWorkItem.SmartParts.Add(vista, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);

                WindowSmartPartInfo info = new WindowSmartPartInfo()
                {
                    Title = vista.Titulo
                };

                _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista, info);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }
        }

        #endregion

        #region AdministrarClientes Members

        public DateTime ObtenerFechaHoraServidor()
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.ObtenerFechaHoraServidor();
        }

        public int Consecutivo()
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesConsecutivo(this.ObtenerSesion());
        }

        public bool Insertar(AdministrarClientes entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesInsertar(this.ObtenerSesion(), entidad) != null;
        }

        public bool Modificar(AdministrarClientes entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesModificar(this.ObtenerSesion(), entidad) != null;
        }

        public bool Eliminar(FiltroAdministrarClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesEliminar(this.ObtenerSesion(), filtro);
        }

        public AdministrarClientes Obtener(FiltroAdministrarClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesObtener(this.ObtenerSesion(), filtro);
        }

        public ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarClientesObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        #endregion

        #region IModuloServiciosMtn<AdministrarClientes> Members

        public void EjecutarServiciosMantenimientoPaleta(AdministrarClientes t, string modo, string invoker)
        {
            string idVista = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA_MANTENIMIENTO, modo);
            try
            {
                SesionModuloWeb sesion = this.ObtenerSesion();
                switch (modo)
                {
                    case ConstantesModulo.OPCIONES.REGISTRAR_GRUPO:
                    case ConstantesModulo.OPCIONES.MODIFICAR_GRUPO:
                        this.CargarVMAdministrarGrupos(sesion, t, modo, invoker, idVista);
                        break;
                    default:
                        this.CargarVMAdministrarClientes(sesion, t, modo, invoker, idVista);
                        break;
                }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);

                if (_WorkItem.RootWorkItem.SmartParts.Contains(idVista))
                {
                    var item = (VMAdministrarClientes)_WorkItem.RootWorkItem.SmartParts.Get(idVista);
                    item.BotonCerrarClick();
                    item.Dispose();

                    _WorkItem.RootWorkItem.SmartParts.Remove(item);
                }
            }
        }

        private void CargarVMAdministrarClientes(SesionModuloWeb sesion, AdministrarClientes t, string modo, string invoker, string idVista)
        {
            VMAdministrarClientes vista = (VMAdministrarClientes)_WorkItem.RootWorkItem.SmartParts[idVista];

            if (vista != null)
            {
                switch (modo)
                {
                    case ConstantesModulo.OPCIONES.PROPIEDADES:
                        vista.BotonCerrarClick();
                        vista.Dispose();

                        _WorkItem.RootWorkItem.SmartParts.Remove(vista);
                        break;
                    default:
                        _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista);
                        Mensaje.MensajeError(string.Format(ListadoMensajes.Error_Accion_Accedida, modo));
                        return;
                }
            }

            WindowSmartPartInfo info = new WindowSmartPartInfo()
            {
                Title = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA_MANTENIMIENTO, modo)
            };

            VMAdministrarClientesPresenter presenter = new VMAdministrarClientesPresenter()
            {
                WorkItem = _WorkItem
            };

            vista = new VMAdministrarClientes(t, sesion, presenter, modo)
            {
                Invoker = invoker,
                Titulo = info.Title
            };
            presenter.View = vista;
            _WorkItem.RootWorkItem.SmartParts.Add(vista, idVista);
            _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista, info);
        }

        private void CargarVMAdministrarGrupos(SesionModuloWeb sesion, AdministrarClientes t, string modo, string invoker, string idVista)
        {
            VMAdministrarGrupos vista = (VMAdministrarGrupos)_WorkItem.RootWorkItem.SmartParts[idVista];

            if (vista != null)
            {
                switch (modo)
                {
                    case ConstantesModulo.OPCIONES.PROPIEDADES:
                        vista.BotonCerrarClick();
                        vista.Dispose();

                        _WorkItem.RootWorkItem.SmartParts.Remove(vista);
                        break;
                    default:
                        _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista);
                        Mensaje.MensajeError(string.Format(ListadoMensajes.Error_Accion_Accedida, modo));
                        return;
                }
            }

            WindowSmartPartInfo info = new WindowSmartPartInfo()
            {
                Title = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA_MANTENIMIENTO, modo)
            };

            VMAdministrarGruposPresenter presenter = new VMAdministrarGruposPresenter()
            {
                WorkItem = _WorkItem
            };

            vista = new VMAdministrarGrupos(t, sesion, presenter, modo)
            {
                Invoker = invoker,
                Titulo = info.Title
            };
            presenter.View = vista;
            _WorkItem.RootWorkItem.SmartParts.Add(vista, idVista);
            _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista, info);
        }

        #endregion

        #region IVMAdministrarGrupos Members

        public ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes UsuariosClienteObtenerTodos(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        public bool UsuariosClienteInsertarModificar(ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes lista)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteInsertarModificar(this.ObtenerSesion(), lista);
        }

        public bool UsuariosClienteInsertar(ImagenSoft.ModuloWeb.Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteInsertar(this.ObtenerSesion(), entidad) != null;
        }

        public bool UsuariosClienteModificar(ImagenSoft.ModuloWeb.Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteModificar(this.ObtenerSesion(), entidad) != null;
        }

        public bool UsuariosClienteEliminar(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteEliminar(this.ObtenerSesion(), filtro);
        }

        public bool UsuariosClienteNuevaContrasenia(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosClienteNuevaContrasenia(this.ObtenerSesion(), filtro);
        }

        #endregion
    }
}
