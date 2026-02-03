<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		PageTitle = "Thank you";
	%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p><strong>We are processing your payment.</strong></p>
	<p>Once we receive your payment, you will be able to use all premium features.</p>
	<p>It usually takes less than a minute, hit "refresh" to reload this page.</p>
	<p>If you have any doubt, don't hesitate to <%=Html.ActionLinkFormatted("contact us", "ContactPremium", "Feedback") %>.</p>
	<div style="padding-top: 10px;">
		<a href="#" class="tri-button" onclick="location.reload(); return false;">Refresh</a>
	</div>
</asp:Content>
