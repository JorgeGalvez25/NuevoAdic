using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Consola.Logic.Entities
{
    public class DPVGCONF
    {
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string EstacionServicio { get; set; }
        public string LectorSerial { get; set; }
        public string UltimoFolioCR { get; set; }
        public string UltimoFolioPP { get; set; }
        public string ImpresoraTickets { get; set; }
        public string PosCliente { get; set; }
        public string LongCliente { get; set; }
        public string PosVehiculo { get; set; }
        public string LongVehiculo { get; set; }
        public string ConfigPuertoTarjeta { get; set; }
        public string RutaVolumetricos { get; set; }
        public string Impo_Bombas { get; set; }
        public string Impo_Estaciones { get; set; }
        public string Impo_Precios { get; set; }
        public string Impo_Tanques { get; set; }
        public string Impo_Combustibles { get; set; }
        public string Impo_Tarjetas { get; set; }
        public string Impo_Config { get; set; }
        public string Permitir_Cambio_FechaHora { get; set; }
        public string Int_Act_Precios { get; set; }
        public string ImpresoraVolumetricos { get; set; }
        public string Comando1 { get; set; }
        public string Comando2 { get; set; }
        public string Levantar_Consolas { get; set; }
        public string Ruta_Respaldos { get; set; }
        public string Licencia { get; set; }
        public string NumeroSerie { get; set; }
        public string Estacion_IGas { get; set; }
        public string ImpresoraGrafica { get; set; }
        public string Dispensarios { get; set; }
        public string Config_Ticket { get; set; }
        public string Mascara_Float { get; set; }
        public string Mascara_Hora { get; set; }
        public string Licencia2 { get; set; }
        public string EsTemporal { get; set; }
        public string FechaVence { get; set; }
        public string DireccionPEMEX { get; set; }
        public string UsuarioPEMEX { get; set; }
        public string ClavePEMEX { get; set; }
        public string Comando3 { get; set; }
        public string Comando4 { get; set; }
        public string Licencia3 { get; set; }
        public string Licencia4 { get; set; }
        public string ModoAdi { get; set; }
        public string UltimoImpreso { get; set; }
        public string ConfigCupon { get; set; }
        public string UsarCuponCortesia { get; set; }
        public string Licencia5 { get; set; }
        public string FechaVence5 { get; set; }
        public string EsTemporal5 { get; set; }
        public string Ban_Name { get; set; }
        public string Ban_Password { get; set; }
        public string Ban_ClienID { get; set; }
        public string ModoPagosBancarios { get; set; }
        public string UsaTurnosAlternativos { get; set; }
        public string Ticket_Promo { get; set; }
        public string AliasMaster { get; set; }
        public string FechaVence2 { get; set; }
        public string EsTemporal2 { get; set; }
    }

    public class ListaDPVGCONF : List<DPVGCONF>
    {
        ~ListaDPVGCONF()
        {
            if (this != null)
            {
                this.Clear();
            }
        }
    }

    public class FiltroDPVGCONF
    {
        public FiltroDPVGCONF()
        {
            this.PosCliente = 0;
            this.RazonSocial = string.Empty;
        }
        public string RazonSocial { get; set; }

        public int PosCliente { get; set; }
    }
}
