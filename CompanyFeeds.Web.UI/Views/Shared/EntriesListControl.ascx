<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntriesListControl.ascx.cs" Inherits="CompanyFeeds.Web.UI.Views.Shared.EntriesListControl" %>
<asp:Repeater ID="rep" runat="server">
	<HeaderTemplate>
		<ul class="main">
	</HeaderTemplate>
	<ItemTemplate>
		<li runat="server" visible="false" id="ad">
			<%--
				Show Ads
			--%>
		</li>
		<li>
			<h3><%# Html.ActionLinkFormatted(Eval("EntryTitle").ToString(), "Detail", "Entries", new{tag = Eval("EntryTag"),id = Eval("EntryId"),companyTag = Eval("CompanyTag")}, null)%></h3>
			<p class="itemDetail">
				<%# Html.ActionLinkFormatted(Eval("CompanyName").ToString(), "Detail", "Companies", new{companyTag=Eval("CompanyTag").ToString()}, null) %>
				- <%# Html.RenderDate(Eval("EntryDate"), "Today", "Yesterday", "MMMM dd, yyyy") %> <%# Html.RenderUser((System.Data.DataRow)Container.DataItem, "by") %></p>
			<div class="image"><%# Html.RenderLogo((string)Eval("CompanyLogo"), (string)Eval("CompanyTag"), "", Convert.ToString(ViewData["LogoUrl"]))%></div>
			<p><%# Eval("EntryTeaser") %></p>				
		</li>
	</ItemTemplate>
	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
<%= Html.Pager(ViewData.Model) %>