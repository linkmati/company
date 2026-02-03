<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Web spam detected</h1>
	<p>We detected web spam on the news release you submitted. The system has avoided publishing it.</p>
	<p>If you think this is due to an error, please <%=Html.ActionLinkFormatted("contact us", "Report Bug", "Feedback") %>.</p>
</asp:Content>