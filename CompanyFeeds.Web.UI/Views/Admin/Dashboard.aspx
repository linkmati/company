<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage<IEnumerable<Entry>>" %>
<asp:Content ID="Head1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .premium a {
            color: #aaaaaa;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Admin Dashboard</h1>
	<div style="padding-bottom:10px;">
		<form action="" onsubmit="document.location.href='?id=' + $('#fromId').val();return false;">
			<label for="fromId">From</label>
			<%=Html.TextBox("fromId") %>
			<input type="submit" value="Submit" />
		</form>
	</div>
<%
	if (Model.Count() > 0)
	{
%>
	<div id="suspected">
		<ul>
<%
		foreach (var entry in Model)
		{
%>
		<li id="e<%=entry.Id %>" class="s">
			<h3 class="<%= entry.IsPremium == true ? "premium" : "" %>">
				<%=Html.ActionLinkFormatted(entry.CompanyName, "Detail", "Companies", new { companyTag = entry.Company.Tag }, null)%>
				&gt;

				<%=Html.ActionLinkFormatted(entry.EntryTitle, "GetShortUrl", "Entries", new { id = entry.Id }, new { onclick = "$('div.detail', $(this).closest('li.s')).slideToggle('fast');return false;" })%>
				<span class="user"><%=entry.User != null ? " by " + Html.ActionLinkFormatted(entry.User.Name, "Detail", "Users", new { id = entry.User.Id }, null) : ""%></span>
			</h3>
			<div class="detail">
				<div class="teaser"><%= entry.Teaser%></div>
				<div class="content"><a href="#" onclick="return showContent(this);">Show full content</a></div>
				<div class="companyUrl"><input type="checkbox" name="url" value="<%=entry.Company.Url %>" /> <a href="<%=entry.Company.Url %>" target="_blank"><%=entry.Company.Url%></a> <em>Company website</em></div>
				<div class="extractedUrls"></div>
				<div class="buttons floatContainer">
					<input type="button" class="button" value="Delete" onclick="deleteEntry(this);" />
					<input type="button" class="button" value="Extract urls" onclick="extractLinks(this);" />
					<input type="button" class="button" value="Add hosts to block list" onclick="addHostsToBlockList(this);" />
				</div>
			</div>
		</li>
<%
		}
%>
		</ul>
		<div>
			<%=Html.ActionLinkFormatted("Validate and Continue >>", "Dashboard", "Admin", new { id = ViewData["LastId"] }, null)%>
		</div>
	</div>
<%
	}
	else
	{
%>
		<p class="statusMessage">No suspected entries.</p>
<%
	}
%>
	<hr style="margin-top: 20px;"/>
	<div>
		<form action="" onsubmit="addHostToBlockListPost($('#hostName').val(), function(){$('#hostName').val('').focus();});return false;">
			<label for="fromId">Manually add host to black list</label>
			<%=Html.TextBox("hostName") %>
			<input type="submit" value="Submit" />
		</form>
	</div>
	<hr style="margin-top: 20px;"/>
	<div>
		<form action="" onsubmit="site.getActivity($('#ip').val(), activityUrl);return false;">
			<label for="fromId">Manually check ip</label>
			<%=Html.TextBox("ip") %>
			<input type="submit" value="Submit" />
		</form>
		<div id="activityContainer"></div>
	</div>
	<script type="text/javascript">
		var activityUrl = '<%=Url.Action("ActivityByIp", "Users") %>';
		var deleteUrl = '<%=Url.Action("Delete", "Entries") %>';
		var extractLinksUrl = '<%=Url.Action("ExtractLinks", "Admin") %>';
		var addHostsToBlockListUrl = '<%=Url.Action("AddHostsToBlackList", "Admin") %>';
		var showContentUrl = '<%=Url.Action("ShowContent", "Entries") %>';

		function getId(sender)
		{
			return parseInt($(sender).closest("li.s").attr("id").substring(1), 10);
		}
		
		function showContent(sender)
		{
			var id = getId(sender);
			var container = $(sender).closest("div.content");
			container.append("&nbsp; <img src='/images/loading-mini.gif' alt='' />");
			$.post(showContentUrl, {id:id}, function(data){
				container.empty().append($("<div />").addClass("loaded").append(data));
			});
			return false;
		}
		
		function deleteEntry(sender)
		{
			if (confirm("Are you sure you want to delete this entry?"))
			{
				var id = getId(sender);
				$.post(deleteUrl, {id:id}, function(){
					$("#e" + id).remove();
				});
			}
		}
		
		function extractLinks(sender)
		{
			var id = getId(sender);
			$.post(extractLinksUrl, {id:id}, function(data){
				var container = $("#e" + id + " div.extractedUrls");
				if (data.length > 0)
				{
					for (var i=0;i<data.length;i++)
					{
						var url = data[i];
						container.append($("<div />")
							.append($('<input type="checkbox" name="url" />').attr("value", url))
							.append(" ")
							.append($("<a target='_blank'/>").attr("href", url).text(url)));
					}
					$("#e" + id + " input:checkbox[name='url']").attr("checked", true);
				}
				else
				{
					alert("The entry does not contain links.");
				}
			});
		}
		
		function addHostsToBlockList(sender)
		{
			var checkboxes = $("input:checked[name='url']", $(sender).closest("li.s"));
			if (checkboxes.length == 0)
			{
				alert("You must select a Url");
			}
			else
			{
				var urls = new Array();
				checkboxes.each(function(){
					urls.push($(this).val());
				});
				addHostToBlockListPost(urls);
			}
		}
		
		function addHostToBlockListPost(urls, callback)
		{
			$.ajax({
				type: "POST",
				url: addHostsToBlockListUrl,
				dataType: "json",
				traditional: true,
				data: {urls: urls},
				success: function(hosts){
					var message = "No host added";
					if (hosts.length > 0)
					{
						message = "The following host were added:";
						for (var i=0;i<hosts.length;i++)
						{
							message += "\n- " + hosts[i];
						}
					}
					alert(message);
					if (callback)
					{
						callback();
					}
				}
			});
		}
	</script>
</asp:Content>