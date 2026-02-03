<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.Es.Views.Feedback.Contact" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Contáctanos</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Contáctanos</h1>
	<p>¿Comentarios? ¿Preguntas? Rellene el formulario a continuación.</p>
	<div id="contact">
		<% Html.RenderPartial("ContactForm"); %>
	</div>
</asp:Content>