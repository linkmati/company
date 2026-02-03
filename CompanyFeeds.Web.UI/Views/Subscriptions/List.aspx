<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Subscriptions.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="alternate" type="application/rss+xml" title="<%=User.Name%> s subscriptions " href="<%= Domain + Html.Url("Detail", "Users", new{id=User.Id,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>" />
	<title>Follow companies</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mySubscriptions">
		<h1>Follow companies</h1>
		<asp:Panel runat="server" ID="pnlCompanies">
			<p>These are the companies you are following:</p>
			<% Html.RenderPartial("SubscribeForm", ViewData); %>
			<h2>Subscriptions: Latest press releases <a rel="nofollow" href="<%= Domain + Html.Url("Detail", "Users", new{id=User.Id,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>"><img src="/images/iconrss.gif" alt="rss" /></a></h2>
			<% Html.RenderPartial("EntriesListControl", ViewData["Entries"]); %>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNoRecords" Visible="false">
			<% Html.RenderPartial("SubscribeForm"); %>
		</asp:Panel>
	</div>
</asp:Content>
