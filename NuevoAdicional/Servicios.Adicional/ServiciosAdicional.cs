using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia;
using Adicional.Entidades;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceModel;
using ServiciosCliente;
using System.Diagnostics;
using System.IO;
using Adicional.Entidades.Web;
using System.Configuration;

namespace Servicios.Adicional
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServiciosAdicional : IServiciosAdicional
    {
        // Para el remoto
        private static Dictionary<string, DateTime> pulsacionesRemoto;
        private static bool remotoActivo = false;
        private string centinelas = "0;192;6;5;207;276;";
        private static System.Diagnostics.EventLog eventLog1;
        private static string ipAddress = string.Empty;
        private static int puerto = 0;
        private static IPEndPoint endPoint;
        private static Socket socket;
        private System.Threading.Thread hiloEscuchar;
        private static bool saliendo = false;
        private static EdoRemoto estadoRemoto = EdoRemoto.Desconectado;
        private static string usuarioPresiono = string.Empty;
        private static byte estadoSocket = 0;  // 0 - Desconectado
        // 1 - Conectado
        // 2 - Reconectando
        private static System.Timers.Timer timerRemotoEncendido;

        public List<Historial> ObtenerUltimosCambios(int AEstacion)
        {
            return new HistorialPersistencia().ObtenerRecientes(AEstacion);
        }

        #region Estaciones
        public string ObtenerNombreEstacion()
        {
            string nombre = string.Empty;

            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            try
            {
                //nombre = appReader.GetValue("Estación", typeof(string)).ToString();
                nombre = new EstacionPersistencia().ObtenerNombreEstacion();
            }
            catch
            {
                throw;
            }

            return nombre;
        }

        public ListaEstacion ObtenerListaEstaciones()
        {
            return new EstacionPersistencia().ObtenerLista();
        }

        public Estacion EstacionInsertar(Estacion entidad)
        {
            return new EstacionPersistencia().EstacionInsertar(entidad);
        }

        public Estacion EstacionActualizar(Estacion entidad)
        {
            return new EstacionPersistencia().EstacionActualizar(entidad);
        }

        public bool EstacionEliminar(int id)
        {
            return new EstacionPersistencia().EstacionEliminar(id);
        }

        public bool EstacionActivarProtecciones(int idEstacion, bool enable)
        {
            return new EstacionPersistencia().EstacionActivarProtecciones(idEstacion, enable);
        }
        #endregion

        #region Usuarios
        public ListaUsuario ObtenerUsuariosActivos()
        {
            return new UsuarioPersistencia().ObtenerListaActivos();
        }

        public Usuario UsuarioObtener(int idUsuario)
        {
            return new UsuarioPersistencia().UsuarioObtener(idUsuario);
        }

        public ListaUsuario UsuarioObtenerLista()
        {
            return new UsuarioPersistencia().ObtenerLista();
        }

        public Usuario UsuarioInsertar(Usuario entidad)
        {
            return new UsuarioPersistencia().UsuarioInsertar(entidad);
        }

        public Usuario UsuarioActualizar(Usuario entidad)
        {
            return new UsuarioPersistencia().UsuarioActualizar(entidad);
        }

        public bool UsuarioEliminar(int idUsuario)
        {
            return new UsuarioPersistencia().UsuarioEliminar(idUsuario);
        }
        #endregion

        #region Bitácora
        public Bitacora BitacoraInsertar(Bitacora entidad)
        {
            return new BitacoraPersistencia().BitacoraInsertar(entidad);
        }

        public ListaBitacora BitacoraObtenerListaPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            return new BitacoraPersistencia().ObtenerListaPorFecha(fechaInicial, fechaFinal);
        }

        public bool UsuarioTieneMovimientosEnBitacora(string idUsuario)
        {
            return new BitacoraPersistencia().UsuarioTieneMovimientosEnBitacora(idUsuario);
        }
        #endregion

        #region Historial
        public List<int> HistorialObtenerPosiciones(int claveEstacion)
        {
            return new HistorialPersistencia().HistorialObtenerPosiciones(claveEstacion);
        }

        public ListaHistorial HistorialObtenerPorPosicion(int claveEstacion, int posicion)
        {
            return new HistorialPersistencia().HistorialObtenerPorPosicion(claveEstacion, posicion);
        }

        public ListaHistorial HistorialObtenerTodos(int claveEstacion, int posicion)
        {
            return new HistorialPersistencia().HistorialObtenerTodos(claveEstacion, posicion);
        }

        public ListaHistorial HistorialObtenerRecientes(int pIdEstacion)
        {
            return new HistorialPersistencia().ObtenerRecientes(pIdEstacion);
        }

        public Historial HistorialInsertar(Historial entidad)
        {
            return new HistorialPersistencia().HistorialInsertar(entidad);
        }

        public bool HistorialEliminar(int id)
        {
            return new HistorialPersistencia().HistorialEliminar(id);
        }

        public Historial HistorialActualizar(Historial entidad)
        {
            return new HistorialPersistencia().HistorialActualizar(entidad);
        }
        #endregion

        #region Derechos
        public ListaDerecho DerechoObtenerListaPorUsuario(int idUsuario)
        {
            return new DerechoPersistencia().ObtenerListaPorUsuario(idUsuario);
        }
        #endregion

        #region Configuracion
        public Configuracion ConfiguracionActualizar(Configuracion pConfig)
        {
            Configuracion cfg = new ConfiguracionPersistencia().ConfiguracionActualizar(pConfig);
            cfg.EstadoRemoto = ServiciosAdicional.estadoRemoto;
            cfg.UsuarioPresiono = ServiciosAdicional.usuarioPresiono;

            return cfg;
        }

        public Configuracion ConfiguracionObtener(int id)
        {
            Configuracion cfg = new ConfiguracionPersistencia().ConfiguracionObtener(id);
            cfg.EstadoRemoto = ServiciosAdicional.estadoRemoto;
            cfg.UsuarioPresiono = ServiciosAdicional.usuarioPresiono;

            return cfg;
        }

        public List<Configuracion> ConfiguracionesObtener()
        {
            List<Configuracion> cfg = new ConfiguracionPersistencia().ConfiguracionesObtener();

            return cfg;
        }

        public bool ConfiguracionActivarProtecciones(int idConfiguracion, bool enable)
        {
            return new ConfiguracionPersistencia().ConfiguracionActivarProtecciones(idConfiguracion, enable);
        }

        public bool ConfiguracionCambiarEstado(string estado)
        {
            return new ConfiguracionPersistencia().ConfiguracionCambiarEstado(estado);
        }

        public DateTime ConfiguracionActualizarUltimoMovimiento(DateTime fecha)
        {
            return new ConfiguracionPersistencia().ConfiguracionActualizarUltimoMovimiento(fecha);
        }

        public DateTime ConfiguracionActualizarUltimaSincronizacion(DateTime fecha)
        {
            return new ConfiguracionPersistencia().ConfiguracionActualizarUltimaSincronizacion(fecha);
        }
        #endregion

        #region Proteccion
        public ListaProteccion ProteccionObtenerLista(int idEstacion)
        {
            return new ProteccionPersistencia().ObtenerLista(idEstacion);
        }

        public ListaProteccion ProteccionInsertarActualizar(ListaProteccion protecciones)
        {
            return new ProteccionPersistencia().ProteccionInsertarActualizar(protecciones);
        }
        #endregion

        #region Remoto
        System.Threading.Thread hiloConectar = null;
        public void Iniciar(string _ipAddress, int _puerto)
        {
            ipAddress = _ipAddress;
            puerto = _puerto;

            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Remoto"))
            {
                System.Diagnostics.EventLog.CreateEventSource("Remoto", "EventosRemoto");
            }
            eventLog1.Source = "Remoto";
            eventLog1.MaximumKilobytes = 10240;

            // Revisar si el remoto está activo
            remotoActivo = false;
            try
            {
                //string centinel = new ConsolaPersistencia().ObtenerNoCentinel();

                //remotoActivo = centinelas.Contains(centinel);
                // Validar Licencia
                //Licencia licRemoto = LicenciaObtener("CVL501");
                //remotoActivo = LicenciaValida(licRemoto, Licencia.Version);
                remotoActivo = true;
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Error al verificar licencia de REMOTO: " + ex.Message);
            }

            pulsacionesRemoto = new Dictionary<string, DateTime>();
            estadoRemoto = EdoRemoto.SinLicencia;
            estadoSocket = 0;

            // Iniciar el socket
            if (remotoActivo)
            {
                estadoRemoto = EdoRemoto.Desconectado;

                eventLog1.WriteEntry("Remoto activado, Iniciando conexiones");
                endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //System.Threading.Thread hiloConectar = new System.Threading.Thread(new System.Threading.ThreadStart(conectar));
                hiloConectar = new System.Threading.Thread(new System.Threading.ThreadStart(conectar));
                hiloConectar.Start();

                try
                {
                    System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                    iniciarTimerEncendido(Convert.ToDouble(appReader.GetValue("rev Remoto", typeof(double))));
                }
                catch
                {
                    iniciarTimerEncendido(180000);//3 minutos
                }
            }
            else
            {
                eventLog1.WriteEntry("El Remoto no está activo, favor de comunicarse con su proveedor");
            }
        }

        public void Terminar()
        {
            saliendo = true;
            try
            {
                eventLog1.WriteEntry("Cerrando el socket...");
                socket.Disconnect(false);
                socket.Close(1);
                hiloEscuchar.Join();

                if (hiloConectar != null && hiloConectar.IsAlive)
                {
                    try { hiloConectar.Abort(); }
                    catch { }
                    hiloConectar = null;
                }
            }
            catch { }

            eventLog1.WriteEntry("Socket Cerrado");
        }

        public EdoRemoto LeerEstadoRemoto()
        {
            if (remotoActivo)
            {
                System.Threading.Thread hiloLeerEstado = new System.Threading.Thread(new System.Threading.ThreadStart(leerEstado));
                hiloLeerEstado.Start();

                System.Threading.Thread.Sleep(2000);
                return ServiciosAdicional.estadoRemoto;
            }
            else
            {
                return EdoRemoto.Desconectado;
            }
        }

        public void ApagarVisual()
        {
            pulsacionesRemoto.Clear();

            if (remotoActivo)
            {
                System.Threading.Thread hiloApagarVisual = new System.Threading.Thread(new System.Threading.ThreadStart(apagarVisual));
                hiloApagarVisual.Start();
            }
        }

        public void EncenderVisual()
        {
            if (remotoActivo)
            {
                System.Threading.Thread hiloEncenderVisual = new System.Threading.Thread(new System.Threading.ThreadStart(encenderVisual));
                hiloEncenderVisual.Start();

                pulsacionesRemoto.Clear();
            }
        }

        private void conectar()
        {
            bool errorEnBitacora = false;

            while (!socket.Connected)
            {
                if (saliendo)
                    break;

                try
                {
                    if (!errorEnBitacora)
                    {
                        eventLog1.WriteEntry("Intentado la conexión...");
                    }

                    estadoSocket = 0;
                    socket.Connect(endPoint);
                    estadoSocket = 1;

                    hiloEscuchar = new System.Threading.Thread(new System.Threading.ThreadStart(leerSocket));
                    hiloEscuchar.Start();

                    eventLog1.WriteEntry("Conectado al remoto en la direcci\x00f3n: " + endPoint.Address.ToString() + ":" + endPoint.Port.ToString());
                    apagarVisual();
                }
                catch (Exception ex)
                {
                    estadoRemoto = EdoRemoto.Desconectado;
                    if (!errorEnBitacora)
                    {
                        eventLog1.WriteEntry("Error al intentar conectar al remoto:\r\n" + ex.Message);
                        errorEnBitacora = true;
                        eventLog1.WriteEntry("Reintentando la conexión...");
                    }
                }
            }
        }

        private void leerSocket()
        {
            List<byte> receive = new List<byte>();
            int checkSum = 0x00;
            int lastByte = 0x00;
            Int16 longitud = 0x00;
            byte numByte = 0x00;

            while (socket.Connected)
            {
                if (saliendo)
                    break;

                try
                {
                    // Si no hay datos que leer, reiniciar el ciclo
                    if (!socket.Poll(100, SelectMode.SelectRead))
                    {
                        continue;
                    }

                    byte[] byteRecibido = new byte[1];
                    socket.Receive(byteRecibido, 1, SocketFlags.Partial);

                    if (lastByte == 0x7E && numByte == 0)
                    {
                        longitud = (Int16)(byteRecibido[0] << 8);
                        numByte = 1;
                        receive = new List<byte>();
                        checkSum = 0x00;
                    }
                    else if (numByte == 1)
                    {
                        longitud += byteRecibido[0];
                        numByte++;
                    }
                    else if ((numByte - 2) < longitud && numByte > 0)
                    {
                        checkSum += byteRecibido[0];
                        numByte++;

                        receive.Add(byteRecibido[0]);
                    }
                    else if ((numByte - 2) == longitud && numByte > 0)
                    {
                        //Revisar checksum
                        if ((byteRecibido[0] == 0XFF - (byte)checkSum))
                        {
                            //Disparar Evento
                            InterpretarComando(receive.ToArray());
                        }

                        receive = new List<byte>();
                        checkSum = 0x00;
                        numByte = 0;
                        longitud = 0;
                    }

                    lastByte = byteRecibido[0];
                }
                catch (Exception ex)
                {
                    if (!saliendo)
                    {
                        estadoSocket = 2;
                        socket.Close();

                        endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        eventLog1.WriteEntry("Error en socket: " + ex.Message);

                        while (!socket.Connected && !saliendo)
                        {
                            try
                            {
                                socket.Connect(endPoint);
                            }
                            catch
                            {
                                estadoRemoto = EdoRemoto.Desconectado;
                                continue;
                            }
                        }

                        eventLog1.WriteEntry("Socket reconectado...");

                        estadoRemoto = EdoRemoto.VisualApagado;
                        estadoSocket = 1;
                    }
                }
            }
        }

        private void InterpretarComando(byte[] trama)
        {
            byte comando = trama[0];
            string usuario = string.Empty;
            string msj = "Trama recibida: ";

            for (int i = 0; i < trama.Length; i++)
            {
                usuario += trama[i].ToString("X2") + " ";
            }

            if (usuario.Equals("92 00 13 A2 00 00 00 00 00 FF FE 02 01 00 01 00 00"))
            {//92 00 13 A2 00 00 00 00 00 FF FE 02 01 00 01 00 00
                return;
            }

            if (comando == 0x92)
            {
                try
                {
                    if (pulsacionesRemoto.ContainsKey(usuario))
                    {
                        DateTime fechaHoraExacta = DateTime.Now;
                        if (fechaHoraExacta.Subtract(pulsacionesRemoto[usuario]) > new TimeSpan(0, 0, 0, 60))
                        {
                            pulsacionesRemoto[usuario] = fechaHoraExacta;
                            eventLog1.WriteEntry(msj + usuario);
                            BitacoraInsertar(new Bitacora() { Id_usuario = usuario, Suceso = "Botón Sostenido" });
                            return;
                        }
                    }
                    else
                    {
                        pulsacionesRemoto.Add(usuario, DateTime.Now);
                        eventLog1.WriteEntry(msj + usuario);
                    }

                    if (estadoRemoto != EdoRemoto.VisualEncendido)
                    {
                        ListaHistorial pListaHistorial = HistorialObtenerRecientes(0);
                        if (pListaHistorial != null && pListaHistorial.Count > 0)
                        {
                            EstacionConsPersistencia estacion = new EstacionConsPersistencia();
                            string respuesta;
                            if (new ConfiguracionPersistencia().ConfiguracionObtener(1).Estado != "Mínimo")
                                respuesta = new ServiciosCliente.ServiciosCliente().AplicarFlujo(false, false, estacion.ObtenerMarcaDispensario(), (from h in pListaHistorial select h).ToList<Historial>());
                            else
                                respuesta = "OK";

                            if (respuesta.Length > 0 && respuesta.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            {
                                BitacoraInsertar(new Bitacora() { Id_usuario = usuario, Suceso = "Bajar flujo" });
                                ConfiguracionPersistencia persistenciaCfg = new ConfiguracionPersistencia();
                                persistenciaCfg.ConfiguracionCambiarEstado("Mínimo");
                                persistenciaCfg.ConfiguracionActualizarUltimoMovimiento(DateTime.Now);

                                System.Threading.Thread hiloEncender = new System.Threading.Thread(new System.Threading.ThreadStart(encenderVisual));
                                hiloEncender.Start();
                            }
                            else
                            {
                                BitacoraInsertar(new Bitacora() { Id_usuario = "Remoto", Suceso = "Error al bajar flujo" });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    eventLog1.WriteEntry("Error al actualizar estado Consola:\r\n" + ex.Message);
                }
            }
            else if (comando == 0x88)
            {
                eventLog1.WriteEntry(msj + usuario);

                if (trama.Length == 6)
                {
                    estadoRemoto = (trama[5] == 0x05 ? EdoRemoto.VisualEncendido : EdoRemoto.VisualApagado);
                }
                //Mandar Mensaje
            }
        }

        private void encenderVisual()
        {
            try
            {
                // Encender la señal visual
                // Encender  [7E 00 05 08 01 44 30 05 7D]
                eventLog1.WriteEntry("Mandando trama de encendido del visual [7E 00 05 08 01 44 30 04 7D]");
                byte[] tramaEnviar = new byte[] { 0x7E, 0x00, 0x05, 0x08, 0x01, 0x44, 0x30, 0x05, 0x7D };

                while (estadoRemoto == EdoRemoto.VisualApagado)
                {
                    if ((estadoSocket == 1) && socket.Poll(100, SelectMode.SelectWrite))
                    {

                        socket.Send(tramaEnviar);

                        System.Threading.Thread.Sleep(2000);
                        leerEstado();
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Error en el socket: " + ex.Message);

                if (!saliendo)
                {
                    estadoSocket = 2;
                    socket.Close();

                    endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    while (!socket.Connected && !saliendo)
                    {
                        try
                        {
                            socket.Connect(endPoint);
                        }
                        catch
                        {
                            estadoRemoto = EdoRemoto.Desconectado;
                            continue;
                        }
                    }

                    estadoRemoto = EdoRemoto.VisualApagado;
                    estadoSocket = 1;
                }
            }
        }

        private void leerEstado()
        {
            try
            {
                // Leer Edo. [7E 00 04 09 01 44 30 81]
                eventLog1.WriteEntry("Mandando trama de leer el estado del visual [7E 00 04 09 01 44 30 81]");
                byte[] tramaEnviar = new byte[] { 0x7E, 0x00, 0x04, 0x09, 0x01, 0x44, 0x30, 0x81 };

                socket.Send(tramaEnviar);
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Error en el socket: " + ex.Message);

                if (!saliendo)
                {
                    estadoSocket = 2;
                    socket.Close();

                    endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    while (!socket.Connected && !saliendo)
                    {
                        try
                        {
                            socket.Connect(endPoint);
                        }
                        catch (Exception ex2)
                        {
                            estadoRemoto = EdoRemoto.Desconectado;
                            eventLog1.WriteEntry(ex2.Message);
                        }
                    }

                    estadoRemoto = EdoRemoto.VisualApagado;
                    estadoSocket = 1;
                }
            }
        }

        private void apagarVisual()
        {
            try
            {
                // apagar la señal visual
                // Apagar [7E 00 05 08 01 44 30 04 7E]
                eventLog1.WriteEntry("Mandando trama de apagado del visual [7E 00 05 08 01 44 30 04 7E]");
                byte[] tramaEnviar = new byte[] { 0x7E, 0x00, 0x05, 0x08, 0x01, 0x44, 0x30, 0x04, 0x7E };

                while (estadoRemoto == EdoRemoto.VisualEncendido)
                {
                    if ((estadoSocket == 1) && socket.Poll(100, SelectMode.SelectWrite))
                    {

                        socket.Send(tramaEnviar);

                        System.Threading.Thread.Sleep(3000);
                        leerEstado();
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Error en el socket: " + ex.Message);
                if (!saliendo)
                {
                    estadoSocket = 2;
                    socket.Close();

                    endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    while (!socket.Connected && !saliendo)
                    {
                        try
                        {
                            socket.Connect(endPoint);
                        }
                        catch
                        {
                            estadoRemoto = EdoRemoto.Desconectado;
                            continue;
                        }
                    }

                    estadoRemoto = EdoRemoto.VisualEncendido;
                    estadoSocket = 1;
                }
            }
        }

        private void timerRemotoEncendido_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Revisar si el remoto está encendido, y si no responde mandar a reconectar.
            timerRemotoEncendido.Stop();
            System.Threading.Thread hiloRevisar = new System.Threading.Thread(new System.Threading.ThreadStart(revisarEncendido));
            hiloRevisar.Start();
        }

        private void revisarEncendido()
        {
            try
            {
                // Leer Edo. [7E 00 04 09 01 44 30 81]
                eventLog1.WriteEntry("Verificar si está en línea el remoto [7E 00 04 09 01 44 30 81]");
                byte[] tramaEnviar = new byte[] { 0x7E, 0x00, 0x04, 0x09, 0x01, 0x44, 0x30, 0x81 };

                socket.Send(tramaEnviar);
            }
            catch (Exception)
            {
                if (!saliendo)
                {
                    estadoSocket = 2;
                    socket.Close();

                    eventLog1.WriteEntry("Remoto sin conexión, intentando reconectar...");
                    endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), puerto);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        socket.Connect(endPoint);
                        eventLog1.WriteEntry("Remoto reconectado");
                    }
                    catch (Exception ex)
                    {
                        estadoRemoto = EdoRemoto.Desconectado;
                        eventLog1.WriteEntry("No se logró reconectar, razón: " + ex.Message);
                    }

                    estadoSocket = 1;
                }
            }

            timerRemotoEncendido.Start();
        }

        private void iniciarTimerEncendido(double intervalo)
        {
            if (timerRemotoEncendido == null)
            {
                timerRemotoEncendido = new System.Timers.Timer();
            }

            timerRemotoEncendido.Interval = intervalo;
            timerRemotoEncendido.Stop();
            timerRemotoEncendido.Elapsed -= timerRemotoEncendido_Elapsed;
            timerRemotoEncendido.Elapsed += new System.Timers.ElapsedEventHandler(timerRemotoEncendido_Elapsed);
            timerRemotoEncendido.Start();
        }
        #endregion

        #region Moviles
        public Moviles MovilesObtener(FiltroMoviles movil)
        {
            return new MovilesPersistencia().Obtener(movil);
        }

        public ListaMoviles MovilesObtenerTodos()
        {
            return new MovilesPersistencia().ObtenerTodos();
        }

        public Moviles MovilesInsertar(Moviles entidad)
        {
            return new MovilesPersistencia().Insertar(entidad);
        }

        public Moviles MovilesActualizar(Moviles entidad)
        {
            return new MovilesPersistencia().Actualizar(entidad);
        }

        public bool MovilesEliminar(string numero)
        {
            return new MovilesPersistencia().Eliminar(numero);
        }
        #endregion

        #region Licencias
        //[DllImport("LibsDelphi.dll", EntryPoint = "LicenciaValidaDLL")]
        //private static extern int LicenciaValidaDLL(string RazonSocial, string Sistema, string Version, string TipoLicencia, string ClaveAutor, int Usuarios, bool LicenciaTemporal, string Fecha);

        public List<Licencia> LicenciasObtener()
        {
            List<Licencia> licencias = new LicenciaPersistencia().LicenciasObtener();

            licencias.ForEach(l =>
                        {
                            l.Valida = true;
                        });
            return licencias;
        }

        public Licencia LicenciaObtener(string modulo)
        {
            return new LicenciaPersistencia().LicenciaObtener(modulo);
        }

        public Licencia LicenciaInsertar(string modulo, string lic)
        {
            return new LicenciaPersistencia().LicenciaInsertar(modulo, lic);
        }

        public Licencia LicenciaActualizar(Licencia licencia)
        {
            return new LicenciaPersistencia().LicenciaActualizar(licencia);
        }

        public bool LicenciaEliminar(string modulo)
        {
            return new LicenciaPersistencia().LicenciaEliminar(modulo);
        }

        public bool LicenciaValida(Licencia lic, string version)
        {
            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"LibsDelphi.exe");
                    p.StartInfo.Arguments = string.Format("licenciavalidacvol {0}", lic.ToString(lic.Modulo));
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;

                    p.Start();
                    p.WaitForExit();

                    string r = (p.StandardOutput.ReadToEnd() ?? string.Empty).Trim();
                    //Valido
                    string result = r.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    return (string.IsNullOrEmpty(r) ? false : (result.Equals("1") || result.Equals("Valido", StringComparison.CurrentCultureIgnoreCase)));
                }
                catch
                {
                    return false;
                }
            }

            //return LicenciaValidaDLL(lic.Razon_social,
            //                         lic.Modulo,
            //                         version,
            //                         lic.TipoLicencia,
            //                         lic.ClaveAutor,
            //                         lic.Usuarios,
            //                         lic.Estemporal.Equals("Si", StringComparison.OrdinalIgnoreCase),
            //                         lic.Fecha_vence) == 1;
        }
        #endregion

        #region Utilerias
        public bool IsAlive()
        {
            return true;
        }
        #endregion

        #region Reporte Ventas Combustible

        public List<ReporteVentasCombustible> ObtenerReporteVentasCombustible(FiltroReporteVentasCombustible filtro)
        {
            ReporteVentasCombustiblePersistencia servicio = new ReporteVentasCombustiblePersistencia();

            switch (filtro.Tipo)
            {
                case TipoVentasCombustible.VentasReales:
                    return servicio.ReporteVentasReales(filtro.Fecha);//, filtro.NoEstacion);
                case TipoVentasCombustible.VentasAjustadas:
                    return servicio.ReporteVentasAjustadas(filtro.Fecha);
                case TipoVentasCombustible.AjustePorCombustible:
                    return servicio.ReporteAjusteCombustible(filtro.Fecha);//, filtro.NoEstacion);
                case TipoVentasCombustible.None:
                default:
                    return null;
            }
        }

        #endregion

        public MarcaDispensario TipoDispensarioCloud()
        {
            string[] arrAux = new EstacionPersistencia().ObtenerNombreEstacion().Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
            string aux = arrAux.Length >= 2 ? arrAux[1] : "0";

            int clave = 0;
            string marcaDisp = ConfigurationManager.AppSettings["MarcaDispensario"];
            clave = marcaDisp != string.Empty ? Convert.ToInt32(marcaDisp) : Convert.ToInt32(aux);

            return (MarcaDispensario)clave;
        }

        public string SubirBajarFlujoCloud(UsuarioCloud usuario, bool std)
        {
            string mensajeResp = string.Empty;
            this.BitacoraInsertar(new Bitacora() { Id_usuario = usuario.Usuario, Suceso = "Aplicar cambio de flujo" });

            ServiciosCliente.ServiciosCliente cliente = new ServiciosCliente.ServiciosCliente();

            if ("Ok".Equals(cliente.AplicarFlujo(std, false, this.TipoDispensarioCloud(), this.HistorialObtenerRecientes(1).ToList()), StringComparison.OrdinalIgnoreCase))
            {
                ConfiguracionCambiarEstado(std ? "Estandar" : "Mínimo");
            }
            else
            {
                this.BitacoraInsertar(new Bitacora() { Id_usuario = usuario.Usuario, Suceso = "Error al aplicar el cambio de flujo." });
                mensajeResp = "No fue posible hacer el cambio de flujo, por favor realice una aplicación manual.";
                return mensajeResp;
            }

            new ProcesosComando().AplicaComando(std, false, out mensajeResp);

            return mensajeResp;
        }

        public bool EstablecerPorcentajeCloud(UsuarioCloud usuario, ListaHistorial entidad, bool esGlobal)
        {
            string mensajeResp = string.Empty;
            this.BitacoraInsertar(new Bitacora()
                {
                    Id_usuario = usuario.Usuario,
                    Suceso = string.Format("Aplicar cambio de porcentaje {0}", esGlobal ? "global" : string.Empty).Trim()
                });

            Licencia lic = this.LicenciaObtener(Licencia.ClabeAutor);
            ServiciosCliente.ServiciosCliente cliente = new ServiciosCliente.ServiciosCliente();

            //if (!LicenciaValida(lic, string.Empty) || !cliente.ValidaLicencia("CVL5"))
            //{
            //    this.BitacoraInsertar(new Bitacora()
            //        {
            //            Id_usuario = usuario.Usuario,
            //            Suceso = "Error al aplicar, Licencia inválida"
            //        });
            //    throw new Exception("No fue posible registrar el porcentaje, por favor realice una aplicación manual.");
            //}

            int count = 0;
            entidad.ForEach(p =>
                {
                    try
                    {
                        // Se hace esto por que la mugre estructura TimeSpan no puede ser serilizada como XML
                        // Pero el DateTime si, ¡que munga! ¬_¬'
                        p.Hora = p.Fecha.TimeOfDay;
                        p.Fecha = p.Fecha.Date;
                        this.HistorialInsertar(p);
                    }
                    catch { count++; }
                });

            try
            {
                string estatus = cliente.ObtenerEstatus();

                if ("Estandar".Equals(estatus, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(this.SubirBajarFlujoCloud(usuario, true)))
                    {
                        ConfiguracionActualizarUltimoMovimiento(DateTime.Now);
                        //EstacionActualizar(estacion);
                        ConfiguracionCambiarEstado("Estandar");
                    }
                    else
                    {
                        this.HistorialObtenerRecientes(1).ForEach(p => this.HistorialEliminar(p.Id));
                    }
                }
            }
            catch (Exception)
            {
                this.HistorialObtenerRecientes(1).ForEach(p => this.HistorialEliminar(p.Id));
            }
            return count == 0;
        }
    }
}
