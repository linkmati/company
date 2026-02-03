<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.Views.Feedback.Contact" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Contact Us</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Contact Us</h1>
	<p>Comments? Questions? Simply fill out the form below.</p>
	<div id="contact">
		<% Html.RenderPartial("ContactForm"); %>
	</div>
</asp:Content>