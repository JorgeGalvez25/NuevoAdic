using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SetupNuevoAdicional.Vistas;
using DevExpress.XtraEditors;

namespace SetupNuevoAdicional
{
    public class WorkItem
    {
        public static DevExpress.XtraWizard.WizardControl Wizard { get; set; }
        public static List<IPage> Vistas { get; set; }

        static WorkItem()
        {
            Vistas = new List<IPage>();
            Vistas.Add(new viewBienvenida());
            Vistas.Add(new viewRuta());
            Vistas.Add(new viewBD());
            Vistas.Add(new viewConfirmacion());
            Vistas.Add(new viewProcesando());
            Vistas.Add(new viewError());
            Vistas.Add(new viewFinalizar());

            foreach (XtraUserControl v in Vistas.OfType<XtraUserControl>())
            {
                v.Dock = System.Windows.Forms.DockStyle.Fill;
                v.LookAndFeel.UseDefaultLookAndFeel = false;
                v.LookAndFeel.UseWindowsXPTheme = true;
                v.Appearance.Options.UseBackColor = true;
                v.BackColor = System.Drawing.Color.Transparent;
            }

            // 
            // wizard
            // 
            Wizard = new DevExpress.XtraWizard.WizardControl();
            ((System.ComponentModel.ISupportInitialize)(Wizard)).BeginInit();
            Wizard.SuspendLayout();

            Wizard.Controls.AddRange(Vistas.Select(i => i.Parent).ToArray());
            Wizard.Name = "wizard";
            Wizard.Pages.AddRange(Vistas.Select(i => i.Parent).ToArray());
            Wizard.Text = "Instalación de la consola del Adicional de Gasolineras";
            Wizard.WizardStyle = DevExpress.XtraWizard.WizardStyle.WizardAero;
            Wizard.PreviousText = "Atrás";
            Wizard.NextText = "Siguiente";
            Wizard.CancelText = "Cancelar";
            Wizard.FinishText = "Finalizar";

            ((System.ComponentModel.ISupportInitialize)(Wizard)).EndInit();
            Wizard.ResumeLayout(false);
        }

        #region Objetos

        public class Obj<T> where T : class
        {
            public Obj()
            {
            }

            public Obj(T valor)
            {
                this.Valor = valor;
            }

            public Type tipo { get; set; }

            public T Valor { get; set; }
        }

        public class Objetos<T> where T : class
        {
            private static Dictionary<string, Obj<T>> objetos = new Dictionary<string, Obj<T>>();

            public static void Add(string llave, T valor)
            {
                if (objetos.ContainsKey(llave))
                {
                    objetos[llave] = new Obj<T>(valor);
                }
                else
                {
                    objetos.Add(llave, new Obj<T>(valor));
                }
            }

            public static void Delete(string llave)
            {
                if (objetos.ContainsKey(llave))
                {
                    objetos.Remove(llave);
                }
            }

            public static T Get(string llave)
            {
                if (objetos.ContainsKey(llave))
                {
                    return objetos[llave].Valor;
                }

                return null;
            }

            public static bool Exist(string llave)
            {
                return objetos.ContainsKey(llave);
            }

            internal static System.IO.DirectoryInfo Get()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
