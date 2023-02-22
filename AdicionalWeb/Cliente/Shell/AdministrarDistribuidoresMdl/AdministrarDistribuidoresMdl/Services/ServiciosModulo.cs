using System;
using System.Collections.Generic;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Interfaces;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;

namespace EstandarCliente.AdministrarDistribuidoresMdl.Services
{
    public class ServiciosModulo : IModulos<AdministrarDistribuidores>,
                                   IVLAdministrarDistribuidores,
                                   IVMAdministrarDistribuidores,
                                   IModuloServiciosMtn<AdministrarDistribuidores>
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

        #region IModulos<AdministrarDistribuidores> Members

        public AdministrarDistribuidores Consultar()
        {
            throw new NotImplementedException();
        }

        public void ImprimirVista(List<AdministrarDistribuidores> t, string filtro, string invoker)
        {
            throw new NotImplementedException();
        }

        public void MostrarVista(AdministrarDistribuidores t)
        {
            try
            {
                SesionModuloWeb sesion = ObtenerSesion();

                VLAdministrarDistribuidores vista = (VLAdministrarDistribuidores)_WorkItem.RootWorkItem.SmartParts[ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.VISTA_LISTADO];

                if (vista != null)
                {
                    _WorkItem.Workspaces[EstandarCliente.Infrastructure.Interface.Constants.WorkspaceNames.RightWorkspace].Show(vista);
                    return;
                }
                VLAdministrarDistribuidoresPresenter presenter = new VLAdministrarDistribuidoresPresenter()
                {
                    WorkItem = _WorkItem
                };

                vista = new VLAdministrarDistribuidores(sesion, presenter)
                {
                    Titulo = ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.TITULO_VISTA_LISTADO
                };

                presenter.View = vista;

                _WorkItem.RootWorkItem.SmartParts.Add(vista, ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.VISTA_LISTADO);

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

        #region IModuloServiciosMtn<AdministrarDistribuidores> Members

        public void EjecutarServiciosMantenimientoPaleta(AdministrarDistribuidores t, string modo, string invoker)
        {
            string idVista = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.TITULO_VISTA_MANTENIMIENTO, modo);

            try
            {
                SesionModuloWeb sesion = this.ObtenerSesion();
                VMAdministrarDistribuidores vista = (VMAdministrarDistribuidores)_WorkItem.RootWorkItem.SmartParts[idVista];

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

                VMAdministrarDistribuidoresPresenter presenter = new VMAdministrarDistribuidoresPresenter()
                {
                    WorkItem = _WorkItem
                };

                WindowSmartPartInfo info = new WindowSmartPartInfo()
                {
                    Title = string.Format(ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.TITULO_VISTA_MANTENIMIENTO, modo)
                };

                vista = new VMAdministrarDistribuidores(t, sesion, presenter, modo)
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
                    var item = (VMAdministrarDistribuidores)_WorkItem.RootWorkItem.SmartParts.Get(idVista);
                    item.BotonCerrarClick();
                    item.Dispose();

                    _WorkItem.RootWorkItem.SmartParts.Remove(item);
                }
            }
        }

        #endregion

        #region IVLAdministrarDistribuidores Members

        public bool Eliminar(FiltroAdministrarDistribuidores filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresEliminar(this.ObtenerSesion(), filtro);
        }

        public ListaAdministrarDistribuidores ObtenerTodosFiltro(FiltroAdministrarDistribuidores filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresObtenerTodosFiltro(this.ObtenerSesion(), filtro);
        }

        #endregion

        #region IVMAdministrarDistribuidores Members

        public int Consecutivo()
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresConsecutivo(this.ObtenerSesion());
        }

        public AdministrarDistribuidores Obtener(FiltroAdministrarDistribuidores filtro)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresObtener(this.ObtenerSesion(), filtro);
        }

        public AdministrarDistribuidores Insertar(AdministrarDistribuidores entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresInsertar(this.ObtenerSesion(), entidad);
        }

        public AdministrarDistribuidores Modificar(AdministrarDistribuidores entidad)
        {
            var servicio = _WorkItem.RootWorkItem.Services.Get<IModuloWebProveedor>();
            return servicio.AdministrarDistribuidoresModificar(this.ObtenerSesion(), entidad);
        }

        #endregion
    }
}
