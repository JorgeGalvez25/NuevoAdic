using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace AdicionalWeb.Code
{
    public enum Posicion
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class Mensajeria
    {
        #region ToolTips

        private const string infoToolTip = "$('[data-toggle=\"tooltip\"]').tooltip();";
        private const string staticToolTip = "$(document).ready(function() { setTimeout(function(){ $('[data-toggle=\"tooltip\"]').tooltip({ 'trigger':'manual' }).tooltip('show'); }, 1000); });";

        public static void ConfigurarToolTip(HtmlControl ctrl, string cadena)
        {
            ConfigurarToolTip(ctrl, cadena, Posicion.Left);
        }

        public static void ConfigurarToolTip(HtmlControl ctrl, string cadena, Posicion pos)
        {
            ctrl.Attributes.Add("data-toggle", "tooltip");
            ctrl.Attributes.Add("data-placement", pos.ToString().ToLower());
            ctrl.Attributes.Add("title", cadena);
        }

        public static void MostrarStaticToolTip()
        {
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "ToolTip", staticToolTip, true);
        }

        public static void MostrarToolTip()
        {
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "ToolTip", infoToolTip, true);
        }

        #endregion

        #region Modal

        private const string modal = "$(document).ready(function(){ " +
            //"$('#msjModal').on('hidden.bs.modal', function (e) {"+
            //     "if ($('#pgLoader').is(':visible')) { $('#pgLoader').hide(); }"+
            //     "$('#warnInfo').show();"+
            // "}).on('show.bs.modal', function (event) {"+
            //   "var modal = $(this);"+
            //   "modal.find('.modal-title').html('{0}');"+
            //   "modal.find('.modal-body').append($('<span>').html('{1}'));"+
            // "}).modal('show');"+
                                           "BootstrapDialog.show({ " +
                                                "title: '{0}', " +
                                                "message: '{1}', " +
                                                "type: {2}, " +
                                                "size:{3}, " +
                                                "buttons: [{ " +
                                                    "label: 'Aceptar', " +
                                                    "action: function(dialog) { " +
                                                        "dialog.close(); " +
                                                    "} " +
                                                "}] " +
                                           "});" +
                                        "});";

        public static void MostrarModalErr(string cadena)
        {
            string msj = modal.ReplaceEx("{0}", "Error")
                              .ReplaceEx("{1}", HttpContext.Current.Server.HtmlEncode(cadena))
                              .ReplaceEx("{2}", "BootstrapDialog.TYPE_DANGER")
                              .ReplaceEx("{3}", "BootstrapDialog." + GetSize(cadena));
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "MensajeError", msj, true);
        }

        public static void MostrarModalWarn(string cadena)
        {
            string msj = modal.ReplaceEx("{0}", "Advertencia")
                              .ReplaceEx("{1}", HttpContext.Current.Server.HtmlEncode(cadena))
                              .ReplaceEx("{2}", "BootstrapDialog.TYPE_WARNING")
                              .ReplaceEx("{3}", "BootstrapDialog." + GetSize(cadena)); ;
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "MensajeAdvertencia", msj, true);
        }

        public static void MostrarModalInfo(string cadena)
        {
            string msj = modal.ReplaceEx("{0}", HttpContext.Current.Server.HtmlEncode("Información"))
                              .ReplaceEx("{1}", HttpContext.Current.Server.HtmlEncode(cadena))
                              .ReplaceEx("{2}", "BootstrapDialog.TYPE_INFO")
                              .ReplaceEx("{3}", "BootstrapDialog." + GetSize(cadena)); ;
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "MensajeInformación", msj, true);
        }

        private static string GetSize(string cadena)
        {
            int len = cadena.Length;
            if (len < 40)
            {
                return "SIZE_SMALL";
            }
            else if (len >= 40 && len < 85)
            {
                return "SIZE_NORMAL";
            }
            else if (len >= 85)
            {
                return "SIZE_WIDE";
            }

            return "SIZE_NORMAL";
        }

        #endregion
    }
}
