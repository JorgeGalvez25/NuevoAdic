namespace NuevoAdicional
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.itSalir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tiEstaciones = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tiPosiciones = new System.Windows.Forms.ToolStripButton();
            this.tiSubir = new System.Windows.Forms.ToolStripButton();
            this.tiBajar = new System.Windows.Forms.ToolStripButton();
            this.tiParo = new System.Windows.Forms.ToolStripButton();
            this.tiActProtecc = new System.Windows.Forms.ToolStripButton();
            this.tiComboBoxProt = new System.Windows.Forms.ToolStripComboBox();
            this.tiDesProtecc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tiConfiguraciones = new System.Windows.Forms.ToolStripButton();
            this.tiUsurios = new System.Windows.Forms.ToolStripButton();
            this.btnMoviles = new System.Windows.Forms.ToolStripButton();
            this.tiBitacora = new System.Windows.Forms.ToolStripButton();
            this.itReporte = new System.Windows.Forms.ToolStripDropDownButton();
            this.reporte01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporte02ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tiRegenerarArchivos = new System.Windows.Forms.ToolStripButton();
            this.tiProtecciones = new System.Windows.Forms.ToolStripButton();
            this.itRefresh = new System.Windows.Forms.ToolStripButton();
            this.tiEscanear = new System.Windows.Forms.ToolStripButton();
            this.tiLicencias = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tiTickets = new System.Windows.Forms.ToolStripDropDownButton();
            this.tiRegistrarTicket = new System.Windows.Forms.ToolStripMenuItem();
            this.tiModificarTicket = new System.Windows.Forms.ToolStripMenuItem();
            this.tiTanques = new System.Windows.Forms.ToolStripButton();
            this.tiLecturasTanques = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.itProgresoActualizar = new System.Windows.Forms.ToolStripProgressBar();
            this.itmEdoRemoto = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bwConectarEstaciones = new System.ComponentModel.BackgroundWorker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvEstaciones = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colNombre = new System.Windows.Forms.ColumnHeader();
            this.colEstado = new System.Windows.Forms.ColumnHeader();
            this.colUltimoMovimiento = new System.Windows.Forms.ColumnHeader();
            this.colProtecciones = new System.Windows.Forms.ColumnHeader();
            this.colEdoConexion = new System.Windows.Forms.ColumnHeader();
            this.colUltimaSinco = new System.Windows.Forms.ColumnHeader();
            this.colDispensarios = new System.Windows.Forms.ColumnHeader();
            this.mnuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.itmActualizar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.itmMostrarError = new System.Windows.Forms.ToolStripMenuItem();
            this.itmSeparaRemoto = new System.Windows.Forms.ToolStripSeparator();
            this.itmConfRemoto = new System.Windows.Forms.ToolStripMenuItem();
            this.generarPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrScanConection = new System.Windows.Forms.Timer(this.components);
            this.bwScanConections = new System.ComponentModel.BackgroundWorker();
            this.tmrVerifica = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrSinc = new System.Windows.Forms.Timer(this.components);
            this.tmrWayne = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mnuListView.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itSalir,
            this.toolStripSeparator3,
            this.tiEstaciones,
            this.toolStripSeparator1,
            this.tiPosiciones,
            this.tiSubir,
            this.tiBajar,
            this.tiParo,
            this.tiActProtecc,
            this.tiComboBoxProt,
            this.tiDesProtecc,
            this.toolStripSeparator2,
            this.tiConfiguraciones,
            this.tiUsurios,
            this.btnMoviles,
            this.tiBitacora,
            this.itReporte,
            this.toolStripSeparator4,
            this.tiRegenerarArchivos,
            this.tiProtecciones,
            this.itRefresh,
            this.tiEscanear,
            this.tiLicencias,
            this.toolStripSeparator5,
            this.tiTickets,
            this.tiTanques,
            this.tiLecturasTanques});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1107, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolMenu";
            // 
            // itSalir
            // 
            this.itSalir.Image = global::NuevoAdicional.Properties.Resources.Salir;
            this.itSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itSalir.Name = "itSalir";
            this.itSalir.Size = new System.Drawing.Size(49, 22);
            this.itSalir.Tag = "27";
            this.itSalir.Text = "&Salir";
            this.itSalir.Click += new System.EventHandler(this.itSalir_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tiEstaciones
            // 
            this.tiEstaciones.Image = global::NuevoAdicional.Properties.Resources.cubes;
            this.tiEstaciones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiEstaciones.Name = "tiEstaciones";
            this.tiEstaciones.Size = new System.Drawing.Size(82, 22);
            this.tiEstaciones.Tag = "0";
            this.tiEstaciones.Text = "&Estaciones";
            this.tiEstaciones.Click += new System.EventHandler(this.tiEstaciones_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tiPosiciones
            // 
            this.tiPosiciones.Image = global::NuevoAdicional.Properties.Resources.drink_green;
            this.tiPosiciones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiPosiciones.Name = "tiPosiciones";
            this.tiPosiciones.Size = new System.Drawing.Size(58, 22);
            this.tiPosiciones.Tag = "4";
            this.tiPosiciones.Text = "&Flujos";
            this.tiPosiciones.Click += new System.EventHandler(this.tiPosiciones_Click);
            // 
            // tiSubir
            // 
            this.tiSubir.Image = global::NuevoAdicional.Properties.Resources.arrow_up_green;
            this.tiSubir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiSubir.Name = "tiSubir";
            this.tiSubir.Size = new System.Drawing.Size(54, 22);
            this.tiSubir.Tag = "6";
            this.tiSubir.Text = "S&ubir";
            this.tiSubir.Click += new System.EventHandler(this.tiSubir_Click);
            // 
            // tiBajar
            // 
            this.tiBajar.Image = global::NuevoAdicional.Properties.Resources.arrow_down_red;
            this.tiBajar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiBajar.Name = "tiBajar";
            this.tiBajar.Size = new System.Drawing.Size(53, 22);
            this.tiBajar.Tag = "7";
            this.tiBajar.Text = "&Bajar";
            this.tiBajar.Click += new System.EventHandler(this.tiBajar_Click);
            // 
            // tiParo
            // 
            this.tiParo.Image = global::NuevoAdicional.Properties.Resources.iconfinder_exclamation_circle_red_69107;
            this.tiParo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiParo.Name = "tiParo";
            this.tiParo.Size = new System.Drawing.Size(51, 22);
            this.tiParo.Tag = "7";
            this.tiParo.Text = "&Paro";
            this.tiParo.Click += new System.EventHandler(this.tiSubir_Click);
            // 
            // tiActProtecc
            // 
            this.tiActProtecc.Image = global::NuevoAdicional.Properties.Resources._lock;
            this.tiActProtecc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiActProtecc.Name = "tiActProtecc";
            this.tiActProtecc.Size = new System.Drawing.Size(124, 22);
            this.tiActProtecc.Tag = "28";
            this.tiActProtecc.Text = "Activar protección";
            this.tiActProtecc.Visible = false;
            this.tiActProtecc.Click += new System.EventHandler(this.tiActProtecc_Click);
            // 
            // tiComboBoxProt
            // 
            this.tiComboBoxProt.AutoSize = false;
            this.tiComboBoxProt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tiComboBoxProt.Items.AddRange(new object[] {
            "1 l",
            "10 l",
            "20 l",
            "Todos"});
            this.tiComboBoxProt.MergeIndex = 0;
            this.tiComboBoxProt.Name = "tiComboBoxProt";
            this.tiComboBoxProt.Size = new System.Drawing.Size(58, 23);
            this.tiComboBoxProt.Tag = "28";
            this.tiComboBoxProt.Visible = false;
            // 
            // tiDesProtecc
            // 
            this.tiDesProtecc.Image = global::NuevoAdicional.Properties.Resources.lock_open;
            this.tiDesProtecc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiDesProtecc.Name = "tiDesProtecc";
            this.tiDesProtecc.Size = new System.Drawing.Size(141, 22);
            this.tiDesProtecc.Tag = "28";
            this.tiDesProtecc.Text = "Desactivar protección";
            this.tiDesProtecc.Visible = false;
            this.tiDesProtecc.Click += new System.EventHandler(this.tiDesProtecc_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tiConfiguraciones
            // 
            this.tiConfiguraciones.Image = global::NuevoAdicional.Properties.Resources.console;
            this.tiConfiguraciones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiConfiguraciones.Name = "tiConfiguraciones";
            this.tiConfiguraciones.Size = new System.Drawing.Size(114, 22);
            this.tiConfiguraciones.Tag = "8";
            this.tiConfiguraciones.Text = "&Configuraciones";
            this.tiConfiguraciones.Visible = false;
            this.tiConfiguraciones.Click += new System.EventHandler(this.tiConfiguraciones_Click);
            // 
            // tiUsurios
            // 
            this.tiUsurios.Image = global::NuevoAdicional.Properties.Resources.user2;
            this.tiUsurios.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiUsurios.Name = "tiUsurios";
            this.tiUsurios.Size = new System.Drawing.Size(72, 22);
            this.tiUsurios.Tag = "10";
            this.tiUsurios.Text = "Usu&arios";
            this.tiUsurios.Click += new System.EventHandler(this.tiUsurios_Click);
            // 
            // btnMoviles
            // 
            this.btnMoviles.Image = global::NuevoAdicional.Properties.Resources.mobilephone2;
            this.btnMoviles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoviles.Name = "btnMoviles";
            this.btnMoviles.Size = new System.Drawing.Size(68, 22);
            this.btnMoviles.Tag = "22";
            this.btnMoviles.Text = "&Móviles";
            this.btnMoviles.Click += new System.EventHandler(this.btnMoviles_Click);
            // 
            // tiBitacora
            // 
            this.tiBitacora.Image = global::NuevoAdicional.Properties.Resources.address_book2;
            this.tiBitacora.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiBitacora.Name = "tiBitacora";
            this.tiBitacora.Size = new System.Drawing.Size(70, 22);
            this.tiBitacora.Tag = "16";
            this.tiBitacora.Text = "&Bitácora";
            this.tiBitacora.Click += new System.EventHandler(this.tiBitacora_Click);
            // 
            // itReporte
            // 
            this.itReporte.AutoToolTip = false;
            this.itReporte.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporte01ToolStripMenuItem,
            this.reporte02ToolStripMenuItem});
            this.itReporte.Image = global::NuevoAdicional.Properties.Resources.document;
            this.itReporte.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itReporte.Name = "itReporte";
            this.itReporte.Size = new System.Drawing.Size(82, 22);
            this.itReporte.Tag = "18";
            this.itReporte.Text = "&Reportes";
            // 
            // reporte01ToolStripMenuItem
            // 
            this.reporte01ToolStripMenuItem.Name = "reporte01ToolStripMenuItem";
            this.reporte01ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.reporte01ToolStripMenuItem.Tag = "1";
            this.reporte01ToolStripMenuItem.Text = "Reporte 0&1";
            this.reporte01ToolStripMenuItem.Click += new System.EventHandler(this.tiReporte_Click);
            // 
            // reporte02ToolStripMenuItem
            // 
            this.reporte02ToolStripMenuItem.Name = "reporte02ToolStripMenuItem";
            this.reporte02ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.reporte02ToolStripMenuItem.Tag = "2";
            this.reporte02ToolStripMenuItem.Text = "Reporte 0&2";
            this.reporte02ToolStripMenuItem.Click += new System.EventHandler(this.tiReporte_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tiRegenerarArchivos
            // 
            this.tiRegenerarArchivos.Image = global::NuevoAdicional.Properties.Resources.history2;
            this.tiRegenerarArchivos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiRegenerarArchivos.Name = "tiRegenerarArchivos";
            this.tiRegenerarArchivos.Size = new System.Drawing.Size(127, 22);
            this.tiRegenerarArchivos.Tag = "19";
            this.tiRegenerarArchivos.Text = "Re&generar archivos";
            this.tiRegenerarArchivos.Visible = false;
            this.tiRegenerarArchivos.Click += new System.EventHandler(this.tiRegenerarArchivos_Click);
            // 
            // tiProtecciones
            // 
            this.tiProtecciones.Image = ((System.Drawing.Image)(resources.GetObject("tiProtecciones.Image")));
            this.tiProtecciones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiProtecciones.Name = "tiProtecciones";
            this.tiProtecciones.Size = new System.Drawing.Size(95, 22);
            this.tiProtecciones.Tag = "20";
            this.tiProtecciones.Text = "&Protecciones";
            this.tiProtecciones.Click += new System.EventHandler(this.tiProtecciones_Click);
            // 
            // itRefresh
            // 
            this.itRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.itRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itRefresh.Image = global::NuevoAdicional.Properties.Resources.refresh;
            this.itRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itRefresh.Name = "itRefresh";
            this.itRefresh.Size = new System.Drawing.Size(23, 22);
            this.itRefresh.Tag = "";
            this.itRefresh.Text = "Ac&tulizar";
            this.itRefresh.ToolTipText = "Actualizar";
            this.itRefresh.Click += new System.EventHandler(this.itRefresh_Click);
            // 
            // tiEscanear
            // 
            this.tiEscanear.Image = global::NuevoAdicional.Properties.Resources.clock_refresh;
            this.tiEscanear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiEscanear.Name = "tiEscanear";
            this.tiEscanear.Size = new System.Drawing.Size(85, 22);
            this.tiEscanear.Tag = "21";
            this.tiEscanear.Text = "S&incronizar";
            this.tiEscanear.Click += new System.EventHandler(this.tiEscanear_Click);
            // 
            // tiLicencias
            // 
            this.tiLicencias.Image = global::NuevoAdicional.Properties.Resources.keys;
            this.tiLicencias.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiLicencias.Name = "tiLicencias";
            this.tiLicencias.Size = new System.Drawing.Size(75, 22);
            this.tiLicencias.Text = "Licencias";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tiTickets
            // 
            this.tiTickets.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiRegistrarTicket,
            this.tiModificarTicket});
            this.tiTickets.Image = global::NuevoAdicional.Properties.Resources.scroll;
            this.tiTickets.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiTickets.Name = "tiTickets";
            this.tiTickets.Size = new System.Drawing.Size(73, 22);
            this.tiTickets.Tag = "25";
            this.tiTickets.Text = "Tickets";
            // 
            // tiRegistrarTicket
            // 
            this.tiRegistrarTicket.Name = "tiRegistrarTicket";
            this.tiRegistrarTicket.Size = new System.Drawing.Size(125, 22);
            this.tiRegistrarTicket.Tag = "24";
            this.tiRegistrarTicket.Text = "Registrar";
            // 
            // tiModificarTicket
            // 
            this.tiModificarTicket.Name = "tiModificarTicket";
            this.tiModificarTicket.Size = new System.Drawing.Size(125, 22);
            this.tiModificarTicket.Tag = "25";
            this.tiModificarTicket.Text = "Modificar";
            // 
            // tiTanques
            // 
            this.tiTanques.Image = global::NuevoAdicional.Properties.Resources.column;
            this.tiTanques.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiTanques.Name = "tiTanques";
            this.tiTanques.Size = new System.Drawing.Size(114, 20);
            this.tiTanques.Tag = "26";
            this.tiTanques.Text = "Entrada Tanques";
            // 
            // tiLecturasTanques
            // 
            this.tiLecturasTanques.Image = global::NuevoAdicional.Properties.Resources.Tank_Reading;
            this.tiLecturasTanques.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tiLecturasTanques.Name = "tiLecturasTanques";
            this.tiLecturasTanques.Size = new System.Drawing.Size(118, 20);
            this.tiLecturasTanques.Tag = "29";
            this.tiLecturasTanques.Text = "Lecturas Tanques";
            this.tiLecturasTanques.Click += new System.EventHandler(this.tiLecturasTanques_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtUsuario,
            this.itProgresoActualizar,
            this.itmEdoRemoto});
            this.statusStrip1.Location = new System.Drawing.Point(0, 464);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1107, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(60, 17);
            this.txtUsuario.Text = "txtUsuario";
            // 
            // itProgresoActualizar
            // 
            this.itProgresoActualizar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.itProgresoActualizar.Name = "itProgresoActualizar";
            this.itProgresoActualizar.Size = new System.Drawing.Size(100, 16);
            this.itProgresoActualizar.Visible = false;
            // 
            // itmEdoRemoto
            // 
            this.itmEdoRemoto.Image = global::NuevoAdicional.Properties.Resources.bullet_ball_glass_gray;
            this.itmEdoRemoto.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.itmEdoRemoto.Name = "itmEdoRemoto";
            this.itmEdoRemoto.Size = new System.Drawing.Size(1032, 17);
            this.itmEdoRemoto.Spring = true;
            this.itmEdoRemoto.Text = "Estado del aviso visual:  ";
            this.itmEdoRemoto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.itmEdoRemoto.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.itmEdoRemoto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow_down_red.png");
            this.imageList1.Images.SetKeyName(1, "arrow_up_green.png");
            this.imageList1.Images.SetKeyName(2, "help2.png");
            this.imageList1.Images.SetKeyName(3, "bullet_ball_glass_gray.png");
            this.imageList1.Images.SetKeyName(4, "bullet_ball_glass_green.png");
            this.imageList1.Images.SetKeyName(5, "icons8-green-circle-24.png");
            this.imageList1.Images.SetKeyName(6, "icons8-red-circle-24.png");
            // 
            // bwConectarEstaciones
            // 
            this.bwConectarEstaciones.WorkerSupportsCancellation = true;
            this.bwConectarEstaciones.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwConectarEstaciones_DoWork);
            this.bwConectarEstaciones.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwConectarEstaciones_RunWorkerCompleted);
            this.bwConectarEstaciones.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwConectarEstaciones_ProgressChanged);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1107, 439);
            this.panel2.TabIndex = 4;
            // 
            // lvEstaciones
            // 
            this.lvEstaciones.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvEstaciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colNombre,
            this.colEstado,
            this.colUltimoMovimiento,
            this.colProtecciones,
            this.colEdoConexion,
            this.colUltimaSinco,
            this.colDispensarios});
            this.lvEstaciones.ContextMenuStrip = this.mnuListView;
            this.lvEstaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEstaciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvEstaciones.FullRowSelect = true;
            this.lvEstaciones.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvEstaciones.HideSelection = false;
            this.lvEstaciones.Location = new System.Drawing.Point(0, 25);
            this.lvEstaciones.MultiSelect = false;
            this.lvEstaciones.Name = "lvEstaciones";
            this.lvEstaciones.Size = new System.Drawing.Size(1107, 439);
            this.lvEstaciones.SmallImageList = this.imageList1;
            this.lvEstaciones.TabIndex = 3;
            this.lvEstaciones.UseCompatibleStateImageBehavior = false;
            this.lvEstaciones.View = System.Windows.Forms.View.Details;
            this.lvEstaciones.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvEstaciones_ItemSelectionChanged);
            // 
            // colId
            // 
            this.colId.Text = "Id";
            this.colId.Width = 50;
            // 
            // colNombre
            // 
            this.colNombre.Text = "Nombre";
            this.colNombre.Width = 240;
            // 
            // colEstado
            // 
            this.colEstado.Text = "Estado";
            this.colEstado.Width = 72;
            // 
            // colUltimoMovimiento
            // 
            this.colUltimoMovimiento.Text = "Último movimiento";
            this.colUltimoMovimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colUltimoMovimiento.Width = 150;
            // 
            // colProtecciones
            // 
            this.colProtecciones.Text = "Protecciones";
            this.colProtecciones.Width = 100;
            // 
            // colEdoConexion
            // 
            this.colEdoConexion.Text = "Edo. Conexión";
            this.colEdoConexion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colEdoConexion.Width = 120;
            // 
            // colUltimaSinco
            // 
            this.colUltimaSinco.Text = "Última sincronización";
            this.colUltimaSinco.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colUltimaSinco.Width = 155;
            // 
            // colDispensarios
            // 
            this.colDispensarios.Text = "Dispensarios";
            this.colDispensarios.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDispensarios.Width = 102;
            // 
            // mnuListView
            // 
            this.mnuListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmConectar,
            this.itmActualizar,
            this.toolStripMenuItem1,
            this.itmMostrarError,
            this.itmSeparaRemoto,
            this.itmConfRemoto,
            this.generarPDFToolStripMenuItem});
            this.mnuListView.Name = "mnuListView";
            this.mnuListView.Size = new System.Drawing.Size(184, 126);
            this.mnuListView.Opening += new System.ComponentModel.CancelEventHandler(this.mnuListView_Opening);
            // 
            // itmConectar
            // 
            this.itmConectar.Image = global::NuevoAdicional.Properties.Resources.plug;
            this.itmConectar.Name = "itmConectar";
            this.itmConectar.Size = new System.Drawing.Size(183, 22);
            this.itmConectar.Text = "&Conectar";
            this.itmConectar.Click += new System.EventHandler(this.itmConectar_Click);
            // 
            // itmActualizar
            // 
            this.itmActualizar.Image = global::NuevoAdicional.Properties.Resources.refresh;
            this.itmActualizar.Name = "itmActualizar";
            this.itmActualizar.Size = new System.Drawing.Size(183, 22);
            this.itmActualizar.Text = "&Actualizar";
            this.itmActualizar.Click += new System.EventHandler(this.itmActualizar_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 6);
            // 
            // itmMostrarError
            // 
            this.itmMostrarError.Image = global::NuevoAdicional.Properties.Resources.bug_red;
            this.itmMostrarError.Name = "itmMostrarError";
            this.itmMostrarError.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.itmMostrarError.Size = new System.Drawing.Size(183, 22);
            this.itmMostrarError.Text = "Mostrar &Error";
            this.itmMostrarError.Click += new System.EventHandler(this.itmMostrarError_Click);
            // 
            // itmSeparaRemoto
            // 
            this.itmSeparaRemoto.Name = "itmSeparaRemoto";
            this.itmSeparaRemoto.Size = new System.Drawing.Size(180, 6);
            // 
            // itmConfRemoto
            // 
            this.itmConfRemoto.Name = "itmConfRemoto";
            this.itmConfRemoto.Size = new System.Drawing.Size(183, 22);
            this.itmConfRemoto.Text = "C&onfigurar Remoto";
            this.itmConfRemoto.Click += new System.EventHandler(this.itmConfRemoto_Click);
            // 
            // generarPDFToolStripMenuItem
            // 
            this.generarPDFToolStripMenuItem.Name = "generarPDFToolStripMenuItem";
            this.generarPDFToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.generarPDFToolStripMenuItem.Text = "Generar PDF";
            this.generarPDFToolStripMenuItem.Click += new System.EventHandler(this.generarPDFToolStripMenuItem_Click);
            // 
            // tmrScanConection
            // 
            this.tmrScanConection.Interval = 1500;
            this.tmrScanConection.Tick += new System.EventHandler(this.tmrScanConection_Tick);
            // 
            // bwScanConections
            // 
            this.bwScanConections.WorkerSupportsCancellation = true;
            this.bwScanConections.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwScanConections_DoWork);
            this.bwScanConections.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwScanConections_RunWorkerCompleted);
            // 
            // tmrVerifica
            // 
            this.tmrVerifica.Interval = 500;
            this.tmrVerifica.Tick += new System.EventHandler(this.tmrVerifica_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Adicional";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // tmrSinc
            // 
            this.tmrSinc.Interval = 3540000;
            this.tmrSinc.Tick += new System.EventHandler(this.tmrSinc_Tick);
            // 
            // tmrWayne
            // 
            this.tmrWayne.Interval = 500;
            this.tmrWayne.Tick += new System.EventHandler(this.tmrWayne_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 486);
            this.Controls.Add(this.lvEstaciones);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Adicional";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.VisibleChanged += new System.EventHandler(this.frmMain_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mnuListView.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tiEstaciones;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tiPosiciones;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tiSubir;
        private System.Windows.Forms.ToolStripButton tiBajar;
        private System.Windows.Forms.ToolStripButton tiConfiguraciones;
        private System.Windows.Forms.ToolStripButton tiUsurios;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txtUsuario;
        private System.Windows.Forms.ToolStripButton tiBitacora;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tiRegenerarArchivos;
        private System.Windows.Forms.ToolStripButton tiProtecciones;
        private System.Windows.Forms.ToolStripButton itRefresh;
        private System.Windows.Forms.ToolStripButton itSalir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.ComponentModel.BackgroundWorker bwConectarEstaciones;
        private System.Windows.Forms.ToolStripProgressBar itProgresoActualizar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lvEstaciones;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colNombre;
        private System.Windows.Forms.ColumnHeader colEstado;
        private System.Windows.Forms.ColumnHeader colUltimoMovimiento;
        private System.Windows.Forms.ColumnHeader colProtecciones;
        private System.Windows.Forms.ColumnHeader colEdoConexion;
        private System.Windows.Forms.ContextMenuStrip mnuListView;
        private System.Windows.Forms.ToolStripMenuItem itmConectar;
        private System.Windows.Forms.ToolStripMenuItem itmActualizar;
        private System.Windows.Forms.Timer tmrScanConection;
        private System.Windows.Forms.ToolStripButton tiEscanear;
        private System.Windows.Forms.ColumnHeader colUltimaSinco;
        private System.Windows.Forms.ToolStripDropDownButton itReporte;
        private System.Windows.Forms.ToolStripMenuItem reporte01ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporte02ToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colDispensarios;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem itmMostrarError;
        private System.ComponentModel.BackgroundWorker bwScanConections;
        private System.Windows.Forms.ToolStripStatusLabel itmEdoRemoto;
        private System.Windows.Forms.ToolStripSeparator itmSeparaRemoto;
        private System.Windows.Forms.ToolStripMenuItem itmConfRemoto;
        private System.Windows.Forms.ToolStripButton btnMoviles;
        private System.Windows.Forms.ToolStripButton tiLicencias;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripDropDownButton tiTickets;
        private System.Windows.Forms.ToolStripMenuItem tiRegistrarTicket;
        private System.Windows.Forms.ToolStripMenuItem tiModificarTicket;
        private System.Windows.Forms.ToolStripButton tiTanques;
        private System.Windows.Forms.Timer tmrVerifica;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarPDFToolStripMenuItem;
        private System.Windows.Forms.Timer tmrSinc;
        private System.Windows.Forms.ToolStripButton tiActProtecc;
        private System.Windows.Forms.ToolStripButton tiDesProtecc;
        private System.Windows.Forms.ToolStripComboBox tiComboBoxProt;
        private System.Windows.Forms.ToolStripButton tiLecturasTanques;
        private System.Windows.Forms.ToolStripButton tiParo;
        private System.Windows.Forms.Timer tmrWayne;
    }
}

