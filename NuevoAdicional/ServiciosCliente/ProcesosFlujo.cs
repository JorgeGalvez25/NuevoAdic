using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using Persistencia;

namespace ServiciosCliente
{
    public class ProcesosFlujo
    {
        private void GuardarFlujo(List<Flujo> AListaFlujo)
        {
            FlujoPersistencia pFlujoPersistencia = new FlujoPersistencia();

            pFlujoPersistencia.FlujoEliminar();

            foreach (var flujo in AListaFlujo)
            {
                pFlujoPersistencia.FlujoInsertar(flujo);
            }
        }

        public void AplicarFlujo(List<Historial> AListaHistorial)
        {
            string[] porcentajes = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
            Dictionary<int, string> comb = new Dictionary<int, string>();
            var pListaFlujo = new ListaFlujo();
            int pPosicion = 0;
            Flujo pFlujo = null;

            foreach (var h in AListaHistorial)
            {
                if (pPosicion != h.Posicion)
                {
                    pFlujo = new Flujo();
                    pFlujo.Poscarga = h.Posicion;
                    pPosicion = h.Posicion;
                    pListaFlujo.Add(pFlujo);
                }

                if (!comb.ContainsKey(h.Combustible))
                {
                    comb.Add(h.Combustible, porcentajes[(int)h.Porcentaje]);
                }

                switch (h.Manguera)//(h.Combustible)
                {
                    case 1: pFlujo.Slowflow = ((double)h.Porcentaje) / 10; break;
                    case 2: pFlujo.Slowflow2 = ((double)h.Porcentaje) / 10; break;
                    case 3: pFlujo.Slowflow3 = ((double)h.Porcentaje) / 10; break;
                }
            }

            if (!comb.ContainsKey(2) && comb.ContainsKey(1))
            {
                comb.Add(2, comb[1]);
            }
            else if (!comb.ContainsKey(2) && !comb.ContainsKey(1))
            {
                comb.Add(1, "A");
                comb.Add(2, "A");
            }
            else if (!comb.ContainsKey(1))
            {
                comb.Add(1, comb[2]);
            }

            if (!comb.ContainsKey(3))
            {
                comb.Add(3, "A");
            }

            GuardarFlujo(pListaFlujo);
            actualizarDPVGCMB(comb);
        }

        private void actualizarDPVGCMB(Dictionary<int, string> combs)
        {
            CombustiblePersistencia persistencia = new CombustiblePersistencia();

            foreach (var item in combs)
            {
                persistencia.CombustibleActualizar(item.Key, item.Value);
            }
        }
    }
}
