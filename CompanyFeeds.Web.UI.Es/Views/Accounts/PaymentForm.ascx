<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl" %>
<%--
	Displays a submit button for the paypal form
    https://cms.paypal.com/us/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_html_Appx_websitestandard_htmlvariables
--%>
<%
	var duration = ViewData["duration"];
	var productCode = Config.Payment.NameYearly;
	var price = Config.Payment.PriceYearly.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
	if (duration != "Y")
	{
		duration = "M";
		productCode = Config.Payment.NameMonthly;
		price = Config.Payment.PriceMonthly.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
	}
%>
<form action="<%=Config.Payment.ServiceUrl %>" method="post"> 
	<%-- Website identifier --%>
	<input type="hidden" name="business" value="<%=Config.Payment.Account %>"> 
	<input type="hidden" name="notify_url" value="<%=Config.Payment.NotificationUrl %>" />
	<input type="hidden" name="return" value="<%=Domain + Url.Action("PaymentDone", "Accounts") %>" />
	<input type="hidden" name="cbt" value="Continuar a prsync.es" />
	
	<!-- Identify the subscription. --> 
	<%-- The subscription: name, code and price --%>
	<input type="hidden" name="item_name" value="Cuenta Premium en prsync.es" />
	<input type="hidden" name="currency_code" value="<%=Config.Payment.Currency %>" />
	<input type="hidden" name="p3" value="1" />
	<input type="hidden" name="item_number" value="<%=productCode%>" /> 
	<input type="hidden" name="a3" value="<%=price %>" />
	<input type="hidden" name="t3" value="<%=duration %>" />
	
	<%-- userData --%>
	<input type="hidden" name="custom" value="<%=User.Id %>" />
	<input type="hidden" name="country" value="ES" />
	
	<%-- fixed --%>
	<input type="hidden" name="cmd" value="_xclick-subscriptions" />
	<input type="hidden" name="no_shipping" value="1" />
	<input type="hidden" name="rm" value="0" />
	<input type="hidden" name="src" value="1" />
	<input type="hidden" name="charset" value="utf-8" />
	<!-- Display the payment button. --> 
	<a class="tri-button" href="#" onclick="$(this).closest('form').submit();return false;"><span>Continuar</span></a>
	<img alt="" border="0" width="1" height="1" src="https://www.paypalobjects.com/es_XC/i/scr/pixel.gif" /> 
</form> 
