using System;
using System.Linq;
using Adicional.Entidades;
using Adicional.Entidades.Web;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Entidades.Web.Adicional;
using ImagenSoft.ModuloWeb.Proveedor.Publicador;
using System.Web;
using System.Threading;
using System.Web.UI;

namespace AdicionalWeb.pages.mangueras
{
    public partial class manguera : System.Web.UI.Page
    {
        public ListaDispensarios Dispensarios = new ListaDispensarios();
        public int Decimales;
        public double PorcentajeMaximo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string estacion = this.Request.QueryString["noEst"];
                if (!string.IsNullOrEmpty(estacion))
                {
                    var sesionCloud = (Session[AdminSession.MODULO_WEB] as SesionModuloWeb);
                    if (sesionCloud != null)
                    {
                        sesionCloud.EstacionActual = sesionCloud.Estaciones.Find(p => p.NoEstacion == estacion);

                        HttpContext.Current.Cache.Remove(string.Format("{0}_mangueras", sesionCloud.EstacionActual.NoEstacion));

                        ObtenerMangueras(sesionCloud);
                        ObtenerPorcentajeMaximo(sesionCloud.EstacionActual.Dispensario);
                    }
                }
            }
        }

        private void ObtenerMangueras(SesionModuloWeb sesionCloud)
        {
            ServiciosModuloWebProveedor servicio = new ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);
            var mangueras = servicio.AdicionalWebObtenerMangueras(sesionCloud, new FiltroMangueras()
                {
                    Estacion = new Adicional.Entidades.Estacion()
                        {
                            TipoDispensario = sesionCloud.EstacionActual.Dispensario
                        }
                });

            mangueras.ForEach(p => p.noEstacion = sesionCloud.EstacionActual.NoEstacion);

            HttpContext.Current.Cache.Insert(string.Format("{0}_mangueras", sesionCloud.EstacionActual.NoEstacion), mangueras, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);

            this.Dispensarios.AddRange(from i in mangueras
                                       group i by i.posicion into g
                                       let one = mangueras.FirstOrDefault(p => p.posicion == g.Key)
                                       select new Dispensarios()
                                       {
                                           id = one.id,
                                           noEstacion = sesionCloud.EstacionActual.NoEstacion,
                                           nombre = one.nombre,
                                           posicion = one.posicion,
                                           dispensario = one.dispensario,
                                           valor = one.valor
                                       });
        }

        private void ObtenerPorcentajeMaximo(MarcaDispensario marca)
        {
            switch (marca)
            {
                case MarcaDispensario.Wayne:
                case MarcaDispensario.Gilbarco:
                    this.PorcentajeMaximo = 9D;
                    this.Decimales = 0;
                    break;
                case MarcaDispensario.HongYang:
                case MarcaDispensario.Bennett:
                    this.PorcentajeMaximo = 9.99D;
                    this.Decimales = 2;
                    break;
                case MarcaDispensario.Team:
                    this.PorcentajeMaximo = 10D;
                    this.Decimales = 0;
                    break;
                default:
                case MarcaDispensario.Ninguno:
                    this.PorcentajeMaximo = 0D;
                    this.Decimales = 0;
                    break;
            }
        }
    }
}
