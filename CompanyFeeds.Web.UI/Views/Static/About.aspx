<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>About prsync.com</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   	<h1>About prsync.com</h1>
	<p><em>Prsync.com</em> is a professional news and information distribution site.</p>
	<p>We would like <em>prsync.com</em> to be a practical site for professionals by <strong>delivering latest top industries/partners/competitors news</strong>.</p>
	<p>Every press release and company information on <em>prsync.com</em> is submitted and edited by our community.</p>
	<p>Once something is submitted, other people see it and if the press release is relevant it is promoted to the front page of the site.</p>
	<p>Submitting press releases on <em>prsync.com</em> is - and always be - free.</p>
	
	<p><%= Html.ActionLinkFormatted("Join us >>", "Edit", "Users") %></p>
	<h2>Logos</h2>
	<p>Available formats:</p>
	<ul>
		<li><a href="/images/logo_high.png">PNG (8 bits)</a></li>
		<li><a href="/images/logo.eps">EPS</a></li>
		<li><a href="/images/logo.svg">SVG</a></li>
	</ul>
	<p style="text-align: center;"><em>Preview</em></p>
	<div class="siteLogo" style="text-align: center;"><img src="/images/logowhite.gif" alt="white logo" /></div>
</asp:Content>
