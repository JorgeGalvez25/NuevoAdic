using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Enumeradores
{
    [Serializable]
    [DataContract]
    public enum Aplicado
    {
        [EnumMember]
        Todos = 100,
        [EnumMember]
        No = 0,
        [EnumMember]
        Si = 1,
        [EnumMember]
        Pendiente = 2,
    }
}
