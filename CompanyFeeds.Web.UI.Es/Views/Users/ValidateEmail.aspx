<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ValidateEmail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Users.ValidateEmail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Verificado</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Dirección correcta</h1>
	<p>Tu dirección de email ha sido verificada.</p>
	<br />
	<ul>
		<li>Ir a la <a href="/">página principal</a>.</li>
		<li><%=Html.ActionLinkFormatted("Enviar nota de prensa", "Edit", "Entries")%>.</li>
		<li><%=Html.ActionLinkFormatted("Seguir empresas", "List", "Subscriptions") %>.</li>
	</ul>
</asp:Content>
