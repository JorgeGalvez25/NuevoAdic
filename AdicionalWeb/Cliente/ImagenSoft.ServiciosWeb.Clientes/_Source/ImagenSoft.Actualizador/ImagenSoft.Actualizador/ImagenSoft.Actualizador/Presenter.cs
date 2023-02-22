using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.Actualizador.Entidades;
using ImagenSoft.Actualizador.Proveedor;

namespace ImagenSoft.Actualizador
{
    public class Presenter
    {
        IUpdater _updater;

        private List<string> errores = new List<string>();

        public Presenter(IUpdater view)
        {
            _updater = view;
        }

        public void Actualizar()
        {
            try
            {
                ListaEnsamblado ensamblados = ServiciosActualizador.CompararVersiones();
                string rutaEnsamblados = System.Configuration.ConfigurationSettings.AppSettings["RutaEnsamblados"];

                if (ensamblados.Count == 0)
                {
                    _updater.Finalizado = true;
                    return;
                }

                _updater.MaxPbar = ensamblados.Count;
                ensamblados.ForEach(item =>
                {
                    try
                    {
                        ServiciosActualizador.ObtenerVersionado(item, rutaEnsamblados);
                        _updater.actualizar(item.Nombre);
                    }
                    catch (Exception ex)
                    {
                        errores.Add(string.Format("{0} : {1}", item.Nombre, ex.Message));
                    }
                });
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                _updater.Error = true;
            }
            finally
            {
                _updater.Finalizado = true;
            }

            if (errores.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                errores.ForEach(item =>
                    {
                        sb.AppendLine(item);
                    });

                _updater.Errores = sb.ToString();
            }
        }
    }
}
