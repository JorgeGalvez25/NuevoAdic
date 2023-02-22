using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Extensiones;
using System.Diagnostics;
using Gurock.SmartInspect;
using System.Xml.Serialization;
using System.Xml;

namespace ImagenSoft.ModuloWeb.Entidades
{
    public class Utilerias
    {
        private static readonly object _locker = new object();

        public static DateTime FechaMexico
        {
            get
            {
                return DateTime.Now.IsDaylightSavingTime()
                                ? DateTime.UtcNow.AddHours(-5)
                                : DateTime.UtcNow.AddHours(-6);
            }
        }

        #region Hashing

        //public static string GetMD5(Stream f)
        //{
        //    lock (_locker)
        //    {
        //        MD5 hash = MD5.Create();
        //        byte[] buffer = hash.ComputeHash(f);
        //        return CompileMD5(buffer);
        //    }
        //}

        //public static string GetMD5(string cadena)
        //{
        //    return GetMD5(Encoding.Default.GetBytes(cadena));
        //}

        //public static string GetMD5(byte[] buffering)
        //{
        //    lock (_locker)
        //    {
        //        MD5 hash = MD5.Create();
        //        byte[] buffer = hash.ComputeHash(buffering);
        //        return CompileMD5(buffer);
        //    }
        //}

