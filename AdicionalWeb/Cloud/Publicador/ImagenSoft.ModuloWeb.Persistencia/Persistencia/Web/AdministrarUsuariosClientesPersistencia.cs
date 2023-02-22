using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Servicios;
using ImagenSoft.ModuloWeb.Persistencia.UtileriasPersistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web
{
    public class AdministrarUsuariosClientesPersistencia
    {
        private string NombreEntidad = typeof(AdministrarUsuariosClientes).Name;

        public int Consecutivo(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            TaskCompletionSource<int> _task = new TaskCompletionSource<int>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        _task.TrySetResult(this.Consecutivo(conexion, sesion, filtro) + 1);
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarUsuariosClientes Insertar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            TaskCompletionSource<AdministrarUsuariosClientes> _task = new TaskCompletionSource<AdministrarUsuariosClientes>(entidad);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        _task.TrySetResult(this.Insertar(conexion, sesion, entidad));
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarUsuariosClientes Modificar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            TaskCompletionSource<AdministrarUsuariosClientes> _task = new TaskCompletionSource<AdministrarUsuariosClientes>(entidad);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        _task.TrySetResult(this.Modificar(conexion, sesion, entidad));
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        _task.TrySetResult(this.Eliminar(conexion, sesion, filtro));
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarUsuariosClientes Obtener(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            TaskCompletionSource<AdministrarUsuariosClientes> _task = new TaskCompletionSource<AdministrarUsuariosClientes>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        _task.TrySetResult(this.Obtener(conexion, sesion, filtro));
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public ListaAdministrarUsuariosClientes ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            TaskCompletionSource<ListaAdministrarUsuariosClientes> _task = new TaskCompletionSource<ListaAdministrarUsuariosClientes>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        ListaAdministrarUsuariosClientes resultado = new ListaAdministrarUsuariosClientes();

                        ListaAdministrarUsuariosClientes aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                        if (aux != null && aux.Count > 0)
                        {
                            resultado.AddRange(aux);
                        }

                        _task.TrySetResult(resultado);
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public bool InsertarModificar(SesionModuloWeb sesion, ListaAdministrarUsuariosClientes lst)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(lst);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        bool result = true;

                        ParametrosConexion parametros = null;

                        lst.ForEach(p =>
                        {
                            parametros = new ParametrosConexion()
                            {
                                operacion = TipoOperacion.Obtener,
                                tabla = this.NombreEntidad,
                                parameters = this.ConfiguraParametros(p)
                            };
                            conexion.ExecuteReader(parametros, async (conn, reader) =>
                            {
                                bool exist = await reader.ReadAsync();
                                if (exist)
                                {
                                    conn.CurrentResult = this.Modificar(conn, sesion, p) != null;
                                }
                                else
                                {
                                    conn.CurrentResult = this.Insertar(conn, sesion, p) != null;
                                }
                                return (bool)conn.CurrentResult;
                            }).Wait();

                            if (!(bool)conexion.CurrentResult)
                            {
                                result = false;
                            }
                        });

                        _task.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        #region Manejo de contraseña

        /// <summary>
        /// Modifica la contraseña y la fecha de modificación del usuario
        /// </summary>
        /// <param name="sesion"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool CambiarContrasenia(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(entidad);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        string tmpPassword = (string)entidad.Password.Clone();
                        entidad.Password = Utilerias.GetMD5(entidad.Password);
                        entidad.FechaCambio = DateTime.Now;

                        bool isSended = true;

                        ParametrosConexion parametros = new ParametrosConexion()
                        {
                            operacion = TipoOperacion.Especial_1,
                            tabla = this.NombreEntidad,
                            parameters = this.ConfiguraParametros(entidad)
                        };

                        _task.TrySetResult(conexion.ExecuteNonQuery(parametros, (conn) =>
                        {
                            isSended = this.EnviarCorreo("Nueva contraseña - Adicional Web", string.Format("Su nueva contraseña es {0}", tmpPassword), entidad.Correo.Trim(), entidad.Usuario, true, "Cambiar contraseña enviar correo");

                            #region OldCode
                            //using (ServicioCorreo srvCorreo = new ServicioCorreo())
                            //{
                            //    MailStringBuilder mb = new MailStringBuilder(ConfigurationManager.AppSettings["CfgMail"]);
                            //    srvCorreo.SetHost(mb.Host, mb.Port, new System.Net.NetworkCredential(mb.User, mb.Pass));
                            //    srvCorreo.SetFrom(mb.User);
                            //    srvCorreo.SetTo(entidad.Correo);
                            //    srvCorreo.ConfigureMessage("Nueva contraseña - Adicional Web");
                            //    srvCorreo.SetMessage(string.Format("Su nueva contraseña es {0}", tmpPassword));

                            //    Exception exAux = null;

                            //    srvCorreo.SendMail(false, (a) =>
                            //    {
                            //        isSended = !a.Cancelled;
                            //        exAux = a.Error;
                            //    });

                            //    if (!isSended)
                            //    {
                            //        if (exAux == null)
                            //        {
                            //            throw new Exception("No se pudo enviar el correo");
                            //        }
                            //        else
                            //        {
                            //            MensajesRegistros.Error("Cambiar contraseña enviar correo", exAux);
                            //            throw new Exception("No se pudo enviar el correo", exAux);
                            //        }
                            //    }
                            //} 
                            #endregion
                        }).Result > 0 && isSended);
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        /// <summary>
        /// Obtiene una nueva contraseña sin confirmaciones
        /// </summary>
        /// <param name="sesion"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool GenerarNuevaContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro.Correo)) { return false; }

            AdministrarUsuariosClientes eAux = this.Obtener(sesion, filtro);

            if (eAux == null) { return false; }

            PasswordsPersistencia srvPassword = new PasswordsPersistencia();
            eAux.Password = srvPassword.ObtenerNuevaContrasenia();

            return this.CambiarContrasenia(sesion, eAux);
        }

        /// <summary>
        /// Se utiliza para restablecer la contraseña de un usuario que asi lo requiera
        /// </summary>
        /// <param name="sesion"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool RestablecerContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            if (!CachePersistencia.Exist(filtro.Id)) { return false; }

            AdministrarUsuariosClientes eAux = this.Obtener(sesion, filtro);

            if (eAux == null) { return false; }

            ServicioEncriptacion srvEncriptacion = new ServicioEncriptacion();
            string pass = CachePersistencia.Get(filtro.Id);

            string[] split = srvEncriptacion.DecryptString(pass, eAux.Usuario, eAux.Password)
                                            .Result
                                            .Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
            eAux.Password = split[0];

            return this.CambiarContrasenia(sesion, eAux);
        }

        /// <summary>
        /// Permite la generación de una contraseña temporal para los usuarios nuevos
        /// </summary>
        /// <param name="sesion"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool SolicitarRestablecerContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        AdministrarUsuariosClientes eAux = this.Obtener(conexion, sesion, filtro);

                        if (eAux == null)
                        {
                            _task.TrySetResult(false);
                        }
                        else
                        {
                            ServicioEncriptacion srvEncriptacion = new ServicioEncriptacion();
                            PasswordsPersistencia srvPassword = new PasswordsPersistencia();

                            string newPass = string.IsNullOrWhiteSpace(filtro.Password) ? srvPassword.ObtenerNuevaContrasenia() : filtro.Password;
                            string cadena = string.Format("{0} | {1} | {2} | {3} | {4}", newPass, eAux.NoEstacion, eAux.Usuario, eAux.Correo, eAux.Password);

                            cadena = srvEncriptacion.EncryptString(cadena, eAux.Usuario, eAux.Password).Result;

                            Random rand = new Random();
                            long id = 0L;
                            do
                            {
                                id = rand.Next();
                            } while (CachePersistencia.Exist(id));
                            CachePersistencia.Add(id, cadena);

                            string template = string.Format("Para terminar el proceso de cambio de contrase&ntilde;a de clic en el siguiente link: <a href=\"{0}/Restore.aspx?id={1}&est={2}&usr={3}\">{0}/Restore.aspx?id={1}&est={2}&usr={3}</a>", filtro.Host, id, eAux.NoEstacion, eAux.Usuario);
                            _task.TrySetResult(this.EnviarCorreo("Recuperación de contraseña - Adicional Web", template, eAux.Correo.Trim(), eAux.Usuario, true, "Solicitar nueva contraseña enviar correo"));
                        }
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        private bool EnviarCorreo(string title, string message, string to, string toName, bool isHtml, string id)
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod(title, "private bool EnviarCorreo(string title, string message, string to, string toName, bool isHtml, string id)"))
            {
                bool isSended = false;
                MailStringBuilder mb = new MailStringBuilder(ConfigurationManager.AppSettings["CfgMail"]);

                string template = isHtml ? string.Format("<html><head></head><body>{0}</body></html>", message) : message;

                try
                {
                    _log.LogMessage("Iniciando envio");
                    using (Smtp smtp = new Smtp())
                    {
                        _log.LogObject("Configuración de correo", mb);
                        if ((mb.UseSsl.HasValue ? mb.UseSsl.Value : false))
                        {
                            smtp.ConnectSSL(mb.Host);
                        }
                        else
                        {
                            smtp.Connect(mb.Host);
                        }

                        smtp.UseBestLogin(mb.User, mb.Pass);

                        MailBuilder builder = new MailBuilder();
                        builder.From.Add(new MailBox(mb.User, "Servicio A-Web"));
                        _log.LogMessage("Destinatario: {0}", to);
                        builder.To.Add(new MailBox(to, toName));
                        builder.Subject = title;

                        if (isHtml)
                        {
                            _log.LogMessage("Con formato HTML");
                            builder.Html = message;
                        }
                        else
                        {
                            _log.LogMessage("Con formato Texto");
                            builder.Text = message;
                        }

                        _log.LogMessage("Creando correo");
                        IMail email = builder.Create();

                        _log.LogMessage("Enviando...");
                        ISendMessageResult result = smtp.SendMessage(email);

                        _log.LogMessage("Enviado!: {0}", result.Status);
                        isSended = result.Status != SendMessageStatus.Failure;

                        smtp.Close();
                    }
                }
                catch (Exception e)
                {
                    _log.LogException(id, e);
                    isSended = false;
                }

                _log.LogMessage("Enviado!: {0}", isSended);
                if (!isSended)
                {
                    _log.LogMessage("Intentando OldSchool");
                    _log.LogMessage("De: {0}, Para: {1}, Titulo: {2}", mb.User, to, title);
                    using (System.Net.Mail.MailMessage msj = new System.Net.Mail.MailMessage(mb.User, to, title, template)
                        {
                            IsBodyHtml = isHtml,
                            BodyEncoding = Encoding.UTF8,
                            Sender = new System.Net.Mail.MailAddress(mb.User),
                            SubjectEncoding = Encoding.UTF8
                        })
                    {
                        _log.LogMessage("Creando cliente... {0}:{1}", mb.Host, mb.Port);
                        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mb.Host, mb.Port)
                            {
                                Credentials = new System.Net.NetworkCredential(mb.User, mb.Pass, IPGlobalProperties.GetIPGlobalProperties().HostName),
                                DeliveryFormat = System.Net.Mail.SmtpDeliveryFormat.International,
                                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                                EnableSsl = mb.UseSsl.HasValue ? mb.UseSsl.Value : false,
                                UseDefaultCredentials = true
                            })
                        {
                            try
                            {
                                _log.LogMessage("Enviando...");
                                smtp.Send(msj);
                                _log.LogMessage("Enviado!");
                                isSended = true;
                            }
                            catch (Exception e)
                            {
                                _log.LogException(id, e);
                                isSended = false;
                            }
                        }
                    }
                }

                if (!isSended)
                {
                    _log.LogMessage("Iniciando envio de correo nuevo ¬_¬");
                    using (ServicioCorreo srvCorreo = new ServicioCorreo())
                    {
                        _log.LogMessage("Usuario: {0}, Contraseña: {1}, Host: {2}:{3}, Para: {4}", mb.User, mb.Pass, mb.Host, mb.Pass, to);
                        //MailStringBuilder mb = new MailStringBuilder(ConfigurationManager.AppSettings["CfgMail"]);
                        srvCorreo.SetHost(mb.Host, mb.Port, new System.Net.NetworkCredential(mb.User, mb.Pass));
                        srvCorreo.SetFrom(mb.User);
                        srvCorreo.SetTo(to);
                        srvCorreo.ConfigureMessage(title);
                        srvCorreo.SetMessage(message);

                        Exception exAux = null;

                        srvCorreo.SendMail(false, (a) =>
                        {
                            _log.LogMessage("Terminado!");
                            _log.LogMessage("Cancelado: {0}, Con Error: {1}", a.Cancelled, a.Error == null);
                            isSended = !a.Cancelled;
                            exAux = a.Error;
                        });

                        if (!isSended)
                        {
                            _log.LogMessage("No se envio el correo... (-_-')");
                            if (exAux == null)
                            {
                                _log.LogError("No se pudo enviar el correo");
                                throw new Exception("No se pudo enviar el correo");
                            }
                            else
                            {
                                _log.LogException(id, exAux);
                                throw new Exception("No se pudo enviar el correo", exAux);
                            }
                        }
                    }
                }

                _log.LogMessage("Terminado! Enviado?:{0}", isSended);

                return isSended;
            }
        }

        #endregion

        #region Manejo de privilegios

        public AdministrarUsuariosClientes ModificarPrivilegios(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            TaskCompletionSource<AdministrarUsuariosClientes> _task = new TaskCompletionSource<AdministrarUsuariosClientes>(entidad);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        AdministrarUsuariosClientes tmpCliente = this.Obtener(conexion, sesion, new FiltroAdministrarUsuariosClientes() { NoEstacion = entidad.NoEstacion });

                        if (tmpCliente == null)
                        {
                            _task.TrySetResult(null);
                        }
                        else
                        {
                            ParametrosConexion parametros = new ParametrosConexion()
                            {
                                operacion = TipoOperacion.Especial_2,
                                tabla = this.NombreEntidad,
                                parameters = this.ConfiguraParametros(entidad)
                            };

                            bool resultado = false;
                            conexion.ExecuteNonQuery(parametros, (conn) => { resultado = (((int)conn.CurrentResult) > 0); }).Wait();

                            _task.TrySetResult(resultado ? entidad : null);
                        }
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }
            }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        #endregion

        private async Task<AdministrarUsuariosClientes> readerToEntidad(SqlDataReader reader)
        {
            AdministrarUsuariosClientes entidad = new AdministrarUsuariosClientes();
            Task<SqlXml> _sqlTask = reader.GetFieldValueAsync<SqlXml>(10);
            _sqlTask.ConfigureAwait(false);

            entidad.Id = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
            entidad.NoEstacion = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
            entidad.Matriz = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
            entidad.Usuario = await reader.IsDBNullAsync(3) ? string.Empty : reader.GetString(3);
            entidad.Nombre = await reader.IsDBNullAsync(4) ? string.Empty : reader.GetString(4);
            entidad.Password = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
            entidad.FechaCambio = await reader.IsDBNullAsync(6) ? SqlDateTime.MinValue.Value : reader.GetDateTime(6);
            entidad.Rol = await reader.IsDBNullAsync(7) ? string.Empty : reader.GetString(7);
            entidad.Correo = await reader.IsDBNullAsync(8) ? string.Empty : reader.GetString(8);
            entidad.Activo = await reader.IsDBNullAsync(9) ? string.Empty : reader.GetString(9);
            if (!await reader.IsDBNullAsync(10))
            {
                SqlXml innerXML = _sqlTask.Result ?? SqlXml.Null;
                if (!innerXML.IsNull)
                {
                    string xml = innerXML.Value.Trim();
                    if (!string.IsNullOrWhiteSpace(xml))
                    {
                        ServicioSerializador serializador = new ServicioSerializador();
                        entidad.Privilegios = serializador.DeserializarFromXML<Privilegios>(Encoding.UTF8.GetBytes(xml));
                    }
                }
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(AdministrarUsuariosClientes entidad)
        {
            ServicioSerializador serializador = new ServicioSerializador();

            string rawXml = Encoding.UTF8.GetString(serializador.SerializarToXml(entidad.Privilegios));
            SqlXml xmlData = this.getSqlXml(rawXml).Result;

            return new SqlParameter[]
                {
                    new SqlParameter("@ID", entidad.Id),
                    new SqlParameter("@NOESTACION", entidad.NoEstacion),
                    new SqlParameter("@MATRIZ", entidad.Matriz),
                    new SqlParameter("@CORREO", entidad.Correo),
                    new SqlParameter("@FECHACAMBIO", entidad.FechaCambio <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaCambio),
                    new SqlParameter("@NOMBRE", entidad.Nombre),
                    new SqlParameter("@PASSWORD", entidad.Password),
                    new SqlParameter("@ROL", entidad.Rol),
                    new SqlParameter("@USUARIO", entidad.Usuario),
                    new SqlParameter("@NEWUSUARIO", entidad.NuevoUsuario),
                    new SqlParameter("@NEWNOESTACION", entidad.NuevoNoEstacion),
                    new SqlParameter("@ACTIVO", entidad.Activo),
                    new SqlParameter("@PRIVILEGIOS", xmlData)
                };
        }

        private static XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Async = true,
            Encoding = Encoding.UTF8,
            ConformanceLevel = ConformanceLevel.Fragment
        };

        private async Task<SqlXml> getSqlXml(string xmlData)
        {
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                return System.Data.SqlTypes.SqlXml.Null;
            }

            MemoryStream memoryStream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(memoryStream, xmlWriterSettings);
            await writer.WriteRawAsync(xmlData.Trim());
            await writer.FlushAsync();
            memoryStream.Position = 0L;
            return new System.Data.SqlTypes.SqlXml(memoryStream);
        }

        private SqlParameter[] ConfiguraParametros(FiltroAdministrarUsuariosClientes filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@ACTIVO", filtro.Activo),
                    new SqlParameter("@USUARIO", filtro.Usuario),
                    new SqlParameter("@FECHACAMBIO", filtro.FechaCambio <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaCambio),
                    new SqlParameter("@MATRIZ", filtro.Matriz),
                    new SqlParameter("@NOESTACION", filtro.NoEstacion),
                    new SqlParameter("@ROL", filtro.Rol)
                };
        }

        #region Transacciones
        internal int Consecutivo(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            int resultado = 0;
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Consecutivo,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            conexion.ExecuteReader(parametros, async (comm, reader) =>
                {
                    if (await reader.ReadAsync())
                    {
                        resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                    }
                    return resultado;
                }).Wait();

            return resultado + 1;
        }

        internal AdministrarUsuariosClientes Insertar(Conexiones conexion, SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            bool resultado = false;
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    resultado = (int)(conn.CurrentResult ?? 0) > 0;
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                            .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                            .AppendFormat("Estación: {0}", entidad.NoEstacion).AppendLine()
                            //.AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                            .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                            .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "UC"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Wait();
            return resultado ? entidad : null;
        }

        internal AdministrarUsuariosClientes Modificar(Conexiones conexion, SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            AdministrarUsuariosClientes tmpCliente = this.Obtener(conexion, sesion, new FiltroAdministrarUsuariosClientes() { NoEstacion = entidad.NoEstacion });

            if (tmpCliente == null)
            {
                return this.Insertar(conexion, sesion, entidad);
            }

            bool resultado = false;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            conexion.ExecuteNonQuery(parametros, (conn) => { resultado = ((int)conn.CurrentResult) > 0; }).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Eliminar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            return conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
                            //.AppendFormat("Zona: {0}", filtro.Zona).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Eliminar: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "UC"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Result > 0;
        }

        internal AdministrarUsuariosClientes Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            AdministrarUsuariosClientes resultado = null;
            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    if (await reader.ReadAsync())
                    {
                        resultado = await this.readerToEntidad(reader);
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }

        internal ListaAdministrarUsuariosClientes ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            ListaAdministrarUsuariosClientes resultado = new ListaAdministrarUsuariosClientes();
            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    while (await reader.ReadAsync())
                    {
                        resultado.Add(await this.readerToEntidad(reader).ConfigureAwait(false));
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }
        #endregion
    }
}
