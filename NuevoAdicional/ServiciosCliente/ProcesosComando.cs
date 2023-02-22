using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;

namespace ServiciosCliente
{
    public class ProcesosComando
    {
        private int InsertarComando(Comandos AComando)
        {
            return new ComandosPersistencia().ComandoInsertarReturning(AComando).Folio;
        }

        private string GetRespuestaComando(int AFolioComando)
        {
            bool pSigue = true;
            string pResult = string.Empty;
            int pContador = 0;

            do
            {
                var pComando = new ComandosPersistencia().ComandosObtener(AFolioComando);

                if (pComando.Resultado.Trim().Length > 0)
                {
                    pResult = pComando.Resultado;
                    pSigue = false;
                }
                else
                {
                    pContador++;
                    if (pContador >= 60)
                    {
                        pSigue = false;
                    }
                    System.Threading.Thread.Sleep(500);
                }

            } while (pSigue == true);

            if (pResult.Trim().Length == 0)
            {
                new ComandosPersistencia().ComandosEliminar(AFolioComando);
            }

            return pResult;
        }

        public bool AplicaComando(bool std, bool paro, out string AMensajeRespuesta)
        {
            AMensajeRespuesta = string.Empty;

            Comandos pComando = new Comandos();
            pComando.Modulo = "DISP";
            if (paro)
                pComando.Comando = "FLUCERO";
            else
                pComando.Comando = std ? "FLUSTD" : "FLUMIN";
            pComando.Aplicado = "No";

            string ComandosPorServicio;
            if (!Utilerias.ObtenerListaVar().TryGetValue("ComandosPorServicio", out ComandosPorServicio))
                ComandosPorServicio = "No";
            if (ComandosPorServicio == "Si")
            {
                string servConsola;
                if (!Utilerias.ObtenerListaVar().TryGetValue("PuertoServicio", out servConsola))
                    servConsola = "http://127.0.0.1:9199/bin/";
                AMensajeRespuesta = new ServicioDisp(servConsola).EjecutaComando(pComando.Comando);
            }
            else
            {
                int pFolioComando = InsertarComando(pComando);

                AMensajeRespuesta = GetRespuestaComando(pFolioComando);
            }

            //if (std)
            //{
            //    new ComandosPersistencia().ComandosEliminar(pFolioComando);
            //}

            return true;
        }

        public bool AplicaComando(string comando, out string mensajeRespuesta)
        {
            mensajeRespuesta = string.Empty;

            Comandos pComando = new Comandos();
            pComando.Modulo = "DISP";
            pComando.Comando = comando;
            pComando.Aplicado = "No";

            string ComandosPorServicio;
            if (!Utilerias.ObtenerListaVar().TryGetValue("ComandosPorServicio", out ComandosPorServicio))
                ComandosPorServicio = "No";

            if (ComandosPorServicio == "Si")
            {
                string servConsola;
                if (!Utilerias.ObtenerListaVar().TryGetValue("PuertoServicio", out servConsola))
                    servConsola = "http://127.0.0.1:9199/bin/";
                mensajeRespuesta = new ServicioDisp(servConsola).EjecutaComando(pComando.Comando);
            }
            else
            {
                int pFolioComando = InsertarComando(pComando);

                mensajeRespuesta = GetRespuestaComando(pFolioComando);
            }
            return true;
        }

        public void ActualizaComando(string comando)
        {
            new ComandosPersistencia().ActualizaComando1(comando);
        }
    }
}