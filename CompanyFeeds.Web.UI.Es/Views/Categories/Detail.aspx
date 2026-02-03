<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="false" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Categories.Detail" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<title>Notas de prensa del sector <%= ViewData["CategoryName"]%></title>
	<link rel="alternate" type="application/rss+xml" title="Notas de prensa - <%=ViewData["CategoryName"]%>" href="<%=  Domain + Html.Url("Detail", "Categories", new{categoryTag=ViewData["CategoryTag"],type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<div id="categoryDetail">
		<div class="rss"><a rel="nofollow" href="<%=  Domain + Html.Url("Detail", "Categories", new{categoryTag=ViewData["CategoryTag"],type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>"><img src="/images/iconrss.gif" alt="rss" /></a></div>
		<h1><%= ViewData["CategoryName"]%> - Notas de prensa</h1>
		<h2><%= ViewData["Description"]%></h2>
<% 
		ViewData["ShowAdvertising"] = false;
		Html.RenderPartial("EntriesListControl", Model); 
%>
	</div>
</asp:Content>
