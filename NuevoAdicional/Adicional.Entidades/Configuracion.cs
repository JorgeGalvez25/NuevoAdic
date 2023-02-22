using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [DataContract]
    [Serializable]
    public class Configuracion : INotifyPropertyChanged
    {
        private int FId;
        private decimal FCantidad_minima;
        private bool FProteccionesActivas;
        private string FEstado;
        private DateTime FUltimoMovimiento;
        private DateTime FUltimaSincro;
        private TimeSpan FHoraSincro;
        private EdoRemoto m_estadoRemoto;
        private string m_usuarioPresiono;

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
        public decimal Cantidad_minima
        {
            get
            {
                return FCantidad_minima;
            }
            set
            {
                FCantidad_minima = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cantidad_minima"));
            }
        }

        [DataMember]
        public bool ProteccionesActivas
        {
            get
            {
                return FProteccionesActivas;
            }
            set
            {
                FProteccionesActivas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProteccionesActivas"));
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
        public DateTime UltimoMovimiento
        {
            get
            {
                return FUltimoMovimiento;
            }
            set
            {
                FUltimoMovimiento = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UltimoMovimiento"));
            }
        }

        [DataMember]
        public DateTime UltimaSincro
        {
            get
            {
                return FUltimaSincro;
            }
            set
            {
                FUltimaSincro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UltimaSincro"));
            }
        }

        [DataMember]
        public TimeSpan HoraSincro
        {
            get
            {
                return FHoraSincro;
            }
            set
            {
                FHoraSincro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HoraSincro"));
            }
        }

        [DataMember]
        public EdoRemoto EstadoRemoto
        {
            get { return this.m_estadoRemoto; }
            set { this.m_estadoRemoto = value; }
        }

        [DataMember]
        public string UsuarioPresiono
        {
            get { return this.m_usuarioPresiono; }
            set { this.m_usuarioPresiono = value; }
        }

        public Configuracion()
        {
            this.FId = 0;
            this.FCantidad_minima = 0;
            this.FEstado = string.Empty;
            this.FUltimoMovimiento = DateTime.MinValue;
            this.FUltimaSincro = DateTime.MinValue;
            this.FHoraSincro = TimeSpan.MinValue;
            this.EstadoRemoto = EdoRemoto.Desconectado;
            this.UsuarioPresiono = string.Empty;
        }

        public Configuracion Clone()
        {
            Configuracion pResult = new Configuracion();

            pResult.Id = this.FId;
            pResult.Cantidad_minima = this.FCantidad_minima;

            return pResult;
        }

        public int CompareTo(Configuracion AConfiguracion)
        {
            if (this.FId.CompareTo(AConfiguracion.Id) != 0) return 1;
            if (this.FCantidad_minima.CompareTo(AConfiguracion.Cantidad_minima) != 0) return 1;
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }

    [DataContract]
    public enum EdoRemoto
    {
        [EnumMember]
        Desconectado,
        [EnumMember]
        Pulsado,
        [EnumMember]
        VisualEncendido,
        [EnumMember]
        VisualApagado,
        [EnumMember]
        SinLicencia
    }
}
