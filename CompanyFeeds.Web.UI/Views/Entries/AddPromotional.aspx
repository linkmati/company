<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<% PageTitle = "Submit press release";%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1><%=PageTitle %></h1>
	<p>Submit your company press releases for free.</p>
	<p>
		In order to submit a press release you have to be <strong><%=Html.ActionLinkFormatted("signed in", "Login", "Users", new{returnUrl=Request.Url.PathAndQuery}, null) %></strong>.
		<br />If you don't have an account, <strong><%=Html.ActionLinkFormatted("sign up now for a free account", "Edit", "Users") %></strong>.
	</p>
	<br />
	<h2>Why join?</h2>
	<ul class="intro">
		<li class="discover">Discover what other companies are officially saying</li>
		<li class="subscribe">Stay up to date by subscribing to companies press releases</li>
		<li class="submit">Submit press releases</li>
	</ul>
</asp:Content>