<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%
	PageTitle = "Ha alcanzado el máximo permitido de envíos diario";
%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
		<li><%= Html.ActionLinkFormatted("Enviar nota de prensa", "Add", "Entries")%></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p><strong>¿Tiene que enviar muchas notas de prensa?</strong></p>
	<h2>Obtén una cuenta Premium</h2>
	<ul>
		<li>Envíos ilimitados de notas de prensa.</li>
		<li>Libre de anuncios: Las notas de prensa y detalles de empresa que envíe, sin anuncios.</li>
		<li>Envíos automatizados, gracias a nuestro servicio FEED.</li>
	</ul>
	<div style="padding: 20px 0 0 25px;">
		<%=Html.ActionLinkFormatted("Continuar", "Premium", "Accounts", null,new { @class="tri-button"})%>
	</div>
</asp:Content>
