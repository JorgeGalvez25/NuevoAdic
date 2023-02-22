using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraBars;

namespace EstandarCliente.Infrastructure.Interface.Services
{
    public interface IServiciosMenuAplicacion
    {
        void AgregaItem(string Menu, String Texto, ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShortCut);
        void AgregaBotonMenuCatalogoGrupo(string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, string Imagen, System.Windows.Forms.Shortcut ShorCut);
        void AgregarBotonSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image ImagenGrande, System.Drawing.Image ImagenChica, System.Windows.Forms.Shortcut ShorCut);
        void AgregarBotonSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut);
        void AgregarBotonSmallSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut);
        void LimpiaMenuCatalogo();
        void HabilitarBotonSeccionMenu(string Seccion, string Grupo, string Texto, bool habilita);
        void OcultarBotonSeccionMenu(string Seccion, string Grupo, string Texto, bool Ocultar);
        void OcultaGrupoSeccionMenu(string Seccion, string Grupo);
        void OcultaSeccionMenu(string Seccion);
        void MostrarSeccionMenu(string Seccion);
        List<string> ObtenerNombresRibbone();
    }
}
