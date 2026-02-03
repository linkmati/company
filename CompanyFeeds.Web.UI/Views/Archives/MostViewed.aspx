<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage<PagedList<EntriesQueries.EntriesListRow>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Most viewed press releases</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
		<li><%= Html.ActionLinkFormatted("Company Index", "ListAll", "Companies")%></li>
	</ul>
	<h1>Most viewed press releases</h1>
	<% Html.RenderPartial("EntriesListControl", Model); %>
</asp:Content>
