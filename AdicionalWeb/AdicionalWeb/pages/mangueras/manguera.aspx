<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Page.Master" AutoEventWireup="true"
    CodeBehind="manguera.aspx.cs" Inherits="AdicionalWeb.pages.mangueras.manguera"
    Async="true" Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        Adicional = $.extend(true, Adicional, { 'Controles': { 'Dispensarios': {}} });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="row">
        <div id="global" class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        </div>
        <script type="text/javascript">
            Adicional = $.extend(true, Adicional, {
                'Controles': {
                    'Dispensarios': {
                        'Global': new Adicional.Dispensarios.Global({
                            'id': 1,
                            'parent': $('#global'),
                            'valor': 0,
                            'estacion': <% Response.Write(this.Estacion); %>
                        })
                    }
                }
            });
            window.Adicional.Controles.Dispensarios.Global.Create();
        </script>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12"><%for (int i = 0; i < this.Dispensarios.Count; i++){ %>
            <%--<div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">--%>
                <div id="parent_<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>">
                </div>
            <%--</div>--%><%} %>
            <script type="text/javascript">
                Adicional = $.extend(true, Adicional, {
                    'Controles': {
                        'Dispensarios': {<%for (int i = 0; i < this.Dispensarios.Count; i++){ %>
                            'Dispensario<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>': new Adicional.Dispensarios.Dispensario({
                                'id': 'pos_<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>',
                                'parent': $('#parent_<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>'),
                                'posicion': <% Response.Write(this.Dispensarios[i].posicion); %>,
                                'Async': {
                                    'url': 'GetMangueras',
                                    'param': { 'est': <% Response.Write(this.Estacion); %>, 'pos': <% Response.Write(this.Dispensarios[i].posicion); %> },
                                    'Data': function(a, b){
                                        Adicional.Controles.Dispensarios.Dispensario<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>.Modificar(b.result);
                                    },
                                    'Timer': 10
                                }
                            }),<%} %>
                        }
                    }
                });<%for (int i = 0; i < this.Dispensarios.Count; i++){ %>
                window.Adicional.Controles.Dispensarios.Dispensario<% Response.Write(this.Dispensarios[i].posicion.ToString("D2")); %>.Create();<%} %>
            </script>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="dlgModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel" style="cursor: move">
                        <small><i class="fa fa-pencil-square-o"></i></small>&nbsp;Posici&oacute;n&nbsp;<span
                            id="ttlPos" class="label label-success">&nbsp;</span></br><small id="ttlText">&nbsp;</small></h4>
                </div>
                <div class="modal-body">
                    <div class="text-center">
                        <input id="spinEdit" type="text" class="text-center" placeholder="%" /><h3 style="display: inline-block;
                            margin: 0; padding: 0; vertical-align: middle;">
                            %</h3>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary">
                        Aceptar</button>
                    <button type="button" class="btn btn-success" data-dismiss="modal">
                        Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
        function getFormatNumericSmall(num) {
            var iX = parseInt(num); // Enteros
            var dY = num - iX; // Decimales
            return iX.toString() + '<small>' + format(dY, '#.00') + '%&nbsp;</small>';
        }
        var fnOperation = function() {
            // Cambio de porcentaje
            $('.modal-dialog').draggable({ 'handle': ".modal-header" });
            $('#dlgModal').on('show.bs.modal', function(event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var recipient = button.data('titulo'); // Extract info from data-* attributes
                var posicion = button.data('posicion');
                var estacion = button.data('estacion');
                var combustible = button.data('combustible');
                var manguera = button.data('manguera');

                var modal = $(this);
                $('#spinEdit').spinedit('setValue', recipient);
                modal.find('.modal-title #ttlPos').html(posicion);
                modal.find('.modal-title #ttlText').html($.trim(button.text()));
                modal.find('.modal-body input').val(recipient);
                modal.find('button.btn-primary')
                     .off('click')
                     .on('click', function(e) {
                         var valor = modal.find('.modal-body input').val();
                         button.data('titulo', valor);
                         if (posicion == 'Global') {
                             Adicional.setGlobal({ 'est' : estacion, 'val': valor });
                         } else {
                             Adicional.setPorcentaje({
                                'est': parseInt(estacion),
                                'pos': parseInt(posicion),
                                'mngra': parseInt(manguera),
                                'comb': combustible,
                                'val': valor
                             }, function(){
                                button.parent().find('h3>span').html(getFormatNumericSmall(valor));
                             });
                         }
                         modal.modal('hide');
                         e.preventDefault();
                     });
            }).on('hidden.bs.modal', function(event) {
                $('.modal-dialog').removeAttr('style');
            });

            $('#spinEdit').spinedit({
                minimum: 0,
                maximum: <% Response.Write(PorcentajeMaximo); %>,
                step: 1,
                value: 0,
                numberOfDecimals: <% Response.Write(Decimales); %>
            });
        };
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
