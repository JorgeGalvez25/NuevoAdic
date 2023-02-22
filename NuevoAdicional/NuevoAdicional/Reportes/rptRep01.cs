using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace NuevoAdicional.Reportes
{
    public partial class rptRep01 : DevExpress.XtraReports.UI.XtraReport
    {
        private Dictionary<string, double> acumDiferenciaVentas = new Dictionary<string, double>();

        public rptRep01(List<Adicional.Entidades.ReporteAjuste> source, string NombreEstacion, string fecha)
        {
            List<Adicional.Entidades.ReporteAjuste> Resumen = (from r in source
                                                               group r by r.Combustible into g
                                                               let f = g.First(p => { return true; })
                                                               select new Adicional.Entidades.ReporteAjuste
                                                               {
                                                                   Combustible = g.Key,
                                                                   InvInicial = f.InvInicial,
                                                                   Entradas = g.Sum(s => s.Entradas),
                                                                   EntradasMermas = g.Sum(s => s.EntradasMermas),
                                                                   EntradasFisicas = g.Sum(s => s.EntradasFisicas),
                                                                   InvFinal = g.Last(p => { return true; }).InvFinal,
                                                                   SalidaDispensarios = g.Sum(s => s.SalidaDispensarios),
                                                                   Jarreos = g.Sum(s => s.Jarreos),
                                                                   Ventas = g.Sum(s => s.Ventas),
                                                                   Precio = g.Sum(s => s.Precio) / g.Count(p => { return true; }),
                                                                   Ajuste = g.Sum(s => s.Ajuste),
                                                                   PorcMerma = f.PorcMerma,
                                                                   Diferencia = g.Sum(s => s.Diferencia),
                                                                   NombreCombustible = f.NombreCombustible,
                                                               }).ToList();
            double[] importes = new double[] {
                source.Where(s => s.Combustible == 1).Sum(s => s.ImporteAntes),//importeAntesMagna
                source.Where(s => s.Combustible == 2).Sum(s => s.ImporteAntes),//importeAntesPremuim
                source.Where(s => s.Combustible == 3).Sum(s => s.ImporteAntes),//importeAntesDiesel

                source.Where(s => s.Combustible == 1).Sum(s => s.ImporteDespues),//importeDespuesMagna
                source.Where(s => s.Combustible == 2).Sum(s => s.ImporteDespues),//importeDespuesPremuim
                source.Where(s => s.Combustible == 3).Sum(s => s.ImporteDespues),//importeDespuesDiesel

                source.Where(s => s.Combustible == 1).Sum(s => s.ImporteAjuste),//importeAjusteMagna
                source.Where(s => s.Combustible == 2).Sum(s => s.ImporteAjuste),//importeAjustePremuim
                source.Where(s => s.Combustible == 3).Sum(s => s.ImporteAjuste),//importeAjusteDiesel

                source.Where(s => s.Combustible == 1).Sum(s => s.ImporteMerma),//importeMermaMagna
                source.Where(s => s.Combustible == 2).Sum(s => s.ImporteMerma),//importeMermaPremuim
                source.Where(s => s.Combustible == 3).Sum(s => s.ImporteMerma)};//importeMermaDiesel

            InitializeComponent();

            this.DataSource = source;

            // Etiquetas
            lblNombreEstacion.Text = NombreEstacion;
            lblFecha.Text = fecha;

            // GroupHeader
            this.ghFecha.GroupFields.Add(new GroupField("Fecha"));
            this.cellFecha.DataBindings.Add("Text", this.DataSource, "Fecha", "{0:dd/MM/yyyy}");

            // Detail
            this.cellComb.DataBindings.Add("Text", this.DataSource, "NombreCombustible");
            this.cellInvIni.DataBindings.Add("Text", this.DataSource, "InvInicial", "{0:#,#0.00}");
            this.cellFactura.DataBindings.Add("Text", this.DataSource, "Entradas", "{0:#,#0.00}");
            this.cellMerma.DataBindings.Add("Text", this.DataSource, "EntradasMermas", "{0:#,#0.00}");
            this.cellFisica.DataBindings.Add("Text", this.DataSource, "EntradasFisicas", "{0:#,#0.00}");
            this.cellInvFin.DataBindings.Add("Text", this.DataSource, "InvFinal", "{0:#,#0.00}");
            this.cellTanque.DataBindings.Add("Text", this.DataSource, "SalidaTanques", "{0:#,#0.00}");
            this.cellDispensario.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellJarreos.DataBindings.Add("Text", this.DataSource, "Jarreos", "{0:#,#0.00}");
            this.cellLtsAntes.DataBindings.Add("Text", this.DataSource, "VentasAntes", "{0:#,#0.00}");
            this.cellLtsDespues.DataBindings.Add("Text", this.DataSource, "VentasDespues", "{0:#,#0.00}");
            this.cellImpAntes.DataBindings.Add("Text", this.DataSource, "ImporteAntes", "{0:#,#0.00}");
            this.cellImpDespues.DataBindings.Add("Text", this.DataSource, "ImporteDespues", "{0:#,#0.00}");
            this.cellPrecio.DataBindings.Add("Text", this.DataSource, "Precio", "{0:#,#0.00}");
            this.cellAjuLitros.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjuImporte.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellAjuPor.DataBindings.Add("Text", this.DataSource, "PorcentajeAjuste", "{0:#,#0.00}");
            this.CellMerPor.DataBindings.Add("Text", this.DataSource, "PorcentajeMerma", "{0:#,#0.00}");
            this.cellMerLitros.DataBindings.Add("Text", this.DataSource, "LitrosMerma", "{0:#,#0.00}");
            this.cellMerImporte.DataBindings.Add("Text", this.DataSource, "ImporteMerma", "{0:#,#0.00}");
            this.cellDif.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");
            this.cellAcumDif.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");

            // GroupFooter
            XRSummary sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDispTotal1.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellDispTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsAntesTotal1.DataBindings.Add("Text", this.DataSource, "VentasAntes", "{0:#,#0.00}");
            this.cellLtsAntesTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsDespuesTotal1.DataBindings.Add("Text", this.DataSource, "VentasDespues", "{0:#,#0.00}");
            this.cellLtsDespuesTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpAntesTotal1.DataBindings.Add("Text", this.DataSource, "ImporteAntes", "{0:#,#0.00}");
            this.cellImpAntesTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpDespuesTotal1.DataBindings.Add("Text", this.DataSource, "ImporteDespues", "{0:#,#0.00}");
            this.cellImpDespuesTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuLitrosTotal1.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjuLitrosTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuImporteTotal1.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellAjuImporteTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerLitrosTotal1.DataBindings.Add("Text", this.DataSource, "LitrosMerma", "{0:#,#0.00}");
            this.cellMerLitrosTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerImpTotal1.DataBindings.Add("Text", this.DataSource, "ImporteMerma", "{0:#,#0.00}");
            this.cellMerImpTotal1.Summary = sumaryFooter;
            sumaryFooter = new XRSummary(SummaryRunning.Group, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDifTotal1.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");
            this.cellDifTotal1.Summary = sumaryFooter;
            this.cellAcumDifTotal1.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");

            // ReportFooter
            XRSummary sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDispTotal2.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellDispTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsAntesTotal2.DataBindings.Add("Text", this.DataSource, "VentasAntes", "{0:#,#0.00}");
            this.cellLtsAntesTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsDespuesTotal2.DataBindings.Add("Text", this.DataSource, "VentasDespues", "{0:#,#0.00}");
            this.cellLtsDespuesTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpAntesTotal2.DataBindings.Add("Text", this.DataSource, "ImporteAntes", "{0:#,#0.00}");
            this.cellImpAntesTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpDespuesTotal2.DataBindings.Add("Text", this.DataSource, "ImporteDespues", "{0:#,#0.00}");
            this.cellImpDespuesTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuLtsTotal2.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjuLtsTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuImpTotal2.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellAjuImpTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerLitrosTotal2.DataBindings.Add("Text", this.DataSource, "LitrosMerma", "{0:#,#0.00}");
            this.cellMerLitrosTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerImpTotal2.DataBindings.Add("Text", this.DataSource, "ImporteMerma", "{0:#,#0.00}");
            this.cellMerImpTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDifTotal2.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");
            this.cellDifTotal2.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAcumDifTotal2.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");
            this.cellAcumDifTotal2.Summary = sumaryReport;

            // SubReport
            rptRep01Resumen reporteResumen = new rptRep01Resumen(Resumen, importes);
            this.xrsubResumen.ReportSource = reporteResumen;

            // Eventos de las celdas
            this.cellAcumDif.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAcumDif_BeforePrint);
            this.cellAcumDifTotal1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAcumDifTotal1_BeforePrint);
            this.cellAcumDifTotal2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAcumDifTotal2_BeforePrint);
        }

        void cellAcumDif_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Adicional.Entidades.ReporteAjuste row = this.GetCurrentRow() as Adicional.Entidades.ReporteAjuste;

            if (acumDiferenciaVentas.ContainsKey(row.NombreCombustible))
            {
                this.acumDiferenciaVentas[row.NombreCombustible] += row.DiferenciaVentas;
            }
            else
            {
                this.acumDiferenciaVentas.Add(row.NombreCombustible, row.DiferenciaVentas);
            }

            this.cellAcumDif.Text = acumDiferenciaVentas[row.NombreCombustible].ToString("#,#0.00");
        }

        void cellAcumDifTotal1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            double AcumTotal = 0D;

            foreach (var item in acumDiferenciaVentas)
            {
                AcumTotal += item.Value;
            }

            cellAcumDifTotal1.Text = AcumTotal.ToString("#,#0.00");
        }

        void cellAcumDifTotal2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            double AcumTotal = 0D;

            foreach (var item in acumDiferenciaVentas)
            {
                AcumTotal += item.Value;
            }

            cellAcumDifTotal2.Text = AcumTotal.ToString("#,#0.00");
        }
    }
}
