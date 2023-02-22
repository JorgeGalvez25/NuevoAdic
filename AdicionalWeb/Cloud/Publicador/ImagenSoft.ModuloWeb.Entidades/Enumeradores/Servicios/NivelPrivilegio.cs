using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public enum NivelPrivilegio
    {
        [EnumMember]
        SinPermiso = -1,
        [EnumMember]
        Administrador = 0,
        [EnumMember]
        Monitor = 1,
        [EnumMember]
        Precios = 2,
        [EnumMember]
        MonitorPrecios = 3,
        [EnumMember]
        Reportes = 4
    }
}
