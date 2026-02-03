var site = 
{
	search : function (q)
	{
		if (jQuery.trim(q) != "" && jQuery.inArray(q, companyNamesList) >= 0)
		{
			//go to company
			
			document.location.href = "/companies/search/?name=" + encodeURI(jQuery.trim(q));
			return false;
		}
		
		return true;
	}
	, wasWindowBlocked : function (poppedWindow) {
		var result = false;

		try {
			if (typeof poppedWindow == 'undefined') {
				// Safari with popup blocker... leaves the popup window handle undefined
				result = true;
			}
			else if (poppedWindow && poppedWindow.closed) {
				result = false;
			}
			else if (poppedWindow && poppedWindow.test) {
				// This is the actual test. The client window should be fine.
				result = false;
			}
			else {
				// Else we'll assume the window is not OK
				result = true;
			}

		} catch (err) {
		
		}

		return result;
	}
	, tryOpenWindow : function (url)
	{
		//Tries to open a new window, if fails changes the current location
		var newWindow = window.open(url);
		if (wasWindowBlocked(newWindow))
		{
			document.location.href = url;
		}
	}
	, trackEvent : function(category, action, opt_label, opt_value)
	{
		_gaq.push(["_trackEvent", category, action, opt_label, opt_value]);
	}
	, trackOutboundLink : function(outboundType, url)
	{
		try 
		{
			site.trackEvent("outbound", outboundType, url);
			setTimeout('site.tryOpenWindow("' + url + '");', 100)
		}
		catch(err)
		{
		
		}
	}
	, initAutocomplete: function (element, url) {
		var cache = {}, lastXhr;
			$(element).autocomplete({
				minLength: 2,
				source: function( request, response ) {
					var term = request.term;
					term = term.toLowerCase();
					if ( term in cache ) {
						response( cache[ term ] );
						return;
					}
					lastXhr = $.getJSON(url, {name:request.term}, function( data, status, xhr ) {
						cache[ term ] = data;
						if ( xhr === lastXhr ) {
							response( data );
						}
					});
				},
				focus: function( event, ui ) {
					$(element).val( ui.item.Value );
					return false;
				},
				select: function( event, ui ) {
					$(element).val( ui.item.Value );
					return false;
				}
			}).data("autocomplete")._renderItem = function( ul, item ){
				return $( "<li></li>" )
					.data( "item.autocomplete", item )
					.append( "<a>" + item.Value + "</a>" )
					.appendTo( ul );
			};
	}
	, getResource: function(message, replacement1, replacement2)
	{
		if (!message)
		{
			throw "Message is not defined";
		}
		var result = message.replace("{0}", replacement1);
		if (replacement2)
		{
			result = message.replace("{1}", replacement2);
		}
		return result;
	}
	, getIps: function(id, url, activityUrl, sender) //admin
	{
		$(sender).hide();
					
		var container = $(".ipContainer", $(sender).closest("p"));
		container.append("<img src='/images/loading-mini.gif' alt='' />");
		$.post(url, {id:id}, function(ips){
			container.empty();
			if (ips.length == 0)
			{
				container.html("None");
			}
			else
			{
				for (var i=0;i<ips.length;i++)
				{
					if (i > 0)
					{
						container.append(", ");
					}
					container.append($("<a href='#' />").text(ips[i]).click(function(){
						site.getActivity($(this).text(), activityUrl);
						return false;
					}));
				}
			}
		});
		return false;
	}
	, getActivity: function (ip, url) //admin
	{
		var container = $("#activityContainer");
		container.append("<img src='/images/loading-mini.gif' alt='' />");
		$.post(url, {ip:ip}, function(data){
			container.empty();
			var entries = data.entries;
			var companies = data.companies;
			if (entries.length == 0 && companies.length == 0)
			{
				alert("No entries or companies for the ip adress " + ip);
			}
			else
			{
				if (entries.length > 0)
				{
					container.append("<h2>Press releases edited from " + ip + "</h2>");
					var listContainer = $("<ul />");
					for (var i=0;i<entries.length;i++)
					{
						var entry = entries[i];
						listContainer.append($("<li />")
							.append($("<a />").text(entry.EntryTitle).attr("href", "/" + entry.Id + "/")));
					}
					container.append(listContainer);
				}
				if (companies.length > 0)
				{
					container.append("<h2>Companies edited from " + ip + "</h2>");
					var listContainer = $("<ul />");
					for (var i=0;i<companies.length;i++)
					{
						var company = companies[i];
						listContainer.append($("<li />")
							.append($("<a />").text(company.Name).attr("href", "/" + company.Tag + "/")));
					}
					container.append(listContainer);
				}
				container.append("<br />");
			}
		});
	},
	regv: function (id) {
        if (document.cookie.indexOf('regv=set') >= 0) {
            return;
        }
	    $.post('/companies/rv/', {id: id}, function() {});
	}
}