<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<% PageTitle = "Enviar nota de prensa";%>
	<title><%=PageTitle %></title>
	<meta name="description" content="Envía una nota de prensa de tu empresa gratis." />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1><%=PageTitle %></h1>
	<p>Envía una nota de prensa de tu empresa gratis.</p>
	<p>
		Para envíar una nota de prensa tienes que tener una cuenta en prsync.es.
		<br />
		Haz <%=Html.ActionLinkFormatted("login", "Login", "Users", new{returnUrl=Request.Url.PathAndQuery}, null) %> o, si aún no tienes una cuenta, <%=Html.ActionLinkFormatted("regístrate gratis", "Edit", "Users") %>.
	</p>
	<br />
	<h2>¿De qué sirve estar registrado?</h2>
	<ul class="intro">
		<li class="discover">Puedes ver qué comunican las empresas españolas.</li>
		<li class="subscribe">Puedes suscribirte a las empresas que te interesen.</li>
		<li class="submit">Puedes enviar notas de prensa.</li>
	</ul>
</asp:Content>