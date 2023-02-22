<%@ Page Title="Reporte de Ventas por Combustible" Language="C#" MasterPageFile="~/Base/Page.Master"
    AutoEventWireup="true" CodeBehind="RptVtasCombustible.aspx.cs" Inherits="AdicionalWeb.pages.reports.RptVtasCombustible"
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <input id="noEstacion" type="hidden" value="<%= (Request.QueryString["noEst"]== null ? string.Empty:Request.QueryString["noEst"]) %>" />
    <div style="background: #fff; padding: 10px 20px; top: -20px; position: relative;">
        <div class="row">
            <button id="btnCollapsable" class="btn btn-primary" type="button" data-toggle="collapse"
                data-target="#collapsable" aria-expanded="false" aria-controls="collapsable">
                <i class="glyphicon glyphicon-menu-hamburger"></i>
            </button>
            <div class="collapse" id="collapsable">
                <div name="fltrOptions" class="form-group col-lg-3 col-md-3 col-sm-4 col-xs-12">
                    <label for="dtFecha" class="control-label">
                        Fecha</label>
                    <input id="dtFecha" type="date" class="form-control" />
                </div>
                <div name="fltrOptions" class="form-group col-lg-2 col-md-2 col-sm-3 col-xs-12">
                    <label for="slTipoReporte" class="control-label">
                        Tipo de Venta</label>
                    <select id="slTipoReporte" class="form-control">
                        <option selected="selected" value="1">Ventas Reales</option>
                        <option value="2">Ventas Ajustadas</option>
                        <option value="3">Ajuste por Combustible</option>
                    </select>
                </div>
                <div name="fltrButtons" class="form-group col-lg-1 col-md-1 col-sm-2 col-xs-6">
                    <button id="btnConsultar" type="button" class="btn btn-primary" style="vertical-align: bottom;">
                        Consultar</button>
                </div>
                <div name="fltrButtons" class="form-group col-lg-2 col-md-2 col-sm-3 col-xs-6 pull-right">
                    <button id="btnGetPDF" type="button" class="btn btn-primary" style="vertical-align: bottom;"
                        data-target="tblContent">
                        Imprimir a PDF...</button>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <h2 style="color: #fff; font-weight: bold;">
                Reporte <span id="ttlReporte">de Ventas por Combustible</span></h2>
        </div>
        <div class="row">
            <div class="col-md-4" style="background: #fff; margin: 0 auto; float: none;">
                <table id="tblContent" class="table table-hover">
                    <thead>
                        <tr>
                            <th>
                                Combustible
                            </th>
                            <th class="text-right">
                                Volúmen
                            </th>
                            <th class="text-right">
                                Importe
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="3" class="text-center">
                                Sin datos
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th>
                                &nbsp;
                            </th>
                            <th class="text-right">
                                0.00
                            </th>
                            <th class="text-right">
                                0.00
                            </th>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript">
        var fnOperation = function() {
            $(window).resize();
            $('#dtFecha').ready(function() {
                var now = new Date();
                var month = (now.getMonth() + 1);
                var day = now.getDate();
                if (month < 10)
                    month = "0" + month;
                if (day < 10)
                    day = "0" + day;
                var today = now.getFullYear() + '-' + month + '-' + day;
                $('#dtFecha').val(today);
            });

            $('#btnConsultar').ready(function() {
                if (!consultThread) {
                    var consultThread = new Adicional.Threading.WorkerThread({
                        'dest': '/WebService.asmx/GetReportes',
                        'parameters': {
                            'd': {}
                        },
                        'onOk': function(a, b, c) { },
                        'timeout': 0,
                        'Always': function() { },
                        'async': true
                    });
                }

                $('#btnConsultar').off('click').on('click', function() {
                    consultThread.options.d = {
                        'd': {
                            'fecha': $('#dtFecha').val(),
                            'tipo': $('#slTipoReporte').val(),
                            'noEstacion': $('#noEstacion').val()
                        }
                    };
                    consultThread.options.always = function() { document.body.style.cursor = 'default'; }
                    consultThread.options.ok = function(a) {
                        try {
                            if (a.IsFaulted) {
                                console.log(a.ExceptionMessage);
                                window.Adicional.Dialog.setTitle('Error');
                                window.Adicional.Dialog.setMessage(a.Message);
                                window.Adicional.Dialog.setClosable(true);
                                window.Adicional.Dialog.setType(BootstrapDialog.TYPE_DANGER);
                                window.Adicional.Dialog.setSize(window.Adicional.Dialogs.GetSize(a.Message))
                                window.Adicional.Dialog.setButtons([{
                                    'id': 'btn-ok',
                                    'label': 'Aceptar',
                                    'cssClass': 'btn btn-success',
                                    'autospin': false,
                                    'action': function(dialogRef) {
                                        if (a.Message == 'No cuenta con sesión activa' && a.Result) {
                                            window.location = a.Result;
                                        }
                                        dialogRef.close();
                                    }
}]);
                                    window.Adicional.Dialog.open();
                                } else {
                                    var jData = JSON.parse(a.Result);
                                    $('#tblContent').data('data', jData);
                                    createDataTable(jData);
                                }
                            } finally {
                                document.body.style.cursor = 'default';
                            }
                        };
                        consultThread.Start();
                        document.body.style.cursor = 'wait';
                        $("#ttlReporte").html($('#slTipoReporte option:selected').html())
                    });

                    function createDataTable(jData) {
                        if (!jData || jData.length <= 0) {
                            $('#tblContent tbody').empty().append($('<tr><td colspan="3" class="text-center">Sin datos</td></tr>'));
                            $('#tblContent tfoot').empty().append($('<tr><th> </th><th class="text-right">0.00</th><th class="text-right">0.00</th></tr>'));
                        } else {
                            var _aux = $('<div>');
                            var sumVolumen = 0;
                            var sumImporte = 0;

                            for (var item in jData) {
                                sumVolumen += jData[item].Volumen;
                                sumImporte += jData[item].Importe;
                                _aux.append($('<tr>').append($('<td>').html(jData[item].Descripcion))
                                             .append($('<td class="text-right">').html(format(jData[item].Volumen, "#,##0.00")))
                                             .append($('<td class="text-right">').html(format(jData[item].Importe, "#,##0.00"))));
                            }

                            var _auxSumary = $('<div>');

                            _auxSumary.append($('<tr>').append($('<th>').html(' '))
                                               .append($('<th class="text-right">').html(format(sumVolumen, "#,##0.00")))
                                               .append($('<th class="text-right">').html(format(sumImporte, "#,##0.00"))));

                            $('#tblContent tbody').empty().html(_aux.html());
                            $('#tblContent tfoot').empty().html(_auxSumary.html());
                        }
                    }
                });

                $('#btnGetPDF').ready(function() {

                    $('#btnConsultar').click();
                    if (!printThread) {
                        var printThread = new Adicional.Threading.WorkerThread({
                            'dest': '/WebService.asmx/GetPDF',
                            'parameters': {
                                'd': {}
                            },
                            'onOk': function(a, b, c) { },
                            'timeout': 0,
                            'Always': function() { },
                            'async': true
                        });
                    }

                    $('#btnGetPDF').off('click').on('click', function() {
                        var toPrint = $('#' + $(this).data('target'));
                        var data = {
                            'id': 'rptVentasComb',
                            'fecha': $('#dtFecha').val(),
                            'tipo': $('#slTipoReporte').val(),
                            'noEstacion': $('#noEstacion').val(),
                            'tblData': $.stringify($('#tblContent').data('data'))
                        };

                        getPDF(data);
                    });

                    function getPDF(data) {
                        document.body.style.cursor = 'wait';

                        printThread.options.d = {
                            'd': data
                        };
                        printThread.options.always = function() { document.body.style.cursor = 'default'; }
                        printThread.options.ok = function(a) {
                            try {
                                if (a.IsFaulted) {
                                    console.log(a.ExceptionMessage);
                                    window.Adicional.Dialog.setTitle('Error');
                                    window.Adicional.Dialog.setMessage(a.Message);
                                    window.Adicional.Dialog.setClosable(true);
                                    window.Adicional.Dialog.setType(BootstrapDialog.TYPE_DANGER);
                                    window.Adicional.Dialog.setSize(window.Adicional.Dialogs.GetSize(a.Message))
                                    window.Adicional.Dialog.setButtons([{
                                        'id': 'btn-ok',
                                        'label': 'Aceptar',
                                        'cssClass': 'btn btn-success',
                                        'autospin': false,
                                        'action': function(dialogRef) {
                                            if (a.Message == 'No cuenta con sesión activa' && a.Result) {
                                                window.location = a.Result;
                                            }
                                            dialogRef.close();
                                        }
}]);
                                        window.Adicional.Dialog.open();
                                    } else {
                                        var $linkButton = $('<a>').attr({
                                            'href': a.Result,
                                            'target': '_blank',
                                            'class': 'btn btn-success',
                                            'style': 'margin: 0 auto'
                                        }).html('<i class="fa fa-download" aria-hidden="true"></i>&nbsp;Descargar');
                                        var div = $('<div>').attr({ 'style': 'display: flex' }).append($linkButton);

                                        window.Adicional.Dialog.setTitle('Reporte Ventas por Combustible');
                                        window.Adicional.Dialog.setMessage(function(dialog) {
                                            var $content = div;
                                            $content.find('a').on('click', function(event) {
                                                dialog.close();
                                            });
                                            return $content;
                                        });
                                        window.Adicional.Dialog.setClosable(true);
                                        window.Adicional.Dialog.setType(BootstrapDialog.TYPE_DEFAULT);
                                        window.Adicional.Dialog.setSize(window.Adicional.Dialogs.GetSize(div.text()));
                                        window.Adicional.Dialog.open();
                                    }
                                } finally {
                                    document.body.style.cursor = 'default';
                                }
                            }

                            printThread.Start();
                        }
                    });
                };
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
