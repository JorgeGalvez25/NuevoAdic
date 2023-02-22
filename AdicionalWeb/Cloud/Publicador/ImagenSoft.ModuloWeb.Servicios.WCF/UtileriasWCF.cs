using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Servicios.WCF
{
    public class UtileriasWCF
    {
        private const int MAXBUFFER = Int16.MaxValue;
        private static BinaryFormatter Formatter = new BinaryFormatter();
        public static readonly object Lock_Serializer = new object();

        #region Serializacion GZipBinaria

        public async Task<byte[]> Serializar<T>(T obj)
        {
            using (MemoryStream msCompressed = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(msCompressed, CompressionMode.Compress))
                {
                    using (BufferedStream buffCompressed = new BufferedStream(gZipStream, MAXBUFFER))
                    {
                        using (MemoryStream msDecompressed = new MemoryStream())
                        {
                            using (BufferedStream buffDecompressed = new BufferedStream(msDecompressed, MAXBUFFER))
                            {
                                msDecompressed.Position = 0;
                                Formatter.Serialize(buffDecompressed, obj);
                                msDecompressed.Position = 0;
                                await buffDecompressed.CopyToAsync(buffCompressed, MAXBUFFER).ConfigureAwait(false);
                            }
                        }
                        buffCompressed.Close();
                    }
                }
                return msCompressed.ToArray();
            }
        }

        public T Deserializar<T>(byte[] arrBytes)
        {
            using (MemoryStream mem = new MemoryStream(arrBytes))
            {
                mem.Position = 0;
                using (GZipStream gZipStream = new GZipStream(mem, CompressionMode.Decompress))
                {
                    using (BufferedStream buffDecompressed = new BufferedStream(gZipStream, MAXBUFFER))
                    {
                        object item = Formatter.Deserialize(buffDecompressed);
                        buffDecompressed.Close();
                        return (T)item;
                    }
                }
            }
        }

        #endregion

        #region Serializacion GZip

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Task<byte[]> SerializarGZip<T>(T obj)
        {
            TaskCompletionSource<byte[]> _task = new TaskCompletionSource<byte[]>();
            Task.Run(async () =>
                {
                    try
                    {
                        using (MemoryStream msCompressed = new MemoryStream())
                        {
                            using (GZipStream gZipStream = new GZipStream(msCompressed, CompressionMode.Compress))
                            {
                                using (MemoryStream msDecompressed = new MemoryStream())
                                {
                                    new BinaryFormatter().Serialize(msDecompressed, obj);
                                    byte[] byteArray = msDecompressed.ToArray();

                                    await gZipStream.WriteAsync(byteArray, 0, byteArray.Length);
                                    gZipStream.Close();
                                    _task.TrySetResult(msCompressed.ToArray());
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                    }
                }).ConfigureAwait(false);
            return _task.Task;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Task<T> DeserializarGZip<T>(byte[] arrBytes)
        {
            TaskCompletionSource<T> _task = new TaskCompletionSource<T>(arrBytes);
            Task.Run(() =>
            {
                try
                {
                    using (MemoryStream mem = new MemoryStream(arrBytes))
                    {
                        using (GZipStream gZipStream = new GZipStream(mem, CompressionMode.Decompress, true))
                        {
                            mem.Position = 0L;
                            BinaryFormatter bin2 = new BinaryFormatter();
                            _task.TrySetResult((T)bin2.Deserialize(gZipStream));
                        }
                    }
                }
                catch (Exception e)
                {
                    _task.TrySetException(e);
                }
            }).ConfigureAwait(false);
            return _task.Task;
        }

        #endregion

        #region Serializacion Binaria

        public async Task<byte[]> SerializarBin<T>(T obj)
        {
            using (MemoryStream msBinaryOut = new MemoryStream())
            {
                using (BufferedStream buffBinaryOut = new BufferedStream(msBinaryOut))
                {
                    using (MemoryStream msBinaryIn = new MemoryStream())
                    {
                        using (BufferedStream buffBinaryIn = new BufferedStream(msBinaryIn, MAXBUFFER))
                        {
                            msBinaryIn.Position = 0;
                            Formatter.Serialize(buffBinaryIn, obj);
                            msBinaryIn.Position = 0;
                            await buffBinaryIn.CopyToAsync(buffBinaryOut, MAXBUFFER).ConfigureAwait(false);
                        }
                    }
                    buffBinaryOut.Close();
                }
                return msBinaryOut.ToArray();
            }
        }

        public T DeserializarBin<T>(byte[] arrBytes)
        {
            using (MemoryStream msBinaryIn = new MemoryStream(arrBytes))
            {
                msBinaryIn.Position = 0;
                using (BufferedStream buffBinaryIn = new BufferedStream(msBinaryIn, MAXBUFFER))
                {
                    object item = Formatter.Deserialize(buffBinaryIn);
                    buffBinaryIn.Close();
                    return (T)item;
                }
            }
        }

        #endregion

        #region Serializacion XML

        private static IDictionary<Type, XmlSerializer> _serializerCache = new Dictionary<Type, XmlSerializer>();

        public static XmlReaderSettings XmlReaderset = new XmlReaderSettings()
        {
            ValidationType = ValidationType.Schema
        };
        public static XmlWriterSettings XmlWriterSettings = new XmlWriterSettings()
        {
            Async = true,
            Encoding = new UTF8Encoding(false),
            Indent = false,
            NewLineHandling = NewLineHandling.None
        };

        public byte[] SerializarXML<T>(T obj)
        {
            //return Adicional.Entidades.Serializador.Serializar(obj, Adicional.Entidades.ProtocoloSerializacion.Socket);
            try
            {
                using (MemoryStream m = new MemoryStream())
                {
                    using (BufferedStream buff = new BufferedStream(m))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(T));

                        using (XmlWriter writer = XmlWriter.Create(buff, XmlWriterSettings))
                        {
                            xml.Serialize(writer, obj);

                            writer.FlushAsync().Wait();
                            buff.FlushAsync().Wait();
                            m.Position = 0L;
                            return m.ToArray();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                return new byte[0];
            }
        }

        public T DeserializarXML<T>(byte[] arrBytes)
        {
            using (MemoryStream m = new MemoryStream(arrBytes))
            {
                using (BufferedStream buff = new BufferedStream(m))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(T));
                    return (T)xml.Deserialize(buff);
                }
            }
        }

        #endregion
    }
}
