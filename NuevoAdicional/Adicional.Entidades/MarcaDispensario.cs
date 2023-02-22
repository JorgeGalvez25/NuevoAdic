using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [DataContract]
    [Serializable]
    public enum MarcaDispensario
    {
        /*Leer  de la tabla DPVGEST, campo TipoDispensario los siguientes valores:
            1-Wayne
            2-Bennett
            3-Team
            4-Gilbarco (PAM)
            5-Hong Jang
        */
        [EnumMember]
        Ninguno = 0,
        [EnumMember]
        Wayne,
        [EnumMember]
        Bennett,
        [EnumMember]
        Team,
        [EnumMember]
        Gilbarco,
        [EnumMember]
        HongYang,
    }
}
