using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NuevoAdicional
{
    public partial class frmReportViewer : Form
    {
        public frmReportViewer(NuevoAdicional.Reportes.rptRep01 reporte01, string titulo)
            : this(titulo)
        {
            this.printControl1.PrintingSystem = reporte01.PrintingSystem;
            reporte01.CreateDocument();
        }

        public frmReportViewer(NuevoAdicional.Reportes.rptRep01Detallado reporte01, string titulo)
            : this(titulo)
        {
            this.printControl1.PrintingSystem = reporte01.PrintingSystem;
            reporte01.CreateDocument();
        }

        public frmReportViewer(NuevoAdicional.Reportes.rptRep02 reporte02, string titulo)
            : this(titulo)
        {
            this.printControl1.PrintingSystem = reporte02.PrintingSystem;
            reporte02.CreateDocument();
        }

        public frmReportViewer(string titulo)
        {
            InitializeComponent();

            this.Text = titulo;
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
        }
    }
}
