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
    public class Bitacora : INotifyPropertyChanged
    {
        private int FId;
        private string FId_usuario;
        private DateTime FFecha;
        private TimeSpan FHora;
        private string FSuceso;

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
        public string Id_usuario
        {
            get
            {
                return FId_usuario;
            }
            set
            {
                FId_usuario = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id_usuario"));
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
        public string Suceso
        {
            get
            {
                return FSuceso;
            }
            set
            {
                FSuceso = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Suceso"));
            }
        }

        public Bitacora()
        {
            this.FId = 0;
            this.FId_usuario = string.Empty;
            this.FFecha = DateTime.Today;
            this.FHora = TimeSpan.MinValue;
            this.FSuceso = string.Empty;
        }

        public Bitacora Clone()
        {
            Bitacora pResult = new Bitacora();

            pResult.Id = this.FId;
            pResult.Id_usuario = this.FId_usuario;
            pResult.Fecha = this.FFecha;
            pResult.Hora = this.FHora;
            pResult.Suceso = this.FSuceso;

            return pResult;
        }

        public int CompareTo(Bitacora ABitacora)
        {
            if (this.FId.CompareTo(ABitacora.Id) != 0) return 1;
            if (this.FId_usuario.CompareTo(ABitacora.Id_usuario) != 0) return 1;
            if (this.FFecha.CompareTo(ABitacora.Fecha) != 0) return 1;
            if (this.FHora.CompareTo(ABitacora.Hora) != 0) return 1;
            if (this.FSuceso.CompareTo(ABitacora.Suceso) != 0) return 1;
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
    public class ListaBitacora : List<Bitacora>
    {
    }
}
