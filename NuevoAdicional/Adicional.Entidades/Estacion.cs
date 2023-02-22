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
    public class Estacion : INotifyPropertyChanged
    {
        private int f_id;
        private string m_nombre;
        private string m_ipServicios;
        private string m_estado;
        private DateTime m_ultimoMovimiento;
        private bool m_proteccionesActivas;
        private MarcaDispensario m_tipoDispensario;
        private EstatusPresetWayne m_estatuspWayne;

        [DataMember]
        public int Id
        {
            get
            {
                return f_id;
            }
            set
            {
                f_id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        [DataMember]
        public string Nombre
        {
            get
            {
                return m_nombre;
            }
            set
            {
                m_nombre = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Nombre"));
            }
        }

        [DataMember]
        public string IpServicios
        {
            get
            {
                return m_ipServicios;
            }
            set
            {
                m_ipServicios = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IpServicios"));
            }
        }

        [DataMember]
        public string Estado
        {
            get
            {
                return m_estado;
            }
            set
            {
                m_estado = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Estado"));
            }
        }

        [DataMember]
        public DateTime UltimoMovimiento
        {
            get
            {
                return m_ultimoMovimiento;
            }
            set
            {
                m_ultimoMovimiento = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UltimoMovimiento"));
            }
        }

        [DataMember]
        public bool ProteccionesActivas
        {
            get
            {
                return m_proteccionesActivas;
            }
            set
            {
                m_proteccionesActivas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProteccionesActivas"));
            }
        }

        [DataMember]
        public MarcaDispensario TipoDispensario
        {
            get
            {
                return m_tipoDispensario;
            }
            set
            {
                m_tipoDispensario = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TipoDispensario"));
            }
        }

        [DataMember]
        public EstatusPresetWayne EstadoPresetWayne
        {
            get
            {
                return m_estatuspWayne;
            }
            set
            {
                m_estatuspWayne = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EstadoPresetWayne"));
            }
        }

        public Estacion()
        {
            this.f_id = 0;
            this.m_nombre = string.Empty;
            this.m_ipServicios = string.Empty;
            this.m_estado = "Mínimo";
            this.m_ultimoMovimiento = DateTime.Today;
        }

        public Estacion Clone()
        {
            Estacion pResult = new Estacion();

            pResult.Id = this.f_id;
            pResult.Nombre = this.m_nombre;
            pResult.IpServicios = this.m_ipServicios;
            pResult.Estado = this.m_estado;
            pResult.UltimoMovimiento = this.m_ultimoMovimiento;

            return pResult;
        }

        public int CompareTo(Estacion AEstacion)
        {
            if (this.f_id.CompareTo(AEstacion.Id) != 0) return 1;
            if (this.m_nombre.CompareTo(AEstacion.Nombre) != 0) return 1;
            if (this.m_ipServicios.CompareTo(AEstacion.IpServicios) != 0) return 1;
            if (this.m_estado.CompareTo(AEstacion.Estado) != 0) return 1;
            if (this.m_ultimoMovimiento.CompareTo(AEstacion.UltimoMovimiento) != 0) return 1;
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
    public class ListaEstacion : List<Estacion>
    {
    }
}
