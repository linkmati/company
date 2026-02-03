<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<%
		ViewData["TrackerUrl"] = "/error/500?requrl=" + HttpUtility.UrlEncode(ViewContext.HttpContext.Request.Url.PathAndQuery);
	%>
	<h1>An error occurred</h1>
	<br />
	<p>An error occurred on the server when processing the URL.</p>
	<p>The error detail was sent to our support team. </p>
	<p>Sorry for the inconvenience.</p>
	<br />
	<p>Go to the <a href="/">homepage</a></p>
	<br />
	<br />
	<br />
	<br />
</asp:Content>
