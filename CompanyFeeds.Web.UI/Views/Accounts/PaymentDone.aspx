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
	<p><strong>We have successfully received your payment.</strong></p>
	<p>
		You can now start using all premium features on prsync.com.
		<br /><br /><br />
		<a href="/" class="tri-button">Continue</a>
	</p>
</asp:Content>
