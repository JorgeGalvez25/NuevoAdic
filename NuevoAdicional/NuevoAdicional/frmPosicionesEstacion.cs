using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using ServiciosCliente;
using Adicional.Entidades;
using System.ServiceModel;
using System.Diagnostics;

namespace NuevoAdicional
{
    public partial class frmPosicionesEstacion : Form
    {
        private int idEstacion;
        private Estacion estacion;
        string tipoClb;
        private Servicios.Adicional.IServiciosAdicional servicioAdicional;
        private decimal porcentajeMaximo
        {
            get
            {
                switch (estacion.TipoDispensario)
                {
                    case MarcaDispensario.Ninguno:
                        return 0;
                    case MarcaDispensario.Wayne:
                        return 9;
                    case MarcaDispensario.Bennett:
                        return (decimal)9.99;
                    case MarcaDispensario.Team:
                        return 10;
                    case MarcaDispensario.Gilbarco:
                        return 9;
                    case MarcaDispensario.HongYang:
                        return (decimal)9.99;
                    default:
                        return 0;
                }
            }
        }

        public bool AplicarCambios { get; private set; }

        public frmPosicionesEstacion(int idEstacion)
        {
            InitializeComponent();
            this.idEstacion = idEstacion;

            AplicarCambios = false;
            servicioAdicional = Configuraciones.ListaCanalesAdicional[this.idEstacion];
            estacion = Configuraciones.Estaciones.Find(p => { return p.Id == idEstacion; });

            tiPorcentajeCalibracion.Visible = estacion.TipoDispensario == MarcaDispensario.Bennett;

            if (!Utilerias.ObtenerListaVar().TryGetValue("TipoClb", out tipoClb))
                tipoClb = "0";

            llenarLista();
        }

        #region Llenar listado
        private void llenarLista()
        {
            List<int> listaPosiciones = null;

            treeView1.Nodes.Clear();

            try
            {
                listaPosiciones = servicioAdicional.HistorialObtenerPosiciones(this.idEstacion);

                if (listaPosiciones == null)
                    return;

                switch (estacion.TipoDispensario)
                {
                    case MarcaDispensario.Ninguno:
                        break;
                    case MarcaDispensario.Wayne:
                        llenarListaWayne(listaPosiciones);
                        break;
                    case MarcaDispensario.Bennett:
                        treeView1.ContextMenuStrip = contextMenuStrip1;
                        llenarListaBennet(listaPosiciones);
                        break;
                    case MarcaDispensario.Team:
                        llenarListaTeam(listaPosiciones);
                        break;
                    case MarcaDispensario.Gilbarco:
                        llenarListaGilbarco(listaPosiciones);
                        break;
                    case MarcaDispensario.HongYang:
                        llenarListaBennet(listaPosiciones);
                        break;
                    default:
                        break;
                }

                treeView1.ExpandAll();
            }
            catch (Exception)
            {
                servicioAdicional = Configuraciones.AbrirCanalAdicional(this.idEstacion);
            }
        }

        private void llenarListaWayne(List<int> listaPosiciones)
        {
            string[] nombresCombustibles = new string[] { "", "Magna", "Premium", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();
            Dictionary<int, bool> posCarga = new Dictionary<int, bool>();

            listaPosiciones.ForEach(p =>
                {
                    ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, p);

                    if (ConfigurationManager.AppSettings["ModoPresetWayne"] == "Si")
                    {
                        mangueras.ForEach(m =>
                        {
                            if (!posCarga.ContainsKey(m.Posicion))
                            {
                                posCarga.Add(m.Posicion, true);

                                TreeNode nodo = new TreeNode(string.Format("{0} ({1}%)", "Posición: " + m.Posicion.ToString(), m.Porcentaje.ToString("##,#0")));
                                nodo.ImageIndex = 0;
                                nodo.SelectedImageIndex = 0;
                                nodo.Tag = m;
                                treeView1.Nodes.Add(nodo);
                            }
                        });
                    }
                    else
                    {
                        mangueras.ForEach(m =>
                        {
                            if (!combustibles.ContainsKey(m.Combustible))
                            {
                                combustibles.Add(m.Combustible, true);

                                TreeNode nodo = new TreeNode(string.Format("{0} ({1}%)", nombresCombustibles[m.Combustible], m.Porcentaje.ToString("##,#0")));
                                nodo.ImageIndex = m.Combustible;
                                nodo.SelectedImageIndex = m.Combustible;
                                nodo.Tag = m;
                                treeView1.Nodes.Add(nodo);
                            }
                        });
                    }
                });
        }

        private void llenarListaBennet(List<int> listaPosiciones)
        {
            foreach (int posicion in listaPosiciones)
            {
                TreeNode nodo = new TreeNode(string.Format("Posición: {0}", posicion.ToString("00")));
                nodo.ImageIndex = 0;
                ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, posicion);

                foreach (Historial manguera in mangueras)
                {
                    TreeNode cnodo = new TreeNode();
                    if (manguera.Calibracion == 0)
                        cnodo.Text = string.Format("Manguera {0} ({1}%)", manguera.Manguera.ToString("00"), manguera.Porcentaje.ToString("##,#0.00"));
                    else
                        cnodo.Text = string.Format("Manguera {0} ({1}%)   Calibración ({2})", manguera.Manguera.ToString("00"), manguera.Porcentaje.ToString("##,#0.00"), manguera.Calibracion.ToString("##,#0"));
                    cnodo.ImageIndex = manguera.Combustible;
                    cnodo.SelectedImageIndex = manguera.Combustible;
                    cnodo.Tag = manguera;
                    nodo.Nodes.Add(cnodo);
                    if (manguera.Estado == "Fuera")
                    {
                        nodo.ImageIndex = 5;
                        nodo.SelectedImageIndex = 5;
                    }
                }

                treeView1.Nodes.Add(nodo);
            }
        }

