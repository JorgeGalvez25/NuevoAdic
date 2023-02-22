using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum FiltrarCliente
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        NombreComercial = 1,

        [EnumMember]
        RazonSocial = 2,

        [EnumMember]
        RFC = 3
    }

    [Serializable]
    [DataContract]
    public enum FiltrarClienteActivo
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        ActivoSi = 1,

        [EnumMember]
        ActivoNo = 2
    }
}
