using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class Tanques
    {
        public Tanques()
        {
            this.Combustible = 0;
            this.Corte = 0;
            this.DatosAdicionales = string.Empty;
            this.EntradasRelacionadas = string.Empty;
            this.Fecha = DateTime.MinValue;
            this.FechaDoc = DateTime.MinValue;
            this.FechaHora = DateTime.MinValue;
            this.FechaHoraDisp = string.Empty;
            this.FechaHoraFinal = DateTime.MinValue;
            this.FechaHoraInicial = DateTime.MinValue;
            this.FechaTurno = DateTime.MinValue;
            this.Folio = 0;
            this.FolioDoc = string.Empty;
            this.Generado = string.Empty;
            this.RelacionMaestro = 0;
            this.Tanque = 0;
            this.Tanque_Origen = 0;
            this.Temperatura = 0D;
            this.TerminalDist = string.Empty;
            this.TipoDoc = string.Empty;
            this.Traspaso = string.Empty;
            this.Turno = 0;
            this.Ventas = 0D;
            this.VolumenDoc = 0D;
            this.VolumenFinal = 0D;
            this.VolumenInicial = 0D;
            this.VolumenRecepcion = 0D;
        }

        [DataMember]
        public int Folio { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int Corte { get; set; }
        [DataMember]
        public int Tanque { get; set; }
        [DataMember]
        public int Combustible { get; set; }
        [DataMember]
        public double VolumenInicial { get; set; }
        [DataMember]
        public double VolumenFinal { get; set; }
        [DataMember]
        public double VolumenRecepcion { get; set; }
        [DataMember]
        public double Temperatura { get; set; }
        [DataMember]
        public string TerminalDist { get; set; }
        [DataMember]
        public string TipoDoc { get; set; }
        [DataMember]
        public DateTime FechaDoc { get; set; }
        [DataMember]
        public string FolioDoc { get; set; }
        [DataMember]
        public double VolumenDoc { get; set; }
        [DataMember]
        public DateTime FechaHoraInicial { get; set; }
        [DataMember]
        public DateTime FechaHoraFinal { get; set; }
        [DataMember]
        public string DatosAdicionales { get; set; }
        [DataMember]
        public string FechaHoraDisp { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public DateTime FechaTurno { get; set; }
        [DataMember]
        public int Turno { get; set; }
        [DataMember]
        public string Traspaso { get; set; }
        [DataMember]
        public int Tanque_Origen { get; set; }
        [DataMember]
        public double Ventas { get; set; }
        [DataMember]
        public string Generado { get; set; }
        [DataMember]
        public string EntradasRelacionadas { get; set; }
        [DataMember]
        public int RelacionMaestro { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaTanques : List<Tanques>
    {
        ~ListaTanques()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroTanques
    {
        public FiltroTanques()
        {
            this.Folio = 0;
            this.Fecha = DateTime.MinValue;
        }

        [DataMember]
        public int Folio { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }
    }
}
