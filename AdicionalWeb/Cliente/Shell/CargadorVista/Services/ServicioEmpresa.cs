using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.Interfaces;
using ImagenSoft.FACELEI.Negocio.Entidades;
using Microsoft.Practices.CompositeUI;
using ImagenSoft.Framework.Entidades;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.FACELEI.Generales.Proveedor;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServicioEmpresa :
        IServiciosModulo<ListaEmpresaFACELEI, EmpresaFACELEI, FiltroEmpresaFACELEI, FiltroEmpresaFACELEI>
    {
        private WorkItem _WorkItem;

        public ServicioEmpresa(WorkItem w)
        {
            this._WorkItem = w;
        }

        private Sesion ObtenerSesion()
        {
            Sesion sesion = this._WorkItem.RootWorkItem.Items[ConstantesFACELEI.SESION_SISTEMA] as Sesion;

            if (sesion != null)
            {
                return sesion;
            }
            else
            {
                throw new Exception(ListadoMensajes.Error_Sesion_No_Existe);
            }
        }

        #region IServiciosModulo<ListaEmpresaFACELEI,EmpresaFACELEI,FiltroEmpresaFACELEI,FiltroEmpresaFACELEI> Members

        public int Consecutivo(FiltroEmpresaFACELEI f)
        {
            throw new NotImplementedException();
        }

        public int Consecutivo()
        {
            return ServiciosGenerales.EmpresaFACELEIConsecutivo(this.ObtenerSesion());
        }

        public bool Eliminar(FiltroEmpresaFACELEI f)
        {
            return ServiciosGenerales.EmpresaFACELEIEliminar(this.ObtenerSesion(), f);
        }

        public bool Existe(FiltroEmpresaFACELEI f)
        {
            return ServiciosGenerales.EmpresaFACELEIExiste(this.ObtenerSesion(), f);
        }

        public EmpresaFACELEI Insertar(EmpresaFACELEI e)
        {
            return ServiciosGenerales.EmpresaFACELEIInsertar(this.ObtenerSesion(), e);
        }

        public EmpresaFACELEI Modificar(EmpresaFACELEI e)
        {
            return ServiciosGenerales.EmpresaFACELEIModificar(this.ObtenerSesion(), e);
        }

        public EmpresaFACELEI Obtener(FiltroEmpresaFACELEI f)
        {
            return ServiciosGenerales.EmpresaFACELEIObtener(this.ObtenerSesion(), f);
        }

        public ListaEmpresaFACELEI ObtenerTodosFiltro(FiltroEmpresaFACELEI t, ImagenSoft.Framework.Entidades.Paginacion p)
        {
            return ServiciosGenerales.EmpresaFACELEIObtenerTodosFiltro(this.ObtenerSesion(), p, t);
        }

        public ListaEmpresaFACELEI ObtenerTodosFiltro(FiltroEmpresaFACELEI t)
        {
            return ServiciosGenerales.EmpresaFACELEIObtenerTodosFiltro(this.ObtenerSesion(), new Paginacion(int.MaxValue, 0, 1, 0, 0), t);
        }

        public bool PermiteEliminar(FiltroEmpresaFACELEI f)
        {
            return ServiciosGenerales.EmpresaFACELEIPermiteEliminar(this.ObtenerSesion(), f);
        }

        #endregion
    }
}
