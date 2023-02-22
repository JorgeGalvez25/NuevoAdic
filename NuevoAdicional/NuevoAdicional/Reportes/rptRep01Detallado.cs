using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace NuevoAdicional.Reportes
{
    public partial class rptRep01Detallado : DevExpress.XtraReports.UI.XtraReport
    {
        public rptRep01Detallado(List<Adicional.Entidades.ReporteAjuste> source, string NombreEstacion, string fecha)
        {
            int fechas = (from r in source
                          group r by r.Fecha into g
                          select new { Fecha = g, Registros = g }).ToArray().Length;
            InitializeComponent();

            this.DataSource = source;

            // Etiquetas
            lblNombreEstacion.Text = NombreEstacion;
            lblFecha.Text = fecha;

            // GroupHeader
            this.ghFecha.GroupFields.Add(new GroupField("Fecha"));
            this.cellFecha.DataBindings.Add("Text", this.DataSource, "Fecha", "{0:dd/MM/yyyy}");
            this.ghCorte.GroupFields.Add(new GroupField("Corte"));
            this.cellCorte.DataBindings.Add("Text", this.DataSource, "Corte", "{0:00}");
            ghFecha.Visible = fechas > 1;

            // Detail
            this.cellComb.DataBindings.Add("Text", this.DataSource, "NombreCombustible");
            this.cellSalidas.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.000}");
            this.cellAjuste.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.000}");
            this.cellDiferencia.DataBindings.Add("Text", this.DataSource, "Diferencia", "{0:#,#0.000}");
            this.cellPrecio.DataBindings.Add("Text", this.DataSource, "Precio", "{0:#,#0.000}");
            this.cellImporte.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.000}");

            // GroupFooter
            XRSummary sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellSalidasTotal1.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellSalidasTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjusteTotal1.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjusteTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDiferenciaTotal1.DataBindings.Add("Text", this.DataSource, "Diferencia", "{0:#,#0.00}");
            this.cellDiferenciaTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImporteTotal1.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellImporteTotal1.Summary = sumaryFooter;

            // ReportFooter
            XRSummary sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellSalidasTotal2.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellSalidasTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjusteTotal2.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjusteTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDiferenciaTotal2.DataBindings.Add("Text", this.DataSource, "Diferencia", "{0:#,#0.00}");
            this.cellDiferenciaTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImporteTotal2.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellImporteTotal2.Summary = sumaryReport;
        }

    }
}
