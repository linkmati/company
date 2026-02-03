<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl<EntriesQueries.EntriesDetailRow>" %>
<%
	bool acceptLarge = false;
	if (!Model.IsEntryContentNull())
	{
		if (!Regex.IsMatch(Model.EntryContent, "<img|<embed|<object", RegexOptions.Compiled | RegexOptions.IgnoreCase))
		{
			acceptLarge = true;
		}
	}
	
	if (acceptLarge)
	{
%>
		<div class="adslotInContent" style="float: left; padding-right: 3px;">
<%
		//Show large adslot (content)
		Html.RenderPartial("Adslot", new
		{
			SlotId = "2685549824",
			Width = 300,
			Height = 250
		});
%>
		</div>
<%
	}
	else
	{
		//Show normal adslot (middle)
		Html.RenderPartial("Adslot", new
		{
			SlotId = "7255350224",
			Width = 728,
			Height = 90
		});
	}
%>