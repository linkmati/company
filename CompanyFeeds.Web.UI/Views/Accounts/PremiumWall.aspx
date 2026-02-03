<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%
	PageTitle = "You've reached the max amount of submissions per day";
%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
		<li><%= Html.ActionLinkFormatted("Submit a press release", "Add", "Entries")%></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p><strong>WOW! You have a lot of press releases to submit!</strong></p>
	<p>You've reached the maximum amount of press releases that can be submitted per day.</p>
	<h2>Get a premium account</h2>
	<ul>
		<li>Unlimited submission of press releases per day.</li>
		<li>Ad free company and press release pages.</li>
		<li>Automatic submission of press releases using our Feed Service.</li>
	</ul>
	<div style="padding: 20px 0 0 25px;">
		<%=Html.ActionLinkFormatted("Continue", "Premium", "Accounts", null,new { @class="tri-button"})%>
	</div>
</asp:Content>
