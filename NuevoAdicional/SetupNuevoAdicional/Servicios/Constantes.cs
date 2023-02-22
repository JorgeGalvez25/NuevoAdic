using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupNuevoAdicional
{
    public static class Constantes
    {
        public const string NombreAplicacion = "SetupNuevoAdicional";
        public const string Version = "4.1";
        public const string TargetDir = @"C:\Gas";
        public const string ApplicationFolder = @"NAdicional\Consola";
        public const string DataBaseFolder = @"NAdicional\Consola\DB";
        public const string FileName = @"Consola\NuevoAdicional.exe";
        public const string WorkingDirectory = @"NAdicional";
        public const string ArchivosZipResourceName = "SetupNuevoAdicional.Referencias.archivos.zip";
        public const string ArchivoZipBD = "SetupNuevoAdicional.Referencias.NADICIONAL_CLIENT.zip";
        public const string NombreBaseDatosCliente = "NADICIONAL_CLIENT.fdb";
        public const string UninstallConsolaResourceName = "SetupNuevoAdicional.Referencias.UninstallNuevoAdicional.zip";
        public const string UninstallConsolaFileName = "UninstallNuevoAdicional.exe";

        internal class Titulos
        {
            public const string Bienvenida = "Bienvenido al programa de instalación de la Consola de Adicional versión " + Version;
            public const string Ruta = "Seleccionar carpeta de instalación";
            public const string RutaBD = "Seleccionar carpeta de instalación de la base de datos";
            public const string Servicio = "Proporcionar datos de servicios WCF";
            public const string Error = "Error de instalación";
            public const string Finalizar = "Instalación finalizada";
            public const string Confirmar = "Confirmar instalación";
            public const string Procesando = "Instalando";
        }

        internal class Mensajes
        {
            public const string CarpetaSeleccionadaEsInvalida = "Carpeta seleccionada es inválida.";
            public const string ServidorEsRequerido = "Servidor es requerido.";
            public const string NombreServicioWcfEsRequerido = "Nombre de servicio WCF es requerido.";
            public const string NombreServicioActualizacionEsRequerido = "Nombre de servicio de actualización es requerido.";
            public const string PuertoEsRequerido = "Puerto es requerido.";
            public const string DeseaSalir = "¿Desea salir del programa de instalación?";
            public const string ExisteAplicacionAbierta = "Existe una instancia del programa de instalación de Gas Adicional ejecutandose en éste equipo, no es posible ejecutar mas de una instancia.";
        }
    }
}
