<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	var values = new RouteValueDictionary(this.Model);
	if (values["SlotId"] == null || values["Width"] == null || values["Height"] == null)
	{
		throw new ArgumentException("SlotId, Width and Height cannot be null on the model passed to Adslot partial view.");
	}
	if (!ViewData.Get<bool>("HideAdvertising", false))
	{
%>
<div class="adslot">
	<script type="text/javascript"><!--
	google_ad_client = 'ca-pub-2423614996377514';
	google_ad_slot = '<%=values["SlotId"] %>';
	google_ad_width = <%=values["Width"] %>;
	google_ad_height = <%=values["Height"] %>;
	//-->
	</script>
	<script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
	</script>
</div>
<%
	}	
%>