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
    public class Derecho : INotifyPropertyChanged
    {
        private int FId;
        private int FId_Usuario;
        private int FId_Derecho;
        private string FNombre;

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
        public int Id_Usuario
        {
            get
            {
                return FId_Usuario;
            }
            set
            {
                FId_Usuario = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id_Usuario"));
            }
        }

        [DataMember]
        public int Id_Derecho
        {
            get
            {
                return FId_Derecho;
            }
            set
            {
                FId_Derecho = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id_Derecho"));
            }
        }

        [DataMember]
        public string Nombre
        {
            get
            {
                return FNombre;
            }
            set
            {
                FNombre = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Nombre"));
            }
        }

        public Derecho()
        {
            this.FId = 0;
            this.FId_Usuario = 0;
            this.FId_Derecho = 0;
            this.FNombre = string.Empty;
        }

        public Derecho Clone()
        {
            Derecho pResult = new Derecho();

            pResult.Id = this.FId;
            pResult.Id_Usuario = this.FId_Usuario;
            pResult.Id_Derecho = this.FId_Derecho;
            pResult.Nombre = this.FNombre;

            return pResult;
        }

        public int CompareTo(Derecho ADerecho)
        {
            if (this.FId.CompareTo(ADerecho.Id) != 0) return 1;
            if (this.FId_Usuario.CompareTo(ADerecho.Id_Usuario) != 0) return 1;
            if (this.FId_Derecho.CompareTo(ADerecho.Id_Derecho) != 0) return 1;
            if (this.FNombre.CompareTo(ADerecho.Nombre) != 0) return 1;
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
    public class ListaDerecho : List<Derecho>
    {
    }
}
