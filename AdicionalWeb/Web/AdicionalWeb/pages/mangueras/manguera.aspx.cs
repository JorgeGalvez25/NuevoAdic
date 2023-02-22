using System;
using System.Linq;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia;
using System.Web;

namespace AdicionalWeb.pages.mangueras
{
    public partial class manguera : System.Web.UI.Page
    {
        public AdicionalWeb.Entidades.ListaDispensarios Dispensarios = new AdicionalWeb.Entidades.ListaDispensarios();

        public int Decimales;
        public double PorcentajeMaximo;
        public int Estacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["est"]))
                {
                    int estacion = 1;
                    if (int.TryParse(this.Request.QueryString["est"], out estacion))
                    {
                        estacion = 1;
                    }

                    Adicional.Entidades.Estacion est = ObtenerEstacion(estacion);
                    this.Estacion = est.Id;
                    ObtenerMangueras(est);
                    ObtenerPorcentajeMaximo(est);

                    Session[AdminSession.ESTACION] = est;
                }
            }
        }

        private Adicional.Entidades.Estacion ObtenerEstacion(int estacion)
        {
            EstacionesAdicionalPersistencia srvEstaciones = new EstacionesAdicionalPersistencia();
            return (Adicional.Entidades.Estacion)srvEstaciones.EstacionesAdicionalObtener(estacion);
        }

        private void ObtenerMangueras(Adicional.Entidades.Estacion estacion)
        {
            ManguerasPersistencia srvMangueras = new ManguerasPersistencia((Usuario)Session["usuario"]);
            var mangueras = srvMangueras.ObtenerPosiciones(estacion);
            this.Dispensarios.AddRange(from i in mangueras
                                       group i by i.posicion into g
                                       let one = mangueras.FirstOrDefault(p => p.posicion == g.Key)
                                       select new AdicionalWeb.Entidades.Dispensarios()
                                       {
                                           dispensario = one.dispensario,
                                           id = one.id,
                                           nombre = one.nombre,
                                           posicion = one.posicion,
                                           valor = one.valor
                                       });
        }

        private void ObtenerPorcentajeMaximo(Adicional.Entidades.Estacion estacion)
        {
            switch (estacion.TipoDispensario)
            {
                case Adicional.Entidades.MarcaDispensario.Wayne:
                case Adicional.Entidades.MarcaDispensario.Gilbarco:
                    this.PorcentajeMaximo = 9D;
                    this.Decimales = 0;
                    break;
                case Adicional.Entidades.MarcaDispensario.HongYang:
                case Adicional.Entidades.MarcaDispensario.Bennett:
                    this.PorcentajeMaximo = 9.99D;
                    this.Decimales = 2;
                    break;
                case Adicional.Entidades.MarcaDispensario.Team:
                    this.PorcentajeMaximo = 10D;
                    this.Decimales = 0;
                    break;
                default:
                case Adicional.Entidades.MarcaDispensario.Ninguno:
                    this.PorcentajeMaximo = 0D;
                    this.Decimales = 0;
                    break;
            }
        }
    }
}
