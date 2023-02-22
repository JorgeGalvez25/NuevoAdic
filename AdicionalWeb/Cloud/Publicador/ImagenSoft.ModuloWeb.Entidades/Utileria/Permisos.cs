using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Xml;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class Permisos
    {
        public Permisos()
        {
            this.Id = string.Empty;
            this.Nombre = string.Empty;
            this.Permitido = false;
            this.SubPermisos = new ListaPermisos();
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public bool Permitido { get; set; }

        [DataMember]
        public ListaPermisos SubPermisos { get; set; }

        public XElement ToXMLItem()
        {
            XElement item = new XElement("Permiso",
                                new XAttribute("Id", this.Id),
                                new XAttribute("Nombre", this.Nombre),
                                new XAttribute("Permitido", this.Permitido));
            if (this.SubPermisos.Count > 0)
            {
                item.Add(this.SubPermisos.ToXMLItems());
            }
            return item;
        }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaPermisos : List<Permisos>
    {
        ~ListaPermisos()
        {
            this.Clear();
        }

        private Object GetObjectType(Type type, string val)
        {
            switch (type.Name)
            {
                case "Boolean": return Boolean.Parse(val);
                case "Float": return float.Parse(val);
                case "Double": return Double.Parse(val);
                case "Decimal": return Decimal.Parse(val);
                case "Integer": return Int32.Parse(val);
                case "DateTime": return DateTime.Parse(val);
            }
            return val;
        }

        private ListaPermisos Ciclo(IEnumerable<XElement> value)
        {
            ListaPermisos resultado = new ListaPermisos();
            Permisos ps = null;
            PropertyInfo[] props = typeof(Permisos).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<XAttribute> attr = null;
            ListaPermisos it = null;
            foreach (XElement item in value)
            {
                ps = new Permisos();
                attr = item.Attributes();

                foreach (XAttribute iAttr in attr)
                {
                    foreach (PropertyInfo iProps in props)
                    {
                        if (iProps != null && iProps.Name.Equals(iAttr.Name.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            iProps.SetValue(ps, this.GetObjectType(iProps.PropertyType, iAttr.Value), null);
                            break;
                        }
                    }
                }
                if (item.HasElements)
                {
                    it = Ciclo(item.Elements());
                    foreach (PropertyInfo iProps in props)
                    {
                        if (iProps != null && iProps.PropertyType.Name.Equals("ListaPermisos", StringComparison.CurrentCultureIgnoreCase))
                        {
                            iProps.SetValue(ps, it, null);
                            break;
                        }
                    }
                }
                resultado.Add(ps);
            }
            return resultado;
        }

        public void FromXML(string xml, bool clear)
        {
            if (clear) { this.Clear(); }
            XDocument doc = XDocument.Parse(xml);
            IEnumerable<XElement> y = doc.Element("root")
                                         .Element("Permisos")
                                         .Elements();
            this.AddRange(this.Ciclo(y));
        }
        public void FromXML(string xml)
        {
            FromXML(xml, true);
        }

        public XDocument ToXML()
        {
            XElement xmlPermisos = new XElement("Permisos", this.ToXMLItems());

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                            new XElement("root", new XElement("Permisos", this.ToXMLItems())));
            return doc;
        }
        public XElement[] ToXMLItems()
        {
            if (this.Count <= 0)
            {
                return new XElement[] { new XElement("Permiso") };
            }

            return this.Select(p => p.ToXMLItem()).ToArray();
        }

        public bool BuscarPermiso(string toFind, bool submodulos)
        {
            foreach (Permisos item in this)
            {
                if (item.Id.Equals(toFind, StringComparison.CurrentCultureIgnoreCase))
                {
                    return item.Permitido;
                }
                else if (item.SubPermisos.Count > 0 && submodulos)
                {
                    if (item.SubPermisos.BuscarPermiso(toFind, submodulos))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Permisos BuscarEntidadPermiso(string toFind, bool submodulos)
        {
            foreach (Permisos item in this)
            {
                if (item.Id.Equals(toFind, StringComparison.CurrentCultureIgnoreCase))
                {
                    return item;
                }
                else if (item.SubPermisos.Count > 0 && submodulos)
                {
                    return item.SubPermisos.BuscarEntidadPermiso(toFind, submodulos);
                }
            }

            return null;
        }
    }
    [Serializable]
    [DataContract]
    public class FiltroPermisos
    {
        public FiltroPermisos()
        {
            this.Id = string.Empty;
            this.Nombre = string.Empty;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }
    }

    public class ConstantesPermisos
    {
        public class Grupos
        {
            public const string MODULO_CATALOGOS = "MostrarCatalogos";
            public const string MODULO_DOCUMENTOS = "MostrarDocumentos";
            public const string MODULO_CONSULTAS = "MostrarConsultas";
            public const string MODULO_DISTRIBUIDORES = "MostrarDistribuidores";
            public const string MODULO_REPORTES = "MostrarReportes";
            public const string MODULO_PROCESOS = "MostrarProcesos";
            public const string MODULO_CONFIGURACIONES = "MostrarConfiguraciones";
        }

        public class Modulos
        {
            public const string CLIENTES = "AdministrarClientes";
            public const string USUARIOS = "AdministrarUsuarios";
            public const string DISTRIBUIDORES = "AdministrarDistribuidores";
            public const string CAMBIO_PRECIOS = "ProgramacionCambioPrecio";
            public const string MONITOR_TRANSMISIONES = "MonitorTransmisiones";
            public const string MONITOR_CONEXIONES = "MonitorConexiones";
            public const string MONITOR_CAMBIO_PRECIOS = "MonitorCambioPrecio";
        }

        public class Operaciones
        {
            public const string OPERACION_MOSTRAR = "Mostrar";
            public const string OPERACION_REGISTRAR = "Registrar";
            public const string OPERACION_MODIFICAR = "Modificar";
            public const string OPERACION_ELIMINAR = "Eliminar";
        }

        public class Opciones
        {
            public const string OPCION_ACTIVAR = "Activar";
            public const string OPCION_DESFACE_MODIFICAR = "DesfaceModificar";
            public const string OPCION_DESFACE_REGISTRAR = "DesfaceRegistrar";
            public const string OPCION_CAMBIAR_CONTRASEÑA = "CambiarContraseña";
        }
    }
}
