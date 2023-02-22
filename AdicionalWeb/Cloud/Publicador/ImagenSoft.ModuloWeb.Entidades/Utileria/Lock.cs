using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ImagenSoft.ModuloWeb.Entidades.Utileria
{
    public static class Lock
    {
        public static readonly object ADMINISTRADOR_SESIONES_LOG = new object();

        public static readonly object CACHE_CLIENTES_SOCKET = new object();

        private static readonly TimeSpan WAIT_MINUTES = new TimeSpan(0, 0, 10);
        private const string MUTEXT_NAME = "Global\\MODULOWEB_MUTEXT";
        private const string MUTEXT_NAME_FRMT = "Global\\MODULOWEB_MUTEXT_{0}";

        public static T Try<T>(object objLock, Func<T> f)
        {
            return Try(objLock, WAIT_MINUTES, f);
        }

        public static T Try<T>(object objLock, int miliseconds, Func<T> f)
        {
            return Try(objLock, TimeSpan.FromMilliseconds(miliseconds), f);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T Try<T>(object objLock, TimeSpan timeout, Func<T> f)
        {
            if (System.Threading.Monitor.TryEnter(objLock, timeout))
            {
                try
                {
                    return f.Invoke();
                }
                finally
                {
                    System.Threading.Monitor.Exit(objLock);
                }
            }
            return default(T);
        }


        public static void Try(object objLock, Action f)
        {
            Try(objLock, TimeSpan.FromTicks(TimeSpan.TicksPerHour), f);
        }

        public static void Try(object objLock, int miliseconds, Action f)
        {
            Try(objLock, TimeSpan.FromMilliseconds(miliseconds), f);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Try(object objLock, TimeSpan timeout, Action f)
        {
            if (Monitor.TryEnter(objLock, timeout))
            {
                try
                {
                    f.Invoke();
                }
                finally
                {
                    Monitor.Exit(objLock);
                }
            }
        }


        public static T TryMutex<T>(Func<T> f)
        {
            return TryMutex(WAIT_MINUTES, f);
        }

        public static T TryMutex<T>(int miliseconds, Func<T> f)
        {
            return TryMutex(TimeSpan.FromMilliseconds(miliseconds), f);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T TryMutex<T>(TimeSpan timeout, Func<T> f)
        {
            bool unique = true;
            Mutex _mutex = new Mutex(true, MUTEXT_NAME, out unique);

            try
            {
                if (!unique)
                {
                    if (!_mutex.WaitOne(timeout, false))
                    {
                        return default(T);
                    }
                }

                return f.Invoke();
            }
            catch
            {
                return f.Invoke();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }


        public static void TryMutex(Action f)
        {
            Lock.TryMutex(WAIT_MINUTES, f);
        }

        public static void TryMutex(int miliseconds, Action f)
        {
            Lock.TryMutex(TimeSpan.FromMilliseconds(miliseconds), f);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void TryMutex(TimeSpan timeout, Action f)
        {
            //Mutex _mutex = Mutex.OpenExisting(MUTEXT_NAME);
            bool unique = true;
            Mutex _mutex = new Mutex(true, MUTEXT_NAME, out unique);
            try
            {
                if (!unique)
                {
                    _mutex.WaitOne(timeout, false);
                }
                f.Invoke();
            }
            catch
            {
                f.Invoke();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }


        public static void TryMutex(string id, Action f)
        {
            Lock.TryMutex(WAIT_MINUTES, f);
        }

        public static void TryMutex(string id, int miliseconds, Action f)
        {
            Lock.TryMutex(TimeSpan.FromMilliseconds(miliseconds), f);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void TryMutex(TimeSpan timeout, string id, Action f)
        {
            bool unique = true;
            Mutex _mutex = new Mutex(true, string.Format(MUTEXT_NAME_FRMT, id), out unique);
            try
            {
                if (!unique)
                {
                    _mutex.WaitOne(timeout, false);
                }
                f.Invoke();
            }
            catch
            {
                f.Invoke();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}
