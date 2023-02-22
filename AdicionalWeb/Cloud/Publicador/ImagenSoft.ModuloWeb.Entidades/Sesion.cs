using ImagenSoft.ModuloWeb.Entidades.Base;
using ImagenSoft.ModuloWeb.Entidades.Web;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public partial class SesionModuloWeb// :ImagenSoft.Framework.Entidades.Sesion
    {
        public SesionModuloWeb()
        //: base()
        {
            //this.Usuario = new ImagenSoft.Framework.Entidades.Usuario();
            this.Usuario = new UsuarioModuloWeb();
            this.Privilegio = NivelPrivilegio.SinPermiso;
            this.Nombre = string.Empty;
            this.NoCliente = string.Empty;
            this.Distribuidor = string.Empty;
            this.Sistema = string.Empty;
            this.Version = AdministrarClientes.VERSION;
            this.FechaHoraCliente = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Aplicaciones = new ListaAplicaciones();
        }

        //public SesionModuloWeb(ImagenSoft.Framework.Entidades.Sesion sesion)
        //    : base(sesion.Id,
        //           sesion.Sistema,
        //           sesion.Estatus,
        //           sesion.HoraInicial,
        //           sesion.HoraFinal,
        //           sesion.DireccionIP,
        //           sesion.Indice,
        //           sesion.Computadora,
        //           sesion.Usuario,
        //           sesion.Empresa)
        //{
        //    this.Privilegio = NivelPrivilegio.SinPermiso;
        //    this.Clave = 0;
        //    this.Nombre = string.Empty;
        //    this.NoCliente = string.Empty;
        //    this.Distribuidor = string.Empty;
        //    this.Version = AdministrarClientes.VERSION;
        //    this.FechaHoraCliente = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        //    this.Aplicaciones = new ListaAplicaciones();
        //}

        [DataMember]
        public NivelPrivilegio Privilegio { get; set; }

        [DataMember]
        public int IdDistribuidor { get; set; }

        [DataMember]
        public string Distribuidor { get; set; }

        [DataMember]
        public string Nombre { get; set; }
        //{
        //    get { return this.Usuario == null ? string.Empty : this.Usuario.Nombre; }
        //    set { if (this.Usuario != null) { this.Usuario.Nombre = value; } }
        //}

        [DataMember]
        public int Clave { get; set; }
        //{
        //    get { return this.Usuario == null ? 0 : this.Usuario.Clave; }
        //    set { if (this.Usuario != null) { this.Usuario.Clave = value; } }
        //}

        [DataMember]
        public string Sistema { get; set; }

        [DataMember]
        public string NoCliente { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string DireccionIP { get; set; }

        [DataMember]
        public UsuarioModuloWeb Usuario { get; set; }

        [DataMember]
        public DateTime FechaHoraCliente { get; set; }

        [DataMember]
        public DateTime HoraInicial { get; set; }

        [DataMember]
        public DateTime HoraFinal { get; set; }

        [DataMember]
        public DatosEmpresa Empresa { get; set; }

        [DataMember]
        public ListaAplicaciones Aplicaciones { get; set; }

        [DataMember]
        public Estacion EstacionActual { get; set; }

        [DataMember]
        public Estacion Estacion { get; set; }

        [DataMember]
        public ListaEstaciones Estaciones { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaSesiones : List<SesionModuloWeb>
    {
        public ListaSesiones()
            : base()
        {

        }
        ~ListaSesiones()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public partial class FiltroSesionModuloWeb //: ImagenSoft.Framework.Entidades.FiltroSesion
    {
        public FiltroSesionModuloWeb()
        //: base()
        {

        }
        //public FiltroSesionModuloWeb(ImagenSoft.Framework.Entidades.EnumFiltroSesion f)
        //    : base(f)
        //{

        //}
    }
}
