using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace NuevoAdicional.Reportes
{
    public partial class rptRep02 : DevExpress.XtraReports.UI.XtraReport
    {
        public rptRep02(List<Adicional.Entidades.ReporteAjuste> source, string NombreEstacion, string fecha)
        {
            InitializeComponent();

            this.DataSource = source;

            // Etiquetas
            lblNombreEstacion.Text = NombreEstacion;
            lblFecha.Text = fecha;

            // Detail
            this.cellCombustible.DataBindings.Add("Text", this.DataSource, "NombreCombustible");
            this.cellVolumen.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.000}");
            this.cellImporte.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");

            // ReportFooter
            XRSummary summary = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.000}");
            this.cellVolumenTotal.DataBindings.Add("Text", this.DataSource, "Ajuste", "{0:#,#0.000}");
            this.cellVolumenTotal.Summary = summary;
            summary = new XRSummary(SummaryRunning.Report, SummaryFunc.RunningSum, "{0:#,#0.00}");
            this.cellImporteTotal.DataBindings.Add("Text", this.DataSource, "ImporteAjuste", "{0:#,#0.00}");
            this.cellImporteTotal.Summary = summary;
        }

    }
}
