using System;
using AdicionalWeb.Extesiones;
using ImagenSoft.ModuloWeb.Entidades.Web;

namespace AdicionalWeb.pages.flujos
{
    public partial class flujo : System.Web.UI.Page
    {
        public string NoEstacionActual = string.Empty;
        public string NombreEstacion = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb current = (Session[AdminSession.MODULO_WEB] as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb);
            NoEstacionActual = this.Request.QueryString["noEst"] ?? current.EstacionActual.NoEstacion;

            Estacion estacion = current.Estaciones.Find(p => p.NoEstacion == NoEstacionActual);
            NombreEstacion = string.Format("{0} - {1}", NoEstacionActual, (estacion == null ? current.EstacionActual.Nombre : estacion.Nombre).Trim().ToLower().ToTitle());
        }
    }
}
