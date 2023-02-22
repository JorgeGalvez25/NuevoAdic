using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Base;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class MonitorConexiones : MonitorBase,
                                     IComparable<MonitorConexiones>
    {
        #region IComparable<MonitorConexiones> Members

        public int CompareTo(MonitorConexiones other)
        {
            if (base.CompareTo(other) == 1) { return 1; }
            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMonitorConexiones : List<MonitorConexiones>
    {
        public ListaMonitorConexiones OrdenarPor(OrdenarMonitor orden)
        {
            ListaMonitorConexiones aux = new ListaMonitorConexiones();

            switch (orden)
            {
                case OrdenarMonitor.DiasAtraso:
                    aux.AddRange(this.OrderByDescending(p => p.DiasAtraso));
                    break;
                case OrdenarMonitor.NoEstacion:
                    aux.AddRange(this.OrderBy(p => p.Estacion));
                    break;
                case OrdenarMonitor.NombreComercial:
                    aux.AddRange(this.OrderBy(p => p.NombreComercial));
                    break;
            }

            return aux;
        }

        public ListaMonitorConexiones BuscarPorNombreComercial(string nomComercial)
        {
            ListaMonitorConexiones aux = new ListaMonitorConexiones();
            aux.AddRange(this.Where(p => p.NombreComercial.ToUpper().Contains(nomComercial.ToUpper())));
            return aux;
        }

        public ListaMonitorConexiones ObtenerPorFiltro(FiltroMonitorConexiones filtro)
        {
            ListaMonitorConexiones aux = new ListaMonitorConexiones();
            aux.AddRange(this.Where(p => p.EstatusConexion == filtro.EstatusConexion));

            return aux;
        }

        public ListaMonitorConexiones ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaMonitorConexiones result = new ListaMonitorConexiones();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaMonitorConexiones()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroMonitorConexiones : FiltroMonitorBase
    {
        public FiltroMonitorConexiones()
            : base()
        {
        }
    }
}
