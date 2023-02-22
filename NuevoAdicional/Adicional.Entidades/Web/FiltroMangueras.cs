using System;
using System.Runtime.Serialization;

namespace Adicional.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class FiltroMangueras
    {
        [DataMember]
        public Estacion Estacion { get; set; }

        [DataMember]
        public int Posicion { get; set; }

        [DataMember]
        public UsuarioCloud Usuario { get; set; }

        [DataMember]
        public ListaHistorial Historial { get; set; }

        ~FiltroMangueras()
        {
            this.Estacion = null;
            this.Posicion = default(int);
            this.Usuario = null;

            if (this.Historial != null)
            {
                this.Historial.Clear();
                this.Historial = null;
            }
        }
    }
}
