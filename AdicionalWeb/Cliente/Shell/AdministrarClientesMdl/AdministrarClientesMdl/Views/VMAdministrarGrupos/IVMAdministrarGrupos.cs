namespace EstandarCliente.AdministrarClientesMdl
{
    public interface IVMAdministrarGrupos
    {
        bool UsuariosClienteInsertar(ImagenSoft.ModuloWeb.Entidades.Web.AdministrarUsuariosClientes entidad);

        bool UsuariosClienteModificar(ImagenSoft.ModuloWeb.Entidades.Web.AdministrarUsuariosClientes entidad);

        bool UsuariosClienteEliminar(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro);

        ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes UsuariosClienteObtenerTodos(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro);


        bool Insertar(ImagenSoft.ModuloWeb.Entidades.AdministrarClientes entidad);

        bool Modificar(ImagenSoft.ModuloWeb.Entidades.AdministrarClientes entidad);

        ImagenSoft.ModuloWeb.Entidades.AdministrarClientes Obtener(ImagenSoft.ModuloWeb.Entidades.FiltroAdministrarClientes filtro);

        ImagenSoft.ModuloWeb.Entidades.ListaAdministrarClientes ObtenerTodosFiltro(ImagenSoft.ModuloWeb.Entidades.FiltroAdministrarClientes filtro);

        bool UsuariosClienteInsertarModificar(ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes lista);

        bool UsuariosClienteNuevaContrasenia(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro);
    }
}

