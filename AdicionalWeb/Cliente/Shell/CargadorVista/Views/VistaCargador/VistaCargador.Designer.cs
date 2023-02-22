namespace EstandarCliente.CargadorVistas.CargadorVistasMdl
{
    partial class VistaCargador
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private EstandarCliente.CargadorVistas.CargadorVistasMdl.VistaCargadorPresenter _presenter = null;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_presenter != null)
                    _presenter.Dispose();

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaCargador));
            this.tlOpciones = new DevExpress.XtraTreeList.TreeList();
            this.colOpcion = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colClave = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.icIconos = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tlOpciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icIconos)).BeginInit();
            this.SuspendLayout();
            // 
            // tlOpciones
            // 
            this.tlOpciones.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colOpcion,
            this.colClave});
            this.tlOpciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlOpciones.Location = new System.Drawing.Point(0, 0);
            this.tlOpciones.Name = "tlOpciones";
            this.tlOpciones.OptionsBehavior.Editable = false;
            this.tlOpciones.OptionsSelection.UseIndicatorForSelection = true;
            this.tlOpciones.OptionsView.ShowColumns = false;
            this.tlOpciones.OptionsView.ShowHorzLines = false;
            this.tlOpciones.OptionsView.ShowIndicator = false;
            this.tlOpciones.OptionsView.ShowVertLines = false;
            this.tlOpciones.Size = new System.Drawing.Size(210, 490);
            this.tlOpciones.StateImageList = this.icIconos;
            this.tlOpciones.TabIndex = 1;
            // 
            // colOpcion
            // 
            this.colOpcion.Caption = "Opcion";
            this.colOpcion.FieldName = "Opcion";
            this.colOpcion.Name = "colOpcion";
            this.colOpcion.OptionsColumn.AllowFocus = false;
            this.colOpcion.Visible = true;
            this.colOpcion.VisibleIndex = 0;
            // 
            // colClave
            // 
            this.colClave.Caption = "Clave";
            this.colClave.FieldName = "Clave";
            this.colClave.Name = "colClave";
            this.colClave.OptionsColumn.AllowFocus = false;
            // 
            // icIconos
            // 
            this.icIconos.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("icIconos.ImageStream")));
            // 
            // VistaCargador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlOpciones);
            this.Name = "VistaCargador";
            this.Size = new System.Drawing.Size(210, 490);
            ((System.ComponentModel.ISupportInitialize)(this.tlOpciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icIconos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList tlOpciones;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOpcion;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colClave;
        private DevExpress.Utils.ImageCollection icIconos;

    }
}

