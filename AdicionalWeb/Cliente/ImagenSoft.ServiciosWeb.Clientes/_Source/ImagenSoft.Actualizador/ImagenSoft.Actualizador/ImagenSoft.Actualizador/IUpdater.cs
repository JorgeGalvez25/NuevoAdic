using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagenSoft.Actualizador
{
    public interface IUpdater
    {
        int MaxPbar { set; }
        bool Finalizado { set; }
        bool Error { set; }
        string Errores { set; }

        void actualizar(string valor);
    }
}
