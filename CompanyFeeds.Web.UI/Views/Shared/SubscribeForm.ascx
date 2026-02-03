<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscribeForm.ascx.cs" Inherits="CompanyFeeds.Web.UI.Views.Shared.SubscribeForm" %>
<script src="/scripts/site.subscriptions.js" type="text/javascript"></script>
<script src="/scripts/jquery-ui-1.8.13.core-all.min.js" type="text/javascript"></script>
<script src="/scripts/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
<script type="text/javascript">
	$(document).ready(function(){
		site.initAutocomplete('#companyName', '<%=Url.Action("Search", "Companies") %>')
	});
	subscriptions.addUrl = '<%=Url.Action("Add", "Subscriptions") %>';
	subscriptions.removeUrl = '<%=Url.Action("Remove", "Subscriptions") %>';
</script>
<p id="noSubscriptionMessage" <%= ViewData["Companies"] != null ? "style=\"display:none;\"" : ""%>>Currently you are not following any company.</p>
<ul id="subscriptionList">
	<asp:Repeater runat="server" ID="repCompanies">
		<ItemTemplate>
			<li id="companySubscription<%# Eval("CompanyId") %>"><span> <%# Eval("CompanyName") %> </span><a href="#" class="remove" onclick="subscriptions.remove(<%# Eval("CompanyId") %>, '<%# Eval("CompanyName") %>');return false;">X</a></li>
		</ItemTemplate>
	</asp:Repeater>
</ul>
<form onsubmit="subscriptions.add();return false;" class="subscribe" style="padding-bottom: 16px;">
	<label>Type the name of the company</label> <input type="text" class="text" id="companyName" />
	<input type="submit" class="button" id="Submit" value="Submit" />
</form>