<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="RegisterDone.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Users.RegisterDone" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<title>Thank you for registering PRsync</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Thank you for registering</h1>
	<p>We've sent an email to <%=User.Email %> containing a URL you'll need to follow to verify your account. </p>
	<p>You should receive the email within the next few minutes. <strong>Check your email</strong>.</p>
	<br />
	<h3>Didn't get the email?</h3>
	<p>Below are some of the most common reasons you might not be getting the message.</p>
	<ul>
		<li>First, be patient, sometimes it takes a while for the email to arrive.</li>
		<li>Check above to ensure you entered your email address correctly. If it's wrong, <%=Html.ActionLinkFormatted("register again", "Edit", "Users") %>.</li>
		<li>Check your junk email box, the message might have been filtered as junk.</li>
		<li><%=Html.ActionLinkFormatted("Contact us", "Contact", "Feedback") %> if you can't get it to work and we'll resend your email.</li>
	</ul>
</asp:Content>
