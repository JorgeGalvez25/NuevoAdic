using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class AdministrarUsuariosClientes
    {
        public AdministrarUsuariosClientes()
        {
            this.Activo = string.Empty;
            this.Correo = string.Empty;
            this.FechaCambio = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Matriz = string.Empty;
            this.NoEstacion = string.Empty;
            this.Nombre = string.Empty;
            this.NuevoUsuario = string.Empty;
            this.NuevoNoEstacion = string.Empty;
            this.Password = string.Empty;
            this.Privilegios = new Privilegios();
            this.Rol = string.Empty;
            this.Usuario = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string NuevoNoEstacion { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string NuevoUsuario { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public DateTime FechaCambio { get; set; }

        [DataMember]
        public string Rol { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public Privilegios Privilegios { get; set; }

        [DataMember]
        public string Activo { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaAdministrarUsuariosClientes
        : List<AdministrarUsuariosClientes>
    {
        ~ListaAdministrarUsuariosClientes()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroAdministrarUsuariosClientes
    {
        public FiltroAdministrarUsuariosClientes()
        {
            this.Activo = string.Empty;
            this.Correo = string.Empty;
            this.FechaCambio = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Host = string.Empty;
            this.Matriz = string.Empty;
            this.NoEstacion = string.Empty;
            this.Password = string.Empty;
            this.Rol = string.Empty;
            this.Usuario = string.Empty;
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string Rol { get; set; }

        [DataMember]
        public string Activo { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public DateTime FechaCambio { get; set; }
    }
}
