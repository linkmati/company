<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.Views.Feedback.Contact" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Reporting Copyright Infringements</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Reporting Copyright Infringements</h1>
	<div class="copyright">
		<p>In accordance with the Digital Millennium Copyright Act (“DMCA”), Pub. L. 105-304, Prsync has created this form to receive notification of alleged copyright infringement occurring in prsync.com domain. If you believe that your copyrighted work is being infringed, notify our team by filling the form below.</p>
		<p>The Digital Millennium Copyright Act requires that all infringement claims must be in writing and must include the following information:</p>
		<ul>
			<li>A physical or electronic signature of the copyright owner or the person authorized to act on its behalf;</li>
			<li>A description of the copyrighted work claimed to have been infringed;</li>
			<li>A description of the infringing material and information reasonably sufficient to permit us to locate the material;</li>
			<li>Your contact information, including your address, telephone number, and email;</li>
			<li>A statement by you that you have a good faith belief that use of the material in the manner complained of is not authorized by the copyright owner, its agent, or the law; and</li>
			<li>A statement that the information in the notification is accurate, and, under the pains and penalties of perjury, that you are authorized to act on behalf of the copyright owner.</li>
		</ul>
	</div>
	<div id="contact">
		<% Html.RenderPartial("ContactForm"); %>
	</div>
</asp:Content>