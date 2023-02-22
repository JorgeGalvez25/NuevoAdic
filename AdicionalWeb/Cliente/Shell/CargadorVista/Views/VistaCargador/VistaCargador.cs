using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CABDevExpress.SmartPartInfos;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace EstandarCliente.CargadorVistas.CargadorVistasMdl
{
    public partial class VistaCargador :
        UserControl,
        IVistaCargador,
        ISmartPartInfoProvider
    {
        private List<ImagenSoft.Framework.Entidades.OpcionMenu> menuCompleto;

        public VistaCargador()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);

            menuCompleto = _presenter.ObtenerMenu();
            this.BeginSafe(this.CrearTreeView);
        }

        #region Metodos

        private void CrearTreeView()
        {
            this.tlOpciones.BeginInit();
            var secciones = (from item in menuCompleto
                             group item by item.Seccion into grupo
                             select new { Seccion = grupo.Key }).ToList();

            secciones.ForEach(s =>
            {
                CrearNodo(new object[] { s.Seccion }, -1, s.Seccion.Equals("Configuraciones") ? 1 : 0);
            });

            int parentNodeId = 0;

            secciones.ForEach(s =>
            {
                var opciones = (from item in menuCompleto
                                where item.Seccion == s.Seccion
                                select item).ToList();

                opciones.ForEach(item =>
                {
                    CrearNodo(new object[] { item.Opcion, item.Clave }, parentNodeId, item.Seccion);
                });

                parentNodeId++;
            });

            tlOpciones.Click += (sender, e) =>
            {
                try
                {
                    OpcionesTreeView(tlOpciones.FocusedNode.GetValue(1));
                }
                catch { }
            };
            this.tlOpciones.EndInit();
            this.tlOpciones.ExpandAll();
        }

        private void CrearNodo(object[] nodeData, int parentNodeId, string seccion)
        {
            int stateImageIndex = 0;
            switch (seccion)
            {
                case "Catálogos": stateImageIndex = 2; break;
                case "Documentos": stateImageIndex = 3; break;
                case "Consultas": stateImageIndex = 4; break;
                case "Reportes": stateImageIndex = 5; break;
                case "Procesos": stateImageIndex = 6; break;
                case "Configuraciones": stateImageIndex = 7; break;
            }

            CrearNodo(nodeData, parentNodeId, stateImageIndex);
        }

        private void CrearNodo(object nodeData, int parentNodeId, int stateImageIndex)
        {
            tlOpciones.AppendNode(nodeData, parentNodeId, 0, 0, stateImageIndex);
        }

        private void OpcionesTreeView(object opcion)
        {
            if (opcion == null) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                switch (opcion.ToString())
                {
                    #region Catalogos
                    case ConstantesModulo.MODULOS.CLAVES.CLIENTES: _presenter.CargarClientes(); break;
                    case ConstantesModulo.MODULOS.CLAVES.USUARIOS: _presenter.CargarUsuarios(); break;
                    case ConstantesModulo.MODULOS.CLAVES.CAMBIO_PRECIOS: _presenter.CargarCambioPrecios(); break;
                    case ConstantesModulo.MODULOS.CLAVES.DISTRIBUIDORES: _presenter.CargarDistribuidores(); break;
                    #endregion

                    #region Servicios

                    case ConstantesModulo.MODULOS.CLAVES.MONITOR_TRANSMISIONES: _presenter.CargarMonitorTransmisiones(); break;
                    case ConstantesModulo.MODULOS.CLAVES.MONITOR_PRECIOS: _presenter.CargarMonitorCambioPrecios(); break;
                    case ConstantesModulo.MODULOS.CLAVES.MONITOR_CONEXIONES: _presenter.CargarMonitorConexiones(); break;
                    //case ConstantesModulo.MODULOS.CLAVES.MONITOR_APLICACIONES: _presenter.CargarMonitorAplicaciones(); break;

                    #endregion

                    default:
                        break;
                }
            }
            finally
            {
                tlOpciones.FocusedNode = tlOpciones.FocusedNode.ParentNode;
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            XtraNavBarGroupSmartPartInfo info = new XtraNavBarGroupSmartPartInfo();
            info.LargeImage = Imagenes.CatalagosSmall;
            info.Title = "Menú Servicios Monitor";
            return info;
        }

        #endregion
    }
}