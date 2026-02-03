<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Prohibido</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Prohibido</h1>
	<p>El servidor ha rechazado la solicitud porque ha excedido el limite máximo de envios.</p>
	<br />
	<p>Ir a la <a href="/">página principal</a></p>
</asp:Content>
