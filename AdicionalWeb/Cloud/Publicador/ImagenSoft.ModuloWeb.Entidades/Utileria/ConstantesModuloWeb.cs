using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagenSoft.ModuloWeb.Entidades
{
    public class ConstantesModuloWeb
    {
        public const string ID_MODULO = "Modulo Web";

        public class Roles
        {
            public const string MAESTRO = "Maestro";

            public const string INVITADO = "Invitado";

            public const string OPERADOR = "Operador";

            public static readonly List<string> RolesModulo = new List<string>() { Roles.MAESTRO, Roles.INVITADO, Roles.OPERADOR };
        }
    }
}
