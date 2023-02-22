using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consola.Logic.Entities;
using Consola.Logic.Persistence;
using Consola.Connect;

namespace Consola.Logic
{
    public class SerivicioLogica
    {
        #region Combustible

        public DPVGTCMB ActualizarCombustible(DPVGTCMB e)
        {
            DPVGTCMBPersistence servicio = new DPVGTCMBPersistence();
            return servicio.ActualizarDPVGTCMB(e);
        }

        public DPVGTCMB ObtenerCombustible(FiltroDPVGTCMB f)
        {
            DPVGTCMBPersistence servicio = new DPVGTCMBPersistence();
            return servicio.ObtenerDPVGTCMB(f);
        }

        public ListaDPVGTCMB ObtenerTodosCombustible(FiltroDPVGTCMB f)
        {
            DPVGTCMBPersistence servicio = new DPVGTCMBPersistence();
            return servicio.ObtenerTodosDPVGTCMB(f);
        }

        #endregion

        #region Bomba

        public DPVGBOMB ObtenerBomba(FiltroDPVGBOMB f)
        {
            DPVGBOMBPersistencia servicio = new DPVGBOMBPersistencia();
            return servicio.ObtenerDPVGBOMB(f);
        }

        public ListaDPVGBOMB ObtenerTodosBomba(FiltroDPVGBOMB f)
        {
            DPVGBOMBPersistencia servicio = new DPVGBOMBPersistencia();
            return servicio.ObtenerTodosDPVGBOMB(f);
        }

        #endregion

        #region Listener

        private static ListenerPosCliente srvListener = new ListenerPosCliente();

        public void ListenPosClie(Action<string> fn)
        {
            if (srvListener == null) { srvListener = new ListenerPosCliente(); }
            try { srvListener.Listener(fn); }
            catch { }
        }

        public void UnListenPosClie()
        {
            if (srvListener != null)
            {
                srvListener.UnListen();
            }
        }

        public bool ActualizarPosClie(FiltroDPVGCONF f)
        {
            bool result = false;

            if (srvListener != null)
            {
                result = srvListener.ActualizarPosCliente(f);
            }

            return result;
        }

        public ListaDPVGCMND ObtenerTodosComandos(FiltroDPVGCMND f)
        {
            return srvListener.ObtenerTodosComandos(f);
        }

        #endregion

        public ListaDPVGESTS ObtenerTodosDPVGESTS(FiltroDPVGESTS f)
        {
            DPVGESTSPersistencia servicio = new DPVGESTSPersistencia();
            return servicio.ObtenerTodosDPVGESTS(f);
        }

        public SerialConnectionConfig GetSerialConfig()
        {
            return srvListener.GetSerialConfig();
        }
    }
}
