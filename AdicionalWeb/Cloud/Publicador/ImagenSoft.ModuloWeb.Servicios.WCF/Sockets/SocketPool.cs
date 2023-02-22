using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Utileria;
using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Threading;

namespace ImagenSoft.ModuloWeb.Servicios.WCF.Sockets
{
    public sealed class Pool : IDisposable
    {
        //private readonly Queue<PoolItem> _items = new Queue<PoolItem>();
        private readonly Queue _items = new Queue();

        private AsyncOperation m_context;

        private int m_threadCount;

        private int m_maxThreads;

        private bool f_disposed;

        public Pool(int maxThreads)
        {
            this._items = Queue.Synchronized(new Queue());
            this.m_maxThreads = maxThreads;
            this.f_disposed = false;
            this.m_context = AsyncOperationManager.CreateOperation(null);
        }

        public void Enqueue(PoolItem worker)
        {
            MensajesRegistros.Informacion("Encolando nuevo socket");
            if (this.f_disposed) return;
            this._items.Enqueue(worker);
            this.verify();
        }

        public void startThread(PoolItem current)
        {
            if (this.f_disposed) return;
            Interlocked.Increment(ref m_threadCount);

            ThreadPool.QueueUserWorkItem(new WaitCallback((i) =>
            {
                using (PoolItem _internal = (i as PoolItem))
                {
                    try
                    {
                        MensajesRegistros.Informacion("Entrando al hilo de ejecución");
                        if (_internal.Task != null)
                        {
                            _internal.Task();
                        }
                        else if (_internal.ParametrizedTask != null)
                        {
                            _internal.ParametrizedTask(_internal.Parameter);
                        }
                    }
                    catch (Exception e)
                    {
                        MensajesRegistros.Excepcion("SocketBidi", e);
                    }
                    finally
                    {
                        MensajesRegistros.Informacion("Terminando hilo de ejecución");
                        if (!this.f_disposed)
                        {
                            Interlocked.Decrement(ref m_threadCount);
                            m_context.Post(_ => verify(), null);
                        }
                    }
                }
            }), current);
        }

        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        private void verify()
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("private void verify()"))
            {
                if (this.f_disposed) return;

                Lock.Try(this._items, () =>
                    {
                        PoolItem current = null;
                        bool isQueue = this._items.Count > 0;
                        _log.LogObject("Encolados", this._items.Count);

                        while ((isQueue && this.m_threadCount <= this.m_maxThreads) && !this.f_disposed)
                        {
                            _log.LogMessage("Tratando de ejecutar socket");
                            current = (PoolItem)this._items.Dequeue();

                            if (current != null) { this.startThread(current); }
                            else { _log.LogMessage("No se pudo, no existe"); }

                            isQueue = (this._items.Count > 0);
                            if (!isQueue) { break; }
                        }
                    });
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!this.f_disposed)
            {
                GC.SuppressFinalize(this);
                this.f_disposed = true;
                this.m_threadCount = 0;
                this.m_maxThreads = 0;

                if (this._items != null && this._items.Count > 0)
                {
                    this._items.Clear();
                }

                if (this.m_context != null)
                {
                    this.m_context.OperationCompleted();
                    this.m_context = null;
                }
            }
        }

        #endregion
    }

    public class PoolItem : IDisposable
    {
        public PoolItem()
        {
            this.disposed = false;
        }

        public PoolItem(Action task)
            : this()
        {
            this.Task = task;
        }

        public PoolItem(Action<object> task, object parameter)
            : this()
        {
            this.ParametrizedTask = task;
            this.Parameter = parameter;
        }

        public Action Task { get; set; }

        public Action<object> ParametrizedTask { get; set; }

        public object Parameter { get; set; }

        #region IDisposable Members

        private bool disposed;

        public void Dispose()
        {
            if (!this.disposed)
            {
                GC.SuppressFinalize(this);
                this.disposed = true;
                this.Task = null;
                this.ParametrizedTask = null;
                this.Parameter = null;
            }
        }

        #endregion
    }
}
