<%@ Page Title="Latest press releases" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Latest.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Home.Latest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Notas de prensa recientes</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="order"><%=Html.ActionLinkFormatted("Destacadas", "Index") %> <span>/</span> <%=Html.ActionLinkFormatted("Recientes", "Latest", "Home", null, new{Class="selected"}) %> <a rel="nofollow" href="/rss/latest/"><img src="/images/iconrss.gif" alt="rss" /></a></div>
	<h1>Notas de prensa recientes</h1>
<% 
	ViewData["ShowAdvertising"] = false;
	Html.RenderPartial("EntriesListControl", Model, ViewData); 
%>
</asp:Content>