        private static string CompileMD5(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder(buffer.Length);
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public static bool ValidarEncriptado(string contrasenia, string encriptado)
        {
            return GetMD5(contrasenia).Equals(encriptado, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        private static readonly HashAlgorithm MD5 = new MD5CryptoServiceProvider();

        public static string GetMD5(Stream f)
        {
            using (MemoryStream mem = (MemoryStream)f)
            {
                return GetEncriptedHash(mem, MD5);
            }
        }

        public static string GetMD5(string cadena)
        {
            return GetMD5(Encoding.ASCII.GetBytes(cadena));
        }

        public static string GetMD5(byte[] buffer)
        {
            using (MemoryStream mem = new MemoryStream(buffer))
            {
                return GetEncriptedHash(mem, MD5);
            }
        }

        private static string GetEncriptedHash(MemoryStream mem, HashAlgorithm algorithm)
        {
            using (BufferedStream stream = new BufferedStream(mem))
            {
                return BitConverter.ToString(algorithm.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
            }
        }
    }
    public class SerializadorModuloWeb
    {
        private static readonly object _locker = new object();

        private const int MAXBUFFER = short.MaxValue;
        private static System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] Serializar<T>(T obj)
        {
            lock (_locker)
            {
                using (MemoryStream msCompressed = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(msCompressed, CompressionMode.Compress))
                    {
                        using (BufferedStream buffCompressed = new BufferedStream(gZipStream, MAXBUFFER))
                        {
                            using (MemoryStream msDecompressed = new MemoryStream())
                            {
                                using (BufferedStream buffDecompressed = new BufferedStream(msDecompressed, MAXBUFFER))
                                {
                                    msDecompressed.Position = 0;
                                    Formatter.Serialize(buffDecompressed, obj);
                                    msDecompressed.Position = 0;
                                    buffDecompressed.CopyTo(buffCompressed);
                                    msDecompressed.Position = 0;
                                    //byte[] byteArray = msDecompressed.ToArray();
                                    //buffCompressed.Write(byteArray, 0, byteArray.Length);
                                    //buffCompressed.Flush();
                                    //buffDecompressed.Close();
                                }
                            }
                            buffCompressed.Close();
                        }
                    }
                    return msCompressed.ToArray();
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T Deserializar<T>(byte[] arrBytes)
        {
            lock (_locker)
            {
                using (MemoryStream mem = new MemoryStream(arrBytes))
                {
                    using (GZipStream gZipStream = new GZipStream(mem, CompressionMode.Decompress))
                    {
                        using (BufferedStream buffDecompressed = new BufferedStream(gZipStream, MAXBUFFER))
                        {
                            object item = Formatter.Deserialize(buffDecompressed);
                            buffDecompressed.Close();
                            return (T)item;
                        }
                    }
                }
            }
        }

        #region Serializacion XML

        public static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings()
        {
            Encoding = new UTF8Encoding(false),
            Indent = false,
            NewLineHandling = NewLineHandling.None
        };

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] SerializarXML<T>(T obj)
        {
            try
            {
                using (MemoryStream m = new MemoryStream())
                {
                    using (BufferedStream buff = new BufferedStream(m, short.MaxValue))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(T));

                        using (XmlWriter writer = XmlWriter.Create(buff, XmlWriterSettings))
                        {
                            xml.Serialize(writer, obj);

                            m.Position = 0L;
                            return m.ToArray();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                return new byte[0];
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T DeserializarXML<T>(byte[] arrBytes)
        {
            using (MemoryStream m = new MemoryStream(arrBytes))
            {
                using (BufferedStream buff = new BufferedStream(m, short.MaxValue))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(T));
                    return (T)xml.Deserialize(buff);
                }
            }
        }

        #endregion
    }

    public class MensajesRegistros
    {
        private static readonly object _lock = new object();

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void baseLog(string id, string name, object parametro, byte type)
        {
            if (!SiAuto.Si.Enabled)
            {
                cargarLog(AppDomain.CurrentDomain.BaseDirectory);
            }

            try
            {
                id = id.Trim();
                name = name.Trim();
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = id;
                }
                if (parametro == null)
                {
                    switch (type)
                    {
                        case 1:
                            SiAuto.Main.LogWarning(name);
                            break;
                        case 2:
                            SiAuto.Main.LogError(name);
                            break;
                        case 3:
                            SiAuto.Main.LogMessage(name);
                            break;
                        default:
                            SiAuto.Main.LogObjectValue(name, "Null");
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case 1:

                            SiAuto.Main.LogWarning(name, parametro);
                            break;
                        case 2:
                            SiAuto.Main.LogError(name, parametro);
                            break;
                        case 99:
                            SiAuto.Main.LogException(name, parametro as Exception);
                            SiAuto.Main.LogError(GetFullMessage(parametro as Exception));
                            break;
                        default:
                            Type t = parametro.GetType();

                            if (t == typeof(byte) || t == typeof(sbyte) || t == typeof(int) || t == typeof(uint) ||
                                t == typeof(short) || t == typeof(ushort) || t == typeof(long) || t == typeof(ulong) ||
                                t == typeof(float) || t == typeof(double) || t == typeof(char) || t == typeof(bool) ||
                                t == typeof(object) || t == typeof(string) || t == typeof(decimal))
                                SiAuto.Main.LogObjectValue(name, parametro);
                            else if (t == typeof(byte[]))
                                SiAuto.Main.LogBinary(name, parametro as byte[]);
                            else
                                SiAuto.Main.LogObject(name, parametro, false);
                            break;
                    }
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = "Main";
                }
            }
        }

        public static void Informacion(string name)
        {
            lock (_lock) { baseLog(string.Empty, name, null, 3); }
        }

        public static void Informacion(string name, object parametro)
        {
            lock (_lock) { baseLog(string.Empty, name, parametro, 0); }
        }

        public static void Informacion(string id, string name, object parametro)
        {
            lock (_lock) { baseLog(id, name, parametro, 0); }
        }

        public static void Advertencia(string name, object parametro)
        {
            lock (_lock) { baseLog(string.Empty, name, parametro, 1); }
        }

        public static void Advertencia(string id, string name, object parametro)
        {
            lock (_lock) { baseLog(id, name, parametro, 0); }
        }

        public static void Error(string name, object parametro)
        {
            lock (_lock) { baseLog(string.Empty, name, parametro, 1); }
        }

        public static void Error(string id, string name, object parametro)
        {
            lock (_lock) { baseLog(id, name, parametro, 0); }
        }

        public static void Excepcion(string name, Exception exepcion)
        {
            lock (_lock) { baseLog(string.Empty, name, exepcion, 99); }
        }

        public static void Excepcion(string id, string name, Exception exepcion)
        {
            lock (_lock) { baseLog(id, name, exepcion, 99); }
        }

        public static void Object(string name, object item)
        {
            lock (_lock) { baseLog(string.Empty, name, item, 3); }
        }

        public static void Object(string id, string name, object item)
        {
            lock (_lock) { baseLog(id, name, item, 3); }
        }

        public static void EntrandaMetodo(string metodo)
        {
            EntrandaMetodo(string.Empty, metodo);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void EntrandaMetodo(string id, string metodo)
        {
            if (!SiAuto.Si.Enabled)
            {
                cargarLog(AppDomain.CurrentDomain.BaseDirectory);
            }

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = id;
                }

                SiAuto.Main.EnterMethod(metodo);
            }
            finally
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = "Main";
                }
            }
        }

        public static void SalidaMetodo(string metodo)
        {
            SalidaMetodo(string.Empty, metodo);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SalidaMetodo(string id, string metodo)
        {
            if (!SiAuto.Si.Enabled)
            {
                cargarLog(AppDomain.CurrentDomain.BaseDirectory);
            }

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = id;
                }

                SiAuto.Main.LeaveMethod(metodo);
            }
            finally
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SiAuto.Main.Name = "Main";
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetFullMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Mensaje: {0}", ex.Message).AppendLine()
              .AppendFormat("Pila: {0}", ex.StackTrace).AppendLine()
              .AppendFormat("Origen: {0}", ex.Source).AppendLine();

            Exception aux = ex.InnerException;

            while (aux != null)
            {
                sb.AppendFormat("Mensaje: {0}", aux.Message).AppendLine()
                  .AppendFormat("Pila: {0}", aux.StackTrace).AppendLine()
                  .AppendFormat("Origen: {0}", aux.Source).AppendLine();

                aux = aux.InnerException;
            }

            return sb.ToString().Trim();
        }

