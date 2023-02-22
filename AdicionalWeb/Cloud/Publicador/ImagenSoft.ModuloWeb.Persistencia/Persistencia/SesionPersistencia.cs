using ImagenSoft.ModuloWeb.Entidades;
using System;
using System.Linq;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class SesionPersistencia
    {
        //public Sesion Obtener(Sesion sesion, FiltroSesion filtro)
        //{
        //    return this.ObtenerTodosFiltro(sesion, filtro).FirstOrDefault();
        //}

        public ListaSesiones ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroSesionModuloWeb filtro)
        {
            ListaSesiones resultado = new ListaSesiones();
            AdministrarUsuariosPersistencia servicios = new AdministrarUsuariosPersistencia();
            ListaAdministrarUsuarios usuarios = servicios.ObtenerTodosFiltro(sesion, new FiltroAdministrarUsuarios() { Activo = "Si" });
            resultado.Capacity = usuarios.Count + 1;
            int idx = 0;
            resultado.AddRange(usuarios.AsParallel()
                                       .AsOrdered()
                                       .OrderBy(p => p.IdDistribuidor)
                                       .Select(p => this.ObtenerNuevaSesion(p, System.Threading.Interlocked.Increment(ref idx))));
            //Parallel.ForEach(usuarios, p => resultado.Add(this.ObtenerNuevaSesion(p, System.Threading.Interlocked.Increment(ref idx))));
            return resultado;
        }

        private SesionModuloWeb ObtenerNuevaSesion(AdministrarUsuarios entidad, int idx)
        {
            return new SesionModuloWeb()
                {
                    //Computadora = new ImagenSoft.Framework.Entidades.Computadora()
                    //{
                    //    Activo = "Si",
                    //    Clave = 1,
                    //    Descripcion = string.Empty,
                    //    Nombre = "Compulocal",
                    //    Variables = new System.Collections.Generic.List<string>()
                    //},
                    DireccionIP = string.Empty,
                    Empresa = new Entidades.Base.DatosEmpresa()// new ImagenSoft.Framework.Entidades.Empresa()
                    {
                        Id = 1
                    },
                    //Estatus = string.Empty,
                    HoraFinal = DateTime.Now.AddDays(1),
                    HoraInicial = DateTime.Now,
                    //Id = entidad.IdDistribuidor,
                    //Indice = idx,
                    Sistema = "MON",
                    Usuario = new Entidades.Base.UsuarioModuloWeb()// new ImagenSoft.Framework.Entidades.Usuario()
                    {
                        Activo = entidad.Activo,
                        Clave = entidad.Clave,
                        CorreoElectronico = entidad.Email,
                        Nombre = entidad.Nombre,
                        Password = entidad.Contrasena,
                        Variables = new System.Collections.Generic.List<string>() { entidad.Permisos.ToXML().ToString() }
                    },
                    Privilegio = NivelPrivilegio.Monitor,
                    Nombre = entidad.Nombre,
                    Clave = entidad.Clave,
                    IdDistribuidor = entidad.IdDistribuidor,
                    Distribuidor = entidad.Distribuidor
                };
        }
    }
}
