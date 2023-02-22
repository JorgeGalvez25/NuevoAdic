<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="LoginRecovery.aspx.cs" Inherits="AdicionalWeb.Login.LoginRecovery" Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10 text-center" style="border-bottom: 1px solid rgba(0, 0, 0, 0.15);">
            <h3>Datos del usuario</h3>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10">
            <div class="input-group">
                <div class="input-group-addon">
                    <b class="fa">#</b>
                </div>
                <input runat="server" type="text" class="form-control" id="txtNoEstacion" placeholder="Estaci&oacute;n"
                    required="">
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10">
            <div class="input-group">
                <div class="input-group-addon">
                    <i class="fa fa-user"></i>
                </div>
                <input runat="server" type="text" class="form-control" id="txtUsuario" placeholder="Usuario"
                    required="">
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-2">
            <a href="/" class="btn btn-link pull-left" style="box-shadow: 0px 0px; -webkit-box-shadow: 0px 0px; -moz-box-shadow: 0px 0px;">
                <small><i class="fa fa-arrow-left"></i>&nbsp;Regresar</small></a>
        </div>
        <div class="col-sm-offset-1 col-sm-8">
            <button runat="server" id="btnRecuperar" type="submit" class="btn btn-primary pull-right"
                onserverclick="btnRecuperar_ServerClick">
                Recuperar&nbsp;<i class="fa fa-key"></i></button>
        </div>
    </div>
    
    <script type="text/javascript">
        window.addEventListener('DOMContentLoaded', function() {
            $(document).ready(function() { $('#ctl00_Content_btnRecuperar').off('click').removeAttr('onclick'); $('form').on('submit', function() { __doPostBack('ctl00$Content$btnRecuperar', ''); return false; }); });
        }); </script>
</asp:Content>
