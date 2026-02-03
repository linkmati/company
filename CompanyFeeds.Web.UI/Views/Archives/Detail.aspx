<%@ Page Title="Archives - Press releases" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Archives.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Press releases for <%=String.Format("{0:d}", ViewData["date"])%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
		<li><%= Html.ActionLinkFormatted("Archives", "ListDates")%></li>
	</ul>
	<h1>Press releases for <%=String.Format("{0:d}", ViewData["date"])%></h1>
	<% Html.RenderPartial("EntriesListControl", Model); %>
	<asp:Panel runat="server" ID="pnlNoEntries" Visible="false">
		<p class="noRecords">There are no press releases for given date.</p>
	</asp:Panel>
</asp:Content>
