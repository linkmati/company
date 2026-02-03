<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage<PagedList<EntriesQueries.EntriesListRow>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Las notas de prensa más vistas</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
		<li><%= Html.ActionLinkFormatted("Índice de Empresas", "ListAll", "Companies")%></li>
	</ul>
	<h1>Las notas de prensa más vistas</h1>
	<% Html.RenderPartial("EntriesListControl", Model); %>
</asp:Content>
