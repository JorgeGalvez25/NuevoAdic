using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [DataContract]
    [Serializable]
    public enum EstatusPresetWayne
    {
        [EnumMember]
        Inactivo=0,
        [EnumMember]
        EsperandoEstandar,
        [EnumMember]
        EsperandoMinimo,
    }
}
