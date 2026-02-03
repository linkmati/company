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
	<p><strong>Estamos procesando su pago.</strong></p>
	<p>Una vez que recibamos el pago, usted será capaz de utilizar todas las características Premium.</p>
	<p>Por lo general, toma menos de un minuto. Pulsa "actualizar" para volver a cargar la página. </p>
	<p>Si tienes cualquier duda, <%=Html.ActionLinkFormatted("contacta con nosotros", "ContactPremium", "Feedback") %>.</p>
	<div style="padding-top: 10px;">
		<a href="#" class="tri-button" onclick="location.reload(); return false;">Actualizar</a>
	</div>
</asp:Content>
