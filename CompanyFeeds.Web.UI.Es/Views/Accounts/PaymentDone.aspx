<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		PageTitle = "Gracias";
	%>
	<title><%=PageTitle %></title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p><strong>Hemos recibido su pago correctamente.</strong></p>
	<p>
		Ahora puede comenzar a utilizar todas las funciones Premium en prsync.es .
		<br /><br /><br />
		<a href="/" class="tri-button">Continue</a>
	</p>
</asp:Content>
