using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Adicional.Entidades.Sockets.Extenciones;

namespace Adicional.Entidades.SocketBidireccional
{
    [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
    public sealed class ClientManager
    {
        private static readonly object _lock = new object();

        private static IDictionary<string, StateObjectBidireccional> clientPoll = new Dictionary<string, StateObjectBidireccional>();

        public static ICollection<string> Keys
        {
            get { lock (_lock) { return clientPoll.Keys; } }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static StateObjectBidireccional Add(StateObjectBidireccional stateObject)
        {
            lock (_lock)
            {
            again:

                StateObjectBidireccional result = null;
                if (clientPoll.TryGetValue(stateObject.Id, out result))
                {
                    if (!internalCheckIntegrity(stateObject.Id))
                    {
                        clientPoll[stateObject.Id].ClearAll();
                        clientPoll[stateObject.Id] = stateObject;
                        return clientPoll[stateObject.Id];
                    }

                    internalRemove(stateObject.Id);
                    goto again;
                }

                clientPoll.Add(stateObject.Id, stateObject);
                return stateObject;
            }
        }

        public static bool Remove(string key)
        {
            lock (_lock)
            {
                return internalRemove(key);
            }
        }

        public static bool Remove(StateObjectBidireccional stateObject)
        {
            return (stateObject == null ? true : Remove(stateObject.Id));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool RemoveAll()
        {
            lock (_lock)
            {
                InternalRemoveAll();
                return true;
            }
        }

        public static void VerifyAll()
        {
            lock (_lock)
            {
                List<string> toRemove = new List<string>(clientPoll.Keys.Count);
                try
                {
                    foreach (string item in clientPoll.Keys)
                    {
                        if (!internalCheckIntegrity(item))
                        {
                            toRemove.Add(item);
                        }
                    }

                    toRemove.ForEach(p => internalRemove(p));
                }
                finally
                {
                    toRemove.Clear();
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static StateObjectBidireccional Get(string key)
        {
            lock (_lock)
            {
                StateObjectBidireccional result = null;
                return clientPoll.TryGetValue(key, out result) ? result : null;
            }
        }

        public static StateObjectBidireccional Get(StateObjectBidireccional item)
        {
            if (item == null) return null;
            return Get(item.Id);
        }

        public StateObjectBidireccional this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        public StateObjectBidireccional this[StateObjectBidireccional item]
        {
            get
            {
                return Get(item);
            }
            set
            {
                Add(item);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void InternalRemoveAll()
        {
            if (clientPoll == null) return;
            if (clientPoll.Count <= 0) return;

            foreach (var item in clientPoll.Values)
            {
                try { item.ClearAll(); }
                catch { }
            }
            clientPoll.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static bool internalRemove(string key)
        {
            if (string.IsNullOrEmpty(key)) { return true; }
            if (clientPoll.ContainsKey(key))
            {
                clientPoll[key].ClearAll();
                //SiAuto.Main.LogWarning("Eliminando {0}", key);
                //SiAuto.Main.LogStackTrace(key, new System.Diagnostics.StackTrace(System.Threading.Thread.CurrentThread, true));
                return clientPoll.Remove(key);
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static bool internalCheckIntegrity(string key)
        {
            if (clientPoll[key] != null)
            {
                return true;
            }
            else if (clientPoll[key].WorkSocket != null)
            {
                return true;
            }
            else if (clientPoll[key].WorkSocket.IsConnected())
            {
                return true;
            }

            return false;
        }
    }
}
