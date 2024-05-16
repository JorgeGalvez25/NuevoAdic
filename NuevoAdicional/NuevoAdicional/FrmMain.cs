using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Adicional.Entidades;
using NuevoAdicional.EntradaTanques;
using System.Diagnostics;
using System.Configuration;
using ServiciosCliente;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Persistencia;
using System.ServiceProcess;

namespace NuevoAdicional
{
    public partial class frmMain : Form
    {
        private const string formatoFecha = "dd/MM/yyyy";
        private const string formatoFechaHora = "dd/MM/yyyy HH:mm";
        private const string formatoHora = "HH:mm";
        private const string usuarioSincro = "Sincronización";

        private bool scanFree = true;
        private Dictionary<int, string> erroresActualizacion = new Dictionary<int, string>();
        private Dictionary<int, EdoRemoto> edoRemoto = new Dictionary<int, EdoRemoto>();
        private Servicios.Adicional.IServiciosAdicional servicioAdicional;

        public frmMain()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            Inicializa();
        }

        private void CrearEventos()
        {
            this.tiRegistrarTicket.Click += this.tiRegistrarTicket_Click;
            this.tiModificarTicket.Click += this.tiModificarTicket_Click;
            this.tiTanques.Click += new EventHandler(tiTanques_Click);
        }

        /// <summary>
        /// Verifica si el usuario actual tiene derecho a una opción
        /// </summary>
        /// <param name="sender">control que invoca el método</param>
        /// <returns>true si tiene derecho, false en caso contrario</returns>
        private bool verificarDerecho(object sender)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void LlenaLista()
        {
            lvEstaciones.Items.Clear();

            ListaEstacion pListaEstacion = Configuraciones.Estaciones;

            foreach (var estacion in pListaEstacion)
            {
                ListViewItem item = new ListViewItem();
                item.Text = estacion.Id.ToString("00");
                item.SubItems.Add(estacion.Nombre);
                item.SubItems.Add(string.Empty);//estacion.Estado);
                item.SubItems.Add(string.Empty);//estacion.UltimoMovimiento.ToString("dd/MMM/yyyy"));
                item.SubItems.Add(string.Empty);
                item.SubItems.Add("En espera");
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(estacion.TipoDispensario.ToString());

                tiActProtecc.Visible = estacion.TipoDispensario.ToString() == "Gilbarco";
                tiDesProtecc.Visible = estacion.TipoDispensario.ToString() == "Gilbarco";
                tiComboBoxProt.Visible = estacion.TipoDispensario.ToString() == "Gilbarco";
                if (tiComboBoxProt.Visible)
                    tiComboBoxProt.SelectedIndex = 1;
                tiProtecciones.Visible = estacion.TipoDispensario.ToString() != "Gilbarco";

                item.ImageIndex = 2;
                item.Focused = false;
                item.ForeColor = Color.Gray;

                tiParo.Visible = estacion.TipoDispensario == MarcaDispensario.HongYang;

                //if (estacion.Estado == "Estandar")
                //{
                //    item.ImageIndex = 1;
                //}
                //else
                //{
                //    item.ImageIndex = 0;
                //}

                lvEstaciones.Items.Add(item);
            }
        }

        private int ObtenerIdSeleccionado()
        {
            if (lvEstaciones.SelectedItems.Count == 0)
            {
                //MessageBox.Show("Selecciona una estación primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            else
            {
                return Convert.ToInt32(lvEstaciones.SelectedItems[0].Text);
            }
        }

        private string ObtenerNombreEstSeleccionada()
        {
            if (lvEstaciones.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona una estación primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            else
            {
                return lvEstaciones.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void Inicializa()
        {
            this.txtUsuario.Text = Configuraciones.NombreUsuario;

            //this.Text = string.Concat("Posiciones ", Licencia.Version);
            this.Text = "Posiciones";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Configuraciones.Inicializar();
            ActivarDesactivarMenu();
            itRefresh.Enabled = false;
            tiEstaciones.Enabled = false;
            LlenaLista();
            //lvEstaciones.FocusedItem.Focused = false;
            toolStrip1.Focus();
            Application.DoEvents();

            //Configuraciones.ActualizarEstadosEstaciones();
            bwConectarEstaciones.RunWorkerAsync();

            tmrScanConection.Enabled = true;
            tmrScanConection.Start();

            itmEdoRemoto.Visible = false;

            this.CrearEventos();

            Cursor.Current = Cursors.Default;

            tmrVerifica.Enabled = ConfigurationManager.AppSettings["Consola3"] != null;
            this.Visible = ConfigurationManager.AppSettings["InicioAuto"] != "Si";

            tmrSinc.Enabled = ConfigurationManager.AppSettings["HoraSinc"] != null;

            if (ConfigurationManager.AppSettings["ModoGateway"] == "Si" && ConfigurationManager.AppSettings["ServicioX"] != string.Empty)
            {
                string estatus = new ConfiguracionPersistencia().ConfiguracionObtener(1).Estado;

                //Detener servicio
                ServiceController sc = new ServiceController(estatus != "Estandar" ? ConfigurationManager.AppSettings["ServicioX"] : ConfigurationManager.AppSettings["ServicioOpengas"]);
                try
                {
                    if (sc != null && sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                    }
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    sc.Close();
                }
                catch
                {
                }

                //Iniciar servicio
                sc = new ServiceController(estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioX"] : ConfigurationManager.AppSettings["ServicioOpengas"]);

                try
                {
                    if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    sc.Close();
                }
                catch
                {
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            scanFree = false;

            if (this.bwConectarEstaciones.IsBusy)
            {
                bwConectarEstaciones.CancelAsync();
            }
            if (this.bwScanConections.IsBusy)
            {
                bwScanConections.CancelAsync();
            }

            foreach (ListViewItem item in lvEstaciones.Items)
            {
                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);
                Estacion est = Configuraciones.Estaciones.Find(s => { return s.Id == idEstacion; });

                if (item.SubItems[5].Text.Equals("En línea"))
                {
                    item.SubItems[5].Text = "Desconectando";
                    Application.DoEvents();

                    GuardarAccesoSalidaSistema(idEstacion, false);
                    Configuraciones.CerrarCanales(idEstacion);

                    item.SubItems[5].Text = "Desconectado";
                    Application.DoEvents();
                }
            }

            base.OnClosing(e);
        }

        private void tiEstaciones_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmEstaciones forma = new frmEstaciones();
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LlenaLista();
                toolStrip1.Focus();
                ActivarDesactivarMenu();
                itRefresh.Enabled = false;
                tiEstaciones.Enabled = false;
                bwConectarEstaciones.RunWorkerAsync();
            }
            Cursor.Current = Cursors.Default;
        }

        private void tiPosiciones_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pIdEstacion = ObtenerIdSeleccionado();

            if (pIdEstacion > 0)
            {
                Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pIdEstacion];
                if (!Configuraciones.CanalEstaActivo(pIdEstacion, true))
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                }

                try
                {
                    servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Entrar a flujos" });
                }
                catch (Exception)
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                }

                frmPosicionesEstacion forma = new frmPosicionesEstacion(pIdEstacion);
                forma.Text = lvEstaciones.FocusedItem.SubItems[1].Text;
                forma.ShowDialog();

                if (lvEstaciones.FocusedItem.SubItems[2].Text.Equals("Estandar", StringComparison.OrdinalIgnoreCase) && forma.AplicarCambios)
                {
                    if (ConfigurationManager.AppSettings["GilbarcoOnOff"] == "Si")
                    {
                        ListaHistorial pListaHistorial = null;

                        try
                        {
                            pListaHistorial = servicioAdicional.HistorialObtenerRecientes(pIdEstacion);
                        }
                        catch (Exception)
                        {
                            servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                        }
                        new ProcesosFlujo().AplicarFlujo((from h in pListaHistorial select h).ToList<Historial>());

                        ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pIdEstacion];

                        string comando;
                        int xpos = pListaHistorial[0].Posicion;
                        comando = pListaHistorial[0].Posicion + ":";
                        for (int i = 0; i < pListaHistorial.Count; i++)
                        {
                            if (xpos != pListaHistorial[i].Posicion)
                                comando = comando.Remove(comando.Length - 1) + ";" + pListaHistorial[i].Posicion + ":";
                            xpos = pListaHistorial[i].Posicion;
                            comando += pListaHistorial[i].Porcentaje.ToString() + ",";
                        }
                        comando = comando.Remove(comando.Length - 1);

                        pServiciosCliente.AplicarFlujoGilbarcoPorcentajes(comando);
                    }
                    else
                    {
                        try
                        {
                            lvEstaciones.FocusedItem.SubItems[2].Text = "Aplicando";
                            Application.DoEvents();
                            Cursor.Current = Cursors.WaitCursor;

                            ListaHistorial pListaHistorial = null;

                            try
                            {
                                pListaHistorial = servicioAdicional.HistorialObtenerRecientes(pIdEstacion);
                            }
                            catch (Exception)
                            {
                                servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                            }

                            ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pIdEstacion];
                            if (!Configuraciones.CanalEstaActivo(pIdEstacion, false))
                            {
                                pServiciosCliente = Configuraciones.AbrirCanalCliente(pIdEstacion);
                            }

                            try
                            {
                                Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
                                string pRespuesta = pServiciosCliente.AplicarFlujo(true, false, estacion.TipoDispensario, (from h in pListaHistorial select h).ToList<Historial>());

                                if (pRespuesta.Length > 0)
                                {
                                    Estacion pEstacionEntidad = Configuraciones.Estaciones.Find(p => { return p.Id == pIdEstacion; });
                                    pEstacionEntidad.Estado = "Estandar";
                                    try
                                    {
                                        pEstacionEntidad.UltimoMovimiento = servicioAdicional.ConfiguracionActualizarUltimoMovimiento(pEstacionEntidad.UltimoMovimiento);

                                        new EstacionPersistencia().EstacionActualizar(pEstacionEntidad);
                                        servicioAdicional.ConfiguracionCambiarEstado(pEstacionEntidad.Estado);

                                        lvEstaciones.FocusedItem.SubItems[3].Text = pEstacionEntidad.UltimoMovimiento.ToString(formatoFecha);
                                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Modificar flujo" });
                                    }
                                    catch (Exception)
                                    {
                                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Error al aplicar flujo" });
                                    }
                                    catch (Exception)
                                    {
                                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pIdEstacion);
                                    }
                                    MessageBox.Show("Hubo un error al aplicar el flujo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception)
                            {
                                Configuraciones.AbrirCanalCliente(pIdEstacion);
                            }
                        }
                        finally
                        {
                            lvEstaciones.FocusedItem.SubItems[2].Text = "Estandar";
                            Cursor.Current = Cursors.Default;
                            //LlenaLista();
                        }
                    }
                }
            }
        }