        private static string GetTitleFormat(string titulo)
        {
            bool isTesting = (ConfigurationManager.AppSettings["Pruebas"] ?? "No").IEquals("Si");
            return (titulo.Trim() + (isTesting ? " (Pruebas)"
                                               : string.Empty) + " (4.1)").Trim();
        }

        private static void cargarLog(string logPath)
        {
            FileInfo info = new FileInfo(Path.Combine(logPath, "HostModuloWeb.sil"));

            if (!info.Directory.Exists)
                info.Directory.Create();

            //Connections = file(append="true", 
            //                   maxparts="10", 
            //                   maxsize="512000", 
            //                   rotate="daily", 
            //                   buffer="8192", 
            //                   async.enabled="true", 
            //                   async.queue="2048",
            //                   async.clearondisconnect="true") 
            //                   Enabled = True

            ConnectionsBuilder builder = new ConnectionsBuilder();
            builder.BeginProtocol("file");
            builder.AddOption("filename", info.FullName);
            builder.AddOption("append", true);
            builder.AddOption("maxsize", 512000);
            builder.AddOption("rotate", FileRotate.Daily); //Valores que acepta: hourly, daily, weekly y monthly
            builder.AddOption("maxparts", "10");
            builder.EndProtocol();

            SiAuto.Si.Connections = builder.Connections;
            SiAuto.Si.Enabled = true;

            SiAuto.Main.LogSeparator();
            SiAuto.Main.LogMessage("Configuracion de log terminada");
        }

        public class EnterExitMethod : IDisposable
        {
            //private static object _lock = new object();

            public const string MAIN = "Main";

            private Stopwatch m_stopwatch;

            private Stopwatch m_stopwatchtemp;

            public EnterExitMethod()
            {
                if (!SiAuto.Si.Enabled)
                {
                    cargarLog(AppDomain.CurrentDomain.BaseDirectory);
                }
                SiAuto.Main.LogSeparator();
            }

