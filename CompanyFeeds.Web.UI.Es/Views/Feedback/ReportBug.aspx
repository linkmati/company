<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.Es.Views.Feedback.ReportBug" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Notificar bug</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Notificar bug</h1>
	<p>¿Ha encontrado un bug? Rellene el formulario a continuación, especificando los detalles.</p>
	<div id="contact">
		<% Html.RenderPartial("ContactForm"); %>
	</div>
</asp:Content>