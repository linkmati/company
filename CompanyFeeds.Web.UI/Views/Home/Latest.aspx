<%@ Page Title="Latest press releases" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Latest.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Home.Latest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Latest press releases</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="order"><%=Html.ActionLinkFormatted("Most relevant", "Index") %> <span>/</span> <%=Html.ActionLinkFormatted("Latest", "Latest", "Home", null, new{@class="selected"}) %> <a rel="nofollow" href="/rss/latest/"><img src="/images/iconrss.gif" alt="rss" /></a></div>
	<h1>Latest press releases</h1>
<% 
	ViewData["ShowAdvertising"] = false;
	Html.RenderPartial("EntriesListControl", Model, ViewData); 
%>
</asp:Content>
