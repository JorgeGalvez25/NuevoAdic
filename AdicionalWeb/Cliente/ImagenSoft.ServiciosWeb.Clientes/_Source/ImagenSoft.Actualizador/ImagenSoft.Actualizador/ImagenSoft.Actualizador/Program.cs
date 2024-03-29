﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImagenSoft.Actualizador
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UpdaterForm(args.Length > 0 ? args[0] ?? string.Empty : string.Empty));
        }
    }
}
