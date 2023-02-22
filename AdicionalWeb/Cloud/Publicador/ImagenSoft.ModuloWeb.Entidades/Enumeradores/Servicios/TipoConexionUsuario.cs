using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum TipoConexionUsuario
    {
        [EnumMember]
        Monitor = 1,

        [EnumMember]
        ZonaNormal = 2,

        [EnumMember]
        ZonaFronteriza = 3,

        [EnumMember]
        UsuarioWeb = 4
    }
}
