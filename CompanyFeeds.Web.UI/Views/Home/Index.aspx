<%@ Page Language="C#" Title="Company feeds" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Home.Index" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<title>Press releases - prsync.com</title>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="home">
		<asp:Panel runat="server" ID="pnlMembers" Visible="false">
			<div id="subscriptions">
				<h2>Companies that I follow</h2>
				<asp:Panel runat="server" ID="pnlSubscriptions" Visible="false">
					<% Html.RenderPartial("EntriesListControl", ViewData["Subscriptions"]); %>
					<div class="continue"><%= Html.ActionLinkFormatted("More >>", "List", "Subscriptions") %></div>
				</asp:Panel>
				<asp:Panel runat="server" ID="pnlNoSubscriptions">
					<% Html.RenderPartial("SubscribeForm"); %>
				</asp:Panel>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNotMembers" Visible="false">
			<div class="intro floatContainer">
				<h2>Welcome to prsync.com</h2>
				<ul class="floatContainer">
					<li class="discover">Discover what other companies are officially saying</li>
					<li class="subscribe">Stay up to date by subscribing to companies press releases</li>
					<li class="submit">Submit press releases</li>
				</ul>
				<p class="start"><%=Html.ActionLinkFormatted("Start here >>", "Edit", "Users") %></p>
			</div>
		</asp:Panel>
		<h2>Featured press releases</h2>
		<div id="orderH2"><%=Html.ActionLinkFormatted("Most relevant", "Index", "Home", null, new{@class="selected"}) %> <a rel="nofollow" href="/rss/"><img src="/images/iconrss.gif" alt="rss" /></a> <span>/</span> <%=Html.ActionLinkFormatted("Latest", "Latest") %></div>
		<% 
			ViewData["ShowAdvertising"] = false;
			Html.RenderPartial("EntriesListControl", Model, ViewData); 
		%>
	</div>
</asp:Content>
