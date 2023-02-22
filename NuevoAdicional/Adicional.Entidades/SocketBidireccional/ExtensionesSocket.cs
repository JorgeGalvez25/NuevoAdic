using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Adicional.Entidades.Sockets.Extenciones
{
    public static class SocketExtenders
    {
        private static byte[] EMPTY_BYTES = new byte[0];

        private static Dictionary<string, CacheItem> m_cache = new Dictionary<string, CacheItem>();

        private class CacheItem
        {
            public CacheItem(DateTime caduca, SocketError result)
            {
                this.Caduca = caduca;
                this.Result = result;
            }

            public DateTime Caduca { get; set; }

            public SocketError Result { get; set; }
        }

        public static bool IsConnected(this Socket socket)
        {
            return IsConnected(socket, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool IsConnected(this Socket socket, bool useCache)
        {
            bool flgError = false;
            SocketError error = SocketError.Success;
            try
            {
                if (socket.Poll(3000, SelectMode.SelectError))
                {
                    flgError = true;
                    return false;
                }

                if (socket.RemoteEndPoint == null)
                {
                    flgError = true;
                    return false;
                }

                IPEndPoint _local = (socket.RemoteEndPoint as IPEndPoint);
                string id = string.Format("{0}:{1}", _local.Address, _local.Port);

                lock (m_cache)
                {
                    if (m_cache.ContainsKey(id))
                    {
                        if (!socket.Poll(0, SelectMode.SelectRead) && !socket.Poll(0, SelectMode.SelectWrite))
                        {
                            m_cache.Remove(id);
                        }
                        else if (useCache)
                        {
                            if (m_cache[id].Caduca > DateTime.Now)
                            {
                                return (m_cache[id].Result == SocketError.Success);
                            }

                            m_cache.Remove(id);
                        }
                        else
                        {
                            m_cache.Remove(id);
                        }
                    }
                }

                int count = 0;
                const int maxRetry = 4;

                if (!socket.Poll(0, SelectMode.SelectRead) && !socket.Poll(0, SelectMode.SelectWrite))
                {
                    flgError = true;
                    return false;
                }
            again:
                error = SocketError.Success;
                IAsyncResult asyncSend = null;
                if (socket.Blocking)
                {
                    socket.Send(EMPTY_BYTES, 0, EMPTY_BYTES.Length, SocketFlags.None, out error);
                }
                else
                {
                    asyncSend = socket.BeginSend(EMPTY_BYTES, 0, EMPTY_BYTES.Length, SocketFlags.None, out error, null, null);

                    if (asyncSend != null)
                    {
                        if (!asyncSend.IsCompleted)
                        {
                            asyncSend.AsyncWaitHandle.WaitOne(socket.SendTimeout);
                        }

                        socket.EndSend(asyncSend, out error);
                    }
                }

                switch (error)
                {
                    case SocketError.TryAgain:
                    case SocketError.TimedOut:
                        if (count++ < maxRetry)
                        {
                            goto again;
                        }
                        break;
                    case SocketError.Success:

                        if (!socket.Poll(0, SelectMode.SelectWrite))
                        {
                            flgError = true;
                            return false;
                        }

                        lock (m_cache) { m_cache.Add(id, new CacheItem(DateTime.Now.AddMinutes(1), error)); }
                        return true;
                }

                flgError = true;
                return false;
            }
            catch //(Exception e)
            {
                flgError = true;
                //SiAuto.Main.LogException(e);
                //SiAuto.Main.LogStackTrace("Exception.IsConnected", new System.Diagnostics.StackTrace(e));
                return false;
            }
            //finally
            //{
            //    if (flgError && (error != SocketError.Success))
            //    {
            //        try
            //        {
            //            SiAuto.Main.LogSeparator();
            //            SiAuto.Main.EnterMethod("public static bool IsConnected(this Socket socket, bool useCache)");
            //            SiAuto.Main.LogMessage("Hubo un error en el envio ({0})", error);
            //            if (socket == null)
            //                SiAuto.Main.LogMessage("Parametro 'socket' nulo");
            //            else
            //                SiAuto.Main.LogObject("socket", socket);
            //            SiAuto.Main.LogObjectValue("useCache", useCache);
            //            SiAuto.Main.LogWarning("No hay conexión activa del socket.");
            //        }
            //        finally
            //        {
            //            SiAuto.Main.LeaveMethod("public static bool IsConnected(this Socket socket, bool useCache)");
            //        }
            //    }
            //}
        }

        public static void SetSocketKeepAliveValues(this Socket instance, int KeepAliveTime, int KeepAliveInterval)
        {
            //KeepAliveTime: default value is 2hr
            //KeepAliveInterval: default value is 1s and Detect 5 times

            //the native structure
            //struct tcp_keepalive {
            //  ULONG onoff;
            //  ULONG keepalivetime;
            //  ULONG keepaliveinterval;
            //};

            int size = Marshal.SizeOf(new uint());
            byte[] inOptionValues = new byte[size * 3]; // 4 * 3 = 12
            bool OnOff = true;

            BitConverter.GetBytes((uint)(OnOff ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)KeepAliveTime).CopyTo(inOptionValues, size);
            BitConverter.GetBytes((uint)KeepAliveInterval).CopyTo(inOptionValues, size * 2);

            instance.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }

    public static class StreamExtensions
    {
        private const int DEFAULT_BUFFER_SIZE = short.MaxValue; // short = 32767, ushort = 65535 (en bytes)

        #region Framework 3.5

        public static void CopyTo(this Stream input, Stream output)
        {
            input.CopyTo(output, DEFAULT_BUFFER_SIZE);
        }

        public static void CopyTo(this Stream input, Stream output, int bufferSize)
        {
            if (!input.CanRead) throw new InvalidOperationException("input must be open for reading");
            if (!output.CanWrite) throw new InvalidOperationException("output must be open for writing");

            if (input is MemoryStream) { input.Seek(0, SeekOrigin.Begin); }
            if (output is MemoryStream) { output.Seek(0, SeekOrigin.Begin); }

            byte[][] buf = { new byte[bufferSize], new byte[bufferSize] };
            int[] bufl = { 0, 0 };
            int bufno = 0;
            IAsyncResult read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            IAsyncResult write = null;

            while (true)
            {
                // wait for the read operation to complete
                read.AsyncWaitHandle.WaitOne();
                bufl[bufno] = input.EndRead(read);

                // if zero bytes read, the copy is complete
                if (bufl[bufno] == 0) { break; }

                // wait for the in-flight write operation, if one exists, to complete
                // the only time one won't exist is after the very first read operation completes
                if (write != null)
                {
                    write.AsyncWaitHandle.WaitOne();
                    output.EndWrite(write);
                }

                // start the new write operation
                write = output.BeginWrite(buf[bufno], 0, bufl[bufno], null, null);

                // toggle the current, in-use buffer
                // and start the read operation on the new buffer.
                //
                // Changed to use XOR to toggle between 0 and 1.
                // A little speedier than using a ternary expression.
                bufno ^= 1; // bufno = ( bufno == 0 ? 1 : 0 ) ;
                read = input.BeginRead(buf[bufno], 0, buf[bufno].Length, null, null);
            }

            // wait for the final in-flight write operation, if one exists, to complete
            // the only time one won't exist is if the input stream is empty.
            if (write != null)
            {
                write.AsyncWaitHandle.WaitOne();
                output.EndWrite(write);
            }

            output.Flush();
        }

        #endregion
    }
}

