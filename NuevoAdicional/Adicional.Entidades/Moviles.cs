using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class Moviles
    {
        public Moviles()
        {
            Telefono = string.Empty;
            Responsable = string.Empty;
            Password = string.Empty;
            Activo = "S";
            PermitirCambioFlujo = "N";
            Permisos = new Permisos(false);
        }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Responsable { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public string PermitirCambioFlujo { get; set; }

        [DataMember]
        public Permisos Permisos { get; set; }
    }
    [Serializable]
    [DataContract]
    public class Permisos
    {
        public Permisos()
            : this(false)
        {

        }
        public Permisos(bool permitir)
        {
            this.SubirBajar = permitir;
        }

        [DataMember]
        public bool SubirBajar { get; set; }

        public string ToXML()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                          new XElement("root",
                                            new XElement("Permisos",
                                                new XElement("Permiso",
                                                    new XAttribute("name", "Subir Bajar"),
                                                    new XAttribute("value", this.SubirBajar ? "Si" : "No")))));
            return doc.ToString(SaveOptions.DisableFormatting);
        }

        public void FromXML(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            foreach (var item in doc.Descendants("Permisos").Descendants())
            {
                var name = item.Attribute("name");

                if (name != null)
                {
                    switch (name.Value.Replace(" ", string.Empty).ToLower())
                    {
                        case "subirbajar":
                            this.SubirBajar = item.Attribute("value").Value.Equals("Si", StringComparison.CurrentCultureIgnoreCase);
                            break;
                    }
                }
            }
        }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMoviles : List<Moviles>
    {
    }

    [Serializable]
    [DataContract]
    public class FiltroMoviles
    {
        public FiltroMoviles()
        {
            this.Responsable = string.Empty;
            this.Telefono = string.Empty;
        }
        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public string Responsable { get; set; }
    }
}
