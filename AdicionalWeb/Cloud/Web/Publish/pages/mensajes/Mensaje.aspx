<%@ Page Title="" Language="C#" MasterPageFile="~/Base/Login.Master" AutoEventWireup="true"
    CodeBehind="Mensaje.aspx.cs" Inherits="AdicionalWeb.pages.mensajes.Mensaje" Buffer="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="jumbotron" style="padding-top: 10px; padding-bottom: 10px;">
        <h2>
            <% Response.Write(Cache["MensajeTtl"].ToString()); %></h2>
        <br />
        <h4>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <strong>Mensaje:</strong>
                </div>
                <div class="panel-body">
                    <% Response.Write(Cache["Mensaje"].ToString()); %>
                </div>
            </div>
        </h4>
    </div>
</asp:Content>
