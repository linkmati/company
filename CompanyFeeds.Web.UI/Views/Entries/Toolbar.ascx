<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl<EntriesQueries.EntriesDetailRow>" %>
<div class="toolbar floatContainer">
	<ul>
		<li class="subscribe"><%=Html.ActionLinkFormatted("Follow " + Model.CompanyName, "List", "Subscriptions", new{companyName=Model.CompanyName}, null) %></li>
		<li class="additional print"><a href="#" onclick="window.print();return false;">Print</a></li>
		<li class="additional reportAbuse"><a href="#" onclick="reportAbuse(<%= Model.EntryId %>);return false;">Report abuse</a></li>
<%
		if (ViewData.Get<bool>("CanEdit", false))
		{
%>
		<li class="edit" runat="server"><%=Html.ActionLinkFormatted("Edit", "Edit", "Entries", new{id=Model.EntryId, tag=Model.EntryTag, companyTag=Model.CompanyTag}, null) %></li>
<%
		}
		if (UserIsAdmin)
		{
%>
			<li class="edit">
				<script type="text/javascript">
					var deleteUrl = '<%=Url.Action("Delete", "Entries") %>';
					function deleteEntry(id)
					{
						if (confirm("Are you sure you want to delete this entry?"))
						{
							$.post(deleteUrl, {id:id},function(){
								window.location.reload();
							});
						}
						return false;
					}
				</script>
				<a href="#" onclick="return deleteEntry(<%=Model.EntryId %>);">Delete</a>
			</li>
<%
		}
%>
		<li class="addthis additional">
			<script type="text/javascript">var addthis_pub = "jorgebg";</script>
			<a href="http://www.addthis.com/bookmark.php" onmouseover="return addthis_open(this, '', '[URL]', '[TITLE]')" onmouseout="addthis_close()" onclick="return addthis_sendto()"><img src="http://s7.addthis.com/static/btn/lg-share-en.gif" width="125" height="16" alt="" /></a>
		</li>
	</ul>
</div>