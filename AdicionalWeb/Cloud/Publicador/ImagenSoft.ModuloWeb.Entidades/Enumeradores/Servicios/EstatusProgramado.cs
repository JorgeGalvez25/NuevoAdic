using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum EstatusProgramado
    {
        [EnumMember]
        Todos = 0,
        [EnumMember]
        Si = 1,
        [EnumMember]
        No = 2
    }
}
