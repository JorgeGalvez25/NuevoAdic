using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class NotificacionesPersistencia
    {
        public void ProcesarNotificaciones()
        {
            ServicioCorreo servicio = new ServicioCorreo();
            MailStringBuilder mb = new MailStringBuilder(ConfigurationManager.AppSettings["CfgMail"]);

            servicio.BeforeSendMail += (s, e) =>
                {
                    BitacoraPersistencia srvBitacora = new BitacoraPersistencia();
                    {
                        Bitacora bita = new Bitacora()
                        {
                            Estacion = "SWEMail",
                            Fecha = DateTime.Now,
                            Tipo = "MP"
                        };

                        StringBuilder ssb = new StringBuilder();
                        {
                            ssb.AppendFormat("Envio de correo: {0}", e.Mensaje.To.Select(p => p.Address).Aggregate((x, y) => x + "; " + y)).AppendLine()
                               .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm:ss}", bita.Fecha).AppendLine();

                            if (e.Excepcion != null)
                            {
                                ssb.AppendFormat("Error: {0}", e.Excepcion.Message);
                                MensajesRegistros.Error(bita.Estacion, ssb.ToString().Trim());
                            }
                            else
                            {
                                ssb.Append("Correcto!");
                                MensajesRegistros.Informacion(bita.Estacion, ssb.ToString().Trim());
                            }
                        }

                        bita.Error = ssb.ToString().Trim();

                        try { srvBitacora.Insertar(new SesionModuloWeb(), bita); }
                        catch { }
                    }
                };
            servicio.SetHost(mb.Host, mb.Port, new System.Net.NetworkCredential(mb.User, mb.Pass));

            ListaNotificacionesDistribuidores listado = this.ObtenerNotificaciones();

            Dictionary<string, List<NotificacionesDistribuidores>> grupos = listado.AsParallel()
                                                                                   .Where(p => p.EstatusConexion != EstatusConexion.EnLinea || p.EstatusTransaccion != EstatusTransaccion.Ok)
                                                                                   .GroupBy(p => p.Distribuidor.Trim())
                                                                                   .ToDictionary(p => p.Key, p => p.OrderByDescending(q => q.DiasAtraso).ToList());

            string rootPath = Path.Combine(Path.GetDirectoryName(Environment.CommandLine), @"Templates");
            string path = Path.Combine(rootPath, "MailDistribuidores.html");
            string template = File.ReadAllText(path);

            Dictionary<string, Stream> files = new Dictionary<string, Stream>();
            {
                files.Add("image001", this.LoadFile(Path.Combine(rootPath, @"image001.jpg")));
                files.Add("image002", this.LoadFile(Path.Combine(rootPath, @"image002.jpg")));
            }

            string page = null;
            StringBuilder sb = null;
            List<string> emails = new List<string>();
            Dictionary<string, object> parametros = null;
            try
            {
                foreach (string item in grupos.Keys)
                {
                    try
                    {
                        servicio.SetFrom(mb.User);
                        if (emails != null) { emails.Clear(); }
                        else { emails = new List<string>(); }

                        char[] seperator = new char[] { ';' };
                        emails.AddRange(grupos[item][0].EMailDistribuidor.Split(seperator, StringSplitOptions.RemoveEmptyEntries));
                        emails.AddRange(grupos[item][0].EMailMatriz.Split(seperator, StringSplitOptions.RemoveEmptyEntries));

                        // Se agregan los correos del distribuidor y las copias
                        servicio.AddToAndCopy(emails.Distinct().ToList());

                        // Se obitienen los datos de la tabla y se crea el cuerpo de la tabla
                        sb = new StringBuilder();
                        {
                            int row = 0;
                            grupos[item].ForEach(p => sb.Append(this.GetRow(p, row++)));
                        }

                        page = GetTableTemplate(sb.ToString());
                        // Se obtienen los parametros a reemplazar dentro de la plantilla
                        parametros = new Dictionary<string, object>();
                        {
                            parametros.Add("[Fecha]", string.Format("{0:dd} de {0:MMMM} de {0:yyyy}", DateTime.Now));
                            parametros.Add("[Distribuidor]", item.ToLower().ToTitle());
                            parametros.Add("[Tabla]", page);
                        }

                        servicio.ConfigureMessage("Notificación Servicios Web", System.Net.Mail.MailPriority.Normal);
                        // Se realiza el reemplazo en la plantilla y la asigna para poder enviarla
                        servicio.SetMessageTemplate(path, true, files, parametros);
                        // Se envia por correo
                        servicio.SendMail();
                    }
                    finally
                    {
                        if (emails != null)
                        {
                            emails.Clear();
                            emails = null;
                        }

                        if (sb != null)
                        {
                            sb.Remove(0, sb.Length);
                            sb = null;
                        }

                        if (parametros != null)
                        {
                            parametros.Clear();
                            parametros = null;
                        }

                        if (page != null)
                        {
                            page.Remove(0);
                            page = null;
                        }

                        servicio.CleanMessage();
                    }
                }
            }
            finally
            {
                if (files != null)
                {
                    //foreach (var item in files.Keys)
                    Parallel.ForEach(files.Keys, item =>
                    {
                        try
                        {
                            if (files[item] != null)
                            {
                                files[item].Flush();
                                files[item].Close();
                                files[item].Dispose();
                            }
                        }
                        catch
                        { }
                    });//.WaitOne();

                    files.Clear();
                    files = null;
                }
            }
        }

        private ListaNotificacionesDistribuidores ObtenerNotificaciones()
        {
            ListaNotificacionesDistribuidores resultado = new ListaNotificacionesDistribuidores();

            using (Conexiones conexion = new Conexiones())
            {
                ParametrosConexion parametros = new ParametrosConexion()
                {
                    operacion = TipoOperacion.ObtenerTodos,
                    tabla = this.NombreEntidad
                };
                //using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                            {
                                                                ListaNotificacionesDistribuidores lAux = new ListaNotificacionesDistribuidores();
                                                                while (await reader.ReadAsync().ConfigureAwait(false))
                                                                {
                                                                    lAux.Add(await this.readerToEntidad(reader).ConfigureAwait(false));
                                                                }

                                                                resultado.AddRange(lAux.AsParallel()
                                                                                       .Where(p => p.NotificarServicioTransaccion.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                                                                                       .Where(p => p.EstatusConexion != EstatusConexion.EnLinea || p.EstatusTransaccion != EstatusTransaccion.Ok));
                                                                return resultado;
                                                            }))
                {
                    aux.ConfigureAwait(false);
                    aux.Wait();
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(new FiltroNotificacionesDistribuidores()));

            //        using (SqlDataReader reader = await comm.ExecuteReaderAsync())// comm.EndExecuteReader(comm.BeginExecuteReader()))
            //        {
            //            try
            //            {
            //                ListaNotificacionesDistribuidores aux = new ListaNotificacionesDistribuidores();
            //                while (await reader.ReadAsync())
            //                {
            //                    aux.Add(await this.readerToEntidad(reader));
            //                }

            //                resultado.AddRange(aux.AsParallel()
            //                                      .Where(p => p.NotificarServicioTransaccion.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
            //                                      .Where(p => p.EstatusConexion != EstatusConexion.EnLinea || p.EstatusTransaccion != EstatusTransaccion.Ok));
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed) { reader.Close(); }
            //            }
            //        }
            //    }).Wait();

            return resultado;
        }

        internal string NombreEntidad = typeof(NotificacionesDistribuidores).Name;

        internal async Task<NotificacionesDistribuidores> readerToEntidad(SqlDataReader reader)
        {
            NotificacionesDistribuidores entidad = new NotificacionesDistribuidores();
            {
                entidad.Estacion = await reader.IsDBNullAsync(0) ? string.Empty : reader.GetString(0);
                entidad.Cliente = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                entidad.FechaUltimaConexion = await reader.IsDBNullAsync(2) ? SqlDateTime.MinValue.Value : reader.GetDateTime(2);
                entidad.FechaUltimaTransaccion = await reader.IsDBNullAsync(3) ? SqlDateTime.MinValue.Value : reader.GetDateTime(3);
                entidad.NotificarServicioTransaccion = await reader.IsDBNullAsync(4) ? string.Empty : reader.GetString(4);
                int horas = await reader.IsDBNullAsync(5) ? 4 : reader.GetInt32(5);
                entidad.Distribuidor = await reader.IsDBNullAsync(6) ? string.Empty : reader.GetString(6);
                entidad.EMailDistribuidor = await reader.IsDBNullAsync(7) ? string.Empty : reader.GetString(7);
                string transaccion = await reader.IsDBNullAsync(8) ? string.Empty : reader.GetString(8);
                entidad.EMailMatriz = await reader.IsDBNullAsync(9) ? string.Empty : reader.GetString(9);

                switch (transaccion)
                {
                    case "O":
                        entidad.EstatusTransaccion = EstatusTransaccion.Ok;
                        break;
                    case "E":
                        entidad.EstatusTransaccion = EstatusTransaccion.Error;
                        break;
                    default:
                        entidad.EstatusTransaccion = EstatusTransaccion.Procesando;
                        break;
                }

                if (entidad.FechaUltimaTransaccion < DateTime.Now.AddHours(-1 * horas))
                {
                    entidad.EstatusTransaccion = EstatusTransaccion.Error;
                }

                entidad.EstatusConexion = (entidad.FechaUltimaConexion.AddMinutes(2) > DateTime.Now) ? EstatusConexion.EnLinea
                                                                                                     : EstatusConexion.FueraDeLinea;

                entidad.DiasAtraso = (int)(DateTime.Now.Date - entidad.FechaUltimaTransaccion.Date).TotalDays;
            }
            return entidad;
        }
        internal SqlParameter[] ConfiguraParametros(NotificacionesDistribuidores entidad)
        {
            string conexion = string.Empty;
            string transaccion = string.Empty;

            switch (entidad.EstatusTransaccion)
            {
                case EstatusTransaccion.Ok:
                    transaccion = "O";
                    break;
                case EstatusTransaccion.Error:
                    transaccion = "E";
                    break;
                default:
                    break;
            }

            switch (entidad.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", entidad.Estacion),
                    new SqlParameter("@CONEXION", conexion),
                    new SqlParameter("@TRANSACCION", transaccion)
                };
        }
        internal SqlParameter[] ConfiguraParametros(FiltroNotificacionesDistribuidores filtro)
        {
            string conexion = string.Empty;
            string transaccion = string.Empty;

            switch (filtro.EstatusTransaccion)
            {
                case EstatusTransaccion.Ok:
                    transaccion = "O";
                    break;
                case EstatusTransaccion.Error:
                    transaccion = "E";
                    break;
                default:
                    break;
            }

            switch (filtro.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", filtro.Estacion),
                    new SqlParameter("@CONEXION", conexion),
                    new SqlParameter("@TRANSACCION", transaccion),
                    new SqlParameter("@ACTIVO", "Si"),
                };
        }

        internal string GetTableTemplate(string rows)
        {
            string template = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"600\" name=\"tblText\" style=\"margin:0 auto;background-color:transparent;\">" +
                                  "<thead>" +
                                      "<tr style=\"background-color:#DEEAFF\">" +
                                          "<th valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                            "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 14px; line-height: 150%;\">Cliente</span>" +
                                          "</th>" +
                                          "<th valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                            "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 14px; line-height: 150%;\">Conexiones</span>" +
                                          "</th>" +
                                          "<th valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                            "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 14px; line-height: 150%;\">Transmisiones</span>" +
                                          "</th>" +
                                          "<th valign=\"top\" align=\"right\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                            "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 14px; line-height: 150%;\">D&iacute;as de Atraso</span>" +
                                          "</th>" +
                                      "</tr>" +
                                  "</thead>" +
                                  "<tbody>" +
                                    "{0}" +
                                  "</tbody>" +
                              "</table>";
            return string.Format(template, rows);
        }

        internal string GetRow(NotificacionesDistribuidores entidad, int rowNum)
        {
            string template1 = "<tr>" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{0}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{1}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{2}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"right\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{3}</span>" +
                                   "</td>" +
                               "</tr>";
            string template2 = "<tr style=\"background-color:#DEEAF6\">" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{0}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{1}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"left\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{2}</span>" +
                                   "</td>" +
                                   "<td valign=\"top\" align=\"right\" name=\"tblCell\" style=\"padding: 10px;width: 171px;height: 41px;\">" +
                                        "<span style=\"font-family: Arial, Helvetica, sans-serif; font-weight: normal; font-size: 14px; line-height: 150%;\">{3}</span>" +
                                   "</td>" +
                               "</tr>";

            string conexion = entidad.EstatusConexion == EstatusConexion.EnLinea ? "Correcto" : string.Format("Error ({0:dd/MM/yyyy})", entidad.FechaUltimaConexion);
            string transmision = entidad.EstatusTransaccion == EstatusTransaccion.Ok ? "Correcto" : string.Format("Error ({0:dd/MM/yyyy})", entidad.FechaUltimaTransaccion);
            return string.Format(((rowNum % 2 == 0) ? template1 : template2), entidad.Cliente, conexion, transmision, entidad.DiasAtraso);
        }
        internal Stream LoadFile(string path)
        {
            return new MemoryStream(File.ReadAllBytes(path));
        }
    }
}
