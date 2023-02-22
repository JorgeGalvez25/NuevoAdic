using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AdicionalCloud = Adicional.Entidades;

namespace ImagenSoft.ModuloWeb.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class Estacion :// ImagenSoft.Entidades.ClaseBase,
                            IComparable<Estacion>
    {
        public Estacion()
        {
            this.Consola = string.Empty;
            this.IP = string.Empty;
            this.NoEstacion = string.Empty;
            this.Nombre = string.Empty;
            this.Matriz = string.Empty;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public int Puerto { get; set; }

        [DataMember]
        public string Consola { get; set; }

        [DataMember]
        public bool ConMembresia { get; set; }

        [DataMember]
        public bool Conexion { get; set; }

        [DataMember]
        public AdicionalCloud.MarcaDispensario Dispensario { get; set; }

        public string GetFromConsola(string toFind)
        {
            if (string.IsNullOrEmpty(this.Consola)) { return string.Empty; }

            string[] fItems = this.Consola.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            char[] splt = new char[] { '=' };

            foreach (string item in fItems)
            {
                if (item.Trim().StartsWith(toFind, StringComparison.CurrentCultureIgnoreCase))
                {
                    string[] sItems = item.Split(splt, StringSplitOptions.RemoveEmptyEntries);
                    return (sItems.Length == 2 && sItems[0].Equals(toFind, StringComparison.CurrentCultureIgnoreCase)) ? sItems[1].Trim() : string.Empty;
                }
            }

            return string.Empty;
        }

        #region IComparable<Estacion> Members

        public int CompareTo(Estacion other)
        {
            if (this.Matriz == other.Matriz) { return 1; }
            if (this.IP == other.IP) { return 1; }
            if (this.NoEstacion == other.NoEstacion) { return 1; }
            if (this.Nombre == other.Nombre) { return 1; }

            return 0;
        }

        public Estacion Clone()
        {
            return (Estacion)this.MemberwiseClone();
        }

        public override bool Equals(Object obj)
        {
            return obj is Estacion && this == (Estacion)obj;
        }

        public override int GetHashCode()
        {
            return this.Matriz.GetHashCode() ^
                   this.IP.GetHashCode() ^
                   this.NoEstacion.GetHashCode() ^
                   this.Nombre.GetHashCode();
        }

        //public static bool operator ==(Estacion x, Estacion y)
        //{
        //    if (x == null || y == null) { return x == y; }
        //    return x.Mariz == y.Mariz &&
        //           x.IP == y.IP &&
        //           x.NoEstacion == y.NoEstacion &&
        //           x.Nombre == y.Nombre;
        //}

        //public static bool operator !=(Estacion x, Estacion y)
        //{
        //    return !(x == y);
        //}

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaEstaciones : List<Estacion>
    {
        public ListaEstaciones ObtenerPorMatriz(string matriz)
        {
            ListaEstaciones lst = new ListaEstaciones();
            lst.AddRange(this.Where(p => p.Matriz.Equals(matriz, StringComparison.CurrentCultureIgnoreCase)));
            return lst;
        }

        public ListaEstaciones ObtenerPorEstacion(string noEstacion)
        {
            if (string.IsNullOrEmpty(noEstacion)) { return new ListaEstaciones(); }
            ListaEstaciones lst = new ListaEstaciones();
            lst.AddRange(this.Where(p => p.NoEstacion.Equals(noEstacion, StringComparison.CurrentCultureIgnoreCase)));
            return lst;
        }

        ~ListaEstaciones()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroEstacion
    {
        public FiltroEstacion()
        {
            this.IP = string.Empty;
            this.NoEstacion = string.Empty;
            this.Matriz = string.Empty;
        }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public bool? Conexion { get; set; }

        [DataMember]
        public bool? Activo { get; set; }
    }
}
