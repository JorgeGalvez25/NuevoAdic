using System;
using System.Net.Sockets;
using Adicional.Entidades.Bloqueos;
using Adicional.Entidades.Sockets.Extenciones;

namespace Adicional.Entidades.SocketBidireccional
{
    public class StateObjectBidireccional
    {
        private static readonly object _lock = new object();

        public StateObjectBidireccional()
            : this(ConstantesSocket.MAX_BUFFER_SIZE)
        {

        }

        public StateObjectBidireccional(int bufferSize)
        {
            this.Buffer = new byte[bufferSize];
            this.Id = string.Empty;
        }

        public string Id;

        public byte[] Buffer;

        public int BytesReceived;

        public int OffSet;

        public Socket WorkSocket;

        public bool IsConnected()
        {
            return this.IsConnected(true);
        }

        public bool IsConnected(bool useCache)
        {
            return this.WorkSocket.IsConnected(useCache);
            //using (LockMutex _lock = new LockMutex(TimeSpan.FromMilliseconds(ConstantesSocket.MAX_TIME_OUT), this.Id))
            //{

            //}
            //return TryMutex<bool>(TimeSpan.FromMilliseconds(ConstantesSocket.MAX_TIME_OUT), this.Id, () =>
            //    {
            //        return this.WorkSocket.IsConnected(useCache);
            //    });
        }

        public void ClearAll()
        {
            lock (_lock)
            {
                this.BytesReceived =
                    this.OffSet = 0;
                Array.Clear(this.Buffer, 0, this.Buffer.Length);
                this.Buffer = default(byte[]);

                if (this.WorkSocket != null)
                {
                    //Gurock.SmartInspect.SiAuto.Main.LogWarning("Socket cerrado para {0}", this.Id);
                    //Gurock.SmartInspect.SiAuto.Main.LogStackTrace(this.Id, new System.Diagnostics.StackTrace(System.Threading.Thread.CurrentThread, true));
                    try { this.WorkSocket.Close(0); }
                    catch { }
                    this.WorkSocket = null;
                }
                this.Id = default(string);
            }
        }

        public void ClearBuffer()
        {
            this.ClearBuffer(ConstantesSocket.MAX_BUFFER_SIZE);
        }

        public void ClearBuffer(int newSize)
        {
            lock (_lock)
            {
                this.BytesReceived =
                    this.OffSet = 0;

                if (this.Buffer != null)
                {
                    Array.Clear(this.Buffer, 0, this.Buffer.Length);
                    Array.Resize(ref this.Buffer, newSize);
                }
                else
                {
                    this.Buffer = new byte[newSize];
                }
            }
        }

        public byte[] SetSynEtxToBuffer(byte[] buffer)
        {
            byte[] _inner = GetCleanBuffer(buffer);
            if (_inner.Length <= 0) { return new byte[0]; }
            lock (_lock)
            {
                byte[] buff = new byte[_inner.Length + 2];
                buff[0] = ConstantesSocket.SYN_BYTE;
                System.Buffer.BlockCopy(_inner, 0, buff, 1, _inner.Length);
                buff[_inner.Length + 1] = ConstantesSocket.ETX_BYTE;
                return buff;
            }
        }

        public byte[] SetStxEtxToBuffer(byte[] buffer)
        {
            byte[] _inner = GetCleanBuffer(buffer);
            if (_inner.Length <= 0) { return new byte[0]; }
            lock (_lock)
            {
                byte[] buff = new byte[_inner.Length + 2];
                buff[0] = ConstantesSocket.STX_BYTE;
                System.Buffer.BlockCopy(_inner, 0, buff, 1, buffer.Length);
                buff[_inner.Length + 1] = ConstantesSocket.ETX_BYTE;
                return buff;
            }
        }

        public byte[] GetCleanBuffer(byte[] arg)
        {
            return GetCleanBuffer(arg, 0, arg.Length);
        }

        public byte[] GetCleanBuffer(byte[] arg, int offset, int length)
        {
            lock (_lock)
            {
                byte[] result = new byte[length];
                int currentIdx = 0;

                for (int i = offset; i < length; i++)
                {
                    switch (arg[i])
                    {
                        case ConstantesSocket.NULL_BYTE:
                        case ConstantesSocket.SOH_BYTE:
                        case ConstantesSocket.SYN_BYTE:
                        case ConstantesSocket.STX_BYTE:
                        case ConstantesSocket.ACK_BYTE:
                        case ConstantesSocket.ESC_BYTE:
                            continue;
                    }
                    if (arg[i] == ConstantesSocket.ETX_BYTE)
                    {
                        result[currentIdx] = arg[i];
                        break;
                    }
                    result[currentIdx++] = arg[i];
                }

                Array.Resize(ref result, currentIdx);

                return result;
            }
        }

        public StateObjectBidireccional Clone()
        {
            return (StateObjectBidireccional)this.MemberwiseClone();
        }

        //private const string MUTEXT_NAME = "Global\\IGAS_SERVICIOS_{0}";
        //private static readonly IDictionary<string, Mutex> m_cacheMutex = new Dictionary<string, Mutex>();

        //[MethodImpl(MethodImplOptions.Synchronized)]
        //[HostProtection(SecurityAction.Demand, Synchronization = true, ExternalProcessMgmt = true, SharedState = true)]
        //public static T TryMutex<T>(TimeSpan timeout, string id, Func<T> f)
        //{
        //    bool mutexAquired = false;
        //    Mutex _mutex = null;
        //    try
        //    {
        //        bool unique = false;
        //    again:
        //        string key = string.Format(MUTEXT_NAME, id);
        //        _mutex = null;

        //        if (!m_cacheMutex.TryGetValue(key, out _mutex))
        //        {
        //            try
        //            {
        //                _mutex = Mutex.OpenExisting(key, MutexRights.Synchronize);
        //                unique = false;
        //            }
        //            catch (Exception)
        //            {
        //                MutexSecurity security = new MutexSecurity();
        //                security.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));
        //                _mutex = new Mutex(false, key, out unique, security);
        //            }
        //            finally
        //            {
        //                if (_mutex != null)
        //                {
        //                    m_cacheMutex.Add(key, _mutex);
        //                }
        //            }
        //        }
        //        if (!(mutexAquired = _mutex.WaitOne(timeout, false)))
        //        {
        //            Thread.Sleep(1);
        //            goto again;
        //        }

        //        return f();
        //    }
        //    finally
        //    {
        //        if (_mutex != null && mutexAquired)
        //        {
        //            _mutex.ReleaseMutex();
        //            mutexAquired = false;
        //        }
        //    }
        //}
    }
}
