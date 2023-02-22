using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Adicional.Entidades
{
    public enum ProtocoloSerializacion
    {
        WCF,
        Socket
    }

    public class ContantesAdicional
    {
        public static readonly CultureInfo CulturaLocal = CultureInfo.CreateSpecificCulture("es-MX");
    }

    sealed public class Serializador
    {
        private static readonly object _xmlLock = new object();
        private static readonly object _binLock = new object();

        private static IDictionary<Type, XmlSerializer> _serializerCache = new Dictionary<Type, XmlSerializer>();

        public static byte[] Serializar<L>(L t)
        {
            lock (_binLock)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(ms, t);
                    ms.Flush();
                    return Compresion.Comprimir(ms.ToArray());
                }
            }
        }

        public static L Deserializar<L>(byte[] buffer)
        {
            lock (_binLock)
            {
                byte[] auxDescomp = Compresion.Descomprimir(buffer);
                using (MemoryStream ms2 = new MemoryStream(auxDescomp))
                {
                    BinaryFormatter bin2 = new BinaryFormatter();
                    return (L)bin2.Deserialize(ms2);
                }
            }
        }

        public static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings()
        {
            Encoding = new UTF8Encoding(false),
            Indent = false,
            NewLineHandling = NewLineHandling.None
        };

        public static byte[] Serializar<L>(L t, ProtocoloSerializacion protocolo)
        {
            switch (protocolo)
            {
                case ProtocoloSerializacion.Socket:
                    lock (_xmlLock)
                    {
                        Type current = typeof(L);
                        using (MemoryStream m = new MemoryStream())
                        {
                            XmlSerializer xml = null;
                            if (!_serializerCache.TryGetValue(current, out xml))
                            {
                                xml = new XmlSerializer(current);
                                _serializerCache.Add(current, xml);
                            }
                            using (BufferedStream buff = new BufferedStream(m, short.MaxValue))
                            {
                                using (XmlWriter writer = XmlWriter.Create(buff, XmlWriterSettings))
                                {
                                    xml.Serialize(writer, t);

                                    m.Position = 0L;
                                    return m.ToArray();
                                }
                            }
                        }
                    }
                case ProtocoloSerializacion.WCF:
                default:
                    return Serializar(t);
            }
        }

        public static L Deserializar<L>(byte[] buffer, ProtocoloSerializacion protocolo)
        {
            switch (protocolo)
            {
                case ProtocoloSerializacion.Socket:
                    lock (_xmlLock)
                    {
                        Type current = typeof(L);
                        using (MemoryStream m = new MemoryStream(buffer))
                        {
                            using (BufferedStream buff = new BufferedStream(m))
                            {
                                XmlSerializer xml = null;
                                if (!_serializerCache.TryGetValue(current, out xml))
                                {
                                    xml = new XmlSerializer(current);
                                    _serializerCache.Add(current, xml);
                                }
                                return (L)xml.Deserialize(buff);
                            }
                        }
                    }
                case ProtocoloSerializacion.WCF:
                default:
                    return Deserializar<L>(buffer);
            }

            //return default(L);
        }
    }

    sealed public class Compresion
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Comprime un Array de Bytes a un buffer en memoria
        /// </summary>
        /// <param name="bytes">el arreglo de bytes a comprimir</param>
        /// <returns>El buffer (array de bytes) comprimido</returns>
        public static byte[] Comprimir(byte[] bytes)
        {
            lock (_lock)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress, true))
                    {
                        zip.Write(bytes, 0, bytes.Length);
                    }

                    stream.Position = 0;
                    byte[] compressed = new byte[stream.Length];
                    stream.Read(compressed, 0, compressed.Length);

                    byte[] gzBuffer = new byte[compressed.Length + 4];
                    System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                    System.Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gzBuffer, 0, 4);

                    return gzBuffer;
                }
            }
        }

        /// <summary>
        /// Descomprime un array de bytes (comprimido) en un buffer en memoria
        /// </summary>
        /// <param name="gzBuffer">el array de bytes comprimido</param>
        /// <returns>El buffer (array de bytes) descomprimido</returns>
        public static byte[] Descomprimir(byte[] gzBuffer)
        {
            lock (_lock)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    int length = BitConverter.ToInt32(gzBuffer, 0);
                    byte[] decompressed = new byte[length];

                    ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
                    ms.Position = 0;

                    using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        zip.Read(decompressed, 0, decompressed.Length);
                    }

                    return decompressed;
                }
            }
        }
    }
}
