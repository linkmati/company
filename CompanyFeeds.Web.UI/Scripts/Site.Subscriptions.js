var subscriptions = 
{
	addUrl: "/user/subscriptions/add/"
	, removeUrl: "/user/subscriptions/remove/"
	, add : function()
	{
		var input = $("#companyName");
		var companyName = input.val();
		var companyId = null;
		if (input.data("autocomplete").selectedItem != null)
		{
			companyId = input.data("autocomplete").selectedItem.Key;
		}
		if (jQuery.trim(companyName) != "")
		{
			$.post(subscriptions.addUrl, {companyName:companyName, companyId:companyId}, function (data){
				//Append to the list
				if (data)
				{
					companyId = data;
					var listItemId = "companySubscription" + companyId;
					if ($("#" + listItemId).length == 0)
					{
						//if does not already exist
						var li = $("<li></li>").attr("id", listItemId).html("<span> " + companyName + " </span><a href='#' class='remove' onclick='subscriptions.remove(" + companyId + ", \"" + companyName + "\");return false;'>X</a>");
						$("#subscriptionList").append(li);
						input.val("");
						input.focus();
						
						site.trackEvent("subscription", "add", companyName);
						
						if ($("#noSubscriptionMessage").get() != "")
						{
							$("#noSubscriptionMessage").fadeOut(800);
						}
					}
					else
					{
						alert(site.getResource(resources.alreadySubscribed, companyName));
						//alert("You are already subscribed to " + companyName);
					}
				}
				else
				{
					alert(site.getResource(resources.companyNotFound, companyName));
				}
			}, "json" );
		}
	}
	, remove : function(companyid, companyName)
	{
		if (confirm(site.getResource(resources.removeSubscription, companyName)))
		{
			$.post(subscriptions.removeUrl, {companyid:companyid}, function (){
				$("#subscriptionList li").remove("#companySubscription" + companyid);
				site.trackEvent("subscription", "remove", companyName);
				$("#companyName").focus();
				if ($("#subscriptionList li").length == 0)
				{
					if ($("#noSubscriptionMessage").get() != "")
					{
						$("#noSubscriptionMessage").show(800);
					}
				}
			}, "json");
		}
	}
}