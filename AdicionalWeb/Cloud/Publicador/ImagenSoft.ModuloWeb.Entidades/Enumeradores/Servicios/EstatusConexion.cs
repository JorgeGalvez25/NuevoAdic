using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum EstatusConexion
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        EnLinea = 1,
        [EnumMember]
        FueraDeLinea = 2
    }
}
