using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Enlaces;
using FirebirdSql.Data.FirebirdClient;
using Servicios.Adicional;
using System.Collections.Generic;

namespace AdicionalWeb.Persistencia
{
    public class LicenciaPersistencia
    {
        public bool ValidarLicenciaWeb(int estacion, ref string msj)
        {
            ChannelFactory<IServiciosAdicional> cfAdicional = new ChannelFactory<IServiciosAdicional>("epAdicional");
            Servicios.Adicional.IServiciosAdicional srvAdi = cfAdicional.CreateChannel(new EndpointAddress(string.Format("net.tcp://{0}/ServiciosCliente", EstacionesAdicionalPersistencia.ListaEstaciones[estacion].IpServicios)));

            try
            {
                var licencia = srvAdi.LicenciaObtener(Adicional.Entidades.Licencia.ClaveWeb);
                if (!srvAdi.LicenciaValida(licencia, Adicional.Entidades.Licencia.Version))
                {
                    msj = "Submódulo Web no tiene licencia";
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (srvAdi != null)
                {
                    using (IClientChannel clientInstance = ((IClientChannel)srvAdi))
                    {
                        if (clientInstance.State == System.ServiceModel.CommunicationState.Faulted)
                        {
                            try { clientInstance.Abort(); }
                            catch { }
                            try { cfAdicional.Abort(); }
                            catch { }
                        }
                        else if (clientInstance.State != System.ServiceModel.CommunicationState.Closed)
                        {
                            try { clientInstance.Close(); }
                            catch { }
                            try { cfAdicional.Close(); }
                            catch { }
                        }
                    }
                }
            }

            return true;
        }

        private Conexiones _enlace;

        public LicenciaPersistencia()
        {
            this._enlace = new Conexiones();
        }

        public AdicionalWeb.Entidades.Licencia LicenciaObtener(FiltroLicencia filtro)
        {
            AdicionalWeb.Entidades.Licencia result = null;
            this._enlace.ConsolaConsulta((comm) =>
            {
                comm.CommandText = "SELECT FIRST(1) CLAVE, RAZONSOCIAL, REVISION, CLAVEACTUALIZACION, FECHAACTUALIZACION FROM DGENEMPR";
                using (FbDataReader reader = comm.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        result = this.ReaderToEntidad(reader);
                    }
                }

            }, new FiltroEstaciones() { Clave = filtro.Clave });
            return result;
        }

        public bool LicenciaValida(AdicionalWeb.Entidades.Licencia licencia)
        {
            if (AdicionalWeb.Entidades.Licencia.FechaLiberacion >= licencia.FechaActualizacion)
            {
                return false;
            }

            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Bin\LibsDelphi.exe");
                    p.StartInfo.Arguments = licencia.ToString();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;

                    p.Start();
                    p.WaitForExit();

                    string r = p.StandardOutput.ReadToEnd();
                    return string.IsNullOrEmpty(r) ? false : r.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Last().Equals("1");
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool LicenciaValidaHost(string Sist, int estacion)
        {
            var variables = ObtenerVariablesEstacion(estacion);
            //"CVL5"
            string valor = string.Empty;
            string version = Adicional.Entidades.Licencia.Version;

            Adicional.Entidades.Licencia lic = new Adicional.Entidades.Licencia();
            lic.Razon_social = this.ObtenerRazonSocial(estacion);
            lic.Sistema = Sist;
            bool temporal = false;

            if (lic.Sistema == Adicional.Entidades.Licencia.ClabeAutor)
            {
                temporal = variables.TryGetValue("Adicional41FechaVence", out valor);

                lic.Modulo = Adicional.Entidades.Licencia.ClabeAutor;
                lic.TipoLicencia = "Abierta";
                lic.ClaveAutor = (variables.TryGetValue("Adicional41Lic", out valor) ? valor : string.Empty).Trim();
                lic.Usuarios = 1;
                lic.Estemporal = (temporal ? "Si" : "No").Trim();// ? "true" : "false";
                lic.Fecha_vence = (temporal ? valor : string.Empty).Trim();
            }
            else if (lic.Sistema == "CVLB")
            {
                temporal = variables.TryGetValue("LicenciaBennett2FechaVence", out valor);

                lic.Modulo = "CVLB";
                lic.TipoLicencia = "Abierta";
                lic.ClaveAutor = (variables.TryGetValue("LicenciaBennett2", out valor) ? valor : string.Empty).Trim();
                lic.Usuarios = 1;
                lic.Estemporal = (temporal ? "Si" : "No").Trim();// ? "true" : "false";
                lic.Fecha_vence = (temporal ? valor : string.Empty).Trim();
            }
            else
            {
                var licAux = ObtenerLicenciaConsola(estacion, Adicional.Entidades.Licencia.ClaveWeb);
                lic.ClaveAutor = licAux.ClaveAutor.Trim();
                lic.Estemporal = (lic.Estemporal.Equals("Si", StringComparison.CurrentCultureIgnoreCase) ? "Si" : "No").Trim();// ? "true" : "false";
                lic.Fecha_vence = lic.Fecha_vence.Trim();
                lic.Usuarios = 1;
            }

            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Bin\LibsDelphi.exe");
                    p.StartInfo.Arguments = string.Format("licenciavalidacvol {0}", string.Format("\"{0}|{1}|3.1|Abierta|{2}|{3}|{4}|{5}\"",
                                                                                         lic.Razon_social,
                                                                                         lic.Modulo,
                                                                                         lic.ClaveAutor,
                                                                                         lic.Usuarios,
                                                                                         (lic.Estemporal.Equals("Si", System.StringComparison.CurrentCultureIgnoreCase) ? "True" : "False"),
                                                                                         lic.Fecha_vence));//, lic.ToString(lic.Modulo).Trim()).Trim();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;

                    p.Start();
                    p.WaitForExit();

                    string r = p.StandardOutput.ReadToEnd();
                    return string.IsNullOrEmpty(r) ? false : r.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Last().Trim().Equals("Valido", StringComparison.CurrentCultureIgnoreCase);
                }
                catch
                {
                    return false;
                }
            }
        }

