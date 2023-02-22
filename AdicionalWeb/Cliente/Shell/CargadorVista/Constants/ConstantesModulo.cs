using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.CargadorVistas.Constants
{
    public class InfoException : Exception
    {
        public InfoException()
            : base()
        {

        }

        public InfoException(string mensaje)
            : base(mensaje)
        {

        }
    }

    public class ConstantesModulo
    {
        public const string SIN_REGISTRO_DE_SELLO = "SR";

        public const string NOMBRE_SISTEMA = "Monitor";

        public const string SESION_SISTEMA = "SesionSistema";

        public const string SERVICIOS_WEB_PROVEEDOR = "ServiciosWebProveedor";

        public const string EJERCICIO_ACTUAL = "EjercicioActual";

        public const string SI = "Si";

        public const string NO = "No";

        public const string TODOS = "Todos";

        public const string TODAS = "Todas";

        [ReadOnly(true)]
        public static List<string> MESES = new List<string>(12) { 
            "Enero", 
            "Febrero", 
            "Marzo",
            "Abril",
            "Mayo", 
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre", 
            "Octubre", 
            "Noviembre",
            "Diciembre" 
        };

        private static List<string> _NivelesPrivilegio;
        public static List<string> NIVELES_PRIVILEGIO
        {
            get
            {
                if (_NivelesPrivilegio == null)
                {
                    _NivelesPrivilegio = new List<string>();
                    foreach (string item in Enum.GetNames(typeof(NivelPrivilegio)))
                    {
                        _NivelesPrivilegio.Add(item);
                    }
                }
                return _NivelesPrivilegio;
            }
        }

        private static Dictionary<string, string> _NivelesPrivilegioDescripcion;
        public static Dictionary<string, string> NIVELES_PRIVILEGIO_DESCRIPCION
        {
            get
            {
                if (_NivelesPrivilegioDescripcion == null)
                {
                    _NivelesPrivilegioDescripcion = NIVELES_PRIVILEGIO.ToDictionary((x) => x, (y) => y);

                    foreach (var item in NIVELES_PRIVILEGIO)
                    {
                        NivelPrivilegio value = (NivelPrivilegio)Enum.Parse(typeof(NivelPrivilegio), _NivelesPrivilegioDescripcion[item]);
                        Type enumType = value.GetType();
                        MemberInfo[] memberInfos = enumType.GetMember(value.ToString());

                        if (memberInfos != null && memberInfos.Length > 0)
                        {
                            object[] attrs = memberInfos[0].GetCustomAttributes(
                                typeof(System.Runtime.Serialization.EnumMemberAttribute),
                                false);

                            if (attrs != null &&
                                attrs.Length > 0 &&
                                ((System.Runtime.Serialization.EnumMemberAttribute)attrs[0]).Value != null)
                            {
                                _NivelesPrivilegioDescripcion[item] = ((System.Runtime.Serialization.EnumMemberAttribute)attrs[0]).Value;
                            }
                        }
                    }
                }

                return _NivelesPrivilegioDescripcion;
            }
        }

        public class MODULOS
        {
            #region Catálogos

            public const string CARGADOR_VISTAS = "CargadorVistas";

            public const string USUARIOS_MDL = "AdministrarUsuariosMdl";

            public const string ZONAS_MDL = "AdministrarZonasMdl";

            public const string ADMINISTRAR_CLIENTES_MDL = "AdministrarClientesMdl";

            public const string ADMINISTRAR_DISTRIBUIDORES_MDL = "AdministrarDistribuidoresMdl";

            public const string ADMINISTRAR_USUARIOS_MDL = "AdministrarUsuariosMdl";

            public const string CAMBIO_PRECIOS = "CambioPreciosMdl";

            public const string ADMINISTRAR_GRUPOS_MDL = "AdministrarGruposMdl";

            #endregion

            #region Servicios

            public const string SESIONES_MDL = "SesionesMdl";

            public const string MONITOR_TRANSMISIONES_MDL = "MonitorTransmisionesMdl";

            public const string MONITOR_CONEXIONES_MDL = "MonitorConexionesMdl";

            public const string MONITOR_CAMBIO_PRECIO_MDL = "MonitorCambioPrecioMdl";

            public const string MONITOR_APLICACIONES_MDL = "MonitorAplicacionesMdl";

            #endregion

            #region Pruebas

            public const string PRUEBAS_MDL = "PruebasTransmisionesMdl";

            #endregion

            public class CLAVES
            {
                #region Catálogos

                public const string CLIENTES = "CNT";
                public const string USUARIOS = "USU";
                public const string DISTRIBUIDORES = "DIS";
                public const string CAMBIO_PRECIOS = "PRC";
                public const string GRUPOS = "GRP";

                #endregion

                #region Servicios

                public const string MONITOR_TRANSMISIONES = "MTR";
                public const string MONITOR_PRECIOS = "MPR";
                public const string MONITOR_CONEXIONES = "MCX";
                public const string MONITOR_APLICACIONES = "MAP";

                #endregion

                #region Pruebas

                public const string PRUEBAS = "PRB";

                #endregion
            }
        }

        public class VISTAS
        {
            public const string LONGITUD_SUCURSAL = "D3";

            #region Catálogos

            public class ADMINISTRAR_GRUPOS_MDL
            {
                public const string VISTA_LISTADO = "VLAdministrarGruposMdl";
                public const string VISTA_MANTENIMIENTO = "VMAdministrarGruposMdl";
                public const string VISTA_IMPRESION = "VIAdministrarGruposMdl";
                public const string TITULO_VISTA = "Administrar Grupos";
                public const string TITULO_VISTA_LISTADO = "Administrar Grupos";
                public const string TITULO_VISTA_MANTENIMIENTO = "Grupo {0}";
                public const string TITULO_VISTA_IMPRESION = "Grupos";
                public const string LONGITUD_CLAVE = "D6";
                public const string LONGITUD_ESTACION = "D2";
                public const string EVENT_HANDLER = "evtAdministrarGrupos";
            }

            public class ADMINISTRAR_CLIENTES_MDL
            {
                public const string VISTA_LISTADO = "VLAdministrarClientesMdl";
                public const string VISTA_MANTENIMIENTO = "VMAdministrarClientesMdl";
                public const string VISTA_IMPRESION = "VIAdministrarClientesMdl";
                public const string TITULO_VISTA = "Administrar Clientes";
                public const string TITULO_VISTA_LISTADO = "Administrar Clientes";
                public const string TITULO_VISTA_MANTENIMIENTO = "{0} Cliente";
                public const string TITULO_VISTA_IMPRESION = "Clientes";
                public const string LONGITUD_CLAVE = "D6";
                public const string LONGITUD_ESTACION = "D2";
                public const string EVENT_HANDLER = "evtAdministrarClientes";
            }

            public class ADMINISTRAR_USUARIOS_MDL
            {
                public const string VISTA_LISTADO = "VLAdministrarUsuariosMdl";
                public const string VISTA_MANTENIMIENTO = "VMAdministrarUsuariosMdl";
                public const string VISTA_IMPRESION = "VIAdministrarUsuariosMdl";
                public const string TITULO_VISTA = "Administrar Usuarios";
                public const string TITULO_VISTA_LISTADO = "Administrar Usuarios";
                public const string TITULO_VISTA_MANTENIMIENTO = "{0} Usuarios";
                public const string TITULO_VISTA_IMPRESION = "Usuarios";
                public const string LONGITUD_CLAVE = "D6";
                public const string LONGITUD_ESTACION = "D2";
                public const string EVENT_HANDLER = "evtAdministrarUsuarios";
            }

            public class ADMINISTRAR_DISTRIBUIDORES_MDL
            {
                public const string VISTA_LISTADO = "VLAdministrarDistribuidoresMdl";
                public const string VISTA_MANTENIMIENTO = "VMAdministrarDistribuidoresMdl";
                public const string VISTA_IMPRESION = "VIAdministrarDistribuidoresMdl";
                public const string TITULO_VISTA = "Administrar Distribuidores";
                public const string TITULO_VISTA_LISTADO = "Administrar Distribuidores";
                public const string TITULO_VISTA_MANTENIMIENTO = "{0} Distribuidor";
                public const string TITULO_VISTA_IMPRESION = "Distribuidores";
                public const string LONGITUD_CLAVE = "D6";
                public const string LONGITUD_ESTACION = "D2";
                public const string EVENT_HANDLER = "evtAdministrarDistribuidores";
            }

            public class CAMBIO_PRECIOS_MDL
            {
                public const string VISTA_LISTADO = "VLCambioPreciosMdl";
                public const string VISTA_MANTENIMIENTO = "VMCambioPreciosMdl";
                public const string VISTA_IMPRESION = "VICambioPreciosMdl";
                public const string TITULO_VISTA = "Programación de Cambio de Precio";
                public const string TITULO_VISTA_LISTADO = "Programación de Cambio de Precio";
                public const string TITULO_VISTA_MANTENIMIENTO = "{0} Cambio de Precio";
                public const string TITULO_VISTA_IMPRESION = "Programación de Cambio de Precio";
                public const string LONGITUD_CLAVE = "D3";
                public const string FORMATO_PRECIOS = "N3";
                public const string LONGITUD_ESTACION = "D2";
                public const string EVENT_HANDLER = "evtCambioPrecios";
            }

            #endregion

            #region Servicios

            public class MONITOR_TRANSMISIONES_MDL
            {
                public const string VISTA_LISTADO = "VLMonitorTransmisionesMdl";
                public const string VISTA_MANTENIMIENTO = "VMMonitorTransmisionesMdl";
                public const string VISTA_IMPRESION = "VIMonitorTransmisionesMdl";
                public const string TITULO_VISTA = "Monitor de Transmisiones";
                public const string TITULO_VISTA_LISTADO = "Monitor de Transmisiones";
                public const string TITULO_VISTA_MANTENIMIENTO_ = "{0} Monitor de Transmisiones";
                public const string TITULO_VISTA_IMPRESION = "Monitor de Transmisiones";
                public const string LONGITUD_CLAVE = "D3";
                public const string LONGITUD_ESTACION = "D2";
            }

            public class MONITOR_CAMBIO_PRECIO_MDL
            {
                public const string VISTA_LISTADO = "VLMonitorCambioPrecioMdl";
                public const string VISTA_MANTENIMIENTO = "VMMonitorCambioPrecioMdl";
                public const string VISTA_IMPRESION = "VIMonitorCambioPrecioMdl";
                public const string TITULO_VISTA = "Monitor de Cambio de Precio";
                public const string TITULO_VISTA_LISTADO = "Monitor de Cambio de Precio {0}";
                public const string TITULO_VISTA_MANTENIMIENTO_ = "{0} Monitor de Cambio de Precio";
                public const string TITULO_VISTA_IMPRESION = "Monitor de Cambio de Precios";
                public const string LONGITUD_CLAVE = "D3";
                public const string LONGITUD_ESTACION = "D2";
            }

            public class MONITOR_CONEXIONES_MDL
            {
                public const string VISTA_LISTADO = "VLMonitorConexionesMdl";
                public const string VISTA_MANTENIMIENTO = "VMMonitorConexionesMdl";
                public const string VISTA_IMPRESION = "VIMonitorConexionesMdl";
                public const string TITULO_VISTA = "Monitor de Conexiones";
                public const string TITULO_VISTA_LISTADO = "Monitor de Conexiones";
                public const string TITULO_VISTA_MANTENIMIENTO_ = "{0} Monitor de Conexiones";
                public const string TITULO_VISTA_IMPRESION = "Monitor de Conexiones";
                public const string LONGITUD_CLAVE = "D3";
                public const string LONGITUD_ESTACION = "D2";
            }

            public class MONITOR_APLICACIONES_MDL
            {
                public const string VISTA_LISTADO = "VLMonitorAplicacionesMdl";
                public const string VISTA_MANTENIMIENTO = "VMMonitorAplicacionesMdl";
                public const string VISTA_IMPRESION = "VIMonitorAplicacionesMdl";
                public const string TITULO_VISTA = "Monitor de Aplicaciones";
                public const string TITULO_VISTA_LISTADO = "Monitor de Aplicaciones";
                public const string TITULO_VISTA_MANTENIMIENTO_ = "{0} Monitor de Aplicaciones";
                public const string TITULO_VISTA_IMPRESION = "Monitor de Aplicaciones";
                public const string LONGITUD_CLAVE = "D3";
                public const string LONGITUD_ESTACION = "D2";
            }

            #endregion

            #region Pruebas

            public class PRUEBAS_TRANSMISOR_MDL
            {
                public const string VISTA_LISTADO = "TransmisorMdl";
                public const string VISTA_MANTENIMIENTO = "VMTransmisorMdl";
                public const string VISTA_IMPRESION = "VITransmisorMdl";
                public const string TITULO_VISTA = "Administrar Distribuidores";
                public const string TITULO_VISTA_LISTADO = "Emulador Transmisor";
                public const string TITULO_VISTA_MANTENIMIENTO_ = "{0} Emulador Transmisor";
                public const string TITULO_VISTA_IMPRESION = "Emulador Transmisor";
                public const string LONGITUD_CLAVE = "D3";
                public const string LONGITUD_ESTACION = "D2";
            }

            #endregion
        }

        public class MENUS
        {
            public const string MENU = "Menú";

            public const string OPCIONES = "Opciones";

            public const string IMPRESION = "Impresión";

            public const string OPCIONES_GRUPO = "Opciones de Grupo";

            public const string OTROS = "Otros";
        }

        public class OPCIONES
        {
            public const string NUEVO = "Nuevo";

            public const string REGISTRAR = "Registrar";

            public const string ACTUALIZAR = "Actualizar";

            public const string ELIMINAR = "Eliminar";

            public const string DEPURAR = "Depurar";

            public const string INSERTAR = "Insertar";

            public const string MODIFICAR = "Modificar";

            public const string IMPRIMIR = "Imprimir";

            public const string IMPRIMIR_LISTADO = "Imprimir Listado";

            public const string PREVER = "Prever";

            public const string PROPIEDADES = "Propiedades";

            public const string CERRAR = "Cerrar";

            public const string GUARDAR = "Guardar";

            public const string GUARDAR_Y_CERRAR = "Guardar y Cerrar";

            public const string CANCELAR = "Cancelar";

            public const string MARCAR_PAGADO = "Marcar Pagado";

            public const string REGISTRAR_GRUPO = "Registrar Grupo";

            public const string MODIFICAR_GRUPO = "Modificar Grupo";
        }

        public class ORDENAMIENTO
        {
            public const string ACTIVO = "Activo";

            public const string INACTIVO = "Inactivo";

            public const string NUMERICO = "Numérico";

            public const string CANCELADO = "Cancelado";

            public const string ALFABETICO = "Alfabético";

            public const string ALFANUMERICO = "Alfanumérico";
        }

        public class CONFIGURACIONES
        {
            public const string TAMAÑO_PAGINA = "TamanoPagina";
            public const string MONITOR_TIEMPO_ESPERA = "MonitorTiempoEspera";
        }
    }

    public class NivelesPrivilegio
    {
        public static NivelPrivilegio ObtenerNivelPrivilegio(int nivel)
        {
            if (nivel <= 0) return (NivelPrivilegio)0;
            return (NivelPrivilegio)nivel;
        }
        public static NivelPrivilegio ObtenerNivelPrivilegio(string nivel)
        {
            if (string.IsNullOrEmpty(nivel)) return (NivelPrivilegio)0;
            int iNivel = 0;
            int.TryParse(nivel, out iNivel);
            return ObtenerNivelPrivilegio(iNivel);
        }

        public static NivelPrivilegio ObtenerNivelPrivilegioPorDescripcion(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion) || descripcion.Equals("0")) return (NivelPrivilegio)0;

            NivelPrivilegio resultado = (NivelPrivilegio)0;

            var dNivel = ConstantesModulo.NIVELES_PRIVILEGIO_DESCRIPCION;

            foreach (var item in dNivel.Keys)
            {
                if (dNivel[item].Equals(descripcion))
                {
                    resultado = (NivelPrivilegio)Enum.Parse(typeof(NivelPrivilegio), item);
                    break;
                }
            }

            return resultado;
        }

        public static string ObtenerDescripcionNivelPrivilegio(int nivel)
        {
            NivelPrivilegio priv = (NivelPrivilegio)nivel;
            return ObtenerDescripcionNivelPrivilegio(priv.ToString());
        }
        public static string ObtenerDescripcionNivelPrivilegio(string nivel)
        {
            var dNivel = ConstantesModulo.NIVELES_PRIVILEGIO_DESCRIPCION;

            return (dNivel.ContainsKey(nivel)) ? dNivel[nivel] : "0";
        }
    }

    public enum ModoModulo
    {
        Registrar,
        Modificar,
        Eliminar,
        Propiedades,
        RegistrarGrupo,
        ModificarGrupo
    }
}
