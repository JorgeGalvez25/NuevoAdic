using System;
using ImagenSoft.Extensiones;
using System.Linq;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Enlaces;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace AdicionalWeb.Persistencia
{
    public class EstacionesPersistencia
    {
        private Conexiones _enlaces;

        public EstacionesPersistencia()
        {
            this._enlaces = new Conexiones();
        }

        public Estaciones EstacionObtener(FiltroEstaciones filtro)
        {
            Estaciones resultado = null;

            this._enlaces.GasolineraConsulta((cmd) =>
                {
                    cmd.CommandText = "SELECT CLAVE, " +
                                            " NOMBRE, " +
                                            " CONSOLA " +
                                       " FROM DGASESTS " +
                                      " WHERE (CLAVE = @CLAVE  OR @CLAVE = 0) " +
                                        " AND (NOMBRE = @NOMBRE OR @NOMBRE = CAST('' AS VARCHAR(10))) ";
                    cmd.Parameters.AddRange(this.ParametrosConsulta(filtro));

                    using (FbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                resultado = new Estaciones();
                                resultado.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                resultado.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                resultado.Consola = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
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
                });

            return resultado;
        }

        public ListaEstaciones EstacionObtenerTodos(FiltroEstaciones filtro)
        {
            ListaEstaciones resultado = new ListaEstaciones();

            this._enlaces.GasolineraConsulta((cmd) =>
            {
                cmd.CommandText = "SELECT CLAVE, " +
                                        " NOMBRE, " +
                                        " CONSOLA " +
                                   " FROM DGASESTS " +
                                  " WHERE (CLAVE = @CLAVE  OR @CLAVE = 0) " +
                                    " AND (NOMBRE = @NOMBRE OR @NOMBRE = CAST('' AS VARCHAR(10))) ";
                cmd.Parameters.AddRange(this.ParametrosConsulta(filtro));

                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        Estaciones enridad = null;
                        while (reader.Read())
                        {
                            enridad = new Estaciones();
                            enridad.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            enridad.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            enridad.Consola = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            resultado.Add(enridad);
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
            });

            return resultado;
        }

        public string EstacionObtenerConsola(FiltroEstaciones filtro)
        {
            string resultado = string.Empty;

            Estaciones estacion = EstacionObtener(filtro);

            if (estacion != null)
            {
                string[] splt = estacion.Consola.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var items = splt.Where(p => p.Trim().StartsWith(filtro.ClaveConsola, StringComparison.CurrentCultureIgnoreCase)).ToList();
                if (items != null && items.Count > 0)
                {
                    var spltChar = new char[] { '=' };
                    foreach (var p in items)
                    {
                        splt = p.Split(spltChar, StringSplitOptions.RemoveEmptyEntries);
                        if ((splt.Length == 2) && (splt[0].Trim().Equals(filtro.ClaveConsola.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                        {
                            resultado = splt[1].Trim();
                            break;
                        }
                    };
                }
            }

            return resultado;
        }

        private FbParameter[] ParametrosConsulta(FiltroEstaciones filtro)
        {
            FbParameter[] parameters = new FbParameter[]
                {
                    new FbParameter("@CLAVE", FbDbType.Integer) { Value = filtro.Clave },
                    new FbParameter("@NOMBRE", FbDbType.VarChar, 40) { Value = filtro.Nombre }
                };

            return parameters;
        }

        public ImagenSoft.ServiciosWeb.Entidades.Web.ListaEstaciones EstacionAdicionalWebObtenerTodos(ImagenSoft.ServiciosWeb.Entidades.Web.FiltroEstacion filtro)
        {
            ImagenSoft.ServiciosWeb.Entidades.Sesion s = new ImagenSoft.ServiciosWeb.Entidades.Sesion() { Empresa = new ImagenSoft.Framework.Entidades.Empresa() { Id = 1 } };
            ImagenSoft.ServiciosWeb.Proveedor.Publicador.ServiciosProveedorAdicionalWeb servicio = new ImagenSoft.ServiciosWeb.Proveedor.Publicador.ServiciosProveedorAdicionalWeb(s, ImagenSoft.ServiciosWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

            return servicio.AdicionalWebObtenerEstacionesTodosFiltro(s, new ImagenSoft.ServiciosWeb.Entidades.Web.FiltroEstacion() { NoEstacion = filtro.NoEstacion, Matriz = filtro.Matriz });
        }
    }
}
