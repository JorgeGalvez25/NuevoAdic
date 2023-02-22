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
    public class Proteccion : INotifyPropertyChanged
    {
        private int m_estacion;
        private int m_litros;
        private string m_activa;

        [DataMember]
        public int Estacion
        {
            get
            {
                return m_estacion;
            }
            set
            {
                m_estacion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Estacion"));
            }
        }

        [DataMember]
        public int Litros
        {
            get
            {
                return m_litros;
            }
            set
            {
                m_litros = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Litros"));
            }
        }

        [DataMember]
        public string Activa
        {
            get
            {
                return m_activa;
            }
            set
            {
                m_activa = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Activa"));
            }
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
    public class ListaProteccion : List<Proteccion>
    {
    }
}
