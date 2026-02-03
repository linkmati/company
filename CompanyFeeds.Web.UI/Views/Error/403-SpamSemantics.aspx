<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Web spam detected</h1>
	<p>We detected web spam on the news release you submitted. The system has avoided publishing it.</p>
	<p>If you think this is due to an error, please <%=Html.ActionLinkFormatted("contact us", "Report Bug", "Feedback") %>.</p>
    <div class="floatContainer" style="margin: 50px 0">
        <h2>Prsync Premium</h2>
        <p>Get more. Do more.</p>
        <h2>See what features you get with a Premium account</h2>
        <ul>
            <li>Unlimited submission of press releases per day.</li>
            <li>Ad free company and press release pages.</li>
            <li>No content filter for your press releases.</li>
            <li>Automatic submission of press releases using our Feed Service.</li>
        </ul>
    
        <div class="payment-choice">
            <div class="monthly">
                <p class="description">
                    Pay per month
                    <br />
                    <strong><%=Config.Payment.PriceMonthly.ToString("C2") %>/month</strong>
                </p>
                <% Html.RenderPartial("../Accounts/PaymentForm", ViewData.Create(new { duration="M"})); %>
            </div>
            <div class="yearly">
                <p class="description">
                    Pay per year
                    <br />
                    <strong><%=(Config.Payment.PriceYearly/12m).ToString("C2") %>/month</strong>
                    <br />
                    <small>Save <%=(Config.Payment.PriceMonthly * 12m - Config.Payment.PriceYearly).ToString("C2")%>/year</small>
                </p>
                <% Html.RenderPartial("../Accounts/PaymentForm", ViewData.Create(new { duration = "Y" })); %>
            </div>
        </div>
    </div>
</asp:Content>