using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum EstatusTransaccion
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Ok = 1,
        [EnumMember]
        Error = 2,
        [EnumMember]
        Procesando = 3
    }
}
