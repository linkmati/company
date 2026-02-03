<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Thank you</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<h1>Thank you</h1>
	<br /><br />
	<p class="successMessage">
	    Your message has been sent.
	</p>
	<br /><br />
	<p><a href="/">Continue &gt;&gt;</a></p>
</asp:Content>

