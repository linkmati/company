<%@ Page Title="Manage subscriptions" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Subscriptions.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="alternate" type="application/rss+xml" title="<%=User.Name%> s subscriptions " href="<%= Domain + Html.Url("Detail", "Users", new{id=User.Id,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>" />
	<title>Empresas que sigo</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
	</ul>
	<div id="mySubscriptions">
		<h1>Empresas que sigo</h1>
		<asp:Panel runat="server" ID="pnlCompanies">
			<p>Empresas:</p>
			<% Html.RenderPartial("SubscribeForm", ViewData); %>
			<h2>Notas de prensa recientes<a rel="nofollow" href="<%= Domain + Html.Url("Detail", "Users", new{id=User.Id,type=CompanyFeeds.Web.ActionResults.ResultType.Rss}) %>"><img src="/images/iconrss.gif" alt="rss" /></a></h2>
			<% Html.RenderPartial("EntriesListControl", ViewData["Entries"]); %>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNoRecords" Visible="false">
			<% Html.RenderPartial("SubscribeForm"); %>
		</asp:Panel>
	</div>
</asp:Content>
