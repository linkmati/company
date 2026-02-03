<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage<List<Company>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Índice de empresas</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
	</ul>
    <h1>Índice de Empresas</h1>
    <h2>Las 100 empresas mas relevantes en prsync.es</h2>
<%
	Char latestPrefix = '-';
	foreach (Company company in Model)
	{
		var currentPrefix = Char.ToUpper(company.Name[0]);
		if (!Char.IsLetter(currentPrefix))
		{
			currentPrefix = '#';
		}
		if (currentPrefix != latestPrefix)
		{
			if (latestPrefix != '-')
			{
%>
				</ul>
<%
			}
%>
			<h3><%=currentPrefix %></h3>
			<ul>
<%
			latestPrefix = currentPrefix;
		}
%>
		<li><%= Html.ActionLinkFormatted(company.Name, "Detail", "Companies", new{companyTag=company.Tag}, null) %></li>
<%
		if (Model.IndexOf(company) == Model.Count - 1)
		{
%>
			</ul>
<%
		}
	}
%>
	<!-- <%=Model.Count %> -->
</asp:Content>