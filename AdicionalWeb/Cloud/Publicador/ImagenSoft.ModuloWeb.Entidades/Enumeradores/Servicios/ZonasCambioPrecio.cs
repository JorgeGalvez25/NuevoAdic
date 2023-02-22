using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum ZonasCambioPrecio
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        ZonaFronteriza = 1,
        [EnumMember]
        ZonaNormal = 2
    }
}
