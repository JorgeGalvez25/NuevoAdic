using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using EstandarCliente.CargadorVistas.CargadorVistasMdl;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServiciosCargador : WorkItemController
    {
        private int longOpciones { get; set; }
        private List<ImagenSoft.Framework.Entidades.OpcionMenu> menuCompleto;
        private Dictionary<string, ItemClickEventHandler> metodosMenu;

        private SesionModuloWeb sesion { get { return WorkItem.RootWorkItem.Items[ConstantesModulo.SESION_SISTEMA] as SesionModuloWeb; } }
        private ListaPermisos _permisos;
        private ListaPermisos Permisos
        {
            get
            {
                if (_permisos == null)
                {
                    _permisos = new ListaPermisos();
                    if (NivelPrivilegioActual != NivelPrivilegio.Administrador)
                    {
                        _permisos.FromXML(sesion.Usuario.Variables[0]);
                    }
                }

                return _permisos;
            }
        }

        private bool BuscarPermiso(ListaPermisos permiso, string toFind, bool submodulo)
        {
            return (NivelPrivilegioActual == NivelPrivilegio.Administrador) ? true : permiso.BuscarPermiso(toFind, submodulo);
        }

        private NivelPrivilegio NivelPrivilegioActual
        {
            get
            {
                return ((sesion.Usuario.Clave == 0) ? NivelPrivilegio.Administrador : (NivelPrivilegio)sesion.Usuario.Clave);
            }
        }

        public ServiciosCargador(WorkItem w)
        {
            this.WorkItem = w;
        }

        #region Metodos

        private delegate void asyncDelegate(string Seccion, bool habilitar);

        private void OcultarSeccionesMenu(bool habilitar)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = null;
            for (int i = 0; i < ribbon.Pages.Count; i++)
            {
                ribbonPage = ribbon.Pages[i];
                if (ribbonPage != null) { ribbonPage.Visible = habilitar; }
            }
        }

        private void SeccionMenu(string Seccion, bool habilitar)
        {
            asyncDelegate delegado = new asyncDelegate((e, h) =>
                {
                    DevExpress.XtraBars.Ribbon.RibbonControl ribbon = WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
                    ribbon.BeginSafe(delegate
                        {
                            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[e];
                            if (ribbonPage != null) { ribbonPage.Visible = h; }
                        });
                });
            delegado.BeginInvoke(Seccion, habilitar, null, null);
        }

        public void ShowView()
        {
            //IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();
            metodosMenu = new Dictionary<string, ItemClickEventHandler>();
            menuCompleto = new List<ImagenSoft.Framework.Entidades.OpcionMenu>();

            //this.SeccionMenu("Ver", false);
            ////this.SeccionMenu("Catálogos", false);
            //this.SeccionMenu("Consultas", false);
            //this.SeccionMenu("Documentos", false);

            this.OcultarSeccionesMenu(false);

            this.SeccionMenu("Catálogos", true);
            MostrarCatalogos();
            MostrarDocumentos();
            MostrarConsultas();
            MostrarReportes();
            MostrarProcesos();
            MostrarConfiguraciones();

            WorkItem.Parent.Items.Add(menuCompleto, ConstantesModulo.MENUS.MENU);

            ShowViewInWorkspace<VistaCargador>("Catálogos", WorkspaceNames.LeftWorkspace).Show();
        }

        private void MostrarCatalogos()
        {
            List<ImagenSoft.Framework.Entidades.OpcionMenu> menu = new List<ImagenSoft.Framework.Entidades.OpcionMenu>();
            if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.CLIENTES, true))
            {
                menu.Add(this.ObtenerCatalogo(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA, ConstantesModulo.MODULOS.CLAVES.CLIENTES, "MostrarClientes", MostrarClientes));
            }
            if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.USUARIOS, true))
            {
                menu.Add(this.ObtenerCatalogo(ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.TITULO_VISTA, ConstantesModulo.MODULOS.CLAVES.USUARIOS, "MostrarUsuarios", MostrarUsuarios));
            }
            //if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.CAMBIO_PRECIOS, true))
            //{
            //    menu.Add(this.ObtenerCatalogo(ConstantesModulo.VISTAS.CAMBIO_PRECIOS_MDL.TITULO_VISTA, ConstantesModulo.MODULOS.CLAVES.CAMBIO_PRECIOS, "MostrarCambioPrecios", MostrarCambioPrecios));
            //}
            if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.DISTRIBUIDORES, true))
            {
                menu.Add(this.ObtenerCatalogo(ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.TITULO_VISTA, ConstantesModulo.MODULOS.CLAVES.DISTRIBUIDORES, "MostrarDistribuidores", MostrarDistribuidores));
            }
            //this.MostrarModulosAdministrador(() =>
            //    {
            //        menu.Add(this.ObtenerCatalogo(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.TITULO_VISTA, ConstantesModulo.MODULOS.CLAVES.USUARIOS, "MostrarUsuarios", MostrarUsuarios));
            //    });

            if (menu.Count > 0)
            {
                menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

                IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

                servicio.OcultaGrupoSeccionMenu("Catálogos", "rbnSimples");
                servicio.OcultaGrupoSeccionMenu("Catálogos", "rbnComplejos");

                CrearSeccionCatalogos(menu, metodosMenu);
                menuCompleto.AddRange(menu);
            }
        }

        private void MostrarDocumentos()
        {
            //List<OpcionMenu> menu = new List<OpcionMenu>();
            //menu.Add(this.ObtenerDocumento(ConstantesModulo.VISTAS.COMPRAS.TITULO_VISTA_LISTADO_COMPRAS, ConstantesModulo.MODULOS.CLAVES.COMPRAS, "MostrarCompras", MostrarCompras));

            //menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

            //IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            //servicio.OcultaGrupoSeccionMenu("Documentos", "rbnDocumentos");

            //CrearSeccionCatalogos(menu, metodosMenu);
            //menuCompleto.AddRange(menu);
        }

        private void MostrarConsultas()
        {
            //List<ImagenSoft.Framework.Entidades.OpcionMenu> menu = new List<ImagenSoft.Framework.Entidades.OpcionMenu>();
            //menu.Add(this.ObtenerConsulta(ConstantesModulo.VISTAS.PRUEBAS_TRANSMISOR_MDL.TITULO_VISTA_LISTADO, ConstantesModulo.MODULOS.CLAVES.PRUEBAS, "MostrarPruebasTransmisiones", MostrarPruebasTransmisiones));

            //if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES, true))
            //{
            //    menu.Add(this.ObtenerConsulta(ConstantesModulo.VISTAS.MONITOR_TRANSMISIONES_MDL.TITULO_VISTA_LISTADO, ConstantesModulo.MODULOS.CLAVES.MONITOR_TRANSMISIONES, "MostrarMonitorTransmisiones", MostrarMonitorTransmisiones));
            //}

            //if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS, true))
            //{
            //    menu.Add(this.ObtenerConsulta(string.Format(ConstantesModulo.VISTAS.MONITOR_CAMBIO_PRECIO_MDL.TITULO_VISTA_LISTADO, string.Empty).Trim(), ConstantesModulo.MODULOS.CLAVES.MONITOR_PRECIOS, "MostrarMonitorCambioPrecios", MostrarMonitorCambioPrecios));
            //}

            //if (BuscarPermiso(Permisos, ConstantesPermisos.Modulos.MONITOR_CONEXIONES, true))
            //{
            //    menu.Add(this.ObtenerConsulta(string.Format(ConstantesModulo.VISTAS.MONITOR_CONEXIONES_MDL.TITULO_VISTA_LISTADO, string.Empty).Trim(), ConstantesModulo.MODULOS.CLAVES.MONITOR_CONEXIONES, "MostrarMonitorConexiones", MostrarMonitorConexiones));
            //}

            //menu.Add(this.ObtenerConsulta(string.Format(ConstantesModulo.VISTAS.MONITOR_APLICACIONES_MDL.TITULO_VISTA_LISTADO, string.Empty).Trim(), ConstantesModulo.MODULOS.CLAVES.MONITOR_APLICACIONES, "MostrarMonitorAplicaciones", MostrarMonitorAplicaciones));

            //if (menu.Count > 0)
            //{
            //    menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

            //    IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            //    servicio.OcultaGrupoSeccionMenu("Consultas", "rbnConsultas");

            //    CrearSeccionCatalogos(menu, metodosMenu);
            //    menuCompleto.AddRange(menu);
            //}
        }

        private void MostrarReportes()
        {
            //List<OpcionMenu> menu = new List<OpcionMenu>();
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTESALDOSPAC.TITULO_VISTA_LISTADO_SALDOS_PAC, ConstantesModulo.MODULOS.CLAVES.REPORTE_SALDOS_PAC, "MostrarReporteSaldoPAC", MostrarReporteSaldoPAC));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTESERVICIOCLIENTE.TITULO_VISTA_LISTADO_SERVICIOS_CLIENTE, ConstantesModulo.MODULOS.CLAVES.REPORTE_SERVICIOS_CLIENTE, "MostrarReporteServiciosCliente", MostrarReporteServiciosCliente));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTECONTRIBUYENTES.TITULO_VISTA_LISTADO_REPORTE_CONTRIBUYENTES, ConstantesModulo.MODULOS.CLAVES.REPORTE_CONTRIBUYENTES, "MostrarReporteContribuyentes", MostrarReporteContribuyentes));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTEDETMOVPAC.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.REPORTE_DETMOVPAC, "MostrarReporteDetMovPAC", MostrarReporteDetMovPAC));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTEDETMOVCLIENTE.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.REPORTE_DETMOVCLIENTE, "MostrarReporteDetMovCliente", MostrarReporteDetMovCliente));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTEANALISISCOMPRASSALDOPAC.TITULO_VISTA_LISTADO_REPORTE_ANALISIS_COMPRAS_SALDO_PAC, ConstantesModulo.MODULOS.CLAVES.REPORTE_ANALISIS_COMPRAS_SALDO_PAC, "MostrarReporteAnalisisComprasSaldoPac", MostrarReporteAnalisisComprasSaldoPac));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.RELACIONSERVICIOSVENDIDOSCLIENTE.TITULO_VISTA_LISTADO_RELACION_SERVICIOS_VENDIDOS_CLIENTE, ConstantesModulo.MODULOS.CLAVES.RELACION_SERVICIOS_VENDIDOS_CLIENTE, "MostrarRelacionServiciosVendidosCliente", MostrarRelacionServiciosVendidosCliente));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.ANALISISFACTURASALDOCLIENTE.TITULO_VISTA_LISTADO_ANALISIS_FACTURA_SALDO_CLIENTE, ConstantesModulo.MODULOS.CLAVES.ANALISISFACTURASALDOCLIENTE, "MostrarFacturaSaldoCliente", MostrarFacturaSaldoCliente));
            //menu.Add(this.ObtenerReporte(ConstantesModulo.VISTAS.REPORTESERVICIOSCOMPRADOSPAC.TITULO_VISTA_LISTADO_REPORTE_SERVICIOS_COMPRADOS_PAC, ConstantesModulo.MODULOS.CLAVES.REPORTE_SERVICIOS_COMPRADOS_PAC, "MostrarReporteServiciosCompradosPac", MostrarReporteServiciosCompradosPac));


            //menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

            //IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            //servicio.OcultaGrupoSeccionMenu("Reportes", "rbnReportes");

            //CrearSeccionCatalogos(menu, metodosMenu);
            //menuCompleto.AddRange(menu);
        }

        private void MostrarProcesos()
        {
            //List<OpcionMenu> menu = new List<OpcionMenu>();
            //menu.Add(this.ObtenerProceso(ConstantesModulo.VISTAS.CAMBIOPACOPERATIVO.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.CAMPO_PAC_OPERATIVO, "MostrarCambioPacOperativo", MostrarCambioPacOperativo));
            //menu.Add(this.ObtenerProceso(ConstantesModulo.VISTAS.NOTIFICARCONTRIBUYENTES.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.NOTIFICAR_CONTRIBUYENTES, "MostrarNotificacionesContribuyente", MostrarNotificacionesContribuyente));

            //this.MostrarModulosAdministrador(() =>
            //{
            //    menu.Add(this.ObtenerProceso(ConstantesModulo.VISTAS.CAMBIOPACDEFAULT.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.CAMPO_PAC_DEFAULT, "MostrarCambioPacDefault", MostrarCambioPacDefault));
            //});

            //menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

            //IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            //CrearSeccionCatalogos(menu, metodosMenu);
            //menuCompleto.AddRange(menu);
        }

        private void MostrarConfiguraciones()
        {
            //List<OpcionMenu> menu = new List<OpcionMenu>();
            //menu.Add(this.ObtenerConfiguracion(ConstantesModulo.VISTAS.CONFIGURACIONES.TITULO_MENU, ConstantesModulo.MODULOS.CLAVES.CONFIGURACIONES, "MostrarConfiguraciones", MostrarConfiguraciones));

            //menu.Sort((item1, item2) => { return item1.Opcion.CompareTo(item2.Opcion); });

            //IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            //CrearSeccionCatalogos(menu, metodosMenu);
            //menuCompleto.AddRange(menu);
        }

        private void CrearSeccionCatalogos(List<ImagenSoft.Framework.Entidades.OpcionMenu> menu, Dictionary<string, ItemClickEventHandler> metodos)
        {
            IServiciosMenuAplicacion servicio = WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();

            for (int i = 0; i < menu.Count; i++)
            {
                DevExpress.XtraBars.ItemClickEventHandler evento = null;
                if (metodos.ContainsKey(menu[i].Evento))
                {
                    evento = metodos[menu[i].Evento];
                }

                servicio.AgregarBotonSeccionMenu(menu[i].Seccion, menu[i].Grupo, menu[i].Opcion, evento, menu[i].IconoGrande, Shortcut.None);
                menu[i].Accesar = true;
            }
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerCatalogo(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Catálogos";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.CatalagosSmall;
            opcion.IconoGrande = Imagenes.Catalogos;
            opcion.Seccion = "Catálogos";
            opcion.TipoFuncionalidad = "C";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerConsulta(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Consultas";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.ConsultaSmall;
            opcion.IconoGrande = Imagenes.Consulta;
            opcion.Seccion = "Consultas";
            opcion.TipoFuncionalidad = "C";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerDocumento(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Documentos";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.documentsmall;
            opcion.IconoGrande = Imagenes.document;
            opcion.Seccion = "Documentos";
            opcion.TipoFuncionalidad = "D";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerReporte(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Reportes";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.ReportesSmall;
            opcion.IconoGrande = Imagenes.Reportes;
            opcion.Seccion = "Reportes";
            opcion.TipoFuncionalidad = "R";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerProceso(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Procesos";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.ProcesosSmall;
            opcion.IconoGrande = Imagenes.Procesos;
            opcion.Seccion = "Procesos";
            opcion.TipoFuncionalidad = "P";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        private ImagenSoft.Framework.Entidades.OpcionMenu ObtenerConfiguracion(string nombre, string clave, string clvEvento, ItemClickEventHandler evento)
        {
            var opcion = new ImagenSoft.Framework.Entidades.OpcionMenu();
            opcion.Opcion = nombre;
            opcion.Clave = clave;
            opcion.Grupo = "Configuraciones";
            opcion.Evento = clvEvento;
            opcion.IconoChico = Imagenes.ConfiguracionSmall;
            opcion.IconoGrande = Imagenes.Configuracion;
            opcion.Seccion = "Configuraciones";
            opcion.TipoFuncionalidad = "P";

            metodosMenu.Add(opcion.Evento, evento);

            return opcion;
        }

        #endregion

        #region Eventos

        #region Catalogos

        public void MostrarClientes(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarClientes();
        }

        public void MostrarUsuarios(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarUsuarios();
        }

        public void MostrarCambioPrecios(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarCambioPrecios();
        }

        public void MostrarDistribuidores(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarDistribuidores();
        }

        #endregion

        #region Servicios

        public void MostrarPruebasTransmisiones(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarPruebasTransmisiones();
        }

        public void MostrarMonitorTransmisiones(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarMonitorTransmisiones();
        }

        public void MostrarMonitorCambioPrecios(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarMonitorCambioPrecios();
        }

        public void MostrarMonitorConexiones(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarMonitorConexiones();
        }

        public void MostrarMonitorAplicaciones(object sender, ItemClickEventArgs e)
        {
            VistaCargadorPresenter presenter = new VistaCargadorPresenter();
            presenter.WorkItem = WorkItem;
            presenter.CargarMonitorAplicaciones();
        }

        #endregion

        #endregion
    }
}
