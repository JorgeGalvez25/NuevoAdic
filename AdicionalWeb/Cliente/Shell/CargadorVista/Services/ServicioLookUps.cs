using System;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Interfaces;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServicioLookUps : ILookUpsFACELEI<LookUpEdit, Object>, ILookUpsFACELEI<RepositoryItemLookUpEdit, Object>
    {
        public WorkItem _WorkItem;

        public ServicioLookUps(WorkItem workitem)
        {
            _WorkItem = workitem;
        }

        #region ILookUpsFACELEI<LookUpEdit,object> Members

        public void CrearLookUp(LookUpEdit t, string lookUp, object f)
        {
            switch (lookUp)
            {
                case ConstantesFACELEI.LOOKUPS.EJERCICIO: luEjercicio(t, f); break;
                case ConstantesFACELEI.LOOKUPS.PACS: luPAC(t, f); break;
            }
        }

        #endregion

        #region ILookUpsFACELEI<RepositoryItemLookUpEdit,object> Members

        public void CrearLookUp(RepositoryItemLookUpEdit t, string lookUp, object f)
        {
            switch (lookUp)
            {
                case ConstantesFACELEI.REPOSITORYLOOKUPS.PACS: repositoryLuPAC(t, f); break;
            }
        }

        #endregion

        #region Privadas

        private void luEjercicio(LookUpEdit t, object f)
        {
            ListaEjercicioFACELEI lista = null;

            IServiciosFechaHoraServidor servicios =
                _WorkItem.RootWorkItem.WorkItems[ConstantesFACELEI.MODULOS.UTILERIASFACELEI].Services.Get<IServiciosFechaHoraServidor>();

            if (servicios != null)
            {
                DateTime fecha = servicios.ObtenerFechaHora();

                lista = new ListaEjercicioFACELEI();
                while (lista.Count < 10)
                {
                    lista.Add(new EjercicioFACELEI(fecha.Year - lista.Count));
                }

                t.Properties.HeaderClickMode = DevExpress.XtraEditors.Controls.HeaderClickMode.AutoSearch;
                t.Properties.Columns.Clear();
                t.Properties.PopupFormMinSize = new System.Drawing.Size(20, 100);
                t.Properties.PopupSizeable = false;
                t.Properties.ShowFooter = false;
                t.Properties.PopupWidth = 30;

                LookUpColumnInfo columna = new LookUpColumnInfo();
                columna.FieldName = "Año";
                columna.Caption = "Año";
                columna.FormatType = FormatType.Custom;
                columna.Width = 1;
                t.Properties.Columns.Add(columna);

                t.Properties.DisplayMember = "Año";
                t.Properties.ValueMember = "Año";

                t.Properties.DataSource = lista;
                t.EditValue = fecha.Year;
            }
            else
            {
                throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesFACELEI.MODULOS.UTILERIASFACELEI));
            }
        }

        private void luPAC(LookUpEdit t, object f)
        {
            IServiciosModulo<ListaPacFACELEI, PacFACELEI, FiltroPacFACELEI, FiltroPacFACELEI> servicio =
                _WorkItem.RootWorkItem.WorkItems[ConstantesFACELEI.MODULOS.PACS].Services.Get<IServiciosModulo<ListaPacFACELEI, PacFACELEI, FiltroPacFACELEI, FiltroPacFACELEI>>();

            if (servicio == null) throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesFACELEI.MODULOS.PACS));

            var lista = servicio.ObtenerTodosFiltro((f as FiltroPacFACELEI));

            t.Properties.HeaderClickMode = DevExpress.XtraEditors.Controls.HeaderClickMode.AutoSearch;
            t.Properties.PopupSizeable = false;
            t.Properties.ShowFooter = false;
            t.Properties.ShowHeader = true;
            t.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            t.Properties.Columns.Clear();

            LookUpColumnInfo columna = new LookUpColumnInfo();
            columna.FormatType = DevExpress.Utils.FormatType.Custom;
            columna.FormatString = ConstantesFACELEI.VISTAS.LONGITUD_SUCURSAL;
            columna.FieldName = "Clave";
            columna.Caption = "Clave";
            columna.Width = 3;
            t.Properties.Columns.Add(columna);

            columna = new LookUpColumnInfo();
            columna.FieldName = "Nombre";
            columna.Caption = "Nombre";
            t.Properties.Columns.Add(columna);

            t.Properties.ValueMember = "Clave";
            t.Properties.DisplayMember = "Nombre";

            if (lista.Count == 0)
            {
                t.Properties.NullText = "No existen PAC.";
                t.Properties.DataSource = null;
                t.EditValue = null;
            }
            else
            {
                t.Properties.NullText = "Seleccione...";
                lista.Insert(0, new PacFACELEI() { Clave = 0, Nombre = "Todos los PAC" });
                t.Properties.DataSource = lista;
                t.EditValue = lista[0].Clave;

                if (lista.Count > 10)
                {
                    t.Properties.PopupFormMinSize = new System.Drawing.Size(140, 100);
                }
                else
                {
                    t.Properties.DropDownRows = lista.Count;
                }
            }
        }

        private void repositoryLuPAC(RepositoryItemLookUpEdit t, object f)
        {
            IServiciosModulo<ListaPacFACELEI, PacFACELEI, FiltroPacFACELEI, FiltroPacFACELEI> servicio =
                _WorkItem.RootWorkItem.WorkItems[ConstantesFACELEI.MODULOS.PACS].Services.Get<IServiciosModulo<ListaPacFACELEI, PacFACELEI, FiltroPacFACELEI, FiltroPacFACELEI>>();

            if (servicio == null) throw new Exception(string.Format(ListadoMensajes.Error_Modulo_No_Encontrado, ConstantesFACELEI.MODULOS.PACS));

            var lista = servicio.ObtenerTodosFiltro((f as FiltroPacFACELEI));

            t.Properties.HeaderClickMode = DevExpress.XtraEditors.Controls.HeaderClickMode.AutoSearch;
            t.Properties.PopupSizeable = false;
            t.Properties.ShowFooter = false;
            t.Properties.ShowHeader = true;
            t.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            t.Properties.Columns.Clear();

            LookUpColumnInfo columna = new LookUpColumnInfo();
            columna.FormatType = DevExpress.Utils.FormatType.Custom;
            columna.FormatString = ConstantesFACELEI.VISTAS.LONGITUD_SUCURSAL;
            columna.FieldName = "Clave";
            columna.Caption = "Clave";
            columna.Width = 3;
            t.Properties.Columns.Add(columna);

            columna = new LookUpColumnInfo();
            columna.FieldName = "Nombre";
            columna.Caption = "Nombre";
            t.Properties.Columns.Add(columna);

            t.Properties.ValueMember = "Clave";
            t.Properties.DisplayMember = "Nombre";

            if (lista.Count == 0)
            {
                t.Properties.NullText = "No existen PAC.";
                t.Properties.DataSource = null;
            }
            else
            {
                t.Properties.NullText = "Sin PAC Default";
                t.Properties.DataSource = lista;

                if (lista.Count > 10)
                {
                    t.Properties.PopupFormMinSize = new System.Drawing.Size(140, 100);
                }
                else
                {
                    t.Properties.DropDownRows = lista.Count;
                }
            }
        }

        #endregion
    }
}
