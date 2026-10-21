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

                // Nombre sin distinguir mayúsculas/minúsculas y sin espacios alrededor del "="
                Dictionary<string, string> variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (string linea in listaVar)
                {
                    string[] partes = linea.Split(new char[] { '=' }, 2);
                    string nombre = partes[0].Trim();
                    if (nombre != "")
                        variables.Add(nombre, partes.Length > 1 ? partes[1].Trim() : "");
                }

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

        /// <summary>
        /// Indica si la marca Gilbarco corresponde al driver Gilbarco 2W y no a PAM.
        /// </summary>
        public static bool EsGilbarco2W()
        {
            return string.Equals(ConfigurationManager.AppSettings["ServicioX"], "SQLGReader", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gilbarco con captura de porcentajes por posición y manguera.
        /// </summary>
        public static bool GilbarcoPorManguera(string tipoClb)
        {
            return ConfigurationManager.AppSettings["GilbarcoOnOff"] == "Si" || tipoClb == "5" || tipoClb == "8" ||
                   (tipoClb == "2" && EsGilbarco2W());
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
