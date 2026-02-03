<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		PageTitle = "Prsync Premium";
	%>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Notas de prensa</a></li>
	</ul>
	<h1><%=PageTitle %></h1>
	<p>Mas posibilidades. Mas fácil.</p>
	<h2>Características Premium</h2>
	<ul>
		<li>Envíos ilimitados de notas de prensa.</li>
		<li>Libre de anuncios: Las notas de prensa y detalles de empresa que envíe, sin anuncios.</li>
		<li>Envíos automatizados, gracias a nuestro servicio FEED.</li>
	</ul>
	<div class="payment-choice">
		<div class="monthly">
			<p class="description">
				Pago mensual
				<br />
				<strong><%=Config.Payment.PriceMonthly.ToString("C2") %>/mes</strong>
			</p>
			<% Html.RenderPartial("PaymentForm", ViewData.Create(new { duration="M"})); %>
		</div>
		<div class="yearly">
			<p class="description">
				Pago anual
				<br />
				<strong><%=(Config.Payment.PriceYearly/12m).ToString("C2") %>/mes</strong>
				<br />
				<small>Ahorre <%=(Config.Payment.PriceMonthly * 12m - Config.Payment.PriceYearly).ToString("C2")%>/año</small>
			</p>
			<% Html.RenderPartial("PaymentForm", ViewData.Create(new { duration="Y"})); %>
		</div>
	</div>
</asp:Content>
