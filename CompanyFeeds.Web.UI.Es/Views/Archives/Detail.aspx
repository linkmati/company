<%@ Page Title="Archives - Press releases" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Archives.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Notas de prensa del día <%=String.Format("{0:d}", ViewData["date"])%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
		<li><%= Html.ActionLinkFormatted("Archivo", "ListDates")%></li>
	</ul>
	<h1>Notas de prensa del día <%=String.Format("{0:d}", ViewData["date"])%></h1>
	<% Html.RenderPartial("EntriesListControl", Model); %>
	<asp:Panel runat="server" ID="pnlNoEntries" Visible="false">
		<p class="noRecords">No se han encontrado notas de prensa.</p>
	</asp:Panel>
</asp:Content>
