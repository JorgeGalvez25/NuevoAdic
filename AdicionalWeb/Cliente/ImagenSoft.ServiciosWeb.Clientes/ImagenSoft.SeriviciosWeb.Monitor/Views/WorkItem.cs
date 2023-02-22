using System;
using System.Collections.Generic;
using ImagenSoft.ServiciosWeb.Entidades;

namespace ImagenSoft.SeriviciosWeb.Monitor.Views
{
    public class MonitorWorkItem
    {
        public static WorkItem WorkItem = new WorkItem();
    }

    public class WorkItem
    {
        public WorkItem()
        {
            this.Services = new Dictionary<Type, object>();
        }
        public Sesion Sesion { get; set; }

        public readonly Dictionary<Type, object> Services;
    }
}
