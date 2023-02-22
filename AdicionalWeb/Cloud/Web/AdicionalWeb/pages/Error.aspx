<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="Error.aspx.cs" Inherits="AdicionalWeb.pages.Error" Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="col-xs-offset-3 col-xs-6">
        <div class="panel panel-danger">
            <div class="panel-heading">
                <h3 class="panel-title">Error</h3>
            </div>
            <div class="panel-body">
                <span>Ocurrio un fallo inesperado.<br>Intentelo nuevamente, si el problema persiste
                    reportelo con su administrador.</span> <span name="me" style="display: none;"><% Response.Write(ErrorMessage); %></span>
            </div>
            <div class="panel-footer">
                <ul class="nav nav-pills">
                    <li class="pull-right"><a href="#" onclick="window.history.back();return false;"
                        class="btn btn-link">Regresar</a></li></ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