        private void llenarListaTeam(List<int> listaPosiciones)
        {
            int dispensario = 1;
            listaPosiciones.ForEach(p =>
            {
                if (p % 2 == 1 || ConfigurationManager.AppSettings["TeamPorPosicion"] == "Si")
                {
                    ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, p);
                    TreeNode nodo = new TreeNode(string.Format("Dispensario: {0} ({1}%)", (dispensario++).ToString("00"), mangueras[0].Porcentaje.ToString("##,#0")));

                    nodo.Tag = mangueras[0];
                    nodo.ImageIndex = 0;
                    treeView1.Nodes.Add(nodo);
                }
            });
        }

        private void llenarListaGilbarco(List<int> listaPosiciones)
        {
            Dictionary<int, bool> posCarga = new Dictionary<int, bool>();
            if (ConfigurationManager.AppSettings["GilbarcoOnOff"] == "Si" || tipoClb == "2")
            {
                foreach (int posicion in listaPosiciones)
                {
                    TreeNode nodo = new TreeNode(string.Format("Posición: {0}", posicion.ToString("00")));
                    nodo.ImageIndex = 0;
                    ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, posicion);

                    foreach (Historial manguera in mangueras)
                    {
                        TreeNode cnodo = new TreeNode();
                        if (manguera.Calibracion == 0)
                            cnodo.Text = string.Format("Manguera {0} ({1}%)", manguera.Manguera.ToString("00"), manguera.Porcentaje.ToString("##,#0.00"));
                        else
                            cnodo.Text = string.Format("Manguera {0} ({1}%)   Calibración ({2})", manguera.Manguera.ToString("00"), manguera.Porcentaje.ToString("##,#0.00"), manguera.Calibracion.ToString("##,#0"));
                        cnodo.ImageIndex = manguera.Combustible;
                        cnodo.SelectedImageIndex = manguera.Combustible;
                        cnodo.Tag = manguera;
                        nodo.Nodes.Add(cnodo);
                        if (manguera.Estado == "Fuera")
                        {
                            nodo.ImageIndex = 5;
                            nodo.SelectedImageIndex = 5;
                        }
                    }

                    treeView1.Nodes.Add(nodo);
                }
            }
            if (tipoClb == "7")
            {
                listaPosiciones.ForEach(p =>
                {
                    ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, p);
                    mangueras.ForEach(m =>
                    {
                        if (!posCarga.ContainsKey(m.Posicion) && m.Posicion % 2 != 0)
                        {
                            posCarga.Add(m.Posicion, true);

                            TreeNode nodo = new TreeNode(string.Format("{0} ({1}%)", "Dispensario: " + ((Convert.ToDouble(m.Posicion) / 2) + 0.5).ToString(), m.Porcentaje.ToString("##,#0")));
                            nodo.ImageIndex = 0;
                            nodo.SelectedImageIndex = 0;
                            nodo.Tag = m;
                            treeView1.Nodes.Add(nodo);
                        }
                    });
                });
            }
            else
            {
                string[] nombresCombustibles = new string[] { "", "Gasolina", "Gasolina", "Diesel" };
                Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

                listaPosiciones.ForEach(p =>
                {
                    ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, p);

                    mangueras.ForEach(m =>
                    {
                        if (!combustibles.ContainsKey(m.Combustible))
                        {
                            if (m.Combustible == 1 || m.Combustible == 2)
                                m.Combustible = 1;
                            if (!combustibles.ContainsKey(m.Combustible))
                            {
                                combustibles.Add(m.Combustible, true);

                                TreeNode nodo = new TreeNode(string.Format("{0} ({1}%)", nombresCombustibles[m.Combustible], m.Porcentaje.ToString("##,#0")));
                                nodo.ImageIndex = m.Combustible;
                                nodo.SelectedImageIndex = m.Combustible;
                                nodo.Tag = m;
                                treeView1.Nodes.Add(nodo);
                            }
                        }
                    });
                });
            }
        }

        private void llenarListaHongYang(List<int> listaPosiciones)
        {
            foreach (int posicion in listaPosiciones)
            {
                TreeNode nodo = new TreeNode(string.Format("CPU: {0}", posicion.ToString("00")));
                nodo.ImageIndex = 4;
                nodo.SelectedImageIndex = 4;
                ListaHistorial mangueras = servicioAdicional.HistorialObtenerPorPosicion(this.idEstacion, posicion);

                foreach (Historial manguera in mangueras)
                {
                    TreeNode cnodo = new TreeNode(string.Format("Lado {0} ({1}%)", manguera.Manguera.ToString("00"), manguera.Porcentaje.ToString("##,#0.00")));
                    cnodo.ImageIndex = manguera.Combustible;
                    cnodo.SelectedImageIndex = manguera.Combustible;
                    manguera.Calibracion = 0;
                    cnodo.Tag = manguera;
                    nodo.Nodes.Add(cnodo);
                }

                treeView1.Nodes.Add(nodo);
            }
        }
        #endregion

        #region Cambiar Porcentajes
        private void cambiarPorcentajeWayne(TreeNode nodo, decimal porcentaje)
        {
            string[] nombresCombustibles = new string[] { "", "Magna", "Premium", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

            if (nodo == null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    Historial histo = n.Tag as Historial;
                    histo.Porcentaje = porcentaje;
                    n.Text = string.Format("{0} ({1}%)", nombresCombustibles[histo.Combustible], porcentaje.ToString("##,#0"));
                }
            }
            else
            {
                Historial histo = nodo.Tag as Historial;
                histo.Porcentaje = porcentaje;
                nodo.Text = string.Format("{0} ({1}%)", nombresCombustibles[histo.Combustible], porcentaje.ToString("##,#0"));
            }
        }

        private void cambiarPorcentajeBennet(TreeNode nodo, decimal porcentaje)
        {
            if (nodo == null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    foreach (TreeNode c in n.Nodes)
                    {
                        Historial pHistoTmp = c.Tag as Historial;
                        pHistoTmp.Porcentaje = porcentaje;
                        if (pHistoTmp.Calibracion == 0)
                            c.Text = string.Format("Manguera {0} ({1}%)", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"));
                        else
                            c.Text = string.Format("Manguera {0} ({1}%)   Calibración ({2})", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"), pHistoTmp.Calibracion.ToString("##,#0"));
                    }
                }
            }
            else
            {
                if (nodo.Nodes != null && nodo.Nodes.Count > 0)
                {
                    foreach (TreeNode c in nodo.Nodes)
                    {
                        Historial pHistoTmp = c.Tag as Historial;
                        pHistoTmp.Porcentaje = porcentaje;
                        if (pHistoTmp.Calibracion == 0)
                            c.Text = string.Format("Manguera {0} ({1}%)", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"));
                        else
                            c.Text = string.Format("Manguera {0} ({1}%)   Calibración ({2})", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"), pHistoTmp.Calibracion.ToString("##,#0"));
                    }
                }
                else
                {
                    Historial histo = nodo.Tag as Historial;
                    histo.Porcentaje = porcentaje;
                    if (histo.Calibracion == 0)
                        nodo.Text = string.Format("Manguera {0} ({1}%)", histo.Manguera.ToString("00"), histo.Porcentaje.ToString("##,#0.00"));
                    else
                        nodo.Text = string.Format("Manguera {0} ({1}%)   Calibración ({2})", histo.Manguera.ToString("00"), histo.Porcentaje.ToString("##,#0.00"), histo.Calibracion.ToString("##,#0"));
                }
            }
        }

        private void cambiarPorcentajeTeam(TreeNode nodo, decimal porcentaje)
        {
            int dispensario = 1;

            if (nodo == null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    Historial histo = n.Tag as Historial;
                    histo.Porcentaje = porcentaje;
                    if (tipoClb == "7")
                        n.Text = string.Format("{0} ({1}%)", "Dispensario: " + (dispensario++).ToString(), porcentaje.ToString("##,#0"));
                    else
                        n.Text = string.Format("Dispensario: {0} ({1}%)", (dispensario++).ToString("00"), porcentaje.ToString("##,#0"));
                }
            }
            else
            {
                Historial histo = (nodo.Tag as Historial);
                foreach (TreeNode n in treeView1.Nodes)
                {
                    if (n.Equals(nodo))
                    {
                        if (tipoClb == "7")
                            n.Text = string.Format("{0} ({1}%)", "Dispensario: " + ((Convert.ToDouble(histo.Posicion) / 2) + 0.5).ToString(), porcentaje.ToString("##,#0"));
                        else
                            nodo.Text = string.Format("Dispensario: {0} ({1}%)", (dispensario++).ToString("00"), porcentaje.ToString("##,#0"));
                        break;
                    }
                    dispensario++;
                }
            }
        }

        private void cambiarPorcentajeGilbarco(TreeNode nodo, decimal porcentaje)
        {
            string[] nombresCombustibles = new string[] { "", "Gasolina", "Gasolina", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

            if (nodo == null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    Historial histo = n.Tag as Historial;
                    histo.Porcentaje = porcentaje;
                    n.Text = string.Format("{0} ({1}%)", nombresCombustibles[histo.Combustible], porcentaje.ToString("##,#0"));
                }
            }
            else
            {
                Historial histo = nodo.Tag as Historial;
                histo.Porcentaje = porcentaje;
                nodo.Text = string.Format("{0} ({1}%)", nombresCombustibles[histo.Combustible], porcentaje.ToString("##,#0"));
            }
        }

        private void cambiarPorcentajeHongYang(TreeNode nodo, decimal porcentaje)
        {
            if (nodo == null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    foreach (TreeNode c in n.Nodes)
                    {
                        Historial pHistoTmp = c.Tag as Historial;
                        pHistoTmp.Porcentaje = porcentaje;
                        c.Text = string.Format("Lado {0} ({1}%)", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"));
                    }
                }
            }
            else
            {
                if (nodo.Nodes != null && nodo.Nodes.Count > 0)
                {
                    foreach (TreeNode c in nodo.Nodes)
                    {
                        Historial pHistoTmp = c.Tag as Historial;
                        pHistoTmp.Porcentaje = porcentaje;
                        c.Text = string.Format("Lado {0} ({1}%)", pHistoTmp.Manguera.ToString("00"), pHistoTmp.Porcentaje.ToString("##,#0.00"));
                    }
                }
                else
                {
                    Historial histo = nodo.Tag as Historial;
                    histo.Porcentaje = porcentaje;
                    nodo.Text = string.Format("Lado {0} ({1}%)", histo.Manguera.ToString("00"), histo.Porcentaje.ToString("##,#0.00"));
                }
            }
        }

        private void itCambiarPorcentaje_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode nodo = treeView1.SelectedNode;
                Historial pHistorial = null;
                //decimal pPorcentaje = 0;

                if (nodo.Nodes.Count > 0)
                {
                    pHistorial = (nodo.Nodes[0].Tag as Historial);
                }
                else
                {
                    pHistorial = (nodo.Tag as Historial);
                }

                frmObtenerPorcentaje formaPorcentajes = new frmObtenerPorcentaje(pHistorial.Porcentaje, porcentajeMaximo,
                    estacion.TipoDispensario != MarcaDispensario.Bennett && estacion.TipoDispensario != MarcaDispensario.HongYang);
                formaPorcentajes.Text = "Captura de porcentaje " + nodo.Text;

                if (formaPorcentajes.ShowDialog() == DialogResult.OK)
                {
                    pHistorial.Porcentaje = formaPorcentajes.Porcentaje;

                    switch (estacion.TipoDispensario)
                    {
                        case MarcaDispensario.Ninguno:
                            break;
                        case MarcaDispensario.Wayne:
                            cambiarPorcentajeWayne(nodo, formaPorcentajes.Porcentaje);
                            break;
                        case MarcaDispensario.Bennett:
                            cambiarPorcentajeBennet(nodo, formaPorcentajes.Porcentaje);
                            break;
                        case MarcaDispensario.Team:
                            cambiarPorcentajeTeam(nodo, formaPorcentajes.Porcentaje);
                            break;
                        case MarcaDispensario.Gilbarco:
                            if (ConfigurationManager.AppSettings["GilbarcoOnOff"] == "Si" || tipoClb == "2")
                                cambiarPorcentajeBennet(nodo, formaPorcentajes.Porcentaje);
                            else if (tipoClb == "7")
                                cambiarPorcentajeTeam(nodo, formaPorcentajes.Porcentaje);
                            else
                                cambiarPorcentajeGilbarco(nodo, formaPorcentajes.Porcentaje);
                            break;
                        case MarcaDispensario.HongYang:
                            cambiarPorcentajeHongYang(nodo, formaPorcentajes.Porcentaje);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una posición o manguera primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tiPorcentajeGlobal_Click(object sender, EventArgs e)
        {
            frmObtenerPorcentaje formaPorcentajes = new frmObtenerPorcentaje(0, porcentajeMaximo,
                estacion.TipoDispensario != MarcaDispensario.Bennett && estacion.TipoDispensario != MarcaDispensario.HongYang);
            formaPorcentajes.Text = "Porcentaje global";

            if (formaPorcentajes.ShowDialog() == DialogResult.OK)
            {
                switch (estacion.TipoDispensario)
                {
                    case MarcaDispensario.Ninguno:
                        break;
                    case MarcaDispensario.Wayne:
                        cambiarPorcentajeWayne(null, formaPorcentajes.Porcentaje);
                        break;
                    case MarcaDispensario.Bennett:
                        cambiarPorcentajeBennet(null, formaPorcentajes.Porcentaje);
                        break;
                    case MarcaDispensario.Team:
                        cambiarPorcentajeTeam(null, formaPorcentajes.Porcentaje);
                        break;
                    case MarcaDispensario.Gilbarco:
                        if (ConfigurationManager.AppSettings["GilbarcoOnOff"] == "Si" || tipoClb == "2")
                            cambiarPorcentajeBennet(null, formaPorcentajes.Porcentaje);
                        else if (tipoClb == "7")
                            cambiarPorcentajeTeam(null, formaPorcentajes.Porcentaje);
                        else
                            cambiarPorcentajeGilbarco(null, formaPorcentajes.Porcentaje);
                        break;
                    case MarcaDispensario.HongYang:
                        cambiarPorcentajeHongYang(null, formaPorcentajes.Porcentaje);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void tiAplicarCambios_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Aplicar los porcentajes?", "Porcentajes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TimeSpan pHora = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                servicioAdicional.BitacoraInsertar(new Bitacora() { Id_usuario = Configuraciones.NombreUsuario, Suceso = "Aplicar cambio de porcentaje" });

                foreach (TreeNode n in treeView1.Nodes)
                {
                    if (n.Nodes != null && n.Nodes.Count > 0)
                    {
                        foreach (TreeNode c in n.Nodes)
                        {
                            Historial pHistoTmp = c.Tag as Historial;
                            pHistoTmp.Fecha = DateTime.Today;
                            pHistoTmp.Hora = pHora;
                            pHistoTmp.Calibracion = 0;
                            servicioAdicional.HistorialInsertar(pHistoTmp);
                        }
                    }
                    else
                    {
                        Historial hist = n.Tag as Historial;
                        hist.Fecha = DateTime.Now;
                        hist.Hora = pHora;
                        hist.Calibracion = 0;
                        servicioAdicional.HistorialInsertar(hist);
                    }
                }

                AplicarCambios = true;
                this.Close();
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                itCambiarPorcentaje_Click(sender, e);
            }
        }

        private void itActualizarPosiciones_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ServiciosCliente.IServiciosCliente serviciosCliente = Configuraciones.ListaCanales[idEstacion];
                List<Historial> listaHistorial = null;

                try
                {
                    listaHistorial = serviciosCliente.ObtenerBombasEstacion();
                }
                catch (Exception)
                {
                    Configuraciones.AbrirCanalCliente(idEstacion);
                    MessageBox.Show("No ha sido posible obtener la lista de posiciones de la estación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string tipoclb;
                    if (Process.GetProcessesByName("PDISMENUX").Length > 0 && ConfigurationManager.AppSettings["CalibracionAuto"] == "Si")
                        MessageBox.Show("Si desea calibrar de forma automática las bombas" + Environment.NewLine +
                            "debe bajar el flujo primero.", "No es posible calibrar bombas.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (!Utilerias.ObtenerListaVar().TryGetValue("TipoClb", out tipoclb))
                        tipoclb = "0";
                    if (estacion.TipoDispensario == MarcaDispensario.Bennett && Process.GetProcessesByName("PDISMENU").Length > 0 && tipoclb == "2" &&
                        ConfigurationManager.AppSettings["CalibracionAuto"] == "Si" && MessageBox.Show("¿Desea calibrar de forma automática las bombas?" +
                        Environment.NewLine + "Esto cambiará los valores configurados actualmente.", "Cambiar valores de calibración.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        List<string> comandos = new List<string>();
                        int posTmp = 0, manguera = 0;
                        string calibracion;
                        foreach (Historial h in listaHistorial)
                        {
                            if (posTmp != h.Posicion)
                                comandos.Add('&' + h.Posicion.ToString("00"));
                            posTmp = h.Posicion;
                        }
                        List<string> calibraciones = new List<string>();
                        if (estacion.TipoDispensario != MarcaDispensario.HongYang)
                            calibraciones = serviciosCliente.CalibrarBombas(comandos, (int)estacion.TipoDispensario);
                        posTmp = 0;
                        int indice = -1;
                        foreach (Historial h in listaHistorial)
                        {
                            if (posTmp != h.Posicion)
                            {
                                indice++;
                                manguera = 1;
                            }
                            else if (manguera == 2)
                                manguera = 3;
                            else
                                manguera = 2;

                            if (calibraciones.Count > 0)
                            {
                                try
                                {
                                    while (calibraciones[indice].Length < 3)
                                        indice++;

                                    calibracion = Utilerias.ObtenValorCal(calibraciones[indice], manguera, '+');
                                    try
                                    {
                                        h.Calibracion = Convert.ToInt32(calibracion != "" ? calibracion : "0");
                                    }
                                    catch
                                    {
                                        h.Calibracion = 0;
                                    }

                                    if (h.Calibracion == 0)
                                    {
                                        calibracion = Utilerias.ObtenValorCal(calibraciones[indice], manguera, '-');
                                        try
                                        {
                                            h.Calibracion = Convert.ToInt32(calibracion != "" ? "-" + calibracion : "0");
                                        }
                                        catch
                                        {
                                            h.Calibracion = 0;
                                        }
                                    }
                                }
                                catch
                                {
                                    h.Calibracion = 0;
                                }
                            }

                            posTmp = h.Posicion;
                        }
                    }

                    foreach (Historial h in listaHistorial)
                    {
                        h.Id_Estacion = idEstacion;
                        h.Hora = new TimeSpan(h.Hora.Hours, h.Hora.Minutes, h.Hora.Seconds);
                        if (estacion.TipoDispensario == MarcaDispensario.HongYang)
                            h.Calibracion = 0;
                        servicioAdicional.HistorialInsertar(h);
                    }
                }
                catch (Exception ex)
                {
                    Configuraciones.AbrirCanalAdicional(idEstacion);

                    MessageBox.Show(/*"No ha sido posible actualizar la lista de posiciones de la estación."*/ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                llenarLista();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void deshabilitarPosiciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nodo in treeView1.SelectedNode.Nodes)
            {
                ((Historial)nodo.Tag).Estado = "Fuera";
                servicioAdicional.HistorialActualizar((Historial)nodo.Tag);
            }
            treeView1.SelectedNode.ImageIndex = 5;
            treeView1.SelectedNode.SelectedImageIndex = 5;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count > 0)
            {
                deshabilitarPosiciónToolStripMenuItem.Visible = ((Historial)treeView1.SelectedNode.Nodes[0].Tag).Estado != "Fuera";
                habilitarPosiciónToolStripMenuItem.Visible = ((Historial)treeView1.SelectedNode.Nodes[0].Tag).Estado == "Fuera";
            }
            else
            {
                deshabilitarPosiciónToolStripMenuItem.Visible = false;
                habilitarPosiciónToolStripMenuItem.Visible = false;
            }
        }

        private void habilitarPosiciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nodo in treeView1.SelectedNode.Nodes)
            {
                ((Historial)nodo.Tag).Estado = "";
                servicioAdicional.HistorialActualizar((Historial)nodo.Tag);
            }
            treeView1.SelectedNode.ImageIndex = 0;
            treeView1.SelectedNode.SelectedImageIndex = 0;
        }

        private void tiPorcentajeCalibracion_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode nodo = treeView1.SelectedNode;
                Historial pHistorial = null;
                //decimal pPorcentaje = 0;

                if (nodo.Nodes.Count > 0)
                {
                    pHistorial = (nodo.Nodes[0].Tag as Historial);
                }
                else
                {
                    pHistorial = (nodo.Tag as Historial);
                }
                frmObtenerCalibracion formaCalibracion = new frmObtenerCalibracion(pHistorial, idEstacion);

                if (formaCalibracion.ShowDialog() == DialogResult.OK)
                {
                    pHistorial.Calibracion = formaCalibracion.Calibracion;
                    servicioAdicional.HistorialActualizar(pHistorial);
                    llenarLista();
                }
            }
        }
    }
}
