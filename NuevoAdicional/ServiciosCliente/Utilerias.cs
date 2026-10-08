using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Adicional.Entidades;

namespace ServiciosCliente
{
    public class Utilerias
    {
        public static Dictionary<string, string> ObtenerListaVar()
        {
            try
            {
                string var;
                var = new EstacionConsPersistencia().ObtenerVariablesDispensario();
                string[] listaVar = var.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, string> variables = new Dictionary<string, string>();

                variables = (new List<string>(listaVar)).ToDictionary(key => key.Split('=')[0], value => (value.Split('=').Length > 1 ? value.Split('=')[1] : ""));

                return variables;
            }
            catch
            {
                throw new System.ArgumentException("Error al cargar variables, revise no tener variables repetidas en DPVGESTS y tener bien configirada la ruta de la base de consola.");
            }
        }

        public static string ExtraeElemStrSep(string cadena, int indice, char sep)
        {
            try
            {
                string[] result = cadena.Split(new char[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                return result[indice - 1];
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Indica si, al subir o bajar flujo, se espera con ESTADI a que el driver termine de aplicarlo
        /// antes de cambiar el estado de la estación y los servicios.
        /// Cada marca de NuevoAdic agrupa drivers con su propia numeración de TipoClb:
        /// Wayne = drivers Wayne y Wayne 2W; Gilbarco = drivers PAM y Gilbarco 2W.
        /// </summary>
        public static bool EsperaAplicacionFlujo(MarcaDispensario marca, string tipoClb)
        {
            switch (marca)
            {
                case MarcaDispensario.Wayne:
                    return ConfigurationManager.AppSettings["ModoPresetWayne"] == "Si";
                case MarcaDispensario.Gilbarco:
                    // TipoClb 6 y 7 son de PAM
                    return tipoClb == "6" || tipoClb == "7";
                default:
                    return false;
            }
        }

        public static string ObtenValorCal(string cadena, int indice, char sep)
        {
            try
            {
                if (cadena.Substring(cadena.IndexOf('&') + (indice * 3) + (indice - 1), 1) == Convert.ToString(sep))
                    return cadena.Substring(cadena.IndexOf('&') + (indice * 3) + indice, 3);
                else
                    return "000";
            }
            catch
            {
                return "000";
            }

        }
    }
}
