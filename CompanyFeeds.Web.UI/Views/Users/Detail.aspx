<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Users.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="alternate" type="application/rss+xml" title="<%=Model.UserName%> s subscriptions " href="<%= Domain + Html.Url("Detail", "Users", new{id=Model.UserId,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>" />
	<title>User: <%=Model.UserName %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="userDetail">
		<div class="rss"><a rel="nofollow" href="<%= Domain + Html.Url("Detail", "Users", new{id=Model.UserId,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>"><img src="/images/iconrss.gif" alt="rss" /></a></div>
		<h1>User: <%=Model.UserName %></h1>
		<asp:Panel runat="server" ID="pnlAdmin" Visible="false">
			<script type="text/javascript">
				var getIpsUrl = '<%=Url.Action("IpsUsed", "Users") %>';
				var activityUrl = '<%=Url.Action("ActivityByIp", "Users") %>';
				var deleteEntriesUrl = '<%=Url.Action("DeleteByUser", "Entries") %>';
				var deleteUserUrl = '<%=Url.Action("Delete", "Users") %>';
			</script>
			<script type="text/javascript" src="/scripts/site.user.admin.js"></script>
			<p>
				<strong>Last Ips used to submit</strong>: <a href="#" onclick="return site.getIps(<%=Model.UserId %>, getIpsUrl, activityUrl, this);">Get Ips</a>
				<span class="ipContainer"></span>
			</p>
			<div id="activityContainer"></div>
			<p>
				<strong>Email</strong>: <%=Model.UserEmail %>
			</p>
			<p>
				<strong><a href="#" onclick="return deleteEntriesByUser(<%=Model.UserId %>, 'Are you sure you want to delete all entries by this user?')">Delete all entries by user</a></strong>
			</p>
			<p>
				<strong><a href="#" onclick="return deleteUser(<%=Model.UserId %>, 'Are you sure you want to delete this user?')">Delete user</a></strong>
			</p>
		</asp:Panel>
		<p><strong>Member since</strong>: <%=Model.UserRegisterDate.ToString("MMMM d, yyyy") %>.</p>
		<uc:HtmlContainer id="pnlSubscriptions" runat="server" visible="false">
			<asp:Repeater runat="server" ID="repCompanies" DataSource='<%# ViewData["Companies"] %>'>
				<HeaderTemplate><p><strong>Subscribed to</strong>: </HeaderTemplate>
				<ItemTemplate><%#Eval("CompanyName") %></ItemTemplate>
				<SeparatorTemplate>, </SeparatorTemplate>
				<FooterTemplate>.</p></FooterTemplate>
			</asp:Repeater>
			<h2><%=Model.UserName %>'s Subscriptions: Latest press releases </h2>
			<% Html.RenderPartial("EntriesListControl", ViewData["Entries"]); %>
		</uc:HtmlContainer>
		<asp:Repeater ID="repSubmits" runat="server" DataSource='<%# ViewData["EntriesSubmitted"]%>'>
			<HeaderTemplate>
				<h2 style="padding-top: 20px;">Latest press releases submitted by <%=Model.UserName %></h2>
				<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li><%#Eval("CompanyName") %> &gt; <%#Html.ActionLinkFormatted(Eval("EntryTitle").ToString(), "Detail", "Entries", new{id=Eval("Id"),tag=Eval("Tag"),companyTag=Eval("CompanyTag")}, null) %></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>
</asp:Content>
