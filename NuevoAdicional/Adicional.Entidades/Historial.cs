using System.ComponentModel;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Adicional.Entidades
{

    [DataContract]
    [Serializable]
    public class Historial : INotifyPropertyChanged
    {
        private int FId;
        private int FId_Estacion;
        private DateTime FFecha;
        private TimeSpan FHora;
        private int FPosicion;
        private int FManguera;
        private decimal FPorcentaje;
        private string FEstado;
        private short FCombustible;
        private int FCalibracion;
        private decimal FConf;
        private string FAbajo;

        [DataMember]
        public int Id
        {
            get
            {
                return FId;
            }
            set
            {
                FId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        [DataMember]
        public int Id_Estacion
        {
            get
            {
                return FId_Estacion;
            }
            set
            {
                FId_Estacion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id_Estacion"));
            }
        }

        [DataMember]
        public DateTime Fecha
        {
            get
            {
                return FFecha;
            }
            set
            {
                FFecha = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fecha"));
            }
        }

        [DataMember]
        public TimeSpan Hora
        {
            get
            {
                return FHora;
            }
            set
            {
                FHora = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Hora"));
            }
        }

        [DataMember]
        public int Posicion
        {
            get
            {
                return FPosicion;
            }
            set
            {
                FPosicion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Posicion"));
            }
        }

        [DataMember]
        public int Manguera
        {
            get
            {
                return FManguera;
            }
            set
            {
                FManguera = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Manguera"));
            }
        }

        [DataMember]
        public decimal Porcentaje
        {
            get
            {
                return FPorcentaje;
            }
            set
            {
                FPorcentaje = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Porcentaje"));
            }
        }

        [DataMember]
        public string Estado
        {
            get
            {
                return FEstado;
            }
            set
            {
                FEstado = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Estado"));
            }
        }

        [DataMember]
        public short Combustible
        {
            get
            {
                return FCombustible;
            }
            set
            {
                FCombustible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Combustible"));
            }
        }

        [DataMember]
        public int Calibracion
        {
            get
            {
                return FCalibracion;
            }
            set
            {
                FCalibracion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Calibracion"));
            }
        }

        [DataMember]
        public decimal Conf
        {
            get
            {
                return FConf;
            }
            set
            {
                FConf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Conf"));
            }
        }

        [DataMember]
        public string Abajo
        {
            get
            {
                return FAbajo;
            }
            set
            {
                FAbajo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Abajo"));
            }
        }

        public Historial()
        {
            this.FId = 0;
            this.FId_Estacion = 0;
            this.FFecha = DateTime.Today;
            this.FHora = TimeSpan.MinValue;
            this.FPosicion = 0;
            this.FManguera = 0;
            this.FPorcentaje = 0;
            this.FEstado = string.Empty;
            this.FCombustible = 0;
            this.FCalibracion = 0;
            this.FConf = 0;
            this.FAbajo = "No";
        }

        public Historial Clone()
        {
            Historial pResult = new Historial();

            pResult.Id = this.FId;
            pResult.Id_Estacion = this.FId_Estacion;
            pResult.Fecha = this.FFecha;
            pResult.Hora = this.FHora;
            pResult.Posicion = this.FPosicion;
            pResult.Manguera = this.FManguera;
            pResult.Porcentaje = this.FPorcentaje;
            pResult.Estado = this.FEstado;
            pResult.Combustible = this.FCombustible;
            pResult.FCalibracion = this.FCalibracion;
            pResult.FConf = this.FConf;
            pResult.Abajo = this.FAbajo;

            return pResult;
        }

        public int CompareTo(Historial AHistorial)
        {
            if (this.FId.CompareTo(AHistorial.Id) != 0) return 1;
            if (this.FId_Estacion.CompareTo(AHistorial.Id_Estacion) != 0) return 1;
            if (this.FFecha.CompareTo(AHistorial.Fecha) != 0) return 1;
            if (this.FHora.CompareTo(AHistorial.Hora) != 0) return 1;
            if (this.FPosicion.CompareTo(AHistorial.Posicion) != 0) return 1;
            if (this.FManguera.CompareTo(AHistorial.Manguera) != 0) return 1;
            if (this.FPorcentaje.CompareTo(AHistorial.Porcentaje) != 0) return 1;
            if (this.FEstado.CompareTo(AHistorial.Estado) != 0) return 1;
            if (this.FCombustible.CompareTo(AHistorial.Combustible) != 0) return 1;
            if (this.FCalibracion.CompareTo(AHistorial.Calibracion) != 0) return 1;
            if (this.FConf.CompareTo(AHistorial.Conf) != 0) return 1;
            if (this.FAbajo.CompareTo(AHistorial.Abajo) != 0) return 1;
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }

    [CollectionDataContract]
    [Serializable]
    public class ListaHistorial : List<Historial>
    {
    }
}