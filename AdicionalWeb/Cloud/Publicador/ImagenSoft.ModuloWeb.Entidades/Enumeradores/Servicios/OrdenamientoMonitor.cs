using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum OrdenarMonitor
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        DiasAtraso = 1,
        [EnumMember]
        NoEstacion = 2,
        [EnumMember]
        NombreComercial = 3,
    }
}
