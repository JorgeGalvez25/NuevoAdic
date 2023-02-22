using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public enum EstatusAplicacionAlertas
    {
        [EnumMember]
        Ok = 0,
        [EnumMember]
        Precaucion = 1,
        [EnumMember]
        Error = 2
    }
}
