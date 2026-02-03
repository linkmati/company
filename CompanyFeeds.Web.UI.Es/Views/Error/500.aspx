<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		ViewData["TrackerUrl"] = "/error/500?requrl=" + HttpUtility.UrlEncode(ViewContext.HttpContext.Request.Url.PathAndQuery);
	%>
	<title>Se ha producido un error</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Se ha producido un error</h1>
	<br />
	<p>Se ha producido un error atendiendo tu solicitud.</p>
	<p>El detalle del error ha sido enviado a nuestro equipo de soporte. </p>
	<p>Disculpa las molestias.</p>
	<br />
	<p>Ir a la <a href="/">página principal</a></p>
	<br />
	<br />
	<br />
	<br />
</asp:Content>
