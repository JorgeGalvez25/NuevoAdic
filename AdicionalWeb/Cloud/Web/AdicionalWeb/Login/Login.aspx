<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="AdicionalWeb.Login.Login" Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <input type="hidden" id="hdValues" runat="server" />
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10 text-center" style="border-bottom: 1px solid rgba(0, 0, 0, 0.15);">
            <h3>
                Acceso</h3>
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
        <div class="col-sm-offset-1 col-sm-10">
            <div class="input-group">
                <div class="input-group-addon">
                    <i class="fa fa-key"></i>
                </div>
                <input runat="server" type="password" class="form-control" id="txtPassword" placeholder="Password"
                    required="">
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-2">
            <a runat="server" id="btnRestore" class="btn btn-link pull-left" onserverclick="btnRestore_ServerClick"
                style="box-shadow: 0px 0px; -webkit-box-shadow: 0px 0px; -moz-box-shadow: 0px 0px;">
                <small>Olvide mi contrase&ntilde;a&nbsp;<i class="fa fa-key"></i></small></a>
        </div>
        <div class="col-sm-offset-1 col-sm-8">
            <button runat="server" id="btnAcceder" type="submit" class="btn btn-primary pull-right"
                onserverclick="btnAcceder_ServerClick">
                Acceder...&nbsp;<i class="fa fa-chevron-right"></i></button>
        </div>
    </div>

    <script type="text/javascript">
        window.addEventListener('DOMContentLoaded', function() {
            $(window).on('load', function() {
                //$(document).ready(function() {
                $('[id$=btnAcceder]').off('click').removeAttr('onclick')
                                .on('click', function() {
                                    __doPostBack('ctl00$Content$btnAcceder', '');
                                });
                $('[id$=btnRestore]').off('click')
                                .on('click', function() {
                                    $('input').removeAttr('required');
                                    __doPostBack('ctl00$Content$btnRestore', '');
                                });
                $('form').submit(function(e) {
                    e.preventDefault();
                    return false;
                });
            });
        });
    </script>

</asp:Content>
