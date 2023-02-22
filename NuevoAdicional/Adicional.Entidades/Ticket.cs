using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class Ticket
    {
        public Ticket()
        {
            this.Facturado = string.Empty;
            this.Folio = 0;
            this.Importe = 0D;
            this.Precio = 0D;
            this.Volumen = 0D;
            this.Fecha = DateTime.MinValue;

            this.Turno = 0;
            this.FechaHoraTurno = DateTime.MinValue;
            this.Total01 = 0D;
            this.Total02 = 0D;

            this.Posicion = 0;
            this.Combustible = 0;
            this.Manguera = 0;

            this.NoAplicar = false;
        }

        [DataMember]
        public int Folio { get; set; }

        [DataMember]
        public double Precio { get; set; }

        [DataMember]
        public double Volumen { get; set; }

        [DataMember]
        public double Importe { get; set; }

        [DataMember]
        public string Facturado { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int Turno { get; set; }

        [DataMember]
        public DateTime FechaHoraTurno { get; set; }

        [DataMember]
        public double Total01 { get; set; }

        [DataMember]
        public double Total02 { get; set; }

        [DataMember]
        public int Posicion { get; set; }

        [DataMember]
        public int Combustible { get; set; }

        [DataMember]
        public int Manguera { get; set; }

        [DataMember]
        public bool NoAplicar { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaTickets : List<Ticket>
    {
        ~ListaTickets()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroTicket
    {
        public FiltroTicket()
        {
            this.Facturado = string.Empty;
            this.Folio = 0;
        }

        [DataMember]
        public int Folio { get; set; }

        [DataMember]
        public string Facturado { get; set; }
    }
}