            public EnterExitMethod(string metodo)
                : this(string.Empty, metodo)
            {
            }

            public EnterExitMethod(string id, string metodo)
                : this()
            {
                lock (_lock)
                {
                    this._id = id;
                    this._methodName = metodo;
                    this.m_stopwatch = new Stopwatch();
                    if (!string.IsNullOrEmpty(this._methodName))
                    {
                        MensajesRegistros.EntrandaMetodo(id, metodo);
                    }
                    this.m_stopwatch.Start();
                }
            }


            public void EnterThread(string name)
            {
                this.EnterThread(name, null);
            }

            public void EnterThread(string name, params object[] parameter)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (parameter == null)
                        {
                            SiAuto.Main.EnterThread(name);
                        }
                        else
                        {
                            SiAuto.Main.EnterThread(name, parameter);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LeaveThread(string name)
            {
                this.LeaveThread(name, null);
            }

            public void LeaveThread(string name, params object[] parameter)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (parameter == null)
                        {
                            SiAuto.Main.LeaveThread(name);
                        }
                        else
                        {
                            SiAuto.Main.LeaveThread(name, parameter);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }


            public void StartTimer(string title)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (this.m_stopwatchtemp == null)
                        {
                            this.m_stopwatchtemp = new Stopwatch();
                        }

                        this.m_stopwatchtemp.Start();
                        SiAuto.Main.LogObjectValue(title, string.Format("a las {0:HH:mm:ss.tttt}", DateTime.Now));
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void StopTimer(string title)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (this.m_stopwatchtemp != null)
                        {
                            this.m_stopwatchtemp.Stop();
                            SiAuto.Main.LogObjectValue(title, string.Format("Fin {0:HH:mm:ss.tttt}. [Total: {1} ({2}ms)]", DateTime.Now, this.m_stopwatchtemp.Elapsed, this.m_stopwatchtemp.ElapsedMilliseconds));
                            this.m_stopwatchtemp.Reset();
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogTimer(string title)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (this.m_stopwatchtemp != null)
                        {
                            SiAuto.Main.LogObjectValue(title, string.Format("Actual: {0} ({1}ms)", this.m_stopwatchtemp.Elapsed, this.m_stopwatchtemp.ElapsedMilliseconds));
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogMessage(string mensaje)
            {
                this.LogMessage(mensaje, null);
            }

            public void LogMessage(string mensaje, params object[] parameter)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (parameter == null)
                        {
                            SiAuto.Main.LogMessage(mensaje);
                        }
                        else
                        {
                            SiAuto.Main.LogMessage(mensaje, parameter);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogWarning(string mensaje)
            {
                this.LogWarning(mensaje, null);
            }

            public void LogWarning(string mensaje, params object[] parameter)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (parameter == null)
                        {
                            SiAuto.Main.LogWarning(mensaje);
                        }
                        else
                        {
                            SiAuto.Main.LogWarning(mensaje, parameter);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogError(string mensaje)
            {
                this.LogError(mensaje, null);
            }

            public void LogError(string mensaje, params object[] parameter)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (parameter == null)
                        {
                            SiAuto.Main.LogError(mensaje);
                        }
                        else
                        {
                            SiAuto.Main.LogError(mensaje, parameter);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogException(Exception e)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        SiAuto.Main.LogException(e);
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogException(string title, Exception e)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        if (string.IsNullOrEmpty(title))
                        {
                            SiAuto.Main.LogException(e);
                        }
                        else
                        {
                            SiAuto.Main.LogException(title, e);
                        }
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void LogObject(string title, object obj)
            {
                lock (_lock)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = this._id;
                        }

                        Type t = obj.GetType();

                        if (t == typeof(byte) || t == typeof(sbyte) || t == typeof(int) || t == typeof(uint) ||
                            t == typeof(short) || t == typeof(ushort) || t == typeof(long) || t == typeof(ulong) ||
                            t == typeof(float) || t == typeof(double) || t == typeof(char) || t == typeof(bool) ||
                            t == typeof(object) || t == typeof(string) || t == typeof(decimal))
                            SiAuto.Main.LogObjectValue(title, obj);
                        else if (t == typeof(byte[]))
                            SiAuto.Main.LogBinary(title, obj as byte[]);
                        else
                            SiAuto.Main.LogObject(title, obj, false);
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(this._id))
                        {
                            SiAuto.Main.Name = MAIN;
                        }
                    }
                }
            }

            public void Dispose()
            {
                if (this.m_stopwatchtemp != null)
                {
                    this.m_stopwatchtemp.Stop();
                    this.m_stopwatchtemp.Reset();
                    this.m_stopwatchtemp = null;
                }

                lock (_lock)
                {
                    if (this.m_stopwatch != null)
                    {
                        this.m_stopwatch.Stop();
                        if (!string.IsNullOrEmpty(this._methodName))
                        {
                            if (!string.IsNullOrEmpty(this._methodName))
                            {
                                MensajesRegistros.SalidaMetodo(this._id, string.Format("{0} ({1} - {2}ms)", this._methodName, this.m_stopwatch.Elapsed, this.m_stopwatch.ElapsedMilliseconds));
                            }
                        }
                        else
                        {
                            SiAuto.Main.LogMessage("Tiempo estimado {0} ({1} - {2}ms)", System.Threading.Thread.CurrentThread.ManagedThreadId, this.m_stopwatch.Elapsed, this.m_stopwatch.ElapsedMilliseconds);
                            SiAuto.Main.LogSeparator();
                        }
                        this.m_stopwatch.Reset();
                        this.m_stopwatch = null;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this._methodName))
                        {
                            MensajesRegistros.SalidaMetodo(this._id, this._methodName);
                        }
                    }

                    this._id = default(string);
                    this._methodName = default(string);
                }
            }

