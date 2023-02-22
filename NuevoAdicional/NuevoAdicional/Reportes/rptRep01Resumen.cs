using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace NuevoAdicional.Reportes
{
    public partial class rptRep01Resumen : DevExpress.XtraReports.UI.XtraReport
    {
        private Dictionary<string, double> acumDiferenciaVentas = new Dictionary<string, double>();
        double[] importes;
        double[] totales = new double[4];

        public rptRep01Resumen(List<Adicional.Entidades.ReporteAjuste> source, double[] importes)
        {
            InitializeComponent();

            this.DataSource = source;
            this.importes = importes;

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
            XRSummary sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDispTotal1.DataBindings.Add("Text", this.DataSource, "SalidaDispensarios", "{0:#,#0.00}");
            this.cellDispTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsAntesTotal1.DataBindings.Add("Text", this.DataSource, "VentasAntes", "{0:#,#0.00}");
            this.cellLtsAntesTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellLtsDespuesTotal1.DataBindings.Add("Text", this.DataSource, "VentasDespues", "{0:#,#0.00}");
            this.cellLtsDespuesTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpAntesTotal1.DataBindings.Add("Text", this.DataSource, "ImporteAntes", "{0:#,#0.00}");
            this.cellImpAntesTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImpDespuesTotal1.DataBindings.Add("Text", this.DataSource, "ImporteDespues", "{0:#,#0.00}");
            this.cellImpDespuesTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuLitrosTotal1.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.00}");
            this.cellAjuLitrosTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellAjuImporteTotal1.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellAjuImporteTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerLitrosTotal1.DataBindings.Add("Text", this.DataSource, "LitrosMerma", "{0:#,#0.00}");
            this.cellMerLitrosTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellMerImpTotal1.DataBindings.Add("Text", this.DataSource, "ImporteMerma", "{0:#,#0.00}");
            this.cellMerImpTotal1.Summary = sumaryReport;
            sumaryReport = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellDifTotal1.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");
            this.cellDifTotal1.Summary = sumaryReport;
            this.cellAcumDifTotal1.DataBindings.Add("Text", this.DataSource, "DiferenciaVentas", "{0:#,#0.00}");

            // Eventos de las celdas
            this.cellImpAntes.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellImpAntes_BeforePrint);
            this.cellImpDespues.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellImpDespues_BeforePrint);
            this.cellAjuImporte.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAjuImporte_BeforePrint);
            this.cellMerImporte.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellMerImporte_BeforePrint);

            this.cellAcumDif.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAcumDif_BeforePrint);
            this.cellAcumDifTotal1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(cellAcumDifTotal1_BeforePrint);

            this.cellImpAntesTotal1.SummaryGetResult += new SummaryGetResultHandler(cellImpAntesTotal1_SummaryGetResult);
            this.cellImpDespuesTotal1.SummaryGetResult += new SummaryGetResultHandler(cellImpDespuesTotal1_SummaryGetResult);
            this.cellAjuImporteTotal1.SummaryGetResult += new SummaryGetResultHandler(cellAjuImporteTotal1_SummaryGetResult);
            this.cellMerImpTotal1.SummaryGetResult += new SummaryGetResultHandler(cellMerImpTotal1_SummaryGetResult);
        }

        void cellMerImpTotal1_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totales[3];
            e.Handled = true;
        }

        void cellAjuImporteTotal1_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totales[2];
            e.Handled = true;
        }

        void cellImpDespuesTotal1_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totales[1];
            e.Handled = true;
        }

        void cellImpAntesTotal1_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = totales[0];
            e.Handled = true;
        }

        void cellMerImporte_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Adicional.Entidades.ReporteAjuste row = this.GetCurrentRow() as Adicional.Entidades.ReporteAjuste;

            this.cellMerImporte.Text = importes[8 + row.Combustible].ToString("#,#0.00");
            this.totales[3] += importes[8 + row.Combustible];
        }

        void cellAjuImporte_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Adicional.Entidades.ReporteAjuste row = this.GetCurrentRow() as Adicional.Entidades.ReporteAjuste;

            this.cellAjuImporte.Text = importes[5 + row.Combustible].ToString("#,#0.00");
            this.totales[2] += importes[5 + row.Combustible];
        }

        void cellImpDespues_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Adicional.Entidades.ReporteAjuste row = this.GetCurrentRow() as Adicional.Entidades.ReporteAjuste;

            this.cellImpDespues.Text = importes[2 + row.Combustible].ToString("#,#0.00");
            this.totales[1] += importes[2 + row.Combustible];
        }

        void cellImpAntes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Adicional.Entidades.ReporteAjuste row = this.GetCurrentRow() as Adicional.Entidades.ReporteAjuste;

            this.cellImpAntes.Text = importes[-1 + row.Combustible].ToString("#,#0.00");
            this.totales[0] += importes[-1 + row.Combustible];
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
    }
}
