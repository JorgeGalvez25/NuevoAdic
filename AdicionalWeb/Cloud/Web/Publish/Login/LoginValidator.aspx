<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="LoginValidator.aspx.cs" Inherits="AdicionalWeb.Login.LoginValidator"
    Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10 text-center" style="border-bottom: 1px solid rgba(0, 0, 0, 0.15);">
            <h3>
                Validaci&oacute;n
            </h3>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10">
            <div class="input-group">
                <div class="input-group-addon">
                    <i class="fa fa-lock"></i>
                </div>
                <input type="text" class="form-control" id="txtValidacion" placeholder="C&oacute;digo"
                    runat="server">
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-1 col-sm-10">
            <button id="btnValidar" type="submit" class="btn btn-primary pull-right" onserverclick="btnValidar_Click"
                runat="server">
                Validar&nbsp;<i class="fa fa-check-circle"></i></button>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <div class="alert alert-info" role="alert">
                <div class="media" style="display: inline-flex">
                    <span class="media-left" aria-hidden="true"><i class="fa fa-exclamation-circle fa-2x">
                    </i></span>
                    <div class="media-body" style="display: inline">
                        <span>Se ha generado un c&oacute;digo de validaci&oacute;n, por favor <b>revise su correo</b>.</span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">        window.addEventListener('DOMContentLoaded', function() { $(document).ready(function() { $('#ctl00_Content_btnValidar').off('click').removeAttr('onclick'); $('form').on('submit', function() { __doPostBack('ctl00$Content$btnValidar', ''); return false; }); }); });</script>

</asp:Content>
