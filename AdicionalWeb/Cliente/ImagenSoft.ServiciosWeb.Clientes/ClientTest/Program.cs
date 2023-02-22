using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace ClientTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //for (int i = 0; i < 5; i++)
            //{
            //    new Thread(new ThreadStart(() =>
            //        {
            //            //new Form1();
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new Form1());
            //        })).Start();
            //}
        }
    }
}
