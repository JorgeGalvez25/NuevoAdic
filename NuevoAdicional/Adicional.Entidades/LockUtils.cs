using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Adicional.Entidades.Bloqueos
{
    public class LockMutex : IDisposable
    {
        private Mutex m_mutex;

        private const string MUTEX_NAME_FORMAT = "Global\\MODULO_WEB_{0}";

        public bool IsAcquired { get; private set; }

        public LockMutex(TimeSpan timeOut, string id)
        {
            bool created;
            var security = new MutexSecurity();
            security.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Allow));
            string name = string.Format(MUTEX_NAME_FORMAT, id);
            m_mutex = new Mutex(false, name, out created, security);
            IsAcquired = m_mutex.WaitOne(timeOut);
        }

        public LockMutex(TimeSpan timeOut)
            : this(timeOut, string.Empty)
        {

        }

        #region IDisposable Members

        public void Dispose()
        {
            if (IsAcquired && (m_mutex != null))
            {
                m_mutex.ReleaseMutex();
                IsAcquired = false;
            }
        }

        #endregion
    }
}
