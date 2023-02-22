using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl.Constants
{
    public class ConstantesEntidad
    {
        //ConstantesEntidad()
        //{
        //    ConstantesEntidad.ListaModulos = new Dictionary<string, string>()
        //        {
        //             { ConstantesPermisos.Modulos.CLIENTES, "Administrar Clientes" },
        //             { ConstantesPermisos.Modulos.USUARIOS, "Administrar Usuarios" },
        //             { ConstantesPermisos.Modulos.DISTRIBUIDORES, "Administrar Distribuidores" },
        //             { ConstantesPermisos.Modulos.CAMBIO_PRECIOS, "Programación de Cambio de Precio" },
        //             { ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS, "Monitor de Cambio de Precio" },
        //             { ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES, "Monitor de Transmisiones" },
        //             { ConstantesPermisos.Modulos.MONITOR_CONEXIONES, "Monitor de Conexiones" }
        //        };
        //}
        public const string CLAVE = "Clave";

        public const string NOMBRE = "Nombre";

        public const string PUESTO = "Puesto";

        public const string E_MAIL = "Email";

        public const string NIVEL_PRIVILEGIO = "NivelPrivilegio";

        public const string ACTIVO = "Activo";

        public const string FECHA = "Fecha";

        public const string PERMISOS = "Permisos";

        public const string DISTRIBUIDOR = "Distribuidor";


        public const string CLAVE_CAPTION = "Clave";

        public const string NOMBRE_CAPTION = "Nombre";

        public const string PUESTO_CAPTION = "Puesto";

        public const string E_MAIL_CAPTION = "E-Mail";

        public const string NIVEL_PRIVILEGIO_CAPTION = "Nivel Privilegio";

        public const string ACTIVO_CAPTION = "Activo";

        public const string FECHA_CAPTION = "Fecha";

        public const string PERMISOS_CAPTION = "Permisos";

        public const string DISTRIBUIDOR_CAPTION = "Distribuidor";

        public class CPermisos
        {
            public const string KEY = "Key";

            public const string VALUE = "Value";

            public const string KEY_CAPTION = "Llave";

            public const string VALUE_CAPTION = "Valor";
        }

        public static Dictionary<string, string> ListaModulos = new Dictionary<string, string>()
        {
             { ConstantesPermisos.Modulos.CLIENTES, "Administrar Clientes" },
             { ConstantesPermisos.Modulos.USUARIOS, "Administrar Usuarios" },
             { ConstantesPermisos.Modulos.DISTRIBUIDORES, "Administrar Distribuidores" },
             //{ ConstantesPermisos.Modulos.CAMBIO_PRECIOS, "Programación de Cambio de Precio" },
             //{ ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS, "Monitor de Cambio de Precio" },
             //{ ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES, "Monitor de Transmisiones" },
             //{ ConstantesPermisos.Modulos.MONITOR_CONEXIONES, "Monitor de Conexiones" }
        };
    }
}
