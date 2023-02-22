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
    public class Comandos : INotifyPropertyChanged
    {
        private int FFolio;
        private string FModulo;
        private DateTime FFechahora;
        private string FComando;
        private string FAplicado;
        private string FResultado;

        [DataMember]
        public int Folio
        {
            get
            {
                return FFolio;
            }
            set
            {
                FFolio = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Folio"));
            }
        }

        [DataMember]
        public string Modulo
        {
            get
            {
                return FModulo;
            }
            set
            {
                FModulo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Modulo"));
            }
        }

        [DataMember]
        public DateTime Fechahora
        {
            get
            {
                return FFechahora;
            }
            set
            {
                FFechahora = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fechahora"));
            }
        }

        [DataMember]
        public string Comando
        {
            get
            {
                return FComando;
            }
            set
            {
                FComando = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comando"));
            }
        }

        [DataMember]
        public string Aplicado
        {
            get
            {
                return FAplicado;
            }
            set
            {
                FAplicado = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Aplicado"));
            }
        }

        [DataMember]
        public string Resultado
        {
            get
            {
                return FResultado;
            }
            set
            {
                FResultado = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Resultado"));
            }
        }

        public Comandos()
        {
            this.FFolio = 0;
            this.FModulo = string.Empty;
            this.FFechahora = DateTime.Today;
            this.FComando = string.Empty;
            this.FAplicado = string.Empty;
            this.FResultado = string.Empty;
        }

        public Comandos Clone()
        {
            Comandos pResult = new Comandos();

            pResult.Folio = this.FFolio;
            pResult.Modulo = this.FModulo;
            pResult.Fechahora = this.FFechahora;
            pResult.Comando = this.FComando;
            pResult.Aplicado = this.FAplicado;
            pResult.Resultado = this.FResultado;

            return pResult;
        }

        public int CompareTo(Comandos AComando)
        {
            if (this.FFolio.CompareTo(AComando.Folio) != 0) return 1;
            if (this.FModulo.CompareTo(AComando.Modulo) != 0) return 1;
            if (this.FFechahora.CompareTo(AComando.Fechahora) != 0) return 1;
            if (this.FComando.CompareTo(AComando.Comando) != 0) return 1;
            if (this.FAplicado.CompareTo(AComando.Aplicado) != 0) return 1;
            if (this.FResultado.CompareTo(AComando.Resultado) != 0) return 1;
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
