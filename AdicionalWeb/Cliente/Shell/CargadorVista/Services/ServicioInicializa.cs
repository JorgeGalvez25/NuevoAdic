using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Interfaces;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServicioInicializa
    {
        [EventPublication(EventTopicNames.evtEjercicioFACELEI, PublicationScope.Global)]
        public event EventHandler<EventArgs> evtEjercicioFACELEI;

        public WorkItem _WorkItem;

        public ServicioInicializa(WorkItem workItem)
        {
            _WorkItem = workItem;
        }

        #region Publicas

        public void InicializaSistema()
        {
            //Activar el icono de cerrar de cada uno de los tabs
            CABDevExpress.Workspaces.XtraTabWorkspace _rightWorkspace = (CABDevExpress.Workspaces.XtraTabWorkspace)_WorkItem.RootWorkItem.Items[UIExtensionSiteNames.RightWorkspace];
            _rightWorkspace.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            _rightWorkspace.HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Always;

            DevExpress.XtraBars.Ribbon.RibbonStatusBar _mainStatusStrip = (DevExpress.XtraBars.Ribbon.RibbonStatusBar)_WorkItem.RootWorkItem.Items[UIExtensionSiteNames.MainStatus];
            _mainStatusStrip.ItemLinks.Clear();

            SesionModuloWeb sesion = ObtenerSesion();

            #region Obsoleto

            //PanelControl panelUsuario = new PanelControl();
            //PanelControl panelTop = new PanelControl();
            //LabelControl labelUsuario = new LabelControl();
            //LabelControl labelUsuarioValor = new LabelControl();
            //LabelControl labelEspacio = new LabelControl();
            //PanelControl panelEjerjcicio = new PanelControl();
            //PanelControl panelTopEjerjcicio = new PanelControl();
            //LookUpEdit luEjercicio = new LookUpEdit();
            //LabelControl labelEjercicio = new LabelControl();

            //labelEspacio.Text = "     ";
            //labelEspacio.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular);
            //labelEspacio.Dock = DockStyle.Left;

            //labelUsuario.Text = " Usuario Actual:";
            //labelUsuario.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //labelUsuario.Dock = DockStyle.Left;

            //labelUsuarioValor.Text = string.Format("  {0}", sesion.Usuario.Nombre);
            //labelUsuarioValor.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular);
            //labelUsuarioValor.Dock = DockStyle.Left;

            //labelEjercicio.Text = " Ejercicio:  ";
            //labelEjercicio.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //labelEjercicio.Dock = DockStyle.Right;

            //panelTop.Dock = DockStyle.Top;
            //panelTop.Height = 6;
            //panelTop.BorderStyle = BorderStyles.NoBorder;
            //panelTop.BackColor = Color.Transparent;

            //panelUsuario.Dock = DockStyle.Fill;
            //panelUsuario.BorderStyle = BorderStyles.NoBorder;
            //panelUsuario.BackColor = Color.Transparent;

            //panelUsuario.Controls.Add(labelUsuarioValor);
            //panelUsuario.Controls.Add(labelUsuario);
            //panelUsuario.Controls.Add(labelEjercicio);
            //panelUsuario.Controls.Add(panelTop);

            //luEjercicio.Dock = DockStyle.Right;
            //luEjercicio.Properties.NullText = string.Empty;
            //luEjercicio.Width = 55;
            //luEjercicioFACELEI(luEjercicio);
            //luEjercicio.Properties.PopupFormMinSize = new Size(10, 10);
            //luEjercicio.EditValueChanged += new EventHandler(luEjercicio_EditValueChanged);

            //panelTopEjerjcicio.Dock = DockStyle.Top;
            //panelTopEjerjcicio.Height = 3;
            //panelTopEjerjcicio.BorderStyle = BorderStyles.NoBorder;
            //panelTopEjerjcicio.BackColor = Color.Transparent;

            //panelEjerjcicio.Dock = DockStyle.Right;
            //panelEjerjcicio.Width = 60;
            //panelEjerjcicio.BorderStyle = BorderStyles.NoBorder;
            //panelEjerjcicio.BackColor = Color.Transparent;

            //panelEjerjcicio.Controls.Add(luEjercicio);
            //panelEjerjcicio.Controls.Add(panelTopEjerjcicio);

            //_mainStatusStrip.Controls.Add(panelUsuario);
            //_mainStatusStrip.Controls.Add(panelEjerjcicio); 

            #endregion

            _mainStatusStrip.BeginInvoke(new MethodInvoker(() =>
            {
                bool manejaEjercicio = false;
                _mainStatusStrip.Controls.AddRange(new Control[]
                    {
                        this.ConfiguraPanelUsuario(sesion,manejaEjercicio),
                        this.ConfigurarEjercicio(manejaEjercicio)
                    });
            }));
        }

        #endregion

        #region Privadas

        private SesionModuloWeb ObtenerSesion()
        {
            SesionModuloWeb sesion = _WorkItem.RootWorkItem.Items[ConstantesModulo.SESION_SISTEMA] as SesionModuloWeb;
            if (sesion == null)
            {
                throw new Exception("No existe sesión de sistema");
            }

            return sesion;
        }

        private Control ConfiguraPanelUsuario(SesionModuloWeb sesion, bool cEjercicio)
        {
            PanelControl panelUsuario = new PanelControl();
            panelUsuario.Dock = DockStyle.Fill;
            panelUsuario.BorderStyle = BorderStyles.NoBorder;
            panelUsuario.BackColor = Color.Transparent;

            panelUsuario.Controls.AddRange(this.ConfigurarUsuario(sesion));
            if (cEjercicio)
            {
                LabelControl labelEjercicio = new LabelControl();
                labelEjercicio.Text = " Ejercicio:  ";
                labelEjercicio.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                labelEjercicio.Dock = DockStyle.Right;

                panelUsuario.Controls.Add(labelEjercicio);
            }
            panelUsuario.Controls.Add(ConfigurarPanelTop());

            return panelUsuario;
        }

        private Control[] ConfigurarUsuario(SesionModuloWeb sesion)
        {
            LabelControl labelUsuario = new LabelControl();
            LabelControl labelUsuarioValor = new LabelControl();

            labelUsuario.Text = " Usuario Actual:";
            labelUsuario.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            labelUsuario.Dock = DockStyle.Left;

            labelUsuarioValor.Text = string.Format("  {0}", sesion.Usuario.Nombre);
            labelUsuarioValor.Appearance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular);
            labelUsuarioValor.Dock = DockStyle.Left;

            return new Control[] { labelUsuarioValor, labelUsuario };
        }

        private Control ConfigurarPanelTop()
        {
            PanelControl panelTop = new PanelControl();
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 6;
            panelTop.BorderStyle = BorderStyles.NoBorder;
            panelTop.BackColor = Color.Transparent;
            return panelTop;
        }

        private Control ConfigurarEjercicio(bool cEjercicio)
        {
            if (!cEjercicio) { return new Control(); }
            PanelControl panelEjerjcicio = new PanelControl();
            PanelControl panelTopEjerjcicio = new PanelControl();
            LookUpEdit luEjercicio = new LookUpEdit();

            luEjercicio.Dock = DockStyle.Right;
            luEjercicio.Properties.NullText = string.Empty;
            luEjercicio.Width = 55;
            luEjercicioFACELEI(luEjercicio);
            luEjercicio.Properties.PopupFormMinSize = new Size(10, 10);
            luEjercicio.EditValueChanged += this.luEjercicio_EditValueChanged;

            panelTopEjerjcicio.Dock = DockStyle.Top;
            panelTopEjerjcicio.Height = 3;
            panelTopEjerjcicio.BorderStyle = BorderStyles.NoBorder;
            panelTopEjerjcicio.BackColor = Color.Transparent;

            panelEjerjcicio.Dock = DockStyle.Right;
            panelEjerjcicio.Width = 60;
            panelEjerjcicio.BorderStyle = BorderStyles.NoBorder;
            panelEjerjcicio.BackColor = Color.Transparent;

            panelEjerjcicio.Controls.Add(luEjercicio);
            panelEjerjcicio.Controls.Add(panelTopEjerjcicio);

            return panelEjerjcicio;
        }

        private void luEjercicioFACELEI(LookUpEdit lookUp)
        {
            try
            {
                ILookUpsFACELEI<LookUpEdit, Object> servicio =
                    _WorkItem.RootWorkItem.WorkItems[ConstantesModulo.MODULOS.CARGADOR_VISTAS].Services.Get<ILookUpsFACELEI<LookUpEdit, Object>>();

                if (servicio == null)
                {
                    throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesModulo.MODULOS.CARGADOR_VISTAS));
                }
                else
                {
                    try
                    {
                        //servicio.CrearLookUp(lookUp, ConstantesModulo.LOOKUPS.EJERCICIO, new object());
                        //EjercicioFACELEI ejercicio = new EjercicioFACELEI((int)lookUp.EditValue);
                        //_WorkItem.RootWorkItem.Items.Add(ejercicio, ConstantesModulo.EJERCICIO_ACTUAL);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ListadoMensajes.Error_Accion_No_Realizada_SB, "crear lista Ejercicios"));
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
        }

        public void DisparaEvento()
        {
            EventTopic evt = _WorkItem.EventTopics[EventTopicNames.evtEjercicioFACELEI];
            EventArgs evtEjercicio = new EventArgs();
            evt.Fire(this, evtEjercicio, _WorkItem, PublicationScope.Global);
        }

        #endregion

        #region Eventos

        protected virtual void OnevtEjercicioFACELEI(EventArgs eventArgs)
        {
            if (evtEjercicioFACELEI != null)
            {
                evtEjercicioFACELEI(this, eventArgs);
            }
        }

        void luEjercicio_EditValueChanged(object sender, EventArgs e)
        {
            //EjercicioFACELEI ejercicio = (EjercicioFACELEI)_WorkItem.RootWorkItem.Items[ConstantesModulo.EJERCICIO_ACTUAL];
            //_WorkItem.RootWorkItem.Items.Remove(ejercicio);
            //ejercicio = new EjercicioFACELEI(Convert.ToInt32((sender as LookUpEdit).EditValue));
            //_WorkItem.RootWorkItem.Items.Add(ejercicio, ConstantesModulo.EJERCICIO_ACTUAL);
            //DisparaEvento();
        }

        #endregion
    }
}
