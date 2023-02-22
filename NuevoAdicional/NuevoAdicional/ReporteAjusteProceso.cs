using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NuevoAdicional
{
    public class ReporteAjusteProceso
    {
        private string[] meses = new string[] { "", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };
        private DataSetReportes dataSetReportes = new DataSetReportes();
        private DataSetReportes dataSetResumen = new DataSetReportes();

        delegate void MethodDelegate();

        public void ReporteDeAjuste01(int estacion, string nombreEstacion, DateTime fechaIni, DateTime fechaFin,
                                      bool de6a6, bool incluirEntradasFisicas, bool incluirNombreEstacion, bool detallado)
        {
            frmAvance formaAvance = null;

            try
            {
                ServiciosCliente.IServiciosCliente channel = Configuraciones.ListaCanales[estacion];
                DateTime fecha = fechaIni.Date;
                StringBuilder cadenaAuxiliar = new StringBuilder();
                double porcMerma = 0D;
                Dictionary<int, double> DifAcumulada = new Dictionary<int, double>();
                int dias = 0;
                List<Adicional.Entidades.ReporteAjuste> reporteAjuste = new List<Adicional.Entidades.ReporteAjuste>();

                cadenaAuxiliar.Append("Reporte 01, Fecha: ");
                cadenaAuxiliar.Append(RangoFechas(fechaIni, fechaFin));

                fechaIni = fechaIni.Date;
                fechaFin = fechaFin.Date;
                dias = (int)fechaFin.Subtract(fechaIni).TotalDays + 1;

                Cursor.Current = Cursors.WaitCursor;
                formaAvance = new frmAvance("Calculando Reporte", dias);
                MethodDelegate async = new MethodDelegate(() =>
                    {
                        while (fecha <= fechaFin)
                        {
                            List<Adicional.Entidades.ReporteAjuste> dia = null;

                            if (de6a6)
                            {
                                dia = channel.ObtenerReporte6a6(fecha);
                            }
                            else
                            {
                                if (detallado)
                                {
                                    dia = channel.ObtenerReporteDetallado(fecha);
                                }
                                else
                                {
                                    dia = channel.ObtenerReporteAjuste(fecha);
                                }
                            }

                            formaAvance.Avanzar();
                            Cursor.Current = Cursors.WaitCursor;

                            fecha = fecha.AddDays(1);
                            reporteAjuste.AddRange(dia);
                        }
                        if (reporteAjuste.Count > 0)
                        {
                            porcMerma = reporteAjuste[0].PorcMerma;
                            cadenaAuxiliar.Append("  % Merma: 0.0074");
                            //cadenaAuxiliar.Append(porcMerma.ToString());
                        }
                    });
                var iasync = async.BeginInvoke(delegate
                    {
                        Cursor.Current = Cursors.Default;
                        formaAvance.Close();
                    }, null);
                formaAvance.ShowDialog();
                async.EndInvoke(iasync);
                if (reporteAjuste.Count > 0)
                {
                    frmReportViewer reportViewer = null;

                    if (!de6a6 && detallado)
                    {
                        NuevoAdicional.Reportes.rptRep01Detallado reporte = new NuevoAdicional.Reportes.rptRep01Detallado(reporteAjuste,
                                                                                                        incluirNombreEstacion ? nombreEstacion : string.Empty,
                                                                                                        cadenaAuxiliar.ToString());
                        reportViewer = new frmReportViewer(reporte, "Reporte 01");
                    }
                    else
                    {
                        NuevoAdicional.Reportes.rptRep01 reporte = new NuevoAdicional.Reportes.rptRep01(reporteAjuste,
                                                                                                        incluirNombreEstacion ? nombreEstacion : string.Empty,
                                                                                                        cadenaAuxiliar.ToString());
                        reportViewer = new frmReportViewer(reporte, "Reporte 01");
                    }
                    reportViewer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                if (formaAvance != null)
                {
                    formaAvance.Close();
                    formaAvance.Dispose();
                }
                MessageBox.Show("Ha ocurrido un error al procesar el archivo. Intente de nuevo, si el problema persiste, comuníquese con su encargado de sistemas\n( " + ex.Message + " ).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Configuraciones.AbrirCanalAdicional(estacion);
            }
        }

        public void ReporteDeAjuste02(int estacion, string nombreEstacion, DateTime fecha,
                                      bool a24hrs, bool incluirNombreEstacion)
        {
            frmAvance formaAvance = null;

            try
            {
                ServiciosCliente.IServiciosCliente channel = Configuraciones.ListaCanales[estacion];
                StringBuilder cadenaAuxiliar = new StringBuilder();
                Dictionary<int, double> DifAcumulada = new Dictionary<int, double>();
                List<Adicional.Entidades.ReporteAjuste> reporteAjuste = new List<Adicional.Entidades.ReporteAjuste>();

                cadenaAuxiliar.Append("Reporte 02, Fecha: ");
                cadenaAuxiliar.Append(FechaCompleta(fecha));

                Cursor.Current = Cursors.WaitCursor;
                formaAvance = new frmAvance("Calculando Reporte", 3);
                formaAvance.Show();

                for (int i = 1; i <= 3; i++)
                {
                    reporteAjuste.Add(channel.ObtenerReporte2(fecha, i, a24hrs));

                    formaAvance.Avanzar();
                    Cursor.Current = Cursors.WaitCursor;
                }


                Cursor.Current = Cursors.Default;

                frmReportViewer reportViewer = null;

                NuevoAdicional.Reportes.rptRep02 reporte = new NuevoAdicional.Reportes.rptRep02(reporteAjuste,
                                                                                                incluirNombreEstacion ? nombreEstacion : string.Empty,
                                                                                                cadenaAuxiliar.ToString());
                reportViewer = new frmReportViewer(reporte, "Reporte 02");
                reportViewer.ShowDialog();
            }
            catch (Exception)
            {
                formaAvance.Close();
                formaAvance.Dispose();
                MessageBox.Show("Ha ocurrido un error al procesar el archivo. Intente de nuevo, si el problema persiste, comuníquese con su encargado de sistemas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Configuraciones.AbrirCanalAdicional(estacion);
            }
        }

        private string RangoFechas(DateTime fechaIni, DateTime fechaFin)
        {
            string resultado = string.Empty;

            if (fechaIni == fechaFin)
            {
                resultado = string.Format("Al {0}", fechaIni.ToString("dd/MM/yyyy"));
            }
            else if (fechaFin.Month == fechaIni.Month && fechaFin.Year == fechaIni.Year)
            {
                resultado = string.Format("Del {0} al {1} de {2} de {3}",
                                        fechaIni.Day.ToString("00"),
                                        fechaFin.Day.ToString("00"),
                                        meses[fechaIni.Month],
                                        fechaIni.Year);
            }
            else if (fechaFin.Month != fechaIni.Month && fechaFin.Year == fechaIni.Year)
            {
                resultado = string.Format("Del {0} de {1} al {2} de {3} de {4}",
                                          fechaIni.Day.ToString("00"),
                                          meses[fechaIni.Month],
                                          fechaFin.Day.ToString("00"),
                                          meses[fechaFin.Month],
                                          fechaIni.Year);
            }
            else
            {
                resultado = string.Format("Del {0} de {1} de {2} al {3} de {4} de {5}",
                                          fechaIni.Day.ToString("00"),
                                          meses[fechaIni.Month],
                                          fechaIni.Year,
                                          fechaFin.Day.ToString("00"),
                                          meses[fechaFin.Month],
                                          fechaFin.Year);
            }

            return resultado;
        }

        private string FechaCompleta(DateTime fecha)
        {
            string diaSemana = string.Empty;
            StringBuilder resultado = new StringBuilder();

            switch (fecha.DayOfWeek)
            {
                case DayOfWeek.Friday: diaSemana = "Viernes";
                    break;
                case DayOfWeek.Monday: diaSemana = "Lunes";
                    break;
                case DayOfWeek.Saturday: diaSemana = "Sábado";
                    break;
                case DayOfWeek.Sunday: diaSemana = "Domingo";
                    break;
                case DayOfWeek.Thursday: diaSemana = "Jueves";
                    break;
                case DayOfWeek.Tuesday: diaSemana = "Martes";
                    break;
                case DayOfWeek.Wednesday: diaSemana = "Miércoles";
                    break;
                default:
                    break;
            }

            //resultado.AppendFormat("{0}, {1:dd} de {1:MMMM} de {1:yyyy HH:mm}", diaSemana, fecha);
            resultado.Append(diaSemana);
            resultado.Append(", ");
            resultado.Append(fecha.Day.ToString("00"));
            resultado.Append(" de ");
            resultado.Append(meses[fecha.Month]);
            resultado.Append(" de ");
            resultado.Append(fecha.Year.ToString("0000"));
            resultado.Append(" ");
            resultado.Append(fecha.ToString("HH:mm"));

            return resultado.ToString();
        }
    }
}
