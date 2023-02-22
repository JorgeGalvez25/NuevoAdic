using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using ImagenSoft.Interfaces;
using ImagenSoft.FACELEI.Generales.Proveedor;
using ImagenSoft.Framework.Entidades;
using EstandarCliente.CargadorVistas.Constants;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServicioFechaHora : IServiciosFechaHoraServidor
    {
        public WorkItem _WorkItem;

        public ServicioFechaHora(WorkItem workItem)
        {
            _WorkItem = workItem;
        }

        #region Privadas

        private Sesion ObtenerSesion()
        {
            Sesion sesion = _WorkItem.RootWorkItem.Items[ConstantesFACELEI.SESION_SISTEMA] as Sesion;
            if (sesion != null)
            {
                return sesion;
            }
            else
            {
                throw new Exception("No existe sesión de sistema");
            }
        }

        #endregion

        #region IServiciosFechaHoraServidor Members

        public DateTime ObtenerFecha()
        {
            return Convert.ToDateTime(ServiciosGenerales.ObtenerFechaHoraServidorFACELEI(ObtenerSesion()).ToShortDateString());
        }

        public DateTime ObtenerFechaHora()
        {
            return ServiciosGenerales.ObtenerFechaHoraServidorFACELEI(ObtenerSesion());
        }

        public DateTime ObtenerHora()
        {
            return Convert.ToDateTime(ServiciosGenerales.ObtenerFechaHoraServidorFACELEI(ObtenerSesion()).ToShortTimeString());
        }

        #endregion
    }
}
