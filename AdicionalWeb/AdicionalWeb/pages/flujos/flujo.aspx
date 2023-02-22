<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Page.Master" AutoEventWireup="true"
    CodeBehind="flujo.aspx.cs" Inherits="AdicionalWeb.pages.flujos.flujo" Async="true"
    Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="row">
        <div id="flujo" class="col-xs-12 col-sm-4 col-md-offset-4 col-md-4 col-lg-offset-4 col-lg-4 centered">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        window.fnOperation = function() {
            var FlujoButton = new Adicional.Flujos.Flujo({
                'id': 'btnFlujo',
                'parent': $('#flujo'),
                'valor': 'Inactivo',
                'Async': {
                    'url': 'GetEstatus',
                    'param': { 'est' : <% Response.Write(Request.QueryString["est"] ?? "1"); %>, 'flujo': false },
                    'Data': function(a, b, c, d, e) {
                        Adicional.Controles.FlujoButton.Modificar({ 'valor': b.result });
                    },
                    'Timer': 5
                }
            });
            Adicional = $.extend(true, {}, Adicional, { 'Controles': { 'FlujoButton': FlujoButton} });
            FlujoButton.Create();
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
