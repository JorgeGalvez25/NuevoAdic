using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using AdicionalWeb.Extesiones;

namespace AdicionalWeb.Code
{
    public class ServicioReportes
    {
        private static string _templatePath;
        private static string templatePath
        {
            get
            {
                if (string.IsNullOrEmpty(_templatePath))
                {
                    _templatePath = AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\template\reportes");
                }
                return _templatePath;
            }
        }

        private bool isGrantAccessDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            if (!dir.Exists)
            {
                return false;
            }

            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.AccessControl.AuthorizationRuleCollection rules = Directory.GetAccessControl(dir.FullName)
                                                                                           .GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

                foreach (System.Security.AccessControl.FileSystemAccessRule rule in rules)
                {
                    if (identity.Groups.Contains(rule.IdentityReference))
                    {
                        if (((System.Security.AccessControl.FileSystemRights.Write & rule.FileSystemRights) == System.Security.AccessControl.FileSystemRights.Write) &&
                            ((System.Security.AccessControl.FileSystemRights.Traverse & rule.FileSystemRights) == System.Security.AccessControl.FileSystemRights.Traverse))
                        {
                            if (rule.AccessControlType == System.Security.AccessControl.AccessControlType.Allow)
                                return true;
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        public void InicializarConfiguraciones(ref TemplateConfiguration config, IDictionary<string, object> data)
        {
            config.Date = DateTime.Now;

            switch (config.Id)
            {
                case TemplateConstants.Reportes.VENTAS_COMBUSTIBLE:
                    config.Date = DateTime.Parse(data["fecha"].ToString());
                    config.TemplatePage = AdicionalUtils.CombinePaths(templatePath, TemplateConstants.TEMPLATE_VENTAS_COMBUSTIBLE);

                    var jsonData = data["tblData"].ToString().FromJSON<List<ReporteVentasCombustible>>();
                    config.TableData = getFormattedTableVentasCombustible(jsonData);
                    config.TableFooter = getFormattedTableVentasCombustibleFotter(jsonData);
                    break;
            }
        }

        public bool ValidarRuta(string path, out string message)
        {
            message = string.Empty;
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                message = string.Format("No existe la ruta '{0}'", path);
                return false;
            }

            if (!isGrantAccessDir(dirInfo.FullName))
            {
                message = string.Format("No se puede generar el archivo.", dirInfo.FullName);
                return false;
            }

            return true;
        }

        public string GenerarPDF(TemplateConfiguration config)
        {
            WebHtmlToPdf html2PDF = new WebHtmlToPdf();
            return Path.GetFileName(html2PDF.HtmlToPdf(config));
        }

        #region Reporte Ventas Combustible

        private const string footerReporteVentasCombustible = "<tr>" +
                                                                "<th>&nbsp;</th>" +
                                                                "<th style=\"text-align: right\">{0:N2}</th>" +
                                                                "<th style=\"text-align: right\">{1:N2}</th>" +
                                                              "</tr>";

        private const string cuerpoReporteVentasCombustible = "<tr>" +
                                                                "<td>{0}</td>" +
                                                                "<td style=\"text-align: right\">{1:N2}</td>" +
                                                                "<td style=\"text-align: right\">{2:N2}</td>" +
                                                              "</tr>";

        private string getFormattedTableVentasCombustibleFotter(List<ReporteVentasCombustible> jsonData)
        {
            return string.Format(footerReporteVentasCombustible, jsonData.Sum(p => p.Volumen), jsonData.Sum(p => p.Importe));
        }

        private string getFormattedTableVentasCombustible(List<ReporteVentasCombustible> jsonData)
        {
            StringBuilder sb = new StringBuilder();
            jsonData.ForEach(p =>
            {
                sb.AppendFormat(cuerpoReporteVentasCombustible, p.Descripcion, p.Volumen, p.Importe);
            });
            return sb.ToString();
        }

        #endregion
    }
}
