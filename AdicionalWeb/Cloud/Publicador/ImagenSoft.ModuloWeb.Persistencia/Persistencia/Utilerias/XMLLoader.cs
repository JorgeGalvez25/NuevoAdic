using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Xml;

namespace ImagenSoft.ModuloWeb.Persistencia.Utilidades
{
    public class XMLLoader
    {
        private string XmlPath
        {
            get
            {
                string value = ConfigurationManager.AppSettings["ConsultasXML"];

                if (string.IsNullOrWhiteSpace(value))
                {
                    return @"C:\ImagenCo\Dbi\Sentencias\SqlServer";
                }

                return value;
            }
        }
        private static Dictionary<string, CacheItems> cache = new Dictionary<string, CacheItems>();

        public string GetOperation(TipoOperacion operacion, string tableName)
        {
            string key = tableName + operacion.ToString();

            if (cache.ContainsKey(key))
            {
                if (cache[key].Expire > DateTime.Now)
                {
                    return cache[key].Value;
                }
            }

            XmlDocument doc = new XmlDocument();
            {
                using (BufferedStream buff = new BufferedStream(File.OpenRead(Path.Combine(XmlPath, string.Concat(tableName, ".xml")))))
                {
                    doc.Load(XmlReader.Create(buff));
                }
            }
            XmlNode stats = FindInXML(doc, operacion);

            if (stats != null)
            {
                CacheItems value = new CacheItems();
                {
                    value.Value = ReadXmlElement(stats);
                    value.Operacion = operacion;
                    value.Expire = DateTime.Now.AddHours(12D);
                }
                if (cache.ContainsKey(key))
                {
                    cache.Remove(key);
                }
                cache.Add(key, value);
                return value.Value;
            }

            return string.Empty;
        }

        private XmlNode FindInXML(XmlDocument doc, TipoOperacion operacion)
        {
            XmlNodeList list = doc.SelectNodes("/Sentencias/Sentencia");
            string op = ((int)operacion).ToString("D3");

            string value = string.Empty;
            return list.Cast<XmlNode>().AsParallel()
                       .Where(p => p.Attributes["Id"].Value.Equals(op, StringComparison.OrdinalIgnoreCase))
                       .FirstOrDefault();
        }

        private string ReadXmlElement(XmlNode element)
        {
            return element.Cast<XmlElement>()
                          .Select(p => p.InnerText)
                          .Aggregate((x, y) => x.Trim() + "\r\n" + y.Trim());
        }
    }

    public enum TipoOperacion
    {
        None = 0,
        Consecutivo = 1,
        Insertar = 2,
        Modificar = 3,
        Eliminar = 4,
        Obtener = 5,
        ObtenerTodos = 6,
        ObtenerTodosPaginacion = 7,
        ObtenerConsecutivoIntermedio = 8,
        Especial_1 = 9,
        Especial_2 = 10,
        Especial_3 = 11,
        Especial_4 = 12
    }

    public class CacheItems
    {
        public CacheItems()
        {
            this.Expire = SqlDateTime.MinValue.Value;
            this.Operacion = TipoOperacion.None;
            this.Value = string.Empty;
        }

        public DateTime Expire { get; set; }

        public string Value { get; set; }

        public TipoOperacion Operacion { get; set; }
    }
}
