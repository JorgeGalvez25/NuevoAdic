using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador
{
    [Serializable]
    [DataContract]
    public class FileUpdater
    {
        public FileUpdater()
        {
            this.Data = new byte[0];
            this.FileName = string.Empty;
            this.MD5 = string.Empty;
            this.Delete = false;
            this.IsPath = false;

        }

        [DataMember]
        public string MD5 { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public bool Delete { get; set; }

        [DataMember]
        public bool IsPath { get; set; }

        [DataMember]
        public string Path { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ResponseUpdater
    {
        public ResponseUpdater()
        {
            this.Files = new ListFileUpdater();
            this.HaveUpdate = false;
        }

        [DataMember]
        public bool HaveUpdate { get; set; }

        [DataMember]
        public ListFileUpdater Files { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RequestUpdater
    {
        public RequestUpdater()
        {
            this.Files = new ListFileUpdater();
            this.HaveUpdate = false;
            this.Target = string.Empty;
        }

        [DataMember]
        public string Target { get; set; }

        [DataMember]
        public bool HaveUpdate { get; set; }

        [DataMember]
        public ListFileUpdater Files { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListFileUpdater : List<FileUpdater>
    {
        ~ListFileUpdater()
        {
            this.Clear();
        }
    }
}
