<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl<EntriesQueries.EntriesDetailRow>" %>
<%
	var acceptLarge = false;
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
			    SlotId = "contentLeft",
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
			SlotId = "contentTop",
			Width = 728,
			Height = 90
		});
	}
%>