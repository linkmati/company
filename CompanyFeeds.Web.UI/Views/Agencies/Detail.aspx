<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Agencies.Detail" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<title><%=Model.Name %></title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div id="companyDetail">
		<ul class="path">
			<li><a href="/">Press releases</a></li>
		</ul>
		<h1>Agency: <%=Model.Name %></h1>
		<asp:Panel runat="server" ID="pnlInfo" CssClass="floatContainer">
			<div class="image" id="imgContainer" runat="server" visible="false"><%= Html.RenderLogo(Model.Logo == null ? null : Model.Logo, Model.Tag, Model.Name + " logo", Convert.ToString(ViewData["LogoUrl"])) %></div>
			<div class="content">
				<%=Model.Description %>
			</div>
<%			if (Model.Url != null)
			{%>
				<p><strong>Website</strong>: <a href="<%=Model.Url %>"><%=UrlUtils.RemovePrefix(Model.Url)%></a></p>
<%			}%>
		</asp:Panel>
		<asp:Repeater ID="repSubmits" runat="server" DataSource='<%# ViewData["EntriesSubmitted"]%>'>
			<HeaderTemplate>
				<h2 style="padding-top: 20px;">Latest press releases submitted by <%=Model.Name %></h2>
				<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li><%#Eval("CompanyName") %> &gt; <%#Html.ActionLinkFormatted(Eval("EntryTitle").ToString(), "Detail", "Entries", new{id=Eval("Id"),tag=Eval("Tag"),companyTag=Eval("CompanyTag")}, null) %></li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>
</asp:Content>