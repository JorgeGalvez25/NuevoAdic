using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ServiciosCliente
{
    public class Bomba : INotifyPropertyChanged
    {
        private int Fmanguera;
        private int Fposcarga;
        private int Fcombustible;
        private int Fisla;
        private int Fcon_precio;
        private int Fcon_posicion;
        private int Fcon_digitoajuste;
        private int Fimpresora;
        private string FActivo;
        private string Fimprimeautom;
        private int Fdigitoajusteprecio;
        private string Fcampolectura;
        private string Fmodooperacion;
        private int FTanque;
        private string Fimpretarjetas;
        private int Fdigitosgilbarco;
        private int Fdecimalesgilbarco;
        private int Fdigitoajustevol;
        private string Frfid;
        private string Fcliente;
        private string Fvehiculo;
        private string Fcontrol_aros;
        private string Ferror;
        private string Fmensaje_error;

        public int manguera
        {
            get
            {
                return Fmanguera;
            }
            set
            {
                Fmanguera = value;
                OnPropertyChanged(new PropertyChangedEventArgs("manguera"));
            }
        }

        public int poscarga
        {
            get
            {
                return Fposcarga;
            }
            set
            {
                Fposcarga = value;
                OnPropertyChanged(new PropertyChangedEventArgs("poscarga"));
            }
        }

        public int combustible
        {
            get
            {
                return Fcombustible;
            }
            set
            {
                Fcombustible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("combustible"));
            }
        }

        public int isla
        {
            get
            {
                return Fisla;
            }
            set
            {
                Fisla = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isla"));
            }
        }

        public int con_precio
        {
            get
            {
                return Fcon_precio;
            }
            set
            {
                Fcon_precio = value;
                OnPropertyChanged(new PropertyChangedEventArgs("con_precio"));
            }
        }

        public int con_posicion
        {
            get
            {
                return Fcon_posicion;
            }
            set
            {
                Fcon_posicion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("con_posicion"));
            }
        }

        public int con_digitoajuste
        {
            get
            {
                return Fcon_digitoajuste;
            }
            set
            {
                Fcon_digitoajuste = value;
                OnPropertyChanged(new PropertyChangedEventArgs("con_digitoajuste"));
            }
        }

        public int impresora
        {
            get
            {
                return Fimpresora;
            }
            set
            {
                Fimpresora = value;
                OnPropertyChanged(new PropertyChangedEventArgs("impresora"));
            }
        }

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

        public string imprimeautom
        {
            get
            {
                return Fimprimeautom;
            }
            set
            {
                Fimprimeautom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("imprimeautom"));
            }
        }

        public int digitoajusteprecio
        {
            get
            {
                return Fdigitoajusteprecio;
            }
            set
            {
                Fdigitoajusteprecio = value;
                OnPropertyChanged(new PropertyChangedEventArgs("digitoajusteprecio"));
            }
        }

        public string campolectura
        {
            get
            {
                return Fcampolectura;
            }
            set
            {
                Fcampolectura = value;
                OnPropertyChanged(new PropertyChangedEventArgs("campolectura"));
            }
        }

        public string modooperacion
        {
            get
            {
                return Fmodooperacion;
            }
            set
            {
                Fmodooperacion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("modooperacion"));
            }
        }

        public int Tanque
        {
            get
            {
                return FTanque;
            }
            set
            {
                FTanque = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tanque"));
            }
        }

        public string impretarjetas
        {
            get
            {
                return Fimpretarjetas;
            }
            set
            {
                Fimpretarjetas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("impretarjetas"));
            }
        }

        public int digitosgilbarco
        {
            get
            {
                return Fdigitosgilbarco;
            }
            set
            {
                Fdigitosgilbarco = value;
                OnPropertyChanged(new PropertyChangedEventArgs("digitosgilbarco"));
            }
        }

        public int decimalesgilbarco
        {
            get
            {
                return Fdecimalesgilbarco;
            }
            set
            {
                Fdecimalesgilbarco = value;
                OnPropertyChanged(new PropertyChangedEventArgs("decimalesgilbarco"));
            }
        }

        public int digitoajustevol
        {
            get
            {
                return Fdigitoajustevol;
            }
            set
            {
                Fdigitoajustevol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("digitoajustevol"));
            }
        }

        public string rfid
        {
            get
            {
                return Frfid;
            }
            set
            {
                Frfid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("rfid"));
            }
        }

        public string cliente
        {
            get
            {
                return Fcliente;
            }
            set
            {
                Fcliente = value;
                OnPropertyChanged(new PropertyChangedEventArgs("cliente"));
            }
        }

        public string vehiculo
        {
            get
            {
                return Fvehiculo;
            }
            set
            {
                Fvehiculo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("vehiculo"));
            }
        }

        public string control_aros
        {
            get
            {
                return Fcontrol_aros;
            }
            set
            {
                Fcontrol_aros = value;
                OnPropertyChanged(new PropertyChangedEventArgs("control_aros"));
            }
        }

        public string error
        {
            get
            {
                return Ferror;
            }
            set
            {
                Ferror = value;
                OnPropertyChanged(new PropertyChangedEventArgs("error"));
            }
        }

        public string mensaje_error
        {
            get
            {
                return Fmensaje_error;
            }
            set
            {
                Fmensaje_error = value;
                OnPropertyChanged(new PropertyChangedEventArgs("mensaje_error"));
            }
        }

        public Bomba()
        {
            this.Fmanguera = 0;
            this.Fposcarga = 0;
            this.Fcombustible = 0;
            this.Fisla = 0;
            this.Fcon_precio = 0;
            this.Fcon_posicion = 0;
            this.Fcon_digitoajuste = 0;
            this.Fimpresora = 0;
            this.FActivo = string.Empty;
            this.Fimprimeautom = string.Empty;
            this.Fdigitoajusteprecio = 0;
            this.Fcampolectura = string.Empty;
            this.Fmodooperacion = string.Empty;
            this.FTanque = 0;
            this.Fimpretarjetas = string.Empty;
            this.Fdigitosgilbarco = 0;
            this.Fdecimalesgilbarco = 0;
            this.Fdigitoajustevol = 0;
            this.Frfid = string.Empty;
            this.Fcliente = string.Empty;
            this.Fvehiculo = string.Empty;
            this.Fcontrol_aros = string.Empty;
            this.Ferror = string.Empty;
            this.Fmensaje_error = string.Empty;
        }

        public Bomba Clone()
        {
            Bomba pResult = new Bomba();

            pResult.manguera = this.Fmanguera;
            pResult.poscarga = this.Fposcarga;
            pResult.combustible = this.Fcombustible;
            pResult.isla = this.Fisla;
            pResult.con_precio = this.Fcon_precio;
            pResult.con_posicion = this.Fcon_posicion;
            pResult.con_digitoajuste = this.Fcon_digitoajuste;
            pResult.impresora = this.Fimpresora;
            pResult.Activo = this.FActivo;
            pResult.imprimeautom = this.Fimprimeautom;
            pResult.digitoajusteprecio = this.Fdigitoajusteprecio;
            pResult.campolectura = this.Fcampolectura;
            pResult.modooperacion = this.Fmodooperacion;
            pResult.Tanque = this.FTanque;
            pResult.impretarjetas = this.Fimpretarjetas;
            pResult.digitosgilbarco = this.Fdigitosgilbarco;
            pResult.decimalesgilbarco = this.Fdecimalesgilbarco;
            pResult.digitoajustevol = this.Fdigitoajustevol;
            pResult.rfid = this.Frfid;
            pResult.cliente = this.Fcliente;
            pResult.vehiculo = this.Fvehiculo;
            pResult.control_aros = this.Fcontrol_aros;
            pResult.error = this.Ferror;
            pResult.mensaje_error = this.Fmensaje_error;

            return pResult;
        }

        public int CompareTo(Bomba ABomba)
        {
            if (this.Fmanguera.CompareTo(ABomba.manguera) != 0) return 1;
            if (this.Fposcarga.CompareTo(ABomba.poscarga) != 0) return 1;
            if (this.Fcombustible.CompareTo(ABomba.combustible) != 0) return 1;
            if (this.Fisla.CompareTo(ABomba.isla) != 0) return 1;
            if (this.Fcon_precio.CompareTo(ABomba.con_precio) != 0) return 1;
            if (this.Fcon_posicion.CompareTo(ABomba.con_posicion) != 0) return 1;
            if (this.Fcon_digitoajuste.CompareTo(ABomba.con_digitoajuste) != 0) return 1;
            if (this.Fimpresora.CompareTo(ABomba.impresora) != 0) return 1;
            if (this.FActivo.CompareTo(ABomba.Activo) != 0) return 1;
            if (this.Fimprimeautom.CompareTo(ABomba.imprimeautom) != 0) return 1;
            if (this.Fdigitoajusteprecio.CompareTo(ABomba.digitoajusteprecio) != 0) return 1;
            if (this.Fcampolectura.CompareTo(ABomba.campolectura) != 0) return 1;
            if (this.Fmodooperacion.CompareTo(ABomba.modooperacion) != 0) return 1;
            if (this.FTanque.CompareTo(ABomba.Tanque) != 0) return 1;
            if (this.Fimpretarjetas.CompareTo(ABomba.impretarjetas) != 0) return 1;
            if (this.Fdigitosgilbarco.CompareTo(ABomba.digitosgilbarco) != 0) return 1;
            if (this.Fdecimalesgilbarco.CompareTo(ABomba.decimalesgilbarco) != 0) return 1;
            if (this.Fdigitoajustevol.CompareTo(ABomba.digitoajustevol) != 0) return 1;
            if (this.Frfid.CompareTo(ABomba.rfid) != 0) return 1;
            if (this.Fcliente.CompareTo(ABomba.cliente) != 0) return 1;
            if (this.Fvehiculo.CompareTo(ABomba.vehiculo) != 0) return 1;
            if (this.Fcontrol_aros.CompareTo(ABomba.control_aros) != 0) return 1;
            if (this.Ferror.CompareTo(ABomba.error) != 0) return 1;
            if (this.Fmensaje_error.CompareTo(ABomba.mensaje_error) != 0) return 1;
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
