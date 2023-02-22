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
    public class Flujo : INotifyPropertyChanged
    {
        private int FPoscarga;
        private double FSlowflow;
        private double FSlowflow2;
        private double FSlowflow3;

        [DataMember]
        public int Poscarga
        {
            get
            {
                return FPoscarga;
            }
            set
            {
                FPoscarga = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Poscarga"));
            }
        }

        [DataMember]
        public double Slowflow
        {
            get
            {
                return FSlowflow;
            }
            set
            {
                FSlowflow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Slowflow"));
            }
        }

        [DataMember]
        public double Slowflow2
        {
            get
            {
                return FSlowflow2;
            }
            set
            {
                FSlowflow2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Slowflow2"));
            }
        }

        [DataMember]
        public double Slowflow3
        {
            get
            {
                return FSlowflow3;
            }
            set
            {
                FSlowflow3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Slowflow3"));
            }
        }

        public Flujo()
        {
            this.FPoscarga = 0;
            this.FSlowflow = 0;
            this.FSlowflow2 = 0;
            this.FSlowflow3 = 0;
        }

        public Flujo Clone()
        {
            Flujo pResult = new Flujo();

            pResult.Poscarga = this.FPoscarga;
            pResult.Slowflow = this.FSlowflow;
            pResult.Slowflow2 = this.FSlowflow2;
            pResult.Slowflow3 = this.FSlowflow3;

            return pResult;
        }

        public int CompareTo(Flujo AFlujo)
        {
            if (this.FPoscarga.CompareTo(AFlujo.Poscarga) != 0) return 1;
            if (this.FSlowflow.CompareTo(AFlujo.Slowflow) != 0) return 1;
            if (this.FSlowflow2.CompareTo(AFlujo.Slowflow2) != 0) return 1;
            if (this.FSlowflow3.CompareTo(AFlujo.Slowflow3) != 0) return 1;
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
