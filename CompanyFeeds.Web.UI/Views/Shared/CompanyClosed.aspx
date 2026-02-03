<%@ Page Title="Forbidden" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Forbidden</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
		ViewData["TrackerUrl"] = "/error/403-locked?requrl=" + HttpUtility.UrlEncode(ViewContext.HttpContext.Request.Url.PathAndQuery);
%>
	<ul class="path">
		<li><a href="/">Press releases</a></li>
	</ul>
	<h1>Forbidden</h1>
	<p>The company you are trying to edit is <strong>locked</strong> by a site moderator.</p>
	<p style="padding: 30px 0;">If you need further details, please <strong><%=Html.ActionLinkFormatted("contact us", "Contact", "Feedback") %></strong>.</p>
</asp:Content>
