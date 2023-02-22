using ImagenSoft.ModuloWeb.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Persistencia.UtileriasPersistencia
{
    public class ServicioSerializador
    {
        private static IDictionary<Type, XmlSerializer> _serializerCache = new Dictionary<Type, XmlSerializer>();

        public static XmlReaderSettings XmlReaderset = new XmlReaderSettings()
            {
                ValidationType = ValidationType.Schema
            };
        public static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings()
            {
                Async = true,
                Encoding = new UTF8Encoding(false),
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };

        #region Utilerias

        public async Task<byte[]> Serializar<T>(T obj)
        {
            try
            {
                using (MemoryStream msDecompressed = new MemoryStream())
                {
                    using (BufferedStream buffDecompressed = new BufferedStream(msDecompressed))
                    {
                        new BinaryFormatter().Serialize(buffDecompressed, obj);
                        byte[] byteArray = msDecompressed.ToArray();

                        using (MemoryStream msCompressed = new MemoryStream())
                        {
                            using (GZipStream gZipStream = new GZipStream(msCompressed, CompressionMode.Compress))
                            {
                                using (BufferedStream buffCompressed = new BufferedStream(gZipStream))
                                {
                                    await buffCompressed.WriteAsync(byteArray, 0, byteArray.Length);
                                    buffCompressed.Close();
                                }
                            }

                            return msCompressed.ToArray();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                return new byte[0];
            }
        }

        public T Deserializar<T>(byte[] arrBytes)
        {
            using (GZipStream gZipStream = new GZipStream(new MemoryStream(arrBytes), CompressionMode.Decompress))
            {
                using (BufferedStream buffDecompressed = new BufferedStream(gZipStream))
                {
                    object item = new BinaryFormatter().Deserialize(buffDecompressed);
                    buffDecompressed.Close();
                    return (T)item;
                }
            }
        }

        #endregion
        public byte[] SerializarToXml<T>(T obj)
        {
            try
            {
                Type current = typeof(T);
                using (MemoryStream m = new MemoryStream())
                {
                    using (BufferedStream buff = new BufferedStream(m))
                    {
                        XmlSerializer xml = new XmlSerializer(current);
                        xml.Serialize(buff, obj);

                        buff.FlushAsync().Wait();
                        m.Position = 0L;
                        return m.ToArray();
                    }
                }
            }
            catch (System.Exception e)
            {
                MensajesRegistros.Error("Host Servicios Web - Serializador", e);
                return new byte[0];
            }
        }

        public T DeserializarFromXML<T>(byte[] buffer)
        {
            try
            {
                Type current = typeof(T);
                using (MemoryStream m = new MemoryStream(buffer))
                {
                    using (BufferedStream buff = new BufferedStream(m))
                    {
                        XmlSerializer xml = new XmlSerializer(current);
                        return (T)xml.Deserialize(buff);
                    }
                }
            }
            catch (Exception e)
            {
                MensajesRegistros.Error("Host Servicios Web - Deserializador", e);
                return default(T);
            }
        }
    }
}
