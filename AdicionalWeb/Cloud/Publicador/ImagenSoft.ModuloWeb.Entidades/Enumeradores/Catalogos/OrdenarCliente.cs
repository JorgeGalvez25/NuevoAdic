using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum OrdenarCliente
    {
        [EnumMember]
        None = 100,

        [EnumMember]
        NoCliente = 0,

        [EnumMember]
        NombreComercial = 1,

        [EnumMember]
        Nombre = 2,

        [EnumMember]
        FechaAlta = 3,

        [EnumMember]
        FechaUltimaConexion = 4,

        [EnumMember]
        Activo = 5
    }
}