        private void tiSubir_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pEstacion = ObtenerIdSeleccionado();

            if (pEstacion > 0)
            {
                if (sender == tiParo)
                {
                    if (MessageBox.Show("¿Aplicar paro de emergencia de la estación seleccionada?", "Paro", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show(sender == tiSubir ? "¿Subir el flujo de la estación seleccionada?" : "¿Activar la protección de la estación seleccionada?",
                        sender == tiSubir ? "Flujo" : "Protección", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Servicios.Adicional.IServiciosAdicional servicioAdicional;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string estadoActual = lvEstaciones.SelectedItems[0].SubItems[2].Text;
                    lvEstaciones.SelectedItems[0].SubItems[2].Text = "Subiendo";
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;

                    servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    ListaHistorial pListaHistorial = servicioAdicional.HistorialObtenerRecientes(pEstacion);

                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
                        string pRespuesta;
                        if (estacion.TipoDispensario == MarcaDispensario.Team)
                        {
                            if (tiComboBoxProt.SelectedIndex == 0)
                            {
                                foreach (var pHistorial in pListaHistorial)
                                {
                                    pHistorial.Abajo = "No";
                                    servicioAdicional.HistorialActualizar(pHistorial);
                                }
                            }
                            else
                            {
                                int posCarga = Convert.ToInt32(tiComboBoxProt.Text.Substring(tiComboBoxProt.Text.Length - 2, 2));
                                if (ConfigurationManager.AppSettings["TeamPorPosicion"] != "Si")
                                    posCarga = (posCarga * 2) - 1;
                                pListaHistorial.Find(p => p.Posicion == posCarga).Abajo = "No";
                                servicioAdicional.HistorialActualizar(pListaHistorial.Find(p => p.Posicion == posCarga));
                                foreach (var pHistorial in pListaHistorial)
                                {
                                    if (pHistorial.Abajo == "Si")
                                        pHistorial.Porcentaje = 0;
                                }
                            }
                        }
                        pRespuesta = pServiciosCliente.AplicarFlujo(true, sender == tiParo, estacion.TipoDispensario, (from h in pListaHistorial select h).ToList<Historial>());
                        if (pRespuesta.Equals("Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            Estacion pEstacionEntidad = Configuraciones.Estaciones.Find(p => { return p.Id == pEstacion; });//new EstacionPersistencia().EstacionObtener(pEstacion);
                            pEstacionEntidad.Estado = "Estandar";

                            string tipoClb;
                            if (!Utilerias.ObtenerListaVar().TryGetValue("TipoClb", out tipoClb))
                                tipoClb = "0";

                            if (ConfigurationManager.AppSettings["ModoPresetWayne"] == "Si" || tipoClb == "6" || tipoClb == "7")
                            {
                                lvEstaciones.SelectedItems[0].ImageIndex = 5;
                                tmrWayne.Enabled = true;
                                tmrSinc.Enabled = false;
                                pEstacionEntidad.EstadoPresetWayne = EstatusPresetWayne.EsperandoEstandar;
                            }
                            else
                            {
                                lvEstaciones.SelectedItems[0].SubItems[2].Text = pEstacionEntidad.Estado;
                                lvEstaciones.SelectedItems[0].SubItems[3].Text = pEstacionEntidad.UltimoMovimiento.ToString(formatoFecha);
                                if (sender == tiParo)
                                    lvEstaciones.SelectedItems[0].ImageIndex = 3;
                                else
                                    lvEstaciones.SelectedItems[0].ImageIndex = 1;
                            }

                            pEstacionEntidad.UltimoMovimiento = servicioAdicional.ConfiguracionActualizarUltimoMovimiento(pEstacionEntidad.UltimoMovimiento);
                            new EstacionPersistencia().EstacionActualizar(pEstacionEntidad);
                            if (estacion.TipoDispensario != MarcaDispensario.Team || tiComboBoxProt.SelectedIndex == 0)
                                servicioAdicional.ConfiguracionCambiarEstado(pEstacionEntidad.Estado);
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Subir flujo" });



                            // apagar el visual del remoto
                            servicioAdicional.ApagarVisual();

                            lvEstaciones_ItemSelectionChanged(sender, null);
                        }
                        else
                        {
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Error al aplicar flujo" });
                            string mensajeError = string.Concat("Hubo un error al aplicar el flujo.\r\nEl mensaje de la consola fue: ", pRespuesta);
                            MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Flujos({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al aplicar el flujo." +
                                        " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                        lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }
                }
                catch (Exception ex)
                {
                    string nombreArchivo = string.Format("ERROR_Flujos({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                    MessageBox.Show("Se ha detectado un error no clasificado al aplicar el flujo." +
                                    " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                    "Para más información verifique el archivo: " + nombreArchivo + ".",
                                    "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    //LlenaLista();
                    if (tiComboBoxProt.Visible)
                        tiComboBoxProt.SelectedIndex = 0;
                }
            }
        }

        private void tiBajar_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pEstacion = ObtenerIdSeleccionado();

            if (pEstacion > 0)
            {
                if (MessageBox.Show("¿Bajar el flujo de la estación seleccionada?", "Flujo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                }
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    lvEstaciones.SelectedItems[0].SubItems[2].Text = "Bajando";
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                    ListaHistorial pListaHistorial = servicioAdicional.HistorialObtenerRecientes(pEstacion);

                    TimeSpan pHora = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                    Configuracion pConfiguracion = servicioAdicional.ConfiguracionObtener(1);

                    string estadoActual = lvEstaciones.SelectedItems[0].SubItems[2].Text;
                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        string pRespuesta;
                        Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
                        if (estacion.TipoDispensario == MarcaDispensario.Team && tiComboBoxProt.SelectedIndex > 0)
                        {
                            int posCarga = Convert.ToInt32(tiComboBoxProt.Text.Substring(tiComboBoxProt.Text.Length - 2, 2));
                            if (ConfigurationManager.AppSettings["TeamPorPosicion"] != "Si")
                                posCarga = (posCarga * 2) - 1;
                            pListaHistorial.Find(p => p.Posicion == posCarga).Abajo = "Si";
                            servicioAdicional.HistorialActualizar(pListaHistorial.Find(p => p.Posicion == posCarga));
                            foreach (var pHistorial in pListaHistorial)
                            {
                                if (pHistorial.Abajo == "Si")
                                    pHistorial.Porcentaje = 0;
                            }
                            pRespuesta = pServiciosCliente.AplicarFlujo(true, false, estacion.TipoDispensario,
                                (from h in pListaHistorial select h).ToList<Historial>());
                        }
                        else
                        {
                            foreach (var pHistorial in pListaHistorial)
                            {
                                pHistorial.Fecha = DateTime.Today;
                                pHistorial.Hora = pHora;
                                pHistorial.Porcentaje = pConfiguracion.Cantidad_minima;
                            }
                            pRespuesta = pServiciosCliente.AplicarFlujo(false, false, estacion.TipoDispensario, (from h in pListaHistorial select h).ToList<Historial>());
                        }

                        if (pRespuesta.Equals("Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            if (estacion.TipoDispensario == MarcaDispensario.Team && tiComboBoxProt.SelectedIndex > 0)
                            {
                                lvEstaciones_ItemSelectionChanged(sender, null);
                                servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Bajar flujo" });
                                return;
                            }

                            Estacion pEstacionEntidad = Configuraciones.Estaciones.Find(p => { return p.Id == pEstacion; });

                            string tipoClb;
                            if (!Utilerias.ObtenerListaVar().TryGetValue("TipoClb", out tipoClb))
                                tipoClb = "0";

                            if (estacion.TipoDispensario == MarcaDispensario.Team && tiComboBoxProt.SelectedIndex > 0)
                            {
                                lvEstaciones_ItemSelectionChanged(sender, null);
                                servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Bajar flujo" });
                                return;
                            }
                            else if ((ConfigurationManager.AppSettings["ModoPresetWayne"] == "Si" || tipoClb == "6" || tipoClb == "7") && pEstacionEntidad.Estado == "Estandar")
                            {
                                pEstacionEntidad.EstadoPresetWayne = EstatusPresetWayne.EsperandoMinimo;
                                lvEstaciones.SelectedItems[0].ImageIndex = 6;
                                tmrWayne.Enabled = true;
                                tmrSinc.Enabled = false;
                                tmrVerifica.Enabled = false;
                            }
                            else
                            {
                                pEstacionEntidad.Estado = "Mínimo";
                                lvEstaciones.SelectedItems[0].SubItems[2].Text = pEstacionEntidad.Estado;
                                lvEstaciones.SelectedItems[0].SubItems[3].Text = pEstacionEntidad.UltimoMovimiento.ToString(formatoFecha);
                                lvEstaciones.SelectedItems[0].ImageIndex = 0;
                            }

                            pEstacionEntidad.UltimoMovimiento = servicioAdicional.ConfiguracionActualizarUltimoMovimiento(pEstacionEntidad.UltimoMovimiento);
                            new EstacionPersistencia().EstacionActualizar(pEstacionEntidad);
                            if (estacion.TipoDispensario != MarcaDispensario.Team || tiComboBoxProt.SelectedIndex == 0)
                                servicioAdicional.ConfiguracionCambiarEstado(pEstacionEntidad.Estado);
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Bajar flujo" });


                        }
                        else
                        {
                            string mensajeError = string.Concat("Hubo un error al aplicar el flujo.\r\nEl mensaje de la consola fue: ", pRespuesta);
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Error al bajar flujo" });
                            MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Bajar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al bajar el flujo." +
                            //"El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                        lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }
                }
                catch (Exception ex)
                {
                    string nombreArchivo = string.Format("ERROR_Bajar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                    MessageBox.Show("Se ha detectado un error no clasificado al bajar el flujo." +
                        //"El mensaje original del error se ha almacenado en la bitácora.\n" +
                                    "Para más información verifique el archivo: " + nombreArchivo + ".",
                                    "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    //LlenaLista();
                    if (tiComboBoxProt.Visible)
                        tiComboBoxProt.SelectedIndex = 0;
                }
            }
        }

        private void tiConfiguraciones_Click(object sender, EventArgs e)
        {
            if (verificarDerecho(sender))
            {
                int idEstacion = ObtenerIdSeleccionado();
                if (idEstacion > 0)
                {
                    Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[idEstacion];
                    if (!Configuraciones.CanalEstaActivo(idEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                    }

                    try
                    {
                        Configuracion pConfiguracion = servicioAdicional.ConfiguracionObtener(1);
                        frmConfiguraciones pForma = new frmConfiguraciones(pConfiguracion, idEstacion);
                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Entrar a configuraciones" });
                        pForma.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Configuraciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al aplicar configuraciones." +
                                        " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                        servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                    }
                }
            }
        }

        private void tiUsurios_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new frmUsuarios().ShowDialog();
        }

        private void btnMoviles_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int estacion = ObtenerIdSeleccionado();

            if (estacion > 0)
                new frmMoviles(estacion).ShowDialog();
        }

        private void tiBitacora_Click(object sender, EventArgs e)
        {
            if (verificarDerecho(sender))
            {
                int estacion = ObtenerIdSeleccionado();

                if (estacion > 0)
                {
                    new frmBitacora(Configuraciones.Estaciones.Find(p => { return p.Id == estacion; }).Nombre, estacion).ShowDialog();
                }
            }
        }

        private void tiReporte_Click(object sender, EventArgs e)
        {
            bool permiteVer = false;
            int numReporte = Convert.ToInt32((sender as System.Windows.Forms.ToolStripMenuItem).Tag);

            switch (numReporte)
            {
                case 1: permiteVer = Configuraciones.GetValorVariable(Configuraciones.ListaVariables[0]).Equals("Si", StringComparison.OrdinalIgnoreCase);
                    break;
                case 2: permiteVer = Configuraciones.GetValorVariable(Configuraciones.ListaVariables[1]).Equals("Si", StringComparison.OrdinalIgnoreCase);
                    break;
                default:
                    break;
            }

            if (permiteVer)
            {
                int pIdEstacion = ObtenerIdSeleccionado();

                if (pIdEstacion > 0)
                {
                    frmReporteAjusteParametros forma = new frmReporteAjusteParametros(numReporte);

                    if (forma.ShowDialog() == DialogResult.OK)
                    {
                        DateTime fechaIni = forma.FechaIni;
                        DateTime fechaFin = forma.FechaFin;

                        ReporteAjusteProceso procesoReporte = new ReporteAjusteProceso();

                        string nombreEstacion = lvEstaciones.SelectedItems[0].SubItems[1].Text;
                        switch (numReporte)
                        {
                            case 1:
                                {
                                    procesoReporte.ReporteDeAjuste01(pIdEstacion, nombreEstacion, fechaIni, fechaFin, forma.De06a06, forma.EntradasFisicas, forma.NombreEstacion, forma.Detallado);
                                } break;
                            case 2:
                                {
                                    procesoReporte.ReporteDeAjuste02(pIdEstacion, nombreEstacion, fechaIni, forma.A24Hrs, forma.NombreEstacion);
                                } break;
                            default:
                                break;
                        }
                    }

                    forma.Dispose();
                }
            }
            else
            {
                MessageBox.Show("El usuario no tiene derechos para visualizar el reporte seleccionado.", "Derechos", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void tiRegenerarArchivos_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pIdEstacion = ObtenerIdSeleccionado();

            if (pIdEstacion > 0)
            {
                frmRegenerarArchivos pForma = new frmRegenerarArchivos();
                pForma.Text = "Regenerar archivos volumétricos " + lvEstaciones.SelectedItems[0].SubItems[1].Text;

                if (pForma.ShowDialog() == DialogResult.OK)
                {
                    string pMensajeError = string.Empty;
                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pIdEstacion];
                    if (!Configuraciones.CanalEstaActivo(pIdEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pIdEstacion);
                    }

                    if (pServiciosCliente.SetRegenerarArchivosVolumetricos(pForma.txtFecha.Value.Date, Convert.ToInt32(pForma.txtCorte.Text), out pMensajeError) == true)
                    {
                        MessageBox.Show("Archivos generados", "Regenerar archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(pMensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tiProtecciones_Click(object sender, EventArgs e)
        {
            if (verificarDerecho(sender))
            {
                int pEstacion = ObtenerIdSeleccionado();
                if (pEstacion > 0)
                {
                    Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Entrar a Protecciones" });
                        frmProtecciones formaProtecciones = new frmProtecciones(Configuraciones.Estaciones.Find(p => { return p.Id == pEstacion; }));
                        formaProtecciones.ShowDialog();
                        formaProtecciones.Dispose();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Se produjo un error al establecer comunicación con la estación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void tiEscanear_Click(object sender, EventArgs e)
        {
            if (verificarDerecho(sender))
            {
                int pEstacion = ObtenerIdSeleccionado();
                if (pEstacion > 0)
                {
                    Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    string estadoActual = lvEstaciones.SelectedItems[0].SubItems[2].Text;

                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        lvEstaciones.SelectedItems[0].SubItems[2].Text = "Sincronizando";
                        Application.DoEvents();
                        Cursor.Current = Cursors.WaitCursor;

                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Sincronizar Estación" });
                        //Sincronizar
                        try
                        {
                            byte status = (byte)(estadoActual.Equals("Mínimo", StringComparison.OrdinalIgnoreCase) ? 0 : 1);
                            string mensaje = string.Empty;
                            servicioAdicional.ConfiguracionActualizarUltimaSincronizacion(DateTime.Now);
                            bool resSicronizar = pServiciosCliente.Sincronizar(status, out mensaje);

                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                if (resSicronizar)
                                {
                                    MessageBox.Show("El estado del dispensario es correcto.", "Sincronización", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    string mensajeRestablecido = "Se ha restablecido el estado.";
                                    string edoAdicional = estadoActual;
                                    string edoDisp = estadoActual.Equals("Mínimo") ? "Estandar" : "Mínimo";
                                    bool restablecido = true;

                                    servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Diferencia de sincronización (Adicional: " + edoAdicional + " Dispensario: " + edoDisp + ")" });
                                    restablecido = subirBajar(status, pEstacion, estadoActual);

                                    if (!restablecido)
                                    {
                                        mensajeRestablecido = "No ha sido posible restablecer el estado.";
                                    }
                                    MessageBox.Show("El estado del dispensario es incorrecto.\n" + mensajeRestablecido, "Sincronización", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                                MessageBox.Show("Hubo un error al sincronizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            string nombreArchivo = string.Format("ERROR_Sincronizar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                            MessageBox.Show("Se ha detectado un error no clasificado al bajar el flujo." +
                                //"El mensaje original del error se ha almacenado en la bitácora.\n" +
                                            "Para más información verifique el archivo: " + nombreArchivo + ".",
                                            "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);
                            pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                        }

                    }
                    catch (Exception)
                    {
                        lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;

                        MessageBox.Show("Se produjo un error al establecer comunicación con la estación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void itRefresh_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            itRefresh.Enabled = false;
            tiEstaciones.Enabled = false;
            scanFree = false;
            Configuraciones.ActualizarEstaciones();
            LlenaLista();
            ActivarDesactivarMenu();
            bwConectarEstaciones.RunWorkerAsync();

            Cursor.Current = Cursors.Default;
        }

        private void itSalir_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as ToolStripItem).Tag)) && Configuraciones.IdUsuario != 1)
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.ExitThread();
        }

        private void lvEstaciones_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ActivarDesactivarMenu();
            MostrarEdoRemoto();

            try
            {
                Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == Convert.ToInt32(lvEstaciones.SelectedItems[0].Text); });
                if (estacion.TipoDispensario == MarcaDispensario.Team)
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion.Id);
                    ListaHistorial pListaHistorial = servicioAdicional.HistorialObtenerRecientes(estacion.Id);
                    tiComboBoxProt.Visible = true;
                    tiComboBoxProt.Items.Clear();
                    tiComboBoxProt.Items.Add("Todos");
                    int dispensario = 1;
                    if (estacion.Estado == "Estandar")
                    {
                        pListaHistorial.ForEach(p =>
                        {
                            if (p.Posicion % 2 == 1 || ConfigurationManager.AppSettings["TeamPorPosicion"] == "Si")
                            {
                                tiComboBoxProt.Items.Add((p.Abajo == "No" ? " ▲ " : " ▼ ") + " Disp. " + p.Posicion.ToString());
                            }
                        });
                    }
                    tiComboBoxProt.SelectedIndex = 0;
                }
                else if (estacion.TipoDispensario != MarcaDispensario.Gilbarco)
                    tiComboBoxProt.Visible = false;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void bwConectarEstaciones_DoWork(object sender, DoWorkEventArgs e)
        {
            scanFree = false;

            foreach (ListViewItem item in lvEstaciones.Items)
            {
                if (item.SubItems[5].Text != "En línea" && item.SubItems[5].Text != "Conectando")
                {
                    int idEstacion = Convert.ToInt32(item.SubItems[0].Text);
                    Estacion est = Configuraciones.Estaciones.Find(s => { return s.Id == idEstacion; });
                    item.SubItems[5].Text = "Conectando";
                    //Application.DoEvents();

                    Servicios.Adicional.IServiciosAdicional serviciosAdicional = Configuraciones.ListaCanalesAdicional.ContainsKey(idEstacion) ? Configuraciones.ListaCanalesAdicional[idEstacion] : Configuraciones.AbrirCanalAdicional(idEstacion);
                    ServiciosCliente.IServiciosCliente serviciosCliente = Configuraciones.ListaCanales.ContainsKey(idEstacion) ? Configuraciones.ListaCanales[idEstacion] : Configuraciones.AbrirCanalCliente(idEstacion);

                    Configuracion cfg = null;
                    try
                    {
                        cfg = serviciosAdicional.ConfiguracionObtener(1);
                        //    List<Licencia> lics = serviciosAdicional.LicenciasObtener();

                        //    if (lics != null)
                        //        Configuraciones.Licencias.Add(idEstacion, lics);
                    }
                    catch (Exception ex)
                    {
                        // Guardar el error en el diccionario
                        if (erroresActualizacion.ContainsKey(idEstacion))
                        {
                            erroresActualizacion[idEstacion] = ex.Message;
                        }
                        else
                        {
                            erroresActualizacion.Add(idEstacion, ex.Message);
                        }
                        serviciosAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                    }


                    if (cfg != null && Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                    {
                        if (!Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion])
                        {
                            GuardarAccesoSalidaSistema(idEstacion, true);
                            Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion] = true;
                        }

                        item.SubItems[2].Text = cfg.Estado;
                        item.SubItems[3].Text = cfg.UltimoMovimiento.ToString(formatoFecha);
                        item.SubItems[4].Text = cfg.ProteccionesActivas ? "Activas" : "No activas";
                        item.SubItems[5].Text = "En línea";
                        item.SubItems[6].Text = (cfg.UltimaSincro.Equals(DateTime.MinValue) ? "" : cfg.UltimaSincro.ToString(formatoFecha)) +
                            (cfg.HoraSincro.Equals(TimeSpan.MinValue) ? "" : " " + cfg.HoraSincro.ToString().Substring(0, 5));

                        if (cfg.Estado == "Estandar")
                        {
                            item.ImageIndex = 1;
                        }
                        else
                        {
                            item.ImageIndex = 0;
                        }
                        item.ForeColor = Color.Black;

                        if (edoRemoto.ContainsKey(idEstacion))
                        {
                            edoRemoto[idEstacion] = cfg.EstadoRemoto;
                        }
                        else
                        {
                            edoRemoto.Add(idEstacion, cfg.EstadoRemoto);
                        }
                    }
                    else
                    {
                        string texto = "Sin conexión";

                        if (cfg != null && !Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                            texto = "Licencia Inválida";

                        item.SubItems[5].Text = texto;
                        item.ImageIndex = 2;
                        item.ForeColor = Color.Gray;
                    }

                    if (item.Selected)
                    {
                        ActivarDesactivarMenu();
                    }
                    //Application.DoEvents(); 
                }
            }
        }

        private void bwConectarEstaciones_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void bwConectarEstaciones_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            itRefresh.Enabled = true;
            tiEstaciones.Enabled = Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32(tiEstaciones.Tag));
            scanFree = true;
        }

        private void mnuListView_Opening(object sender, CancelEventArgs e)
        {
            ListViewItem itm = lvEstaciones.SelectedItems != null && lvEstaciones.SelectedItems.Count > 0 ? lvEstaciones.SelectedItems[0] : null;
            int itmSel = -1;

            if (itm != null)
            {
                itmSel = ObtenerIdSeleccionado();

                EdoRemoto edo = edoRemoto.ContainsKey(itmSel) ? edoRemoto[itmSel] : EdoRemoto.SinLicencia;

                itmSeparaRemoto.Visible = itmConfRemoto.Visible = edo != EdoRemoto.SinLicencia;
            }

            itmConectar.Enabled = lvEstaciones.SelectedItems.Count > 0 && lvEstaciones.SelectedItems[0].SubItems[5].Text != "En línea" && lvEstaciones.SelectedItems[0].SubItems[5].Text != "Conectando";
            itmActualizar.Enabled = lvEstaciones.SelectedItems.Count > 0 && lvEstaciones.SelectedItems[0].SubItems[5].Text == "En línea";

            if (lvEstaciones.SelectedItems.Count > 0)
            {
                ListViewItem item = lvEstaciones.SelectedItems[0];
                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);

                itmMostrarError.Enabled = erroresActualizacion.ContainsKey(idEstacion);
            }
        }

        private void itmConectar_Click(object sender, EventArgs e)
        {
            scanFree = false;

            if (lvEstaciones.SelectedItems.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                ListViewItem item = lvEstaciones.SelectedItems[0];

                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);
                Estacion est = Configuraciones.Estaciones.Find(s => { return s.Id == idEstacion; });
                item.SubItems[5].Text = "Conectando";
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                Servicios.Adicional.IServiciosAdicional serviciosAdicional = Configuraciones.ListaCanalesAdicional.ContainsKey(idEstacion) ? Configuraciones.ListaCanalesAdicional[idEstacion] : Configuraciones.AbrirCanalAdicional(idEstacion);
                ServiciosCliente.IServiciosCliente serviciosCliente = Configuraciones.ListaCanales.ContainsKey(idEstacion) ? Configuraciones.ListaCanales[idEstacion] : Configuraciones.AbrirCanalCliente(idEstacion);

                Configuracion cfg = null;
                try
                {
                    cfg = serviciosAdicional.ConfiguracionObtener(1);
                }
                catch (Exception)
                {
                    serviciosAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                }

                if (cfg != null)
                {
                    if (!Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion])
                    {
                        GuardarAccesoSalidaSistema(idEstacion, true);
                        Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion] = true;
                    }

                    item.SubItems[2].Text = cfg.Estado;
                    item.SubItems[3].Text = cfg.UltimoMovimiento.ToString(formatoFecha);
                    item.SubItems[4].Text = cfg.ProteccionesActivas ? "Activas" : "No activas";
                    item.SubItems[5].Text = "En línea";
                    if (cfg.Estado == "Estandar")
                    {
                        item.ImageIndex = 1;
                    }
                    else
                    {
                        item.ImageIndex = 0;
                    }
                    item.ForeColor = Color.Black;
                }
                else
                {
                    item.SubItems[5].Text = "Sin conexión";
                    item.ImageIndex = 2;
                    item.ForeColor = Color.Gray;
                }

                if (item.Selected)
                {
                    ActivarDesactivarMenu();
                }
                Application.DoEvents();
                Cursor.Current = Cursors.Default;
            }

            scanFree = true;
        }

        private void itmActualizar_Click(object sender, EventArgs e)
        {
            scanFree = false;

            if (lvEstaciones.SelectedItems.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                ListViewItem item = lvEstaciones.SelectedItems[0];

                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);
                Estacion est = Configuraciones.Estaciones.Find(s => { return s.Id == idEstacion; });
                item.SubItems[5].Text = "Actualizando";
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                Servicios.Adicional.IServiciosAdicional serviciosAdicional = Configuraciones.ListaCanalesAdicional.ContainsKey(idEstacion) ? Configuraciones.ListaCanalesAdicional[idEstacion] : Configuraciones.AbrirCanalAdicional(idEstacion);
                ServiciosCliente.IServiciosCliente serviciosCliente = Configuraciones.ListaCanales.ContainsKey(idEstacion) ? Configuraciones.ListaCanales[idEstacion] : Configuraciones.AbrirCanalCliente(idEstacion);

                Configuracion cfg = null;
                try
                {
                    cfg = serviciosAdicional.ConfiguracionObtener(1);
                }
                catch (Exception ex)
                {
                    // Guardar el error en el diccionario
                    if (erroresActualizacion.ContainsKey(idEstacion))
                    {
                        erroresActualizacion[idEstacion] = ex.Message;
                    }
                    else
                    {
                        erroresActualizacion.Add(idEstacion, ex.Message);
                    }
                    serviciosAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                }

                if (cfg != null && Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                {
                    if (!Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion])
                    {
                        GuardarAccesoSalidaSistema(idEstacion, true);
                        Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion] = true;
                    }

                    item.SubItems[2].Text = cfg.Estado;
                    item.SubItems[3].Text = cfg.UltimoMovimiento.ToString(formatoFecha);
                    item.SubItems[4].Text = cfg.ProteccionesActivas ? "Activas" : "No activas";
                    item.SubItems[5].Text = "En línea";
                    if (cfg.Estado == "Estandar")
                    {
                        item.ImageIndex = 1;
                    }
                    else
                    {
                        item.ImageIndex = 0;
                    }
                    item.ForeColor = Color.Black;
                }
                else
                {
                    string texto = "Sin conexión";

                    if (cfg != null && !Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                        texto = "Licencia Inválida";

                    item.SubItems[2].Text = string.Empty;
                    item.SubItems[3].Text = string.Empty;
                    item.SubItems[4].Text = string.Empty;
                    item.SubItems[5].Text = texto;
                    item.ImageIndex = 2;
                    item.ForeColor = Color.Gray;
                }

                if (item.Selected)
                {
                    ActivarDesactivarMenu();
                }
                Application.DoEvents();
                Cursor.Current = Cursors.Default;
            }

            scanFree = true;
        }

        private void itmMostrarError_Click(object sender, EventArgs e)
        {
            if (lvEstaciones.SelectedItems.Count > 0)
            {
                ListViewItem item = lvEstaciones.SelectedItems[0];
                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);

                if (erroresActualizacion.ContainsKey(idEstacion))
                {
                    MessageBox.Show(erroresActualizacion[idEstacion], "Errores Actualización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("No existen errores registrados", "Errores Actualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tmrScanConection_Tick(object sender, EventArgs e)
        {
            if (scanFree)
            {
                tmrScanConection.Stop();

                bwScanConections.RunWorkerAsync();
            }
        }

        private bool subirBajar(byte status, int estacion, string estadoActual)
        {
            bool resultado = false;
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[estacion];
            ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[estacion];
            Estacion estacionEnt = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ListaHistorial pListaHistorial = servicioAdicional.HistorialObtenerRecientes(estacion);

                if (status == 1)
                {
                    #region Subir
                    try
                    {
                        string pRespuesta = pServiciosCliente.AplicarFlujo(true, false, estacionEnt.TipoDispensario, (from h in pListaHistorial select h).ToList<Historial>());

                        if (pRespuesta.Length > 0)
                        {
                            Estacion pEstacionEntidad = Configuraciones.Estaciones.Find(p => { return p.Id == estacion; });
                            pEstacionEntidad.Estado = "Estandar";

                            pEstacionEntidad.UltimoMovimiento = servicioAdicional.ConfiguracionActualizarUltimoMovimiento(pEstacionEntidad.UltimoMovimiento);
                            new EstacionPersistencia().EstacionActualizar(pEstacionEntidad);
                            servicioAdicional.ConfiguracionCambiarEstado(pEstacionEntidad.Estado);
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Subir flujo" });

                            lvEstaciones.SelectedItems[0].SubItems[2].Text = pEstacionEntidad.Estado;
                            lvEstaciones.SelectedItems[0].SubItems[3].Text = pEstacionEntidad.UltimoMovimiento.ToString(formatoFecha);
                            lvEstaciones.SelectedItems[0].ImageIndex = 1;

                            resultado = true;
                        }
                        else
                        {
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Error al aplicar flujo" });
                            MessageBox.Show("Hubo un error al aplicar el flujo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Sincronizar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al aplicar el flujo." +
                                        " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                        lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(estacion);
                    }
                    #endregion
                }
                else
                {
                    #region Bajar
                    try
                    {
                        string pRespuesta = pServiciosCliente.AplicarFlujo(false, false, estacionEnt.TipoDispensario, (from h in pListaHistorial select h).ToList<Historial>());

                        if (pRespuesta.Length > 0)
                        {
                            Estacion pEstacionEntidad = Configuraciones.Estaciones.Find(p => { return p.Id == estacion; });
                            pEstacionEntidad.Estado = "Mínimo";

                            pEstacionEntidad.UltimoMovimiento = servicioAdicional.ConfiguracionActualizarUltimoMovimiento(pEstacionEntidad.UltimoMovimiento);
                            new EstacionPersistencia().EstacionActualizar(pEstacionEntidad);
                            servicioAdicional.ConfiguracionCambiarEstado(pEstacionEntidad.Estado);
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Bajar flujo" });

                            lvEstaciones.SelectedItems[0].SubItems[2].Text = pEstacionEntidad.Estado;
                            lvEstaciones.SelectedItems[0].SubItems[3].Text = pEstacionEntidad.UltimoMovimiento.ToString(formatoFecha);
                            lvEstaciones.SelectedItems[0].ImageIndex = 0;

                            resultado = true;
                        }
                        else
                        {
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Entrar al bajar flujo" });
                            MessageBox.Show("Hubo un error al aplicar el flujo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Sincronizar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al bajar el flujo." +
                            //"El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                        lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(estacion);
                    }
                    #endregion
                }

                this.MostrarEdoRemoto();
            }
            catch (Exception ex)
            {
                string nombreArchivo = string.Format("ERROR_Flujos({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                MessageBox.Show("Se ha detectado un error no clasificado al aplicar el flujo." +
                                " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                "Para más información verifique el archivo: " + nombreArchivo + ".",
                                "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);

                servicioAdicional = Configuraciones.AbrirCanalAdicional(estacion);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                //LlenaLista();
            }

            return resultado;
        }

        private void itmConfRemoto_Click(object sender, EventArgs e)
        {
            frmConfRemoto frm = new frmConfRemoto();

            frm.ShowDialog();
        }

        private void ActivarDesactivarMenu()
        {
            this.txtUsuario.Text = Configuraciones.NombreUsuario;
            ListViewItem itm = lvEstaciones.SelectedItems != null && lvEstaciones.SelectedItems.Count > 0 ? lvEstaciones.SelectedItems[0] : null;
            bool SeleccionadoyEnLinea = itm != null && itm.SubItems[5].Text.Equals("En línea", StringComparison.OrdinalIgnoreCase);
            itSalir.Visible = true;
            /**/
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (Configuraciones.IdUsuario == 1)
                {
                    item.Enabled = true;
                    continue;
                }

                bool itemsExcluidos = item.Name.Equals("itSalir") ||
                                      item.Name.Equals("itRefresh") ||
                                      item.GetType().Equals(typeof(ToolStripSeparator));

                if (!itemsExcluidos)
                {
                    if (item.Equals(tiUsurios))
                    {
                        item.Enabled = Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32(item.Tag));
                    }
                    else if (SeleccionadoyEnLinea)
                    {
                        item.Enabled = Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32(item.Tag));
                    }
                    else
                    {
                        item.Enabled = item.Name.Equals("tiEstaciones") && !bwConectarEstaciones.IsBusy;
                    }
                }

                if (ConfigurationManager.AppSettings["ModoOculto"] != "Si" && item.Name.Equals("itSalir"))
                    item.Visible = false;
            }/**/
        }

        private void GuardarAccesoSalidaSistema(int idEstacion, bool acceso)
        {
            Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[idEstacion];
            try
            {
                servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = acceso ? "Entrar al Sistema" : "Salir del Sistema" });
            }
            catch (Exception)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
            }
        }

        private void lvEstaciones_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bwScanConections_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (ListViewItem item in lvEstaciones.Items)
            {
                int idEstacion = Convert.ToInt32(item.SubItems[0].Text);
                Estacion est = Configuraciones.Estaciones.Find(s => { return s.Id == idEstacion; });

                if (est != null)
                {
                    Servicios.Adicional.IServiciosAdicional serviciosAdicional = Configuraciones.ListaCanalesAdicional.ContainsKey(idEstacion) ? Configuraciones.ListaCanalesAdicional[idEstacion] : Configuraciones.AbrirCanalAdicional(idEstacion);
                    ServiciosCliente.IServiciosCliente serviciosCliente = Configuraciones.ListaCanales.ContainsKey(idEstacion) ? Configuraciones.ListaCanales[idEstacion] : Configuraciones.AbrirCanalCliente(idEstacion);

                    Configuracion cfg = null;
                    try
                    {
                        cfg = serviciosAdicional.ConfiguracionObtener(1);

                    }
                    catch (Exception ex)
                    {
                        // Guardar el error en el diccionario
                        if (erroresActualizacion.ContainsKey(idEstacion))
                        {
                            erroresActualizacion[idEstacion] = ex.Message;
                        }
                        else
                        {
                            erroresActualizacion.Add(idEstacion, ex.Message);
                        }

                        serviciosAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                    }

                    if (cfg != null && Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                    {
                        // si existe un error guardado, eliminarlo
                        if (erroresActualizacion.ContainsKey(idEstacion))
                        {
                            erroresActualizacion.Remove(idEstacion);
                        }

                        if (!Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion])
                        {
                            GuardarAccesoSalidaSistema(idEstacion, true);
                            Configuraciones.EstacionBitacoraGuardarAcceso[idEstacion] = true;
                        }

                        item.SubItems[2].Text = cfg.Estado;
                        item.SubItems[3].Text = cfg.UltimoMovimiento.ToString(formatoFecha);
                        item.SubItems[4].Text = cfg.ProteccionesActivas ? "Activas" : "No activas";
                        item.SubItems[5].Text = "En línea";
                        item.SubItems[6].Text = (cfg.UltimaSincro.Equals(DateTime.MinValue) ? "" : cfg.UltimaSincro.ToString(formatoFecha)) +
                            (cfg.HoraSincro.Equals(TimeSpan.MinValue) ? "" : " " + cfg.HoraSincro.ToString().Substring(0, 5));
                        if (!tmrWayne.Enabled)
                        {
                            if (cfg.Estado == "Estandar")
                            {
                                item.ImageIndex = 1;
                            }
                            else
                            {
                                item.ImageIndex = 0;
                            }
                        }

                        item.ForeColor = Color.Black;

                        if (edoRemoto.ContainsKey(idEstacion))
                        {
                            edoRemoto[idEstacion] = cfg.EstadoRemoto;
                        }
                        else
                        {
                            edoRemoto.Add(idEstacion, cfg.EstadoRemoto);
                        }
                    }
                    else
                    {
                        string texto = "Sin conexión";

                        if (cfg != null && !Configuraciones.LicenciaValida(idEstacion, Configuraciones.claveSistema))
                            texto = "Licencia Inválida";

                        item.SubItems[2].Text = string.Empty;
                        item.SubItems[3].Text = string.Empty;
                        item.SubItems[4].Text = string.Empty;
                        item.SubItems[5].Text = texto;
                        item.ImageIndex = 2;
                        item.ForeColor = Color.Gray;

                        if (edoRemoto.ContainsKey(idEstacion))
                        {
                            edoRemoto[idEstacion] = EdoRemoto.Desconectado;
                        }
                        else
                        {
                            edoRemoto.Add(idEstacion, EdoRemoto.Desconectado);
                        }
                    }

                    if (item.Selected)
                    {
                        ActivarDesactivarMenu();
                        this.MostrarEdoRemoto();
                    }
                }
            }
        }

        private void bwScanConections_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tmrScanConection.Start();
        }

        private void tiModificarTicket_Click(object sender, EventArgs e)
        {
            if (Configuraciones.ListaDerechos.ContainsKey(25) || Configuraciones.IdUsuario == 1)
            {
                try
                {
                    using (frmTicketsModificar ticket = new frmTicketsModificar())
                    {
                        ticket.Id = ObtenerIdSeleccionado();
                        ticket.Text = ticket.Text + " " + ObtenerNombreEstSeleccionada();
                        if (ticket.ShowDialog() == DialogResult.OK)
                        {
                            MessageBox.Show("Terminado...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Usuario no puede realizar esta acción.", "No cuenta con derecho de modificar tickets", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tiRegistrarTicket_Click(object sender, EventArgs e)
        {
            if (Configuraciones.ListaDerechos.ContainsKey(24) || Configuraciones.IdUsuario == 1)
            {
                try
                {
                    using (frmTicketsRegistrar ticket = new frmTicketsRegistrar())
                    {
                        ticket.Id = ObtenerIdSeleccionado();
                        ticket.Text = ticket.Text + " " + ObtenerNombreEstSeleccionada();
                        if (ticket.ShowDialog() == DialogResult.OK)
                        {
                            MessageBox.Show("Terminado...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Usuario no puede realizar esta acción.", "No cuenta con derecho de registar tickets", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tiTanques_Click(object sender, EventArgs e)
        {
            if (Configuraciones.ListaDerechos.ContainsKey(26) || Configuraciones.IdUsuario == 1)
            {
                using (frmTanquesListado tanques = new frmTanquesListado())
                {
                    tanques.Id = ObtenerIdSeleccionado();
                    tanques.Text = tanques.Text + " " + ObtenerNombreEstSeleccionada();
                    tanques.ShowDialog();
                }
            }
            else
                MessageBox.Show("Usuario no puede realizar esta acción.", "No cuenta con derecho de acceder a tanques", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MostrarEdoRemoto()
        {
            EdoRemoto edo;
            ListViewItem itm = lvEstaciones.SelectedItems != null && lvEstaciones.SelectedItems.Count > 0 ? lvEstaciones.SelectedItems[0] : null;
            int itmSel = -1;

            if (itm != null)
            {
                itmSel = ObtenerIdSeleccionado();
                edo = edoRemoto.ContainsKey(itmSel) ? edoRemoto[itmSel] : EdoRemoto.SinLicencia;
                itmEdoRemoto.Visible = edo != EdoRemoto.SinLicencia;

                switch (edo)
                {
                    case EdoRemoto.SinLicencia:
                        itmEdoRemoto.Visible = false;
                        break;
                    case EdoRemoto.Pulsado:
                    case EdoRemoto.VisualEncendido:
                        itmEdoRemoto.Image = NuevoAdicional.Properties.Resources.bullet_ball_glass_green;
                        break;
                    case EdoRemoto.Desconectado:
                        itmEdoRemoto.Image = NuevoAdicional.Properties.Resources.plug_delete;
                        break;
                    case EdoRemoto.VisualApagado:
                    default:
                        itmEdoRemoto.Image = NuevoAdicional.Properties.Resources.bullet_ball_glass_gray;
                        break;
                }
            }
            else
            {
                itmEdoRemoto.Visible = false;
            }
        }

        private void tmrVerifica_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrVerifica.Enabled = false;

                string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
                ruta = System.IO.Path.GetDirectoryName(ruta);
                if (System.IO.File.Exists(ruta + @"\levanta.txt"))
                {
                    System.IO.File.Delete(ruta + @"\levanta.txt");
                    notifyIcon1_MouseClick(null, null);
                }

                if (Configuraciones.ListaCanales.Count == 0)
                    return;
                ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[1];
                if (!Configuraciones.CanalEstaActivo(1, false))
                {
                    pServiciosCliente = Configuraciones.AbrirCanalCliente(1);
                }
                string Estatus = "";
                try
                {
                    Estatus = pServiciosCliente.ObtenerEstatus();
                }
                catch
                {
                    if (Estatus == "")
                        return;
                }

                string ComandosPorServicio;
                if (!Utilerias.ObtenerListaVar().TryGetValue("ComandosPorServicio", out ComandosPorServicio))
                    ComandosPorServicio = "No";
                if (Estatus == "Estandar")
                {
                    if (Process.GetProcessesByName("PDISMENUX").Length > 0)
                        return;
                    if (ComandosPorServicio != "Si")
                    {
                        Comandos comando = new Comandos();
                        comando.Modulo = "DISP";
                        comando.Comando = "CERRAR";
                        pServiciosCliente.ComandoInsertar(comando);
                    }
                    else if (Process.GetProcessesByName("PDISMENU").Length > 0)
                    {
                        string servConsola;
                        if (Utilerias.ObtenerListaVar().TryGetValue("PuertoServicio", out servConsola))
                            servConsola = "http://127.0.0.1:9199/bin/";
                        try
                        {
                            new ServicioDisp(servConsola).EjecutaComando("CERRAR");
                        }
                        catch
                        { }
                    }

                    while (true)
                    {
                        if (Process.GetProcessesByName("PDISMENU").Length == 0)
                            break;
                    }
                    Process p = new Process();
                    p.StartInfo.FileName = ConfigurationManager.AppSettings["Consola3"];
                    p.StartInfo.Arguments = ConfigurationManager.AppSettings["AliasConsola"];
                    p.Start();
                }
                else
                {
                    if (Process.GetProcessesByName("PDISMENU").Length > 0)
                        return;
                    if (ComandosPorServicio != "Si")
                    {
                        Comandos comando = new Comandos();
                        comando.Modulo = "DISP";
                        comando.Comando = "GLOG";
                        pServiciosCliente.ComandoInsertar(comando);
                        comando.Modulo = "DISP";
                        comando.Comando = "CERRAR";
                        pServiciosCliente.ComandoInsertar(comando);
                    }
                    //else
                    //{
                    //    string servConsola;
                    //    if (Utilerias.ObtenerListaVar().TryGetValue("PuertoServicio", out servConsola))
                    //        servConsola = "http://127.0.0.1:9199/bin/";
                    //    try
                    //    {
                    //        new ServicioDisp(servConsola).EjecutaComando("CERRAR");
                    //    }
                    //    catch
                    //    { }
                    //}

                    while (true)
                    {
                        if (Process.GetProcessesByName("PDISMENUX").Length == 0)
                            break;
                    }

                    Process p = new Process();
                    p.StartInfo.FileName = ConfigurationManager.AppSettings["Consola4"];
                    p.StartInfo.Arguments = ConfigurationManager.AppSettings["AliasConsola"];
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                string nombreArchivo = string.Format("ERROR_Inicio({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                MessageBox.Show("Se ha detectado un error no clasificado." +
                                "Para más información verifique el archivo: " + nombreArchivo + ".",
                                "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + " " + ex.Source + " " + ex.TargetSite + " " + ex.StackTrace);
            }
            finally
            {
                tmrVerifica.Enabled = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ConfigurationManager.AppSettings["Consola3"] != null)
            {
                this.Visible = false;
                if (ConfigurationManager.AppSettings["ModoOculto"] != "Si")
                    notifyIcon1.Visible = true;
                e.Cancel = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            var pForma = new frmLogin();
            if (/*ConfigurationManager.AppSettings["InicioAuto"] != "Si" ||*/ pForma.ShowDialog() == DialogResult.OK)
            {
                this.Visible = true;
                notifyIcon1.Visible = false;
                itmConectar_Click(sender, e);
            }
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Configuraciones.ListaDerechos.ContainsKey(23) || Configuraciones.IdUsuario == 1)
                Application.ExitThread();
            else
                MessageBox.Show("Usuario no puede realizar esta acción.", "No cuenta con derecho de cerrar aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void generarPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();

            guardar.Filter = "Archivos PDF (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
            guardar.FilterIndex = 2;
            guardar.RestoreDirectory = true;
            guardar.FileName = "Reporte.pdf";

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                string archivo = guardar.FileName;

                if (archivo.Substring(archivo.Length - 4, 4).ToLower() != ".pdf")
                    archivo += ".pdf";

                Document document = new Document(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Height, iTextSharp.text.PageSize.LETTER.Width));
                PdfWriter.GetInstance(document, new FileStream(archivo, FileMode.OpenOrCreate));
                document.Open();
                document.Add(new Paragraph("Estación                             Estado           Último Movimiento       Protecciones       Edo. Conexión" +
                       "      Última sincronización\n\n", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                foreach (ListViewItem item in lvEstaciones.Items)
                {
                    document.Add(new Paragraph(String.Format("{0,-31} {1,-16} {2,-29} {3,-23} {4,-25} {5,-30}\n", item.SubItems[0].Text + " " + item.SubItems[1].Text, item.SubItems[2].Text,
                        item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text, item.SubItems[6].Text)));
                }
                document.Add(new Paragraph(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                document.Add(new Paragraph(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                document.Add(new Paragraph(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                document.Add(new Paragraph(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                document.Add(new Paragraph("Hora de impresión: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
                document.Close();
            }
        }

        private void tmrSinc_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour != Convert.ToInt32(ConfigurationManager.AppSettings["HoraSinc"]))
                return;
            int pEstacion;
            foreach (ListViewItem item in lvEstaciones.Items)
            {
                pEstacion = Convert.ToInt32(item.Text);
                if (pEstacion > 0)
                {
                    Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    string estadoActual = item.SubItems[2].Text;

                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        item.SubItems[2].Text = "Sincronizando";
                        Application.DoEvents();
                        Cursor.Current = Cursors.WaitCursor;

                        servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Sincronizar Estación" });
                        //Sincronizar
                        try
                        {
                            byte status = (byte)(estadoActual.Equals("Mínimo", StringComparison.OrdinalIgnoreCase) ? 0 : 1);
                            string mensaje = string.Empty;
                            servicioAdicional.ConfiguracionActualizarUltimaSincronizacion(DateTime.Now);
                            bool resSicronizar = pServiciosCliente.Sincronizar(status, out mensaje);

                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                if (!resSicronizar)
                                {
                                    string edoAdicional = estadoActual;
                                    string edoDisp = estadoActual.Equals("Mínimo") ? "Estandar" : "Mínimo";
                                    bool restablecido = true;

                                    servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Diferencia de sincronización (Adicional: " + edoAdicional + " Dispensario: " + edoDisp + ")" });
                                    restablecido = subirBajar(status, pEstacion, estadoActual);
                                }
                            }
                            else
                            {
                                item.SubItems[2].Text = estadoActual;
                            }
                        }
                        catch (Exception ex)
                        {
                            string nombreArchivo = string.Format("ERROR_Sincronizar({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                            Configuraciones.GuardarMensaje(nombreArchivo, ex.Message);
                            pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                        }

                    }
                    catch (Exception)
                    {
                        item.SubItems[2].Text = estadoActual;
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            Configuraciones.Inicializar();
            ActivarDesactivarMenu();
            LlenaLista();
        }

        private void tiActProtecc_Click(object sender, EventArgs e)
        {

            int pEstacion = ObtenerIdSeleccionado();

            if (pEstacion > 0)
            {
                if (MessageBox.Show("¿Activar la protección de la estación seleccionada?", "Protección", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                Servicios.Adicional.IServiciosAdicional servicioAdicional;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string estadoActual = lvEstaciones.SelectedItems[0].SubItems[2].Text;
                    lvEstaciones.SelectedItems[0].SubItems[2].Text = "Activando";
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;

                    servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }

                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
                        string pRespuesta = pServiciosCliente.AplicarProteccionGilbarco(true, tiComboBoxProt.Text);
                        if (pRespuesta.Equals("Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            new ConfiguracionPersistencia().ConfiguracionActivarProtecciones(estacion.Id, true);
                            MessageBox.Show("Protección activada con éxito", "Protección activada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Error al activar protección" });
                            string mensajeError = string.Concat("Hubo un error al activar la proteción.\r\nEl mensaje de la consola fue: ", pRespuesta);
                            MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Protecciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al activar la protección." +
                                        " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }
                }
                catch (Exception ex)
                {
                    string nombreArchivo = string.Format("ERROR_Protecciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                    MessageBox.Show("Se ha detectado un error no clasificado al activar la protección." +
                                    " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                    "Para más información verifique el archivo: " + nombreArchivo + ".",
                                    "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    //LlenaLista();
                }
            }
        }

        private void tiDesProtecc_Click(object sender, EventArgs e)
        {
            int pEstacion = ObtenerIdSeleccionado();

            if (pEstacion > 0)
            {
                if (MessageBox.Show("¿Desactivar la protección de la estación seleccionada?", "Protección", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                Servicios.Adicional.IServiciosAdicional servicioAdicional;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string estadoActual = lvEstaciones.SelectedItems[0].SubItems[2].Text;
                    lvEstaciones.SelectedItems[0].SubItems[2].Text = "Desactivando";
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;

                    servicioAdicional = Configuraciones.ListaCanalesAdicional[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, true))
                    {
                        servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                    }

                    ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                    if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                    {
                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }

                    try
                    {
                        Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == ObtenerIdSeleccionado(); });
                        string pRespuesta = pServiciosCliente.AplicarProteccionGilbarco(false, tiComboBoxProt.Text);
                        if (pRespuesta.Equals("Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            new ConfiguracionPersistencia().ConfiguracionActivarProtecciones(estacion.Id, false);
                            MessageBox.Show("Protección desactivada con éxito", "Protección desactivada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            lvEstaciones.SelectedItems[0].SubItems[2].Text = estadoActual;
                            servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Error al desactivar protección" });
                            string mensajeError = string.Concat("Hubo un error al aplicar la proteción.\r\nEl mensaje de la consola fue: ", pRespuesta);
                            MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string nombreArchivo = string.Format("ERROR_Protecciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                        MessageBox.Show("Se ha detectado un error no clasificado al desactivar la protección." +
                                        " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                        "Para más información verifique el archivo: " + nombreArchivo + ".",
                                        "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                        pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                    }
                }
                catch (Exception ex)
                {
                    string nombreArchivo = string.Format("ERROR_Protecciones({0}).txt", DateTime.Now.ToString("yyMMddHHmmss"));
                    MessageBox.Show("Se ha detectado un error no clasificado al desactivar la protección." +
                                    " El mensaje original del error se ha almacenado en la bitácora.\n" +
                                    "Para más información verifique el archivo: " + nombreArchivo + ".",
                                    "Error no clasificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Configuraciones.GuardarMensaje(nombreArchivo, ex.Message + ex.TargetSite + ex.StackTrace);

                    servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    //LlenaLista();
                }
            }
        }

        private void tiLecturasTanques_Click(object sender, EventArgs e)
        {
            if (Configuraciones.ListaDerechos.ContainsKey(29) || Configuraciones.IdUsuario == 1)
            {
                try
                {
                    using (frmLectTanques lectura = new frmLectTanques())
                    {
                        lectura.Id = ObtenerIdSeleccionado();
                        if (lectura.Id == -1)
                            return;
                        lectura.Text = lectura.Text + " " + ObtenerNombreEstSeleccionada();
                        if (lectura.ShowDialog(this) == DialogResult.OK)
                        {
                            MessageBox.Show("Terminado...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Usuario no puede realizar esta acción.", "No cuenta con derecho de registar lecturas de tanques", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tmrWayne_Tick(object sender, EventArgs e)
        {
            int pEstacion;
            foreach (ListViewItem item in lvEstaciones.Items)
            {
                pEstacion = Convert.ToInt32(item.Text);
                if (pEstacion > 0)
                {
                    try
                    {
                        string resp = string.Empty;
                        Estacion estacion = Configuraciones.Estaciones.Find(p => { return p.Id == pEstacion; });

                        if (ConfigurationManager.AppSettings["ModoGateway"].ToUpper() == "SI")
                        {
                            ServiceController sc = new ServiceController(ConfigurationManager.AppSettings["ServicioX"]);
                            if (sc != null && sc.Status == ServiceControllerStatus.Stopped && estacion.EstadoPresetWayne == EstatusPresetWayne.EsperandoMinimo)
                            {
                                sc.Close();
                                resp = "OK";
                            }
                            else
                            {
                                ServiciosCliente.IServiciosCliente pServiciosCliente = Configuraciones.ListaCanales[pEstacion];
                                if (!Configuraciones.CanalEstaActivo(pEstacion, false))
                                {
                                    pServiciosCliente = Configuraciones.AbrirCanalCliente(pEstacion);
                                }

                                try
                                {
                                    resp = pServiciosCliente.SeguimientoRspCmnd(pServiciosCliente.ComandoSocket("DISPENSERSX|EJECCMND|ESTADI"), true).ToUpper();
                                }
                                catch
                                {
                                    resp = "OK";
                                }
                            }
                        }
                        else
                        {
                            string servConsola;
                            if (!Utilerias.ObtenerListaVar().TryGetValue("PuertoServicio", out servConsola))
                                servConsola = "http://127.0.0.1:9199/bin/";

                            if (Process.GetProcessesByName("PDISMENUX").Length == 0 && estacion.EstadoPresetWayne == EstatusPresetWayne.EsperandoMinimo)
                                resp = "OK";
                            else
                            {
                                try
                                {
                                    resp = new ServicioDisp(servConsola).EjecutaComando("ESTADI");
                                }
                                catch
                                {
                                    resp = string.Empty;
                                }
                            }
                        }


                        if (resp == "OK" && estacion.EstadoPresetWayne != EstatusPresetWayne.Inactivo)
                        {
                            if (estacion.EstadoPresetWayne == EstatusPresetWayne.EsperandoEstandar)
                            {
                                item.SubItems[2].Text = estacion.Estado;
                                item.SubItems[3].Text = estacion.UltimoMovimiento.ToString(formatoFecha);
                                item.ImageIndex = 1;
                                tmrWayne.Enabled = false;
                                estacion.EstadoPresetWayne = EstatusPresetWayne.Inactivo;
                                tmrSinc.Enabled = ConfigurationManager.AppSettings["HoraSinc"] != null;
                            }
                            else
                            {
                                if (ConfigurationManager.AppSettings["ModoGateway"].ToUpper() == "SI")
                                {
                                    ServiceController sc = new ServiceController(ConfigurationManager.AppSettings["ServicioOpengas"]);
                                    if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                                        sc.Start();
                                    sc.WaitForStatus(ServiceControllerStatus.Running);
                                    sc.Close();
                                }

                                estacion.Estado = "Mínimo";
                                item.SubItems[2].Text = estacion.Estado;
                                item.SubItems[3].Text = estacion.UltimoMovimiento.ToString(formatoFecha);
                                item.ImageIndex = 0;
                                tmrWayne.Enabled = false;
                                estacion.EstadoPresetWayne = EstatusPresetWayne.Inactivo;
                                new EstacionPersistencia().EstacionActualizar(estacion);

                                Servicios.Adicional.IServiciosAdicional servicioAdicional;
                                servicioAdicional = Configuraciones.AbrirCanalAdicional(pEstacion);
                                servicioAdicional.ConfiguracionCambiarEstado(estacion.Estado);

                                tmrSinc.Enabled = ConfigurationManager.AppSettings["HoraSinc"] != null;
                                tmrVerifica.Enabled = ConfigurationManager.AppSettings["Consola3"] != null;
                            }
                        }

                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }
    }
}
