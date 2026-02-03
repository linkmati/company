<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Companies.Detail" %>
<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
<% 
	this.PageTitle = Model.CompanyName + " press releases";
	if (Convert.ToInt32(ViewData["Page"]) == 1)
	{
		this.PageTitle = Model.CompanyName + " news releases";
	}
	else if (Convert.ToInt32(ViewData["Page"]) == 2)
	{
		this.PageTitle = Model.CompanyName + " news release archive";
	}
	else if (Convert.ToInt32(ViewData["Page"]) == 3)
	{
		this.PageTitle = Model.CompanyName + " press release archive";
	}
%>
	<title><%= this.PageTitle%> </title>
    <link rel="canonical" href="<%=Html.Url("Detail", "Companies", new { companyTag = Model.CompanyTag, page = (int)ViewData["Page"] == 0 ? null : ViewData["Page"] }) %>" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	
	<div id="companyDetail">
		<ul class="path">
			<li><a href="/">Press releases</a></li>
			<li><%= Html.ActionLinkFormatted(Model.CategoryName, "Detail", "Categories", new{categoryTag=Model.CategoryTag}, null) %></li>
		</ul>
		<h1><%=Model.CompanyName %></h1>
		<asp:Panel runat="server" ID="pnlInfo" CssClass="floatContainer">
			<div class="image" id="imgContainer" runat="server" visible="false"><%= Html.RenderLogo(Model.IsCompanyLogoNull() ? null : Model.CompanyLogo, Model.CompanyTag, Model.CompanyName + " logo", Convert.ToString(ViewData["LogoUrl"])) %></div>
			<div class="content">
				<%=Model.CompanyDescription %>
			</div>
			<p><strong>Website</strong>: <a href="<%=Model.CompanyUrl %>" rel="nofollow"><%=UrlUtils.RemovePrefix(Model.CompanyUrl)%></a></p>
		</asp:Panel>
		<div class="toolbar floatContainer">
			<ul>
				<li class="subscribe"><%=Html.ActionLinkFormatted("Follow " + Model.CompanyName, "List", "Subscriptions", new{companyName=Model.CompanyName}, null) %></li>
				<li class="print"><a href="#" onclick="window.print();return false;">Print</a></li>
				<li class="edit" runat="server" id="linkEdit" visible="false"><%=Html.ActionLinkFormatted("Edit", "Edit", "Companies", new{companyTag=Model.CompanyTag}, null) %></li>
				<li class="edit" runat="server" id="linkNoRevence" visible="false"><%=Html.ActionLinkFormatted("Set no relevance", "SetNoRelevance", "Companies", new{id=Model.CompanyId}, null) %></li>
				<li class="edit" runat="server" id="linkDelete" visible="false"><a href="#" onclick="return deleteCompany(<%=Model.CompanyId %>, '<%=Model.CompanyName %>');">Delete</a></li>
			</ul>
		</div>
		<br />
		<h2><%=this.PageTitle %></h2>
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
								<span class="date"><%= entry.EntryDate.ToString("MMMM dd, yyyy") %></span>
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
			<%= Html.Pager(Config.CompanyDetailPageSize, (int)ViewData["Page"], Model.Entries.TotalCount, "<< Previous", "Next >>") %>
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
				if (confirm("Are you sure you want to delete this company and all the entries?"))
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
