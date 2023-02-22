using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class AdministrarDistribuidores : //ImagenSoft.Entidades.ClaseBase,
                                             IComparable<AdministrarDistribuidores>
    {
        public AdministrarDistribuidores()
        {
            this.Activo = string.Empty;
            this.Descripcion = string.Empty;
            this.EMail = string.Empty;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string EMail { get; set; }

        [DataMember]
        public string Activo { get; set; }

        public AdministrarDistribuidores Clonar()
        {
            return (AdministrarDistribuidores)base.MemberwiseClone();
        }

        #region IComparable<AdministrarDistribuidores> Members

        public int CompareTo(AdministrarDistribuidores other)
        {
            if (this.Clave != other.Clave) return 1;
            if (this.Descripcion != other.Descripcion) return 1;
            if (this.EMail != other.EMail) return 1;
            if (this.Activo != other.Activo) return 1;

            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaAdministrarDistribuidores : List<AdministrarDistribuidores>
    {
        public ListaAdministrarDistribuidores ObtenerPorActivo(string activo)
        {
            if (string.IsNullOrEmpty(activo)) { return this; }
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();
            resultado.AddRange(this.Where(p => p.Activo.Equals(activo, StringComparison.CurrentCultureIgnoreCase)));

            return resultado;
        }

        public ListaAdministrarDistribuidores ObtenerPorNombre(string aBuscar)
        {
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();
            resultado.AddRange(this.Where(p => p.Descripcion.ToLower().Contains(aBuscar.ToLower())));

            return resultado;
        }

        ~ListaAdministrarDistribuidores()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroAdministrarDistribuidores
    {
        public FiltroAdministrarDistribuidores()
        {
            this.Activo = string.Empty;
            this.Descripcion = string.Empty;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Activo { get; set; }
    }
}
