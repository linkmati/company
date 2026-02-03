<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		PageTitle = "Prsync Premium";
	%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p>Get more. Do more.</p>
	<h2>See what features you get with a Premium account</h2>
	<ul>
		<li>Unlimited submission of press releases per day.</li>
		<li>Ad free company and press release pages.</li>
		<li>Automatic submission of press releases using our Feed Service.</li>
	</ul>

	<div class="payment-choice">
		<div class="monthly">
			<p class="description">
				Pay per month
				<br />
				<strong><%=Config.Payment.PriceMonthly.ToString("C2") %>/month</strong>
			</p>
			<% Html.RenderPartial("PaymentForm", ViewData.Create(new { duration="M"})); %>
		</div>
		<div class="yearly">
			<p class="description">
				Pay per year
				<br />
				<strong><%=(Config.Payment.PriceYearly/12m).ToString("C2") %>/month</strong>
				<br />
				<small>Save <%=(Config.Payment.PriceMonthly * 12m - Config.Payment.PriceYearly).ToString("C2")%>/year</small>
			</p>
			<% Html.RenderPartial("PaymentForm", ViewData.Create(new { duration="Y"})); %>
		</div>
	</div>
</asp:Content>
