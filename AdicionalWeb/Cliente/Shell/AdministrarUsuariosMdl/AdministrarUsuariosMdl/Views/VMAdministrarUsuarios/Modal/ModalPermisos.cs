using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EstandarCliente.AdministrarUsuariosMdl.Constants;
using EstandarCliente.CargadorVistas.Constants;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl.Views.VMAdministrarUsuarios.Modal
{
    public partial class ModalPermisos : DevExpress.XtraEditors.XtraForm
    {
        private class Estatus
        {
            public Estatus()
            {
                this.Id = string.Empty;
                this.Permisos = new ListaPermisos();
            }

            public string Id { get; set; }

            public ListaPermisos Permisos { get; set; }

            ~Estatus()
            {
                if (this.Permisos != null)
                {
                    this.Permisos.Clear();
                }
            }
        }

        public bool ReadOnly = false;
        public ListaPermisos Resultado = new ListaPermisos();

        private SesionModuloWeb Sesion;
        private ListaPermisos _permiso;
        private ListaPermisos Permiso
        {
            get
            {
                if (_permiso == null)
                {
                    _permiso = new ListaPermisos();
                    if (Sesion.Usuario.Clave == 0)
                    {
                        _permiso.FromXML(this.Sesion.Usuario.Variables[0]);
                    }
                }

                return _permiso;
            }
        }
        private AdministrarUsuarios usuario;
        private VMAdministrarUsuariosPresenter Presenter;
        private Dictionary<string, Estatus> auxEstatus = new Dictionary<string, Estatus>();

        public ModalPermisos(VMAdministrarUsuariosPresenter presenter, AdministrarUsuarios usuario)
        {
            InitializeComponent();
            this.Presenter = presenter;
            this.usuario = usuario;
            this.Sesion = this.Presenter.WorkItem.RootWorkItem.Items[ConstantesModulo.SESION_SISTEMA] as SesionModuloWeb;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InicializarControles();
            this.chkMostrar_CheckedChanged(null, null);
            if (this.ReadOnly)
            {
                this.SetReadOnly();
            }
            this.CrearEventos();
        }

        private void CrearEventos()
        {
            this.gridView1.FocusedRowChanged += this.gridView1_FocusedRowChanged;
            if (!this.ReadOnly)
            {
                this.btnAceptar.Click += this.btnAceptar_Click;
                this.btnCancelar.Click += this.btnCancelar_Click;
                this.chkMostrar.CheckedChanged += this.chkMostrar_CheckedChanged;
                this.chkRegistrar.CheckedChanged += this.chkRegistrar_CheckedChanged;
                this.chkModificar.CheckedChanged += this.chkModificar_CheckedChanged;
            }
        }
        private void SetEstatus(String item)
        {
            if (this.auxEstatus.ContainsKey(item))
            {
                this.auxEstatus[item] = new Estatus()
                {
                    Id = item,
                    Permisos = new ListaPermisos()
                            {
                                this.ObtenerPermisosActuales(item)
                            }
                };
            }
            else
            {
                this.auxEstatus.Add(item, new Estatus()
                    {
                        Id = item,
                        Permisos = new ListaPermisos()
                            {
                                this.ObtenerPermisosActuales(item)
                            }
                    });
            }
        }
        private Estatus GetEstatus(String item)
        {
            return (this.auxEstatus.ContainsKey(item)) ? this.auxEstatus[item] : null;
        }
        private Permisos ObtenerPermisosActuales(string item)
        {
            Permisos permiso = new Permisos();
            {
                permiso.Id = item;// Permiso para el Modulo
                permiso.Nombre = ConstantesEntidad.ListaModulos[item];
                permiso.Permitido = this.chkMostrar.Checked;
                permiso.SubPermisos.AddRange(new Permisos[]
                    {
                        new Permisos() // Permiso Mostrar
                        {
                            Id = ConstantesPermisos.Operaciones.OPERACION_MOSTRAR,
                            Nombre = ConstantesPermisos.Operaciones.OPERACION_MOSTRAR,
                            Permitido = this.chkMostrar.Checked
                        },new Permisos() // Permiso Registrar
                        {
                            Id = ConstantesPermisos.Operaciones.OPERACION_REGISTRAR,
                            Nombre = ConstantesPermisos.Operaciones.OPERACION_REGISTRAR,
                            Permitido =this.chkMostrar.Checked ? this.chkRegistrar.Checked : false,
                            SubPermisos = new ListaPermisos()
                                {
                                    new Permisos() // Permiso para editar desface (solo para el modulo clientes)
                                             {
                                                 Id = ConstantesPermisos.Opciones.OPCION_DESFACE_REGISTRAR,
                                                 Nombre = "Desface",
                                                 Permitido = this.chkMostrar.Checked ? (this.chkRegistrar.Checked ? this.chkCambiarDesfaceRegistrar.Checked : false) : false
                                             }
                                }
                        }, new Permisos()// Permiso Modificar
                        {
                            Id = ConstantesPermisos.Operaciones.OPERACION_MODIFICAR,
                            Nombre = ConstantesPermisos.Operaciones.OPERACION_MODIFICAR,
                            Permitido = this.chkMostrar.Checked ? this.chkModificar.Checked : false,
                            SubPermisos = new ListaPermisos()
                                 {
                                     new Permisos()// Permiso para cambiar contraseña
                                         {
                                              Id = ConstantesPermisos.Opciones.OPCION_CAMBIAR_CONTRASEÑA,
                                              Nombre="Cambiar contraseña",
                                              Permitido = this.chkMostrar.Checked ? (this.chkModificar.Checked ? this.chkCambiarPassword.Checked : false) : false
                                         },
                                     new Permisos() //Permiso para cambiar entre activo o no
                                        {
                                              Id = ConstantesPermisos.Opciones.OPCION_ACTIVAR,
                                              Nombre="Activar",
                                              Permitido = this.chkMostrar.Checked ? (this.chkModificar.Checked ? this.chkCambiarActivo.Checked : false) : false
                                         },
                                     new Permisos()// Permiso para modificar desface (Solo Modulo cliente)
                                         {
                                             Id = ConstantesPermisos.Opciones.OPCION_DESFACE_MODIFICAR,
                                             Nombre = "Desface",
                                             Permitido = this.chkMostrar.Checked ? (this.chkModificar.Checked ? this.chkCambiarDesface.Checked : false) : false
                                         }
                                 }
                        }, new Permisos()// Permiso Eliminar
                        {
                            Id = ConstantesPermisos.Operaciones.OPERACION_ELIMINAR,
                            Nombre = ConstantesPermisos.Operaciones.OPERACION_ELIMINAR,
                            Permitido = this.chkMostrar.Checked ? this.chkEliminar.Checked : false
                        }
                    });
            }
            return permiso;
        }

        private void SetReadOnly()
        {
            this.chkMostrar.BeginSafe(delegate { this.chkMostrar.Enabled = !this.ReadOnly; });
            this.chkRegistrar.BeginSafe(delegate { this.chkRegistrar.Enabled = !this.ReadOnly; });
            this.chkModificar.BeginSafe(delegate { this.chkModificar.Enabled = !this.ReadOnly; });
            this.chkEliminar.BeginSafe(delegate { this.chkEliminar.Enabled = !this.ReadOnly; });
            this.grpModificar.BeginSafe(delegate { this.grpModificar.Enabled = !this.ReadOnly; });
            this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Enabled = !this.ReadOnly; });
        }
        private void InicializarControles()
        {
            this.gridControl1.BeginSafe(delegate { this.gridControl1.DataSource = ConstantesEntidad.ListaModulos.Values.ToArray(); });

            this.grpModificar.BeginSafe(delegate { this.grpModificar.Visible = false; });
            this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Visible = false; });

            this.BeginSafe(delegate
            {
                if (this.usuario.Permisos.Count > 0)
                {
                    this.usuario.Permisos.ForEach(p =>
                    {
                        this.auxEstatus.Add(p.Id, new Estatus()
                            {
                                Id = p.Id,
                                Permisos = new ListaPermisos()
                                {
                                    p
                                }
                            });
                    });

                    var item = this.usuario.Permisos.BuscarEntidadPermiso(ConstantesPermisos.Modulos.CLIENTES, false);
                    if (item != null)
                    {
                        var estatus = this.GetEstatus(item.Id);//FirstOrDefault().Id);
                        this.CambiarControles(estatus);
                    }
                }
            });
        }
        private void CambiarControles(Estatus estatus)
        {
            this.BeginSafe(delegate
                {
                    this.chkMostrar.BeginSafe(delegate { this.chkMostrar.Checked = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_MOSTRAR, true)); });
                    this.chkRegistrar.BeginSafe(delegate { this.chkRegistrar.Checked = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_REGISTRAR, true)); });
                    this.chkModificar.BeginSafe(delegate { this.chkModificar.Checked = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_MODIFICAR, true)); });
                    this.chkEliminar.BeginSafe(delegate { this.chkEliminar.Checked = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_ELIMINAR, true)); });

                    this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Visible = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_REGISTRAR, true)); });
                    this.grpModificar.BeginSafe(delegate { this.grpModificar.Visible = (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Operaciones.OPERACION_MODIFICAR, true)); });
                    this.chkCambiarPassword.BeginSafe(delegate
                        {
                            var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
                            this.chkCambiarPassword.Visible = (key.Equals(ConstantesPermisos.Modulos.USUARIOS));
                            this.chkCambiarPassword.Checked = this.chkModificar.Checked ? (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Opciones.OPCION_CAMBIAR_CONTRASEÑA, true)) : false;
                        });
                    this.chkCambiarDesface.BeginSafe(delegate
                        {
                            var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
                            this.chkCambiarDesface.Visible = (key.Equals(ConstantesPermisos.Modulos.CLIENTES));
                            this.chkCambiarDesface.Checked = this.chkModificar.Checked ? (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Opciones.OPCION_DESFACE_MODIFICAR, true)) : false;
                        });

                    this.chkCambiarDesfaceRegistrar.BeginSafe(delegate
                    {
                        var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
                        this.chkCambiarDesfaceRegistrar.Visible = (key.Equals(ConstantesPermisos.Modulos.CLIENTES));
                        this.chkCambiarDesfaceRegistrar.Checked = this.chkRegistrar.Checked ? (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Opciones.OPCION_DESFACE_REGISTRAR, true)) : false;
                    });

                    this.chkCambiarActivo.BeginSafe(delegate { this.chkCambiarActivo.Checked = this.chkModificar.Checked ? (estatus == null ? false : estatus.Permisos.BuscarPermiso(ConstantesPermisos.Opciones.OPCION_ACTIVAR, true)) : false; });
                    this.chkMostrar_CheckedChanged(null, null);
                    this.chkRegistrar_CheckedChanged(null, null);
                    //this.chkModificar_CheckedChanged(null, null);
                });
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var x = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
            this.SetEstatus(x);
            if (this.Resultado == null) { this.Resultado = new ListaPermisos(); }
            this.auxEstatus.Select(p => p.Value.Permisos).ToList().ForEach(p =>
            {
                this.Resultado.AddRange(p.ToArray());
            });
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;

            switch (key)
            {
                case ConstantesPermisos.Modulos.MONITOR_CONEXIONES:
                case ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES:
                case ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS:
                    this.chkRegistrar.BeginSafe(delegate { this.chkRegistrar.Enabled = false; });
                    this.chkModificar.BeginSafe(delegate { this.chkModificar.Enabled = false; });
                    this.chkEliminar.BeginSafe(delegate { this.chkEliminar.Enabled = false; });
                    this.grpModificar.BeginSafe(delegate
                    {
                        this.grpModificar.Visible = false;
                        this.grpModificar.Enabled = false;
                    });
                    this.grpRegistrar.BeginSafe(delegate
                    {
                        this.grpRegistrar.Visible = false;
                        this.grpRegistrar.Enabled = false;
                    });
                    return;
                case ConstantesPermisos.Modulos.CAMBIO_PRECIOS:
                    this.grpModificar.BeginSafe(delegate
                    {
                        this.grpModificar.Visible = false;
                        this.grpModificar.Enabled = false;
                    });
                    this.grpRegistrar.BeginSafe(delegate
                    {
                        this.grpRegistrar.Visible = false;
                        this.grpRegistrar.Enabled = false;
                    });
                    break;
                case ConstantesPermisos.Modulos.CLIENTES:
                    this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Enabled = (this.chkRegistrar.CheckState == CheckState.Checked); });
                    break;
            }

            bool isActive = (this.chkMostrar.CheckState == CheckState.Checked);
            this.chkRegistrar.BeginSafe(delegate { this.chkRegistrar.Enabled = isActive; });
            this.chkModificar.BeginSafe(delegate { this.chkModificar.Enabled = isActive; });
            this.chkEliminar.BeginSafe(delegate { this.chkEliminar.Enabled = isActive; });
            this.grpModificar.BeginSafe(delegate { this.grpModificar.Enabled = isActive; });
        }

        private void chkRegistrar_CheckedChanged(object sender, EventArgs e)
        {
            var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;

            switch (key)
            {
                case ConstantesPermisos.Modulos.USUARIOS:
                case ConstantesPermisos.Modulos.CAMBIO_PRECIOS:
                case ConstantesPermisos.Modulos.DISTRIBUIDORES:
                case ConstantesPermisos.Modulos.MONITOR_CONEXIONES:
                case ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES:
                case ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS:
                    this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Visible = false; });
                    return;
                case ConstantesPermisos.Modulos.CLIENTES:
                default:
                    this.grpRegistrar.BeginSafe(delegate { this.grpRegistrar.Visible = this.chkRegistrar.Checked; this.grpRegistrar.Enabled = true; });
                    break;
            }
        }
        private void chkModificar_CheckedChanged(object sender, EventArgs e)
        {
            var key = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(this.gridView1.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;

            switch (key)
            {
                case ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS:
                case ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES:
                case ConstantesPermisos.Modulos.MONITOR_CONEXIONES:
                    return;
                case ConstantesPermisos.Modulos.CAMBIO_PRECIOS:
                    this.grpModificar.BeginSafe(delegate { this.grpModificar.Visible = false; });
                    return;
                default:
                    this.grpModificar.BeginSafe(delegate { this.grpModificar.Visible = this.chkModificar.Checked; });
                    break;
            }

            var permiso = this.Permiso.FirstOrDefault(p => p.Id.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            var existe = (permiso != null);

            this.chkCambiarDesface.BeginSafe(delegate
                {
                    this.chkCambiarDesface.Visible = (key.Equals(ConstantesPermisos.Modulos.CLIENTES, StringComparison.CurrentCultureIgnoreCase));
                });

            this.chkCambiarActivo.BeginSafe(delegate
                {
                    if (existe)
                    {
                        var aux = permiso.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Opciones.OPCION_ACTIVAR, StringComparison.CurrentCultureIgnoreCase));

                        var itemExiste = (aux != null);
                        this.chkCambiarActivo.Checked = existe && itemExiste;
                    }
                    else
                    {
                        this.chkCambiarActivo.Checked = false;
                    }
                });
            this.chkCambiarPassword.BeginSafe(delegate
                {
                    if (key.Equals(ConstantesPermisos.Modulos.USUARIOS, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.chkCambiarPassword.Visible = true;
                        if (existe)
                        {
                            var aux = permiso.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Opciones.OPCION_CAMBIAR_CONTRASEÑA, StringComparison.CurrentCultureIgnoreCase));

                            var itemExiste = (aux != null);
                            this.chkCambiarPassword.Checked = existe && itemExiste;
                        }
                        else
                        {
                            this.chkCambiarPassword.Checked = false;
                        }
                    }
                    else
                    {
                        this.chkCambiarPassword.Checked = false;
                        this.chkCambiarPassword.Visible = false;
                    }
                });
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var x = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(e.PrevFocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
            this.SetEstatus(x);
            x = ConstantesEntidad.ListaModulos.FirstOrDefault(p => p.Value.Equals(this.gridView1.GetRow(e.FocusedRowHandle).ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
            var estatus = this.GetEstatus(x);
            this.CambiarControles(estatus);
            if (this.ReadOnly)
            {
                this.SetReadOnly();
            }
        }
    }
}