using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [DataContract]
    public class NotificacionesDistribuidores
    {
        public NotificacionesDistribuidores()
        {
            this.Cliente = string.Empty;
            this.Distribuidor = string.Empty;
            this.EMailDistribuidor = string.Empty;
            this.EMailMatriz = string.Empty;
            this.Estacion = string.Empty;
            this.EstatusConexion = EstatusConexion.None;
            this.EstatusTransaccion = EstatusTransaccion.None;
            this.FechaUltimaConexion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaUltimaTransaccion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.NotificarServicioTransaccion = string.Empty;
        }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public string Cliente { get; set; }

        [DataMember]
        public EstatusConexion EstatusConexion { get; set; }

        [DataMember]
        public EstatusTransaccion EstatusTransaccion { get; set; }

        [DataMember]
        public DateTime FechaUltimaConexion { get; set; }

        [DataMember]
        public DateTime FechaUltimaTransaccion { get; set; }

        [DataMember]
        public string NotificarServicioTransaccion { get; set; }

        [DataMember]
        public string EMailDistribuidor { get; set; }

        [DataMember]
        public string EMailMatriz { get; set; }

        [DataMember]
        public string Distribuidor { get; set; }

        [DataMember]
        public int DiasAtraso { get; set; }
    }

    [CollectionDataContract]
    public class ListaNotificacionesDistribuidores : List<NotificacionesDistribuidores>
    {
        ~ListaNotificacionesDistribuidores()
        {
            this.Clear();
        }
    }

    [DataContract]
    public class FiltroNotificacionesDistribuidores
    {
        public FiltroNotificacionesDistribuidores()
        {
            this.Estacion = string.Empty;
            this.EstatusConexion = EstatusConexion.None;
            this.EstatusTransaccion = EstatusTransaccion.None;
        }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public EstatusConexion EstatusConexion { get; set; }

        [DataMember]
        public EstatusTransaccion EstatusTransaccion { get; set; }
    }
}
