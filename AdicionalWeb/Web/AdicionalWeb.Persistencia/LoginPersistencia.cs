using System.Data;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Enlaces;
using FirebirdSql.Data.FirebirdClient;

namespace AdicionalWeb.Persistencia
{
    public class LoginPersistencia
    {
        private Conexiones _enlace;

        public LoginPersistencia()
        {
            this._enlace = new Conexiones();
        }

        public Sesion LoginObtener(FiltroSesion filtro)
        {
            Sesion result = null;
            this._enlace.AdicionalClienteConsulta((com) =>
                {
                    com.CommandText = @"SELECT ID, " +
                                             " NOMBRE, " +
                                             " CLAVE, " +
                                             " ACTIVO, " +
                                             " VARIABLES, " +
                                             " CORREO " +
                                        " FROM USUARIO " +
                                       " WHERE NOMBRE = @NOMBRE ";
                    com.Parameters.AddRange(this.ObtenerParametros(filtro));
                    using (FbDataReader reader = com.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                result = new Sesion();
                                result = this.ReaderToEntidad(reader);
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
            return result;
        }

        public ListaSesion LoginObtenerTodos(FiltroSesion filtro)
        {
            ListaSesion result = new ListaSesion();
            this._enlace.AdicionalClienteConsulta((com) =>
            {
                com.CommandText = @"SELECT ID, " +
                                         " NOMBRE, " +
                                         " CLAVE, " +
                                         " ACTIVO, " +
                                         " VARIABLES, " +
                                         " CORREO " +
                                    " FROM USUARIO " +
                                   " WHERE NOMBRE = @NOMBRE ";
                com.Parameters.AddRange(this.ObtenerParametros(filtro));
                using (FbDataReader reader = com.ExecuteReader(CommandBehavior.SingleRow))
                {
                    try
                    {
                        while (reader.Read())
                        {
                            result.Add(this.ReaderToEntidad(reader));
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
            return result;
        }

        private Sesion ReaderToEntidad(FbDataReader reader)
        {
            Sesion entidad = new Sesion();
            entidad.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            entidad.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            entidad.Clave = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            entidad.Activo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            entidad.Variables = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            entidad.Correo = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
            return entidad;
        }

        private FbParameter[] ObtenerParametros(FiltroSesion filtro)
        {
            FbParameter[] result = new FbParameter[]
                {
                    new FbParameter("@ID", FbDbType.Integer){ Value = filtro.Id },
                    new FbParameter("@NOMBRE", FbDbType.VarChar){ Value = filtro.Nombre },
                    new FbParameter("@ACTIVO", FbDbType.VarChar){ Value = filtro.Activo },
                };
            return result;
        }
    }
}
