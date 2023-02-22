using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class AdministrarUsuarios : //ImagenSoft.Entidades.ClaseBase,
                                       IComparable<AdministrarUsuarios>
    {
        public AdministrarUsuarios()
        {
            this.Nombre = string.Empty;
            this.Puesto = string.Empty;
            this.Email = string.Empty;
            this.Contrasena = string.Empty;
            this.Activo = string.Empty;
            this.Fecha = SqlDateTime.MinValue.Value;
            this.UltimoCambio = SqlDateTime.MinValue.Value;
            this.Permisos = new ListaPermisos();
            this.Distribuidor = string.Empty;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Puesto { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Contrasena { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public DateTime UltimoCambio { get; set; }

        [DataMember]
        public int IdDistribuidor { get; set; }

        [DataMember]
        public string Distribuidor { get; set; }

        [DataMember]
        public ListaPermisos Permisos { get; set; }

        public AdministrarUsuarios Clone()
        {
            return (AdministrarUsuarios)base.MemberwiseClone();
        }

        public int CompareTo(AdministrarUsuarios other)
        {
            if (this.Clave != other.Clave) return 1;
            if (this.Nombre != other.Nombre) return 1;
            if (this.Puesto != other.Puesto) return 1;
            if (this.Email != other.Email) return 1;
            if (this.Contrasena != other.Contrasena) return 1;
            if (this.Activo != other.Activo) return 1;
            if (this.Fecha != other.Fecha) return 1;
            if (this.UltimoCambio != other.UltimoCambio) return 1;
            if (this.IdDistribuidor != other.IdDistribuidor) return 1;
            if (this.Distribuidor != other.Distribuidor) return 1;

            return 0;
        }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaAdministrarUsuarios : List<AdministrarUsuarios>
    {
        public ListaAdministrarUsuarios ObtenerPorActivo(string activo)
        {
            if (string.IsNullOrEmpty(activo)) { return this; }
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();
            resultado.AddRange(this.Where(p => p.Activo.Equals(activo, StringComparison.CurrentCultureIgnoreCase)));

            return resultado;
        }

        public ListaAdministrarUsuarios ObtenerPorNombre(string aBuscar)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();
            resultado.AddRange(this.Where(p => p.Nombre.ToLower().Contains(aBuscar.ToLower())));

            return resultado;
        }

        public ListaAdministrarUsuarios ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaAdministrarUsuarios result = new ListaAdministrarUsuarios();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaAdministrarUsuarios()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroAdministrarUsuarios
    {
        public FiltroAdministrarUsuarios()
        {
            this.Nombre = string.Empty;
            this.Activo = string.Empty;
            this.Fecha = SqlDateTime.MinValue.Value;
            this.UltimoCambio = SqlDateTime.MinValue.Value;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public DateTime UltimoCambio { get; set; }

        [DataMember]
        public int IdDistribuidor { get; set; }
    }

    //[Serializable]
    //[DataContract]
    //public class PermisosAdministrarUsuarios
    //{
    //    public PermisosAdministrarUsuarios()
    //    {
    //        this.Key = string.Empty;
    //        this.Value = string.Empty;
    //    }

    //    [DataMember]
    //    [Conversion(Title = "Llave", Indice = 0, Visible = true, DataTableConversion = true)]
    //    public string Key { get; set; }

    //    [DataMember]
    //    [Conversion(Title = "Valor", Indice = 1, Visible = true, DataTableConversion = true)]
    //    public string Value { get; set; }
    //}

    //[Serializable]
    //[CollectionDataContract]
    //public class ListaPermisosAdministrarUsuarios : List<PermisosAdministrarUsuarios>
    //{
    //    ~ListaPermisosAdministrarUsuarios()
    //    {
    //        this.Clear();
    //    }

    //    public void FromXML(string xml, bool clear)
    //    {
    //        if (clear) { this.Clear(); }

    //        FromXML(xml);
    //    }
    //    public void FromXML(string xml)
    //    {
    //        XDocument doc = XDocument.Parse(xml);

    //        var xmlPermisos = doc.Document.Root.Element("Permisos");

    //        if (xmlPermisos == null) { return; }

    //        PermisosAdministrarUsuarios entidad = null;

    //        foreach (var enlaces in xmlPermisos.Elements())
    //        {
    //            entidad = new PermisosAdministrarUsuarios();
    //            foreach (var atributos in enlaces.Attributes())
    //            {
    //                switch (atributos.Name.ToString().ToLower())
    //                {
    //                    case "key":
    //                        entidad.Key = atributos.Value;
    //                        break;
    //                    case "value":
    //                        entidad.Value = atributos.Value;
    //                        break;
    //                }
    //            }

    //            this.Add(entidad);
    //        }
    //    }

    //    public XDocument ToXML()
    //    {
    //        var xmlPermisos = new XElement("Permisos");

    //        foreach (var item in this)
    //        {
    //            xmlPermisos.Add(new XElement("Permiso",
    //                               new XAttribute("Key", item.Key),
    //                               new XAttribute("Value", item.Value)));
    //        }

    //        XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
    //                                        new XElement("root", xmlPermisos)); ;
    //        return doc;
    //    }
    //}

    //[Serializable]
    //[DataContract]
    //public class FiltroPermisosAdministrarUsuarios
    //{
    //    public FiltroPermisosAdministrarUsuarios()
    //    {
    //        this.Key = string.Empty;
    //    }

    //    [DataMember]
    //    public string Key { get; set; }
    //}
}
