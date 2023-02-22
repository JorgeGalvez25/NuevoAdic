
using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class UsuarioWeb : //ImagenSoft.Entidades.ClaseBase,
                              IComparable<UsuarioWeb>
    {
        public UsuarioWeb()
        {
            this.Estacion = new Estacion();
            this.Correo = string.Empty;
            this.NoEstacion = string.Empty;
            this.Password = string.Empty;
            this.Privilegios = new object();
            this.Usuario = string.Empty;
            this.Rol = string.Empty;
        }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public string Rol { get; set; }

        [DataMember]
        public object Privilegios { get; set; }

        [DataMember]
        public Estacion Estacion { get; set; }

        #region IComparable<Estacion> Members

        public int CompareTo(UsuarioWeb other)
        {
            if (this.Usuario == other.Usuario) { return 1; }
            if (this.Password == other.Password) { return 1; }
            if (this.Estacion == other.Estacion) { return 1; }

            return 0;
        }

        public UsuarioWeb Clone()
        {
            return (UsuarioWeb)this.MemberwiseClone();
        }

        public override bool Equals(Object obj)
        {
            return obj is UsuarioWeb && this == (UsuarioWeb)obj;
        }

        public override int GetHashCode()
        {
            return this.Usuario.GetHashCode() ^
                   this.Password.GetHashCode() ^
                   this.Estacion.GetHashCode() ^
                   this.Correo.GetHashCode() ^
                   this.Rol.GetHashCode() ^
                   this.Privilegios.GetHashCode();
        }

        //public static bool operator ==(UsuarioWeb x, UsuarioWeb y)
        //{
        //    return x.Usuario == y.Usuario &&
        //           x.Password == y.Password &&
        //           x.Estacion == y.Estacion &&
        //           x.Correo == y.Correo;
        //}

        //public static bool operator !=(UsuarioWeb x, UsuarioWeb y)
        //{
        //    return !(x == y);
        //}

        #endregion
    }
}