        private AdicionalWeb.Entidades.Licencia ReaderToEntidad(FbDataReader reader)
        {
            AdicionalWeb.Entidades.Licencia licencia = new AdicionalWeb.Entidades.Licencia();
            {
                licencia.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                licencia.RazonSocial = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                licencia.Revision = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                licencia.ClaveActualizacion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                licencia.FechaActualizacion = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4);
            }
            return licencia;
        }

        private Adicional.Entidades.Licencia ObtenerLicenciaConsola(int estacion, string mod)
        {
            Adicional.Entidades.Licencia lic = new Adicional.Entidades.Licencia();
            //string rSocial = string.Empty;
            string tLic = "1 Usuarios";
            int usrs = 1;

            this._enlace.ConsolaConsulta((comm) =>
                {
                    comm.CommandText = "SELECT FIRST 1 * FROM DPVGCONF";

                    using (FbDataReader reader = comm.ExecuteReader())
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                lic.Modulo = mod;
                                lic.Razon_social = reader["RazonSocial"].ToString(); //rSocial = reader["RazonSocial"].ToString();
                                lic.TipoLicencia = tLic;
                                lic.Usuarios = usrs;
                                //lic.Estemporal = esTmp;
                                //lic.Fecha_vence = fechaVence;
                                lic.ClaveAutor = reader["Licencia2"].ToString().Trim();
                            }
                        }
                        finally
                        {
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                        }
                    }
                }, new FiltroEstaciones() { Clave = estacion });
            return lic;
        }

        private Dictionary<string, string> ObtenerVariablesEstacion(int estacion)
        {
            string consola = string.Empty;

            this._enlace.ConsolaConsulta((comm) =>
            {
                comm.CommandText = "SELECT CONSOLA FROM DPVGESTS";

                using (FbDataReader reader = comm.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            consola = (reader["CONSOLA"] ?? "").ToString();
                        }
                    }
                    finally
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                }
            }, new FiltroEstaciones() { Clave = estacion });

            string[] listaVar = consola.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> variables = (new List<string>(listaVar)).ToDictionary(key => key.Split('=')[0], value => (value.Split('=').Length > 1 ? value.Split('=')[1] : ""));
            return variables;
        }

        private string ObtenerRazonSocial(int estacion)
        {
            string result = string.Empty;

            this._enlace.ConsolaConsulta((comm) =>
            {
                comm.CommandText = "SELECT FIRST 1 RAZONSOCIAL FROM DGENEMPR";

                using (FbDataReader reader = comm.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            result = (reader["RAZONSOCIAL"] ?? "").ToString();
                        }
                    }
                    finally
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                }
            }, new FiltroEstaciones() { Clave = estacion });

            return result;
        }
    }
}