            private string _methodName;

            private string _id;
        }
    }

    //public class MensajesRegistros
    //{
    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
    //    public static void Informacion(string titulo, string msj)
    //    {
    //        try { System.Diagnostics.EventLog.WriteEntry(titulo + ((ConfigurationManager.AppSettings["Pruebas"] ?? "No").IEquals("Si") ? " Pruebas" : string.Empty) + " (4.1)", msj, System.Diagnostics.EventLogEntryType.Information); }
    //        catch { }
    //    }

    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
    //    public static void Advertencia(string titulo, string msj)
    //    {
    //        try { System.Diagnostics.EventLog.WriteEntry(titulo + ((ConfigurationManager.AppSettings["Pruebas"] ?? "No").IEquals("Si") ? " Pruebas" : string.Empty) + " (4.1)", msj, System.Diagnostics.EventLogEntryType.Warning); }
    //        catch { }
    //    }

    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
    //    public static void Error(string titulo, string msj)
    //    {
    //        try { System.Diagnostics.EventLog.WriteEntry(titulo + ((ConfigurationManager.AppSettings["Pruebas"] ?? "No").IEquals("Si") ? " Pruebas" : string.Empty) + " (4.1)", msj, System.Diagnostics.EventLogEntryType.Error); }
    //        catch { }
    //    }

    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
    //    public static void Error(string titulo, Exception ex)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendFormat("Mensaje: {0}", ex.Message).AppendLine()
    //          .AppendFormat("Pila: {0}", ex.StackTrace).AppendLine()
    //          .AppendFormat("Origen: {0}", ex.Source).AppendLine();

    //        Exception aux = ex.InnerException;

    //        while (aux != null)
    //        {
    //            sb.AppendFormat("Mensaje: {0}", aux.Message).AppendLine()
    //              .AppendFormat("Pila: {0}", aux.StackTrace).AppendLine()
    //              .AppendFormat("Origen: {0}", aux.Source).AppendLine();

    //            aux = aux.InnerException;
    //        }

    //        Error(titulo, sb.ToString().Trim());
    //    }
    //}
}