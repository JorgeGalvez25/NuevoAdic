using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class AdministrarClientes : //ImagenSoft.Entidades.ClaseBase,
                                       IComparable<AdministrarClientes>
    {
        public const string VERSION = "4.1";

        public AdministrarClientes()
        {
            this.Activo = "No";
            this.Conexion = "No";
            this.Contacto = string.Empty;
            this.EMail = string.Empty;
            this.Host = string.Empty;
            this.FechaAlta = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaUltimaConexion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.NoEstacion = string.Empty;
            this.NombreComercial = string.Empty;
            this.Telefono = string.Empty;
            this.Zona = ZonasCambioPrecio.None;
            this.Enlaces = new ListaEnlacesAdministrarClientes();
            this.MonitorearCambioPrecio = string.Empty;
            this.MonitorearTransmisiones = string.Empty;
            this.Version = string.Empty;
            this.Matriz = string.Empty;
            this.AuxiliarGrupo = string.Empty;
            this.NombreGrupo = string.Empty;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string NombreComercial { get; set; }

        [DataMember]
        public string EMail { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Contacto { get; set; }

        [DataMember]
        public DateTime FechaAlta { get; set; }

        [DataMember]
        public DateTime FechaUltimaConexion { get; set; }

        [DataMember]
        public int DiasAtraso { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public string Conexion { get; set; }

        [DataMember]
        public ListaEnlacesAdministrarClientes Enlaces { get; set; }

        [DataMember]
        public int HorasCorte { get; set; }

        [DataMember]
        public int IdDistribuidor { get; set; }

        [DataMember]
        public string Distribuidor { get; set; }

        [DataMember]
        public int Desface { get; set; }

        [DataMember]
        public string MonitorearCambioPrecio { get; set; }

        [DataMember]
        public string MonitorearTransmisiones { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public bool? Membrecia { get; set; }

        [DataMember]
        public bool EsMatriz { get { return this.NoEstacion.Equals(this.Matriz, StringComparison.OrdinalIgnoreCase); } }

        [DataMember]
        public DateTime FechaMembrecia { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string AuxiliarGrupo { get; set; }

        [DataMember]
        public string NombreGrupo { get; set; }

        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public int Puerto { get; set; }

        #region IComparable<AdministrarClientes> Members

        public int CompareTo(AdministrarClientes other)
        {
            if (this.Activo != other.Activo) { return 1; }
            if (this.Contacto != other.Contacto) { return 1; }
            if (this.EMail != other.EMail) { return 1; }
            if (this.FechaAlta != other.FechaAlta) { return 1; }
            if (this.FechaUltimaConexion != other.FechaUltimaConexion) { return 1; }
            if (this.NoEstacion != other.NoEstacion) { return 1; }
            if (this.NombreComercial != other.NombreComercial) { return 1; }
            if (this.Telefono != other.Telefono) { return 1; }
            if (this.Clave != other.Clave) { return 1; }
            if (this.DiasAtraso != other.DiasAtraso) { return 1; }
            if (this.MonitorearTransmisiones != other.MonitorearTransmisiones) { return 1; }
            if (this.MonitorearCambioPrecio != other.MonitorearCambioPrecio) { return 1; }
            if (this.Enlaces != other.Enlaces) { return 1; }

            if (this.Membrecia != other.Membrecia) { return 1; }
            if (this.FechaMembrecia != other.FechaMembrecia) { return 1; }
            if (this.Matriz != other.Matriz) { return 1; }
            if (this.AuxiliarGrupo != other.AuxiliarGrupo) { return 1; }
            if (this.NombreGrupo != other.NombreGrupo) { return 1; }

            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaAdministrarClientes : List<AdministrarClientes>
    {
        public ListaAdministrarClientes ObtenerPorActivo(string activo)
        {
            if (string.IsNullOrEmpty(activo)) { return this; }
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            resultado.AddRange(this.Where(p => p.Activo.Equals(activo, StringComparison.CurrentCultureIgnoreCase)));

            return resultado;
        }

        public ListaAdministrarClientes ObtenerPorZona(ZonasCambioPrecio zona)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            switch (zona)
            {
                case ZonasCambioPrecio.ZonaFronteriza:
                case ZonasCambioPrecio.ZonaNormal:
                    resultado.AddRange(this.Where(p => p.Zona == zona));
                    return resultado;
            }

            return this;
        }

        public ListaAdministrarClientes OrdenarPor(OrdenarCliente orden)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            switch (orden)
            {
                case OrdenarCliente.NoCliente:
                    resultado.AddRange(this.OrderBy(p => p.NoEstacion));
                    break;
                case OrdenarCliente.NombreComercial:
                    resultado.AddRange(this.OrderBy(p => p.NombreComercial));
                    break;
                case OrdenarCliente.FechaAlta:
                    resultado.AddRange(this.OrderBy(p => p.FechaAlta));
                    break;
                case OrdenarCliente.FechaUltimaConexion:
                    resultado.AddRange(this.OrderBy(p => p.FechaUltimaConexion));
                    break;
                case OrdenarCliente.Activo:
                    resultado.AddRange(this.OrderBy(p => p.Activo).ThenBy(p => p.NombreComercial));
                    break;
                case OrdenarCliente.None:
                default:
                    break;
            }

            return resultado;
        }

        public ListaAdministrarClientes ObtenerPor(FiltrarCliente por, string aBuscar)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();

            switch (por)
            {
                case FiltrarCliente.NombreComercial:
                    resultado.AddRange(this.Where(p => p.NombreComercial.ToLower().Contains(aBuscar.ToLower())));
                    break;
                case FiltrarCliente.RazonSocial:
                case FiltrarCliente.None:
                default:
                    return this;
            }

            return resultado;
        }

        public ListaAdministrarClientes ObtenerEntreFechas(DateTime inicio, DateTime fin)
        {
            //var fechaFin = new DateTime(fin.Year, fin.Month, fin.Day, 23, 59, 59);
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            resultado.AddRange(this.Where(p => p.FechaAlta.Date.Between(inicio.Date, fin.Date)));

            return resultado;
        }

        public ListaAdministrarClientes ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaAdministrarClientes result = new ListaAdministrarClientes();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaAdministrarClientes()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroAdministrarClientes
    {
        public FiltroAdministrarClientes()
        {
            this.Activo = string.Empty;
            this.Conexion = string.Empty;
            this.FechaFinAlta = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaFinUltimaConexion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaInicioAlta = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaInicioUltimaConexion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaHoraCliente = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaUltimaConexion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaMembrecia = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.NoEstacion = string.Empty;
            this.NombreComercial = string.Empty;
            this.RFC = string.Empty;
            this.Zona = ZonasCambioPrecio.None;
            this.Matriz = string.Empty;
            this.Version = string.Empty;
        }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string RFC { get; set; }

        [DataMember]
        public string NombreComercial { get; set; }

        [DataMember]
        public DateTime FechaHoraCliente { get; set; }

        [DataMember]
        public DateTime FechaInicioAlta
        {
            get { return _fechaInicioAlta.Date; }
            set { _fechaInicioAlta = value; }
        }

        [DataMember]
        public DateTime FechaFinAlta
        {
            get { return this.FechaFin(_fechaFinAlta); }
            set { _fechaFinAlta = value; }
        }

        [DataMember]
        public DateTime FechaInicioUltimaConexion
        {
            get { return _fechaInicioUltimaConexion.Date; }
            set { _fechaInicioUltimaConexion = value; }
        }

        [DataMember]
        public DateTime FechaFinUltimaConexion
        {
            get { return this.FechaFin(_fechaFinUltimaConexion); }
            set { _fechaFinUltimaConexion = value; }
        }

        [DataMember]
        public DateTime FechaUltimaConexion { get; set; }

        [DataMember]
        public int Distribuidor { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public bool? Membrecia { get; set; }

        [DataMember]
        public DateTime FechaMembrecia { get; set; }

        [DataMember]
        public string Conexion { get; set; }

        [DataMember]
        private DateTime _fechaInicioAlta;

        [DataMember]
        private DateTime _fechaFinAlta;

        [DataMember]
        private DateTime _fechaInicioUltimaConexion;

        [DataMember]
        private DateTime _fechaFinUltimaConexion;

        private DateTime FechaFin(DateTime date)
        {
            if (date <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
            {
                DateTime d = DateTime.Now;
                return new DateTime(d.Year, d.Month, d.Day, 23, 59, 59, 999);
            }
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }
    }

    [Serializable]
    [DataContract]
    public class EnlacesAdministrarClientes
    {
        public EnlacesAdministrarClientes()
        {
            this.ContrasenaCuenta = string.Empty;
            this.ContrasenaPC = string.Empty;
            this.Cuenta = string.Empty;
            this.PC = string.Empty;
            this.Servicio = string.Empty;
            this.UsuarioCuenta = string.Empty;
            this.UsuarioPC = string.Empty;
        }

        [DataMember]
        public string Servicio { get; set; }

        [DataMember]
        public string Cuenta { get; set; }

        [DataMember]
        public string UsuarioCuenta { get; set; }

        [DataMember]
        public string ContrasenaCuenta { get; set; }

        [DataMember]
        public string PC { get; set; }

        [DataMember]
        public string UsuarioPC { get; set; }

        [DataMember]
        public string ContrasenaPC { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaEnlacesAdministrarClientes : List<EnlacesAdministrarClientes>
    {
        ~ListaEnlacesAdministrarClientes()
        {
            this.Clear();
        }

        public void FromXML(string xml, bool clear)
        {
            if (clear) { this.Clear(); }

            FromXML(xml);
        }
        public void FromXML(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            XElement xmlEnlaces = doc.Document.Root.Element("Enlaces");

            EnlacesAdministrarClientes entidad = null;

            foreach (XElement enlaces in xmlEnlaces.Elements())
            {
                entidad = new EnlacesAdministrarClientes();
                foreach (XAttribute atributos in enlaces.Attributes())
                {
                    switch (atributos.Name.ToString().ToLower())
                    {
                        case "servicio":
                            entidad.Servicio = atributos.Value;
                            break;
                        case "cuenta":
                            entidad.Cuenta = atributos.Value;
                            break;
                        case "usuariocuenta":
                            entidad.UsuarioCuenta = atributos.Value;
                            break;
                        case "contrasenacuenta":
                            entidad.ContrasenaCuenta = atributos.Value;
                            break;
                        case "pc":
                            entidad.PC = atributos.Value;
                            break;
                        case "usuariopc":
                            entidad.UsuarioPC = atributos.Value;
                            break;
                        case "contrasenapc":
                            entidad.ContrasenaPC = atributos.Value;
                            break;
                    }
                }

                this.Add(entidad);
            }
        }

        public XDocument ToXML()
        {
            XElement xmlEnlaces = new XElement("Enlaces");

            foreach (EnlacesAdministrarClientes item in this)
            {
                xmlEnlaces.Add(new XElement("Enlace",
                                   new XAttribute("Servicio", item.Servicio),
                                   new XAttribute("Cuenta", item.Cuenta),
                                   new XAttribute("UsuarioCuenta", item.UsuarioCuenta),
                                   new XAttribute("ContrasenaCuenta", item.ContrasenaCuenta),
                                   new XAttribute("PC", item.PC),
                                   new XAttribute("UsuarioPC", item.UsuarioPC),
                                   new XAttribute("ContrasenaPC", item.ContrasenaPC)));
            }

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                            new XElement("root", xmlEnlaces)); ;
            return doc;
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroEnlacesAdministrarClientes
    {
        public FiltroEnlacesAdministrarClientes()
        {
            this.Cuenta = string.Empty;
            this.PC = string.Empty;
            this.Servicio = string.Empty;
        }

        [DataMember]
        public string Servicio { get; set; }

        [DataMember]
        public string Cuenta { get; set; }

        [DataMember]
        public string PC { get; set; }
    }
}
