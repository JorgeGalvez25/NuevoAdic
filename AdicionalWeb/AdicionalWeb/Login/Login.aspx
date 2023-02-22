<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="AdicionalWeb.Login.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
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
                <input runat="server" type="password" class="form-control" id="txtPassword" placeholder="Contrase&ntilde;a"
                    required="">
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10">
            <button runat="server" id="btnAcceder" type="submit" class="btn btn-primary pull-right"
                onserverclick="btnAcceder_ServerClick">
                Acceder...&nbsp;<i class="fa fa-chevron-right"></i></button>
        </div>
    </div>

    <script type="text/javascript"> $(document).ready(function() { $('#ctl00_Content_btnAcceder').off('click').removeAttr('onclick'); $('form').on('submit', function() { __doPostBack('ctl00$Content$btnAcceder', ''); return false;}); }); </script>

</asp:Content>
