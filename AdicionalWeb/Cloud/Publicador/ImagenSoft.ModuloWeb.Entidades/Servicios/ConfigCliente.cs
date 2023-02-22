using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Servicios
{
    [Serializable]
    [DataContract]
    public class ConfigCliente
    {
        public ConfigCliente(ZonasCambioPrecio zona, int dias, int horasCorte)
        {
            this.Zona = zona;
            this.Dias = dias;
            this.HorasCorte = horasCorte;
        }

        [DataMember]
        public ZonasCambioPrecio Zona { get; private set; }

        [DataMember]
        public int Dias { get; private set; }

        [DataMember]
        public int HorasCorte { get; private set; }
    }
}
