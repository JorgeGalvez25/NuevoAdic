using System.Data;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Enlaces;
using FirebirdSql.Data.FirebirdClient;
using ImagenSoft.ServiciosWeb.Entidades.Web;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador;
using ImagenSoft.ServiciosWeb.Entidades.Enumeradores;

namespace AdicionalWeb.Persistencia
{
    public class LoginPersistencia
    {
        private Conexiones _enlace;

        public LoginPersistencia()
        {
            this._enlace = new Conexiones();
        }

        public UsuarioWeb LoginObtener(FiltroSesion filtro)
        {
            string message = string.Empty;
            UsuarioWeb sesion = new UsuarioWeb();

            ServiciosProveedorAdicionalWeb servicios = new ServiciosProveedorAdicionalWeb(new ImagenSoft.ServiciosWeb.Entidades.Sesion(), TipoConexionUsuario.UsuarioWeb);
            var resp = servicios.AdicionalWebValidarLogin(new UsuarioWeb()
                {
                    NoEstacion = filtro.NoEstacion,
                    Usuario = filtro.Nombre,
                    Password = filtro.Password,
                }, ref message);

            if (resp == null) { return null; }

            sesion.Usuario = resp.Usuario;
            sesion.Password = resp.Password;
            sesion.Correo = resp.Correo;
            sesion.NoEstacion = resp.NoEstacion;
            sesion.Estacion = resp.Estacion;
            sesion.Privilegios = (resp.Privilegios ?? string.Empty).ToString();


            //this._enlace.AdicionalClienteConsulta((com) =>
            //    {
            //        com.CommandText = @"SELECT ID, " +
            //                                 " NOMBRE, " +
            //                                 " CLAVE, " +
            //                                 " ACTIVO, " +
            //                                 " VARIABLES, " +
            //                                 " CORREO " +
            //                            " FROM USUARIO " +
            //                           " WHERE NOMBRE = @NOMBRE ";
            //        com.Parameters.AddRange(this.ObtenerParametros(filtro));
            //        using (FbDataReader reader = com.ExecuteReader(CommandBehavior.SingleRow))
            //        {
            //            try
            //            {
            //                if (reader.Read())
            //                {
            //                    sesion = new Sesion();
            //                    sesion = this.ReaderToEntidad(reader);
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed)
            //                {
            //                    reader.Close();
            //                }
            //            }
            //        }
            //    });
            return sesion;
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
