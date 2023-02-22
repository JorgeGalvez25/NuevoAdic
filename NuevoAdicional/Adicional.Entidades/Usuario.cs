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
    public class Usuario : INotifyPropertyChanged
    {
        private int FId;
        private string FNombre;
        private string FClave;
        private string FActivo;
        private string FVariables;
        private ListaEstacion FEStaciones;

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

        [DataMember]
        public string Clave
        {
            get
            {
                return FClave;
            }
            set
            {
                FClave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Clave"));
            }
        }

        [DataMember]
        public string Activo
        {
            get
            {
                return FActivo;
            }
            set
            {
                FActivo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Activo"));
            }
        }

        [DataMember]
        public string Variables
        {
            get
            {
                return FVariables;
            }
            set
            {
                FVariables = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Variables"));
            }
        }

        [DataMember]
        public ListaEstacion Estaciones
        {
            get { return this.FEStaciones; }
            set { this.FEStaciones = value; }
        }

        public string CadenaEstaciones()
        {
            List<string> nombres;

            if (Estaciones != null && Estaciones.Count > 0)
            {
                nombres = (from e in Estaciones
                           select e.Nombre).ToList();
            }
            else
            {
                nombres = new List<string>();
            }

            return nombres != null && nombres.Count > 0 ? nombres.Aggregate((first, second) => string.Concat(first, ", ", second)) : string.Empty;
        }

        public string GetValorVariable(string variable)
        {
            string valor = string.Empty;

            if (string.IsNullOrEmpty(variable))
            {
                throw new ArgumentException("El parámetro variable no puede ir vacío");
            }

            if (string.IsNullOrEmpty(this.Variables))
            {
                return string.Empty;
            }

            variable = string.Concat(variable, "=");

            string[] vars = this.FVariables.Split(new string[] { ",", ";", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> valores = (vars.Where(s => s.StartsWith(variable, StringComparison.OrdinalIgnoreCase))).ToList();

            if (valores != null && valores.Count > 0)
            {
                valor = valores[0].Split('=')[1];
            }

            return valor;
        }

        public Usuario()
        {
            this.FId = 0;
            this.FNombre = string.Empty;
            this.FClave = string.Empty;
            this.FActivo = string.Empty;
            this.FVariables = string.Empty;
            this.FEStaciones = new ListaEstacion();
        }

        public Usuario Clone()
        {
            Usuario pResult = new Usuario();

            pResult.Id = this.FId;
            pResult.Nombre = this.FNombre;
            pResult.Clave = this.FClave;
            pResult.Activo = this.FActivo;
            pResult.Variables = this.FVariables;

            return pResult;
        }

        public int CompareTo(Usuario AUsuario)
        {
            if (this.FId.CompareTo(AUsuario.Id) != 0) return 1;
            if (this.FNombre.CompareTo(AUsuario.Nombre) != 0) return 1;
            if (this.FClave.CompareTo(AUsuario.Clave) != 0) return 1;
            if (this.FActivo.CompareTo(AUsuario.Activo) != 0) return 1;
            return 0;
        }

        public override string ToString()
        {
            return this.Nombre;
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
    public class ListaUsuario : List<Usuario>
    {
    }
}
