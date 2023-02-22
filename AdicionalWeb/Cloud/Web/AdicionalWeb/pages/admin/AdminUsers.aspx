<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Page.Master" AutoEventWireup="true"
    CodeBehind="AdminUsers.aspx.cs" Inherits="AdicionalWeb.pages.admin.AdminUsers"
    Async="true" Buffer="true" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="AdicionalWeb.Extesiones" %>
<asp:Content ID="Content5" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Content" runat="server">
    <%--<div class="tab-content">
        <div role="tabpanel" class="tab-pane active" id="listado" style="background-color: #fff">--%>
    <input type="hidden" id="hdActualRol" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <div>
                <% if (usuarios.Count > 1)
                   { %>
                <div class="row">
                    <div class="navbar-form navbar-left">
                        <input type="text" id="inTblSearch" class="form-control" placeholder="Buscar por usuario..." />
                        <select id="cmbEstaciones" class="btn btn-primary">
                            <option value="Todos" selected>Todas las estaciones</option>
                            <% foreach (var est in usuarios.Select(p => p.NoEstacion).Distinct())
                               {%>
                            <option value="<% Response.Write(est); %>">Solo
                                <% Response.Write(est); %></option>
                            <% } %>
                        </select>
                        <select id="cmbActivo" class="btn btn-primary">
                            <option value="Todos" selected>Todos los registros</option>
                            <option value="Si">Solo Activos</option>
                            <option value="No">Solo Inactivos</option>
                        </select>
                    </div>
                </div>
                <%} if (EsAdministrador)
                   { %>
                <div class="row">
                    <div class="navbar-form navbar-left">
                        <button type="button" class="btn btn-success " data-toggle="modal" data-target="#dlgModal"
                            data-type="registrar">
                            Registrar
                        </button>
                    </div>
                </div>
                <%} %>
            </div>
        </div>
        <div class="panel-body">
            <table class="table table-hover">
                <thead>
                    <% if (usuarios.Count > 1)
                       { %>
                    <tr>
                        <th colspan="5">
                            <small><span id="lblCantidad">
                                <% Response.Write(usuarios.Count); %>
                                registros</span></small>
                        </th>
                    </tr>
                    <%} %>
                    <tr>
                        <th>
                            Usuario
                        </th>
                        <th>
                            Nombre
                        </th>
                        <th>
                            Mail
                        </th>
                        <th class="text-center">
                            Estaci&oacute;n
                        </th>
                        <th class="text-center">
                            Activo
                        </th>
                    </tr>
                </thead>
                <tbody id="tblContentUsers">
                    <% int count = 0;
                       foreach (var usuario in usuarios)
                       { %>
                    <tr data-toggle="popover" data-original-title="Opciones" data-content='{"id":<% Response.Write(count); %>,"user":<% Response.Write(usuario.ToJSON()); %>}'>
                        <td name="usuario">
                            <% Response.Write(usuario.Usuario); %>
                        </td>
                        <td name="nombre">
                            <% Response.Write(usuario.Nombre); %>
                        </td>
                        <td name="mail">
                            <a href="mailto:<% Response.Write(usuario.Correo); %>">
                                <% Response.Write(usuario.Correo); %></a>
                        </td>
                        <td name="estacion" class="text-center">
                            <abbr title="<% var item = estaciones.Find(p=>p.NoEstacion == usuario.NoEstacion);
                            if(item == null)
                            {
                                Response.Write("Sin estación");
                            }else {
                                Response.Write(item.Nombre.Trim().ToLower().ToTitle()); 
                            }%>">
                                <% Response.Write(usuario.NoEstacion); %></abbr>
                        </td>
                        <td name="activo" class="text-center">
                            <i class="fa fa-<% Response.Write(usuario.Activo.Equals("Si", StringComparison.OrdinalIgnoreCase) ? "check-square" : "square"); %>">
                            </i>
                        </td>
                    </tr>
                    <%count++;
                       } %>
                </tbody>
            </table>
        </div>
    </div>
    <%-- </div>
        <div role="tabpanel" class="tab-pane" id="tags">
            <% foreach (var usuario in usuarios)
               { %>
            <div class="col-xs-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4><i class="fa fa-user fa-2x"></i><strong>&nbsp;<% Response.Write(usuario.Usuario); %></strong>
                            <%if (EsAdministrador)
                              { %> <button type="button" class="btn btn-success pull-right" data-toggle="modal"
                                  data-target="#dlgModal" data-type="modificar" data-user="<% Response.Write(usuario.Usuario); %>"
                                  data-nombre="<% Response.Write(usuario.Nombre); %>" data-mail="<% Response.Write(usuario.Correo); %>"
                                  data-estacion="<% Response.Write(usuario.NoEstacion); %>" data-activo="<% Response.Write(usuario.Activo.Equals("Si", StringComparison.OrdinalIgnoreCase) ? "True": "False"); %>">
                                  <i class="fa fa-gear"></i></button><%} %> </h4>
                    </div>
                    <table class="table table-hover">
                        <tbody>
                            <tr>
                                <td>
                                    <strong>Nombre</strong>
                                </td>
                                <td>
                                    <% Response.Write(usuario.Nombre); %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Correo</strong>
                                </td>
                                <td>
                                    <a href="mailto:<% Response.Write(usuario.Correo); %>"><% Response.Write(usuario.Correo); %></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Estaci&oacute;n</strong>
                                </td>
                                <td>
                                    <% Response.Write(usuario.NoEstacion); %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Activo</strong>
                                </td>
                                <td>
                                    <i class="fa fa-<% Response.Write(usuario.Activo.Equals("Si", StringComparison.OrdinalIgnoreCase) ? "check-square" : "square"); %>">
                                    </i>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <% } %>
            <div class="col-xs-4">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="col-xs-2">
                            <i class="fa fa-user fa-4x"></i>
                        </div>
                        <div class="col-xs-9">
                            <div class="row">
                                <strong><abbr title="Horario Ruiz">Usuario</abbr></strong>
                            </div>
                            <div class="row">
                                <small><a href="mailto:horacioruiz@igas.com.mx">horacioruiz@igas.com.mx</a></small>
                            </div>
                            <div class="row">
                                <small><abbr title="Estación Demo">E07651</abbr></small>
                            </div>
                            <div class="row">
                                <small>Activo</small>
                            </div>
                        </div>
                        <div class="col-xs-1">
                            <a href="#" data-toggle="modal" data-target="#dlgModal" data-title="Administrar usuario Usuario"
                                data-user="Usuario" data-nombre="Usuario" data-mail="horacioruiz@igas.com.mx"
                                data-estacion="E07651" data-activo="True"><i class="fa fa-gear"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
    <!-- Modal -->
    <div class="modal fade" id="dlgModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h3 class="modal-title" id="myModalLabel">
                        <span id="ttlPos">&nbsp; <span id="ttlText"></span></span>
                    </h3>
                </div>
                <div class="modal-body" style="padding: 0 30px;">
                    <div id="modal-custom-content">
                        <asp:HiddenField ID="hdValues" runat="server" />
                        <asp:HiddenField ID="hdUsuario" runat="server" />
                        <asp:HiddenField ID="hdEstacion" runat="server" />
                        <asp:HiddenField ID="hdModo" runat="server" />
                        <asp:HiddenField ID="hdPassword" runat="server" />
                        <asp:HiddenField ID="hdNewPassword" runat="server" />
                        <asp:HiddenField ID="hdConfirmPassword" runat="server" />
                        <table id="tblUserData" class="table table-striped">
                            <tbody>
                                <tr id="rowUsuario">
                                    <td>
                                        <strong>Usuario</strong>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Nombre</strong>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Correo</strong>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtMail" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Estaci&oacute;n</strong>
                                    </td>
                                    <td>
                                        <select id="cmbEstacionUsuario" class="btn btn-primary" runat="server">
                                            <option value="Ninguno" selected>Seleccione una...</option>
                                            <%--<% foreach (var est in usuarios.Select(p => p.NoEstacion).Distinct())
                                               {%>
                                                <option value="<% Response.Write(est); %>"><% Response.Write(est); %></option>
                                            <% } %>--%>
                                        </select>
                                    </td>
                                    <td>
                                        <strong>Rol</strong>
                                    </td>
                                    <td>
                                        <select id="cmbRoles" class="btn btn-primary" runat="server">
                                            <option selected="selected" value="Ninguno">Seleccione una...</option>
                                            <option value="Maestro">Maestro</option>
                                            <option value="Invitado">Invitado</option>
                                            <option value="Operador">Operador</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr id="rowActivo">
                                    <td>
                                        <strong>Activo</strong>
                                    </td>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkActivo" runat="server" CssClass="pull-left" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="row">
                            <ul class="nav nav-tabs" role="tablist">
                                <%if (PermisosActuales.Permisos.CambiarPassword)
                                  { %>
                                <li role="apass" class="active"><a href="#passwd" aria-controls="listado" role="tab"
                                    data-toggle="tab">Contrase&ntilde;a</a></li>
                                <%} %>
                                <% if (EsAdministrador)
                                   { %>
                                <li role="apriv"><a href="#priv" aria-controls="tags" role="tab" data-toggle="tab">Privilegios</a>
                                </li>
                                <%} %>
                            </ul>
                            <div class="tab-content">
                                <%if (PermisosActuales.Permisos.CambiarPassword)
                                  { %>
                                <div role="tabpanel" class="tab-pane active" id="passwd" style="background-color: #fff">
                                    <table id="tblPassword" class="table table-striped">
                                        <thead>
                                            <tr>
                                                <th colspan="2">
                                                    <h3>
                                                        <span class="pull-left"><span id="lblPasswordTitle"></span>
                                                            <asp:CheckBox ID="chkChangePassword" runat="server" />
                                                        </span>
                                                    </h3>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr id="rowPassword">
                                                <td>
                                                    <strong id="lblPassword">Contraseña</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox TextMode="Password" Enabled="false" ID="txtPassword" CssClass="form-control"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="rowNewPassword">
                                                <td>
                                                    <strong id="lblNewPassword">Nueva contraseña</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox TextMode="Password" Enabled="false" ID="txtNewPassword" CssClass="form-control"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="rowConfirmNewPassword">
                                                <td>
                                                    <strong id="lblConfirmNewPassword">Confirmar contraseña</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox TextMode="Password" Enabled="false" ID="txtConfirm" CssClass="form-control"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <%} %>
                                <% if (EsAdministrador)
                                   { %>
                                <div role="tabpanel" class="tab-pane" id="priv" style="background-color: #fff">
                                    <table id="tblPrivilegios" class="table table-striped">
                                        <thead>
                                            <tr>
                                                <th colspan="2">
                                                    <h3>
                                                        <span class="pull-left"><span id="lblPrivilegios">Permisos y Privilegios&nbsp;</span>
                                                            <asp:CheckBox ID="chkPrivilegios" runat="server" />
                                                        </span>
                                                    </h3>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="bdyPrivilegios">
                                            <tr id="rowValidacion2Pasos">
                                                <td>
                                                    <strong id="lblValidacion2Pasos">Validaci&oacute;n 2 pasos</strong>
                                                </td>
                                                <td>
                                                    <input id="chkValidacion2Pasos" type="checkbox" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="rowCambiarPass">
                                                <td>
                                                    <strong id="lblCambiarPass">Cambiar Contrase&ntilde;a</strong>
                                                </td>
                                                <td>
                                                    <input id="chkCambiarPass" type="checkbox" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="rowVerEstaciones">
                                                <td>
                                                    <strong id="lblVerEstaciones">Ver todas las estaciones</strong>
                                                </td>
                                                <td>
                                                    <input id="chkVerEstaciones" type="checkbox" runat="server" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <%} %>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnAceptar"
                        runat="server" onserverclick="btnAceptar_ServerClick">
                        <%--onclick="btnAceptar_ServerClick" --%>
                        Aceptar</button>
                    <button type="button" class="btn btn-success" data-dismiss="modal">
                        Cancelar</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function modificar(item, opc) {
            $(item).find('#lblPasswordTitle').html('Modificar contrase&ntilde;a&nbsp;');
            $(item).find('input[id$=hdModo]').val('M');
            $(item).find('input[id$=hdUsuario]').val(opc.Usuario);
            $(item).find('input[id$=hdEstacion]').val(opc.NoEstacion);
            $(item).find('input[id$=txtUsuario]').val(opc.Usuario);
            $(item).find('input[id$=txtNombre]').val(opc.Nombre);
            $(item).find('input[id$=txtMail]').val(opc.Correo);

            $('select[id$=cmbEstacionUsuario] option').each(function() {
                $(this).prop('selected', false);
                if ($(this).val() == opc.NoEstacion) {
                    $(this).prop({ 'selected': true });
                }
            });
            if ($('input[id$=hdActualRol]').val() != '1') {
                $('select[id$=cmbRoles]').prop({ 'disabled': true });
            }
            $('select[id$=cmbRoles] option').each(function() {
                $(this).prop('selected', false);
                if ($(this).val() == opc.Rol) {
                    $(this).prop({ 'selected': true });
                }
            });

            $(item).find('tr[id$=rowUsuario]').show();
            $(item).find('tr[id$=rowPassword]').show();

            if (opc.Activo == 'Si') {
                $(item).find('input[id$=chkActivo]').prop('checked', true);
            } else {
                $(item).find('input[id$=chkActivo]').prop('checked', false);
            }

            if (opc.Usuario != 'Administrador') {
                $(item).find('tr[id$=rowActivo]').show();
                $(item).find('tr[id$=rowUsuario]').show();
            } else {
                $(item).find('tr[id$=rowActivo]').hide();
                $(item).find('tr[id$=rowUsuario]').hide();
            }

            $(item).find('tr[id$=lblPassword]').off('focusout');

            if ($(item).find('input[id$=chkChangePassword]').is(':checked')) {
                $(item).find('input[id$=chkChangePassword]').trigger("click");
            }

            $(item).find('input[id$=chkChangePassword]').show();

            $('.modal-body #bdyPrivilegios input[type=checkbox]').prop({
                'disabled': true,
                'checked': false
            });

            if (opc.Privilegios.Configuraciones.Validacion2Pasos) {
                $(item).find('input[id$=chkValidacion2Pasos]').prop('checked', true);
            }

            if (opc.Privilegios.Permisos.CambiarPassword) {
                $(item).find('input[id$=chkCambiarPass]').prop('checked', true);
            }

            if (opc.Privilegios.Permisos.VerTodasEstaciones) {
                $(item).find('input[id$=chkVerEstaciones]').prop('checked', true);
            }

            if ($(item).find('input[id$=chkPrivilegios]').is(':checked')) {
                $(item).find('input[id$=chkPrivilegios]').trigger("click");
            } else {
                $('.modal-body #bdyPrivilegios input[type=checkbox]').prop({ 'disabled': true });
            }
        }
        function registrar(item, opc) {
            $(item).find('input').val('');
            $(item).find('input[id$=hdModo]').val('R');
            $(item).find('#lblPasswordTitle').html('Contrase&ntilde;a&nbsp;');
            $(item).find('input[id$=chkActivo]').prop('checked', true);

            $(item).find('tr[id$=rowUsuario]').show();
            $(item).find('tr[id$=rowActivo]').hide();
            $(item).find('tr[id$=rowNewPassword]').hide();

            $('select[id$=cmbEstacionUsuario] option').prop('selected', false).first().prop('selected', true);
            $('select[id$=cmbRoles]').prop({ 'disabled': false });
            $('select[id$=cmbRoles] option').prop('selected', false).first().prop('selected', true);

            $(item).find('tr[id$=lblPassword]').off('focusout').on('focusout', function() {
                $(item).find('tr[id$=txtNewPassword]').val($(this).val());
            });

            $('.modal-body #tblUserData input[type!=checkbox]').prop({
                'disabled': false,
                'checked': false
            });
            $('.modal-body #bdyPrivilegios input[type=checkbox]').prop('checked', false);

            if (!$(item).find('input[id$=chkChangePassword]').is(':checked')) {
                $(item).find('input[id$=chkChangePassword]').trigger("click");
            }

            if (!$(item).find('input[id$=chkPrivilegios]').is(':checked')) {
                $(item).find('input[id$=chkPrivilegios]').trigger("click");
            }

            $(item).find('input[id$=chkChangePassword]').hide();
            $(item).find('input[id$=chkPrivilegios]').hide();
        }
        function ModalConfig() {
            $('#dlgModal').off('show.bs.modal').on('show.bs.modal', function(event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var data = button.data();
                var opc = data.user;
                var modal = $(this);
                var temp;

                switch (data.type) {
                    case 'modificar':
                        temp = modal.find('.modal-title #ttlText').html($.trim('Modificar usuario ' + opc.Usuario));
                        modal.find('.modal-title #ttlPos').append(temp);
                        modificar(modal.find('.modal-body'), opc);
                        break;
                    case 'registrar':
                        temp = modal.find('.modal-title #ttlText').html($.trim('Registrar nuevo usuario'));
                        modal.find('.modal-title #ttlPos').append(temp);
                        registrar(modal.find('.modal-body'), opc);
                        break;
                }
                modal.find('button.btn-primary')
                     .removeAttr('onclick')
                     .removeAttr('type')
                     .attr({ 'type': 'button' })
                     .off('click')
                     .on('click', function(e) {
                         var valor = modal.find('.modal-body input').val();
                         var _body = modal.find('.modal-body');

                         if (_body.find('input[type$=txtNewPassword]').val() != _body.find('input[type$=txtConfirmNewPassword]').val()) {
                             alert('La nueva contraseña y la confirmación no son iguales coinciden');
                             _body.find('input[type$=txtNewPassword]').focus();
                             _body.find('input[type$=txtConfirmNewPassword]').val('');
                             e.preventDefault();
                         } else {
                             $('input[type=checkbox]').each(function() { $(this).val($(this).is(':checked') ? 'On' : 'Off'); });
                             //var data = {};
                             //modal.find('select,input').serializeArray().map(function(x) { data[x.name] = x.value; });
                             //modal.modal('hide');
                             //modal.find('input').each(function() { $(this).val(''); });
                             //$('input[id$=hdValues]').val(btoa(Adicional.Compressors.LZW.compress($.stringify(data))));
                             __doPostBack('ctl00$Content$btnAceptar', '');
                             e.preventDefault();
                         }
                     });
            }).off('hidden.bs.modal').on('hidden.bs.modal', function(event) {
                $('.modal-dialog').removeAttr('style');
            });
        }
        var fnOperation = function() {
            $("tr[data-toggle='popover']").popover({
                'animation': 'true',
                'html': 'true',
                'placement': 'right',
                'template': '<div class="popover" role="tooltip">' +
						    '<div class="arrow"></div>' +
						    '<h3 class="popover-title"></h3>' +
						    '<div class="popover-content"style="display:none"></div>' +
						    '<div class="btn-group btn-group-justified" role="group">' +
							    '<div class="btn-group" role="group">' +
								    '<button id="btnModificar" type="button" class="btn btn-primary" ' +
									    'data-toggle="modal" ' +
									    'data-target="#dlgModal" ' +
									    'data-type="modificar">Modificar</button>' +
							    '</div>' +
						    '</div>' +
					    '</div>',
                'trigger': 'click'
            }).on('shown.bs.popover', function() {
                ModalConfig();
                var local = $.parseJSON($(".popover-content").text());
                var lPopover = $(this);
                $('.popover .btn-group').find('#btnModificar').data(local).off('click').on('click', function() {
                    $(lPopover).popover('hide');
                });
                $('.popover .btn-group').find('#btnEliminar').data(local).off('click').on('click', function() {
                    $(lPopover).popover('hide');
                });
            }).on('click', function(e) {
                var offset = $(this).offset();
                var left = e.pageX;
                var top = e.pageY;
                var theHeight = $('.popover').height();
                $('.popover').show()
                             .css({
                                 'left': ((left + 10) + 'px'),
                                 'top': ((top - (theHeight / 2) - 10) + 'px')
                             });
            });
            $('#myTabs a').click(function(e) {
                e.preventDefault();
                $(this).tab('show');
            }); // Cambio de porcentaje
            $('.modal-dialog').draggable({
                'handle': ".modal-header"
            });
            $('.modal-body #tblUserData input[type!=checkbox]').prop({ 'required': true });
            $('.modal-body input[id$=chkChangePassword]').on('click', function() {
                if ($(this).is(':checked')) {
                    $('.modal-body #tblPassword input[type=password]').prop({
                        'disabled': false,
                        'required': false
                    });
                } else {
                    $('.modal-body #tblPassword input[type=password]').prop({
                        'disabled': true,
                        'required': true
                    });
                }
            });
            $('.modal-body input[id$=chkPrivilegios]').on('click', function() {
                if ($(this).is(':checked')) {
                    $('.modal-body #bdyPrivilegios input[type=checkbox]').prop({ 'disabled': false });
                } else {
                    $('.modal-body #bdyPrivilegios input[type=checkbox]').prop({ 'disabled': true });
                }
            });
            $("#cmbActivo").on("change", function() {
                var value = $(this).val();
                var $items = $("#tblContentUsers tr td[name=activo]");
                switch (value) {
                    case "Si":
                        $items.find("i.fa-check-square").parent().parent().show();
                        $items.find("i.fa-square").parent().parent().hide();
                        break;
                    case "No":
                        $items.find("i.fa-check-square").parent().parent().hide();
                        $items.find("i.fa-square").parent().parent().show();
                        break;
                    default:
                        $items.parent().show();
                        break;
                }
            });
            $("#cmbEstaciones").on("change", function() {
                var value = $(this).val();
                if (value == 'Todos') {
                    $("#tblContentUsers tr").show();
                } else {
                    $("#tblContentUsers tr").each(function(index) {
                        $row = $(this);
                        var id = $.trim($row.find("td[name=estacion]").text());
                        if (id == value) {
                            $row.show();
                        } else {
                            $row.hide();
                        }
                    });
                }
            });
            //            $('#tblContentUsers').bind('contextmenu', function(e) {
            //                // evito que se ejecute el evento
            //                e.preventDefault();
            //                // conjunto de acciones a realizar
            //                $(".custom-menu").finish().toggle(100).// In the right position (the mouse)
            //                    css({
            //                        top: e.pageY + "px",
            //                        left: e.pageX + "px"
            //                    });
            //                });
            $(document).bind("mousedown", function(e) {
                // If the clicked element is not the menu
                if (!$(e.target).parents(".custom-menu").length > 0) {
                    // Hide it
                    $(".custom-menu").hide(100);
                }

                if (!$(e.target).parents(".popover").length > 0) {
                    $("tr[data-toggle='popover']").popover('hide');
                }
            });
            $("#inTblSearch").on("keyup", function() {
                var value = $(this).val();
                $("#tblContentUsers tr").each(function(index) {
                    $row = $(this);
                    var id = $.trim($row.find("td[name=usuario]").text());
                    if (id.toLowerCase().indexOf(value.toLowerCase()) >= 0) {
                        $row.show();
                    } else {
                        $row.hide();
                    }
                });
            });
            ModalConfig();
            $('form').on('submit', function() {
                return false;
            });
            //$('body').show();
        };
    </script>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
