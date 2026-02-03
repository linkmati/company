<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Companies.Detail" %>
<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
<%
	this.PageTitle = "Notas de prensa de " + Model.CompanyName;
	if (Convert.ToInt32(ViewData["Page"]) == 1)
	{
		this.PageTitle = "Comunicados de prensa de " + Model.CompanyName;
	}
%>
	<title><%=this.PageTitle %></title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	
	<div id="companyDetail">
		<ul class="path">
			<li><a href="/">Notas de prensa</a></li>
			<li><%= Html.ActionLinkFormatted(Model.CategoryName, "Detail", "Categories", new{categoryTag=Model.CategoryTag}, null) %></li>
		</ul>
		<h1><%=Model.CompanyName %></h1>
		<asp:Panel runat="server" ID="pnlInfo" CssClass="floatContainer">
			<div class="image" id="imgContainer" runat="server" visible="false"><%= Html.RenderLogo(Model.IsCompanyLogoNull() ? null : Model.CompanyLogo, Model.CompanyTag, Model.CompanyName + " logo", Convert.ToString(ViewData["LogoUrl"])) %></div>
			<div class="content">
				<%=Model.CompanyDescription %>
			</div>
			<p><strong>Web</strong>: <a href="<%=Model.CompanyUrl %>" rel="nofollow"><%=UrlUtils.RemovePrefix(Model.CompanyUrl)%></a></p>
		</asp:Panel>
		<div class="toolbar floatContainer">
			<ul>
				<li class="subscribe"><%=Html.ActionLinkFormatted("Seguir a " + Model.CompanyName, "List", "Subscriptions", new{companyName=Model.CompanyName}, null) %></li>
				<li class="print"><a href="#" onclick="window.print();return false;">Imprimir</a></li>
				<li class="edit" runat="server" id="linkEdit" visible="false"><%=Html.ActionLinkFormatted("Editar", "Edit", "Companies", new{companyTag=Model.CompanyTag}, null) %></li>
				<li class="edit" runat="server" id="linkNoRevence" visible="false"><%=Html.ActionLinkFormatted("Sin relevancia", "SetNoRelevance", "Companies", new{id=Model.CompanyId}, new{onclick="return confirm('¿Está seguro que desea establecer a esta empresa como SIN relevancia?');"}) %></li>
				<li class="edit" runat="server" id="linkDelete" visible="false"><a href="#" onclick="return deleteCompany(<%=Model.CompanyId %>, '<%=Model.CompanyName %>');">Eliminar</a></li>
			</ul>
		</div>
		<br />
		<h2><%=PageTitle %></h2>
		<% 
		if (Model.Entries.Count > 0)
		{
		%>
			<ul class="mini">
			<%			
				foreach (var entry in Model.Entries)
				{
					var entryUri = Html.Url("Detail", "Entries", new { tag = entry.EntryTag, id = entry.EntryId, companyTag = entry.CompanyTag });
			%>
					<li>
						<div>
							<a href="<%= entryUri %>">
								<span class="date"><%= entry.EntryDate.ToString("dd 'de' MMMM 'de' yyyy")%></span>
								<span class="title"><%= entry.EntryTitle %></span>
							</a>
						</div>
						<p><%= entry.EntryTeaser %></p>
					</li>	
			<%
				}
			%>
			</ul>
		<%
		}
		%>
		<div class="floatContainer">
			<%= Html.Pager(Config.CompanyDetailPageSize, (int)ViewData["Page"], Model.Entries.TotalCount, "<< Anterior", "Siguiente >>") %>
		</div>
		<% Html.RenderPartial("StatsTrackingCustomVars", new CompanyFeeds.Web.StatsTrackingCustomVar(2, "company", this.Model.CompanyName)); %>
	</div>
	<%
	if (UserIsAdmin)
	{
	%>
		<script type="text/javascript">
			function deleteCompany(id, companyName)
			{
				if (confirm("¿Está seguro que desea eliminar esta empresa?"))
				{
					var postUrl = '<%=Url.Action("Delete") %>';
					$.post(postUrl, {id:id}, function(data){
						if (data == "OK")
						{
							window.location.reload();
						}
					});
				}
				return false;
			}
		</script>
	<%
	 }
	%>
</asp:Content>
