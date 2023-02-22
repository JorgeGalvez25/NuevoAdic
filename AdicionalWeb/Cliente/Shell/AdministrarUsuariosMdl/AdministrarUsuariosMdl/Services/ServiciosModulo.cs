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

namespace EstandarCliente.AdministrarUsuariosMdl.Services
{
    public class ServiciosModulo : IModulos<AdministrarUsuarios>,
                                   IVLAdministrarUsuarios,
                                   IVMAdministrarUsuarios,
                                   IModuloServiciosMtn<AdministrarUsuarios>
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

        #region AdministrarUsuarios Members

        public DateTime ObtenerFechaHoraServidor()
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.ObtenerFechaHoraServidor();
        }

        public int Consecutivo()
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosConsecutivo(this.ObtenerSesion());
        }

        public bool Insertar(AdministrarUsuarios entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosInsertar(this.ObtenerSesion(), entidad) != null;
        }

        public bool Modificar(AdministrarUsuarios entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosModificar(this.ObtenerSesion(), entidad) != null;
        }

        public bool Eliminar(FiltroAdministrarUsuarios filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosEliminar(this.ObtenerSesion(), filtro);
        }

        public AdministrarUsuarios Obtener(FiltroAdministrarUsuarios filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosObtener(this.ObtenerSesion(), filtro);
        }

        public ListaAdministrarUsuarios ObtenerTodos(FiltroAdministrarUsuarios filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarUsuariosObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        #endregion

        #region IModulos<AdministrarUsuarios> Members

        public AdministrarUsuarios Consultar()
        {
            throw new NotImplementedException();
        }

        public void ImprimirVista(List<AdministrarUsuarios> t, string filtro, string invoker)
        {
            throw new NotImplementedException();
        }

        public void MostrarVista(AdministrarUsuarios t)
        {
            try
            {
                SesionModuloWeb sesion = ObtenerSesion();

                VLAdministrarUsuarios vista = (VLAdministrarUsuarios)_WorkItem.RootWorkItem.SmartParts[ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.VISTA_LISTADO];

                if (vista != null)
                {
                    _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista);
                    return;
                }
                VLAdministrarUsuariosPresenter presenter = new VLAdministrarUsuariosPresenter()
                {
                    WorkItem = _WorkItem
                };

                vista = new VLAdministrarUsuarios(sesion, presenter)
                {
                    Titulo = ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.TITULO_VISTA_LISTADO
                };

                presenter.View = vista;

                _WorkItem.RootWorkItem.SmartParts.Add(vista, ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.VISTA_LISTADO);

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

        #region IModuloServiciosMtn<AdministrarUsuarios> Members

        public void EjecutarServiciosMantenimientoPaleta(AdministrarUsuarios t, string modo, string invoker)
        {
            string idVista = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.TITULO_VISTA_MANTENIMIENTO, modo);

            try
            {
                SesionModuloWeb sesion = this.ObtenerSesion();

                VMAdministrarUsuarios vista = (VMAdministrarUsuarios)_WorkItem.RootWorkItem.SmartParts[idVista];

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

                VMAdministrarUsuariosPresenter presenter = new VMAdministrarUsuariosPresenter()
                {
                    WorkItem = _WorkItem
                };

                WindowSmartPartInfo info = new WindowSmartPartInfo()
                {
                    Title = idVista
                };

                vista = new VMAdministrarUsuarios(t, sesion, presenter, modo)
                {
                    Invoker = invoker,
                    Titulo = info.Title
                };

                presenter.View = vista;

                _WorkItem.RootWorkItem.SmartParts.Add(vista, idVista);
                _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista, info);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);

                if (_WorkItem.RootWorkItem.SmartParts.Contains(idVista))
                {
                    var item = (VMAdministrarUsuarios)_WorkItem.RootWorkItem.SmartParts.Get(idVista);
                    item.BotonCerrarClick();
                    item.Dispose();

                    _WorkItem.RootWorkItem.SmartParts.Remove(item);
                }
            }
        }

        #endregion
    }
}
