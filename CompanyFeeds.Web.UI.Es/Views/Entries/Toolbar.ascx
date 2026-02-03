<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl<EntriesQueries.EntriesDetailRow>" %>
<div class="toolbar floatContainer">
	<ul>
		<li class="subscribe"><%=Html.ActionLinkFormatted("Seguir a " + Model.CompanyName, "List", "Subscriptions", new{companyName=Model.CompanyName}, null) %></li>
		<li class="print"><a href="#" onclick="window.print();return false;">Imprimir</a></li>
		<li class="reportAbuse"><a href="#" onclick="reportAbuse(<%= Model.EntryId %>);return false;">Reportar abuso</a></li>
<%
		if (ViewData.Get<bool>("CanEdit", false))
		{
%>
		<li class="edit" runat="server"><%=Html.ActionLinkFormatted("Editar", "Edit", "Entries", new{id=Model.EntryId, tag=Model.EntryTag, companyTag=Model.CompanyTag}, null) %></li>
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
						if (confirm("¿Está seguro que desea eliminar esta nota de prensa?"))
						{
							$.post(deleteUrl, {id:id},function(){
								window.location.reload();
							});
						}
						return false;
					}
				</script>
				<a href="#" onclick="return deleteEntry(<%=Model.EntryId %>);">Eliminar</a>
			</li>
<%
		}
%>
		<li class="addthis">
			<a href="https://twitter.com/share" class="twitter-share-button" data-via="prsynces" data-lang="es">Twittear</a>
			<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src="//platform.twitter.com/widgets.js";fjs.parentNode.insertBefore(js,fjs);}}(document,"script","twitter-wjs");</script>
		</li>
		<li class="addthis">
			<div id="fb-root"></div>
			<script>(function(d, s, id) {
			  var js, fjs = d.getElementsByTagName(s)[0];
			  if (d.getElementById(id)) return;
			  js = d.createElement(s); js.id = id;
			  js.src = "//connect.facebook.net/es_ES/all.js#xfbml=1";
			  fjs.parentNode.insertBefore(js, fjs);
			}(document, 'script', 'facebook-jssdk'));</script>
			<div class="fb-like" data-send="false" data-layout="button_count" data-width="100" data-show-faces="false"></div>
		</li>
	</ul>
</div>