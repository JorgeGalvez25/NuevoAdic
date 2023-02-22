using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Persistencia;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public static class Configuraciones
    {
        public static string NombreAplicacion = "NuevoAdicionalGas";
        //public static string versionSistema = Licencia.Version;
        public static string claveSistema = "CVL5";
        public static string claveBoton = "CVL501";
        public static string claveAndroid = "CVL502";

        public static Dictionary<int, ServiciosCliente.IServiciosCliente> ListaCanales { get; set; }
        public static Dictionary<int, Servicios.Adicional.IServiciosAdicional> ListaCanalesAdicional { get; set; }
        public static Dictionary<int, Derecho> ListaDerechos { get; set; }
        public static int IdUsuario { get; set; }
        public static string NombreUsuario { get; set; }
        public static Dictionary<string, string> VariablesUsuario { get; private set; }

        public static bool canalAdicionalAbierto { get; set; }
        public static ListaEstacion Estaciones { get; private set; }
        public static Dictionary<int, bool> EstacionBitacoraGuardarAcceso { get; set; }

        public static Dictionary<int, List<Licencia>> Licencias { get; set; }

        private static ChannelFactory<Servicios.Adicional.IServiciosAdicional> factoryAdicional;
        private static ChannelFactory<ServiciosCliente.IServiciosCliente> factoryCliente;

        public static string[] ListaVariables = new string[] { "Reporte 1", "Reporte 2", "Restringido a 1 día" };

        static Configuraciones()
        {
            Licencias = new Dictionary<int, List<Licencia>>();
        }

        public static void Inicializar()
        {
            ListaCanales = new Dictionary<int, ServiciosCliente.IServiciosCliente>();
            ListaCanalesAdicional = new Dictionary<int, Servicios.Adicional.IServiciosAdicional>();
            EstacionBitacoraGuardarAcceso = new Dictionary<int, bool>();

            Estaciones = null;

            try
            {
                ActualizarEstaciones();
            }
            catch
            {
                canalAdicionalAbierto = false;
                throw;
            }
        }

        public static void InicializarDerechos()
        {
            ListaDerechos = new Dictionary<int, Derecho>();

            ListaDerecho pListaDerechos = new DerechoPersistencia().ObtenerListaPorUsuario(IdUsuario);

            ListaDerechos = pListaDerechos.ToDictionary(k => k.Id_Derecho, v => v);
        }

        public static void InicializarDerechos(string variables)
        {
            InicializarDerechos();

            string[] vars = variables.Split(new string[] { ",", ";", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            VariablesUsuario = new Dictionary<string, string>();
            foreach (string item in vars)
            {
                string[] partes = item.Split('=');

                if (partes.Length > 1 && !VariablesUsuario.ContainsKey(partes[0]))
                {
                    VariablesUsuario.Add(partes[0], partes[1]);
                }
            }
        }

        public static void ActualizarEstaciones()
        {
            if (NombreUsuario.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                Estaciones = new EstacionPersistencia().ObtenerLista();
            }
            else
            {
                Estaciones = new EstacionPersistencia().EstacionObtenerPorUsuario(IdUsuario);
            }

            if (factoryAdicional != null && factoryCliente != null)
            {
                foreach (Estacion item in Estaciones)
                {
                    if (!ListaCanales.ContainsKey(item.Id))
                    {
                        var estChannel1 = factoryCliente.CreateChannel(new EndpointAddress(string.Concat("net.tcp://", item.IpServicios, "/ServiciosCliente")));
                        ListaCanales.Add(item.Id, estChannel1);
                    }
                    if (!ListaCanalesAdicional.ContainsKey(item.Id))
                    {
                        ListaCanalesAdicional.Add(item.Id, factoryAdicional.CreateChannel(new EndpointAddress(string.Concat("net.tcp://", item.IpServicios, "/ServiciosAdicional"))));
                    }
                    if (!EstacionBitacoraGuardarAcceso.ContainsKey(item.Id))
                    {
                        EstacionBitacoraGuardarAcceso.Add(item.Id, false);
                    }
                }
            }
            else
            {
                EstacionBitacoraGuardarAcceso = Estaciones.ToDictionary(k => k.Id, v => false);
            }
        }

        public static string GetValorVariable(string variable)
        {
            if (VariablesUsuario.ContainsKey(variable))
                return VariablesUsuario[variable];

            return string.Empty;
        }

        internal static void EliminarEstacion(int idEstacion)
        {
            int indice = Estaciones.FindIndex(p => { return p.Id == idEstacion; });

            if (indice >= 0)
            {
                if (((IClientChannel)ListaCanales[idEstacion]).State == CommunicationState.Opened)
                {
                    try
                    {
                        ((IClientChannel)ListaCanales[idEstacion]).Close();
                    }
                    catch { }
                    ListaCanales.Remove(idEstacion);
                }

                if (((IClientChannel)ListaCanalesAdicional[idEstacion]).State == CommunicationState.Opened)
                {
                    try
                    {
                        ((IClientChannel)ListaCanalesAdicional[idEstacion]).Close();
                    }
                    catch (Exception) { }
                    ListaCanalesAdicional.Remove(idEstacion);
                }

                EstacionBitacoraGuardarAcceso.Remove(idEstacion);

                Estaciones.RemoveAt(indice);
            }
        }

        internal static Servicios.Adicional.IServiciosAdicional AbrirCanalAdicional(int idEstacion)
        {
            Estacion est = Estaciones.Find(p => { return p.Id.Equals(idEstacion); });

            if (factoryAdicional == null)
            {
                factoryAdicional = new ChannelFactory<Servicios.Adicional.IServiciosAdicional>("epAdicional");
            }
            ListaCanalesAdicional[idEstacion] = factoryAdicional.CreateChannel(new EndpointAddress(string.Concat("net.tcp://", est.IpServicios, "/ServiciosAdicional")));

            return ListaCanalesAdicional[idEstacion];
        }

        internal static ServiciosCliente.IServiciosCliente AbrirCanalCliente(int idEstacion)
        {
            Estacion est = Estaciones.Find(p => { return p.Id.Equals(idEstacion); });

            if (factoryCliente == null)
            {
                factoryCliente = new ChannelFactory<ServiciosCliente.IServiciosCliente>("epCliente");
            }
            ListaCanales[idEstacion] = factoryCliente.CreateChannel(new EndpointAddress(string.Concat("net.tcp://", est.IpServicios, "/ServiciosCliente")));

            return ListaCanales[idEstacion];
        }

        internal static void CerrarCanales(int idEstacion)
        {
            //if (factoryAdicional != null && factoryAdicional.State == CommunicationState.Opened)
            if ((ListaCanalesAdicional[idEstacion] as IClientChannel).State == CommunicationState.Opened)
            {
                try
                {
                    (ListaCanalesAdicional[idEstacion] as IClientChannel).Close();
                    //factoryAdicional.Close();
                }
                catch { }
            }

            //if (factoryCliente != null && factoryCliente.State == CommunicationState.Opened)
            if ((ListaCanales[idEstacion] as IClientChannel).State == CommunicationState.Opened)
            {
                try
                {
                    (ListaCanales[idEstacion] as IClientChannel).Close();
                    //factoryCliente.Close();
                }
                catch (Exception) { }
            }
        }

        internal static void GuardarMensaje(string archivo, string mensaje)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(archivo, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                System.IO.StreamWriter m_streamWriter = new System.IO.StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                m_streamWriter.WriteLine(mensaje);
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch { }
        }

        internal static void ActualizarEstadosEstaciones()
        {
            foreach (Estacion item in Estaciones)
            {
                try
                {
                    string estado = ListaCanalesAdicional[item.Id].ConfiguracionObtener(item.Id).Estado;

                    if (!item.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Estado = estado;
                    }
                }
                catch (Exception)
                {
                    AbrirCanalAdicional(item.Id);
                }
            }
        }

        internal static bool CanalEstaActivo(int idEstacion, bool adicional)
        {
            bool result = false;

            try
            {
                if (adicional)
                {
                    result = ListaCanalesAdicional[idEstacion].IsAlive();
                }
                else
                {
                    result = ListaCanales[idEstacion].IsAlive();
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        internal static bool LicenciaValida(int estacion, string modulo)
        {
            //List<Licencia> licenciasPorEstacion = null;

            //if (Licencias.ContainsKey(estacion))
            //{
            //    licenciasPorEstacion = Licencias[estacion];

            //    if (licenciasPorEstacion == null || licenciasPorEstacion.Count == 0)
            //        return false;

            //    Licencia licenciaModulo = licenciasPorEstacion.Find(l =>
            //                                {
            //                                    return l.Modulo.Equals(modulo, StringComparison.OrdinalIgnoreCase);
            //                                });

            //    if (licenciaModulo != null)
            //        return licenciaModulo.Valida;
            //    else
            //        return false;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
    }
}
