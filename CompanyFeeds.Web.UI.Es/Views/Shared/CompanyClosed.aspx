<%@ Page Title="Forbidden" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Prohibido</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
		ViewData["TrackerUrl"] = "/error/403-locked?requrl=" + HttpUtility.UrlEncode(ViewContext.HttpContext.Request.Url.PathAndQuery);
%>
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
	</ul>
	<h1>Prohibido</h1>
	<p>No tiene permisos para editar la información de esta empresa.</p>
	<p style="padding: 30px 0;">Si necesita mas detalles, por favor <strong><%=Html.ActionLinkFormatted("contáctenos", "Contact", "Feedback") %></strong>.</p>
</asp:Content>
