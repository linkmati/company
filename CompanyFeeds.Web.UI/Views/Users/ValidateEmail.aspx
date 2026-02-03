<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ValidateEmail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Users.ValidateEmail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Thank you</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Thank you...</h1>
	<p>...for verifying your email address.</p>
	<h2>Now you are able to:</h2>
	<ul>
		<li><a href="/">Browse</a> to see what other companies are officially saying</li>
		<li><%=Html.ActionLinkFormatted("Submit press releases", "Edit", "Entries")%></li>
		<li><%=Html.ActionLinkFormatted("Follow companies", "List", "Subscriptions") %></li>
	</ul>
</asp:Content>
