<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Users.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="alternate" type="application/rss+xml" title="Notas de prensa seleccionadas por <%=Model.UserName%>" href="<%= Domain + Html.Url("Detail", "Users", new{id=Model.UserId,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>" />
	<title>Usuario <%=Model.UserName %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="userDetail">
		<div class="rss"><a rel="nofollow" href="<%= Domain + Html.Url("Detail", "Users", new{id=Model.UserId,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>"><img src="/images/iconrss.gif" alt="rss" /></a></div>
		<h1>Usuario: <%=Model.UserName %></h1>
		<asp:Panel runat="server" ID="pnlAdmin" Visible="false">
			<script type="text/javascript">
				var getIpsUrl = '<%=Url.Action("IpsUsed", "Users") %>';
				var activityUrl = '<%=Url.Action("ActivityByIp", "Users") %>';
				var deleteEntriesUrl = '<%=Url.Action("DeleteByUser", "Entries") %>';
				var deleteUserUrl = '<%=Url.Action("Delete", "Users") %>';
			</script>
			<script type="text/javascript" src="/scripts/site.user.admin.js"></script>
			<p>
				<strong>Últimas Ips utilizadas para enviar</strong>: <a href="#" onclick="return site.getIps(<%=Model.UserId %>, getIpsUrl, activityUrl, this);">Get Ips</a>
				<span class="ipContainer"></span>
			</p>
			<div id="activityContainer"></div>
			<p>
				<strong>Email</strong>: <%=Model.UserEmail %>
			</p>
			<p>
				<strong><a href="#" onclick="return deleteEntriesByUser(<%=Model.UserId %>, '¿Eliminar las notas de prensa enviadas?')">Eliminar las notas de prensa enviadas</a></strong>
			</p>
			<p>
				<strong><a href="#" onclick="return deleteUser(<%=Model.UserId %>, '¿Eliminar usuario?')">Eliminar usuario</a></strong>
			</p>
		</asp:Panel>
		<p><strong>Miembro desde</strong>: <%=Model.UserRegisterDate.ToString("MMMM d, yyyy") %>.</p>
		<uc:HtmlContainer id="pnlSubscriptions" runat="server" visible="false">
			<asp:Repeater runat="server" ID="repCompanies" DataSource='<%# ViewData["Companies"] %>'>
				<HeaderTemplate><p><strong>Suscripciones</strong>: </HeaderTemplate>
				<ItemTemplate><%#Eval("CompanyName") %></ItemTemplate>
				<SeparatorTemplate>, </SeparatorTemplate>
				<FooterTemplate>.</p></FooterTemplate>
			</asp:Repeater>
			<h2>Suscripciones de <%=Model.UserName %>: Notas de prensa recientes</h2>
			<% Html.RenderPartial("EntriesListControl", ViewData["Entries"]); %>
		</uc:HtmlContainer>
		<asp:Repeater ID="repSubmits" runat="server" DataSource='<%# ViewData["EntriesSubmitted"]%>'>
			<HeaderTemplate>
				<h2 style="padding-top: 20px;">Últimas notas de prensa enviadas por <%=Model.UserName %></h2>
				<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li><%#Eval("CompanyName") %> &gt; <%#Html.ActionLinkFormatted(Eval("EntryTitle").ToString(), "Detail", "Entries", new{id=Eval("Id"),tag=Eval("Tag"),companyTag=Eval("CompanyTag")}, null) %></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>
</asp:Content>
