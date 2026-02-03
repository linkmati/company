<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="searchBoxMini">
	<form action="/search/" onsubmit="return site.search($('#txtSearchMini').val());">
	  <div>
		<input type="hidden" name="cx" value="partner-pub-2423614996377514:2140891529" />
		<input type="hidden" name="cof" value="FORID:11" />
		<input type="hidden" name="ie" value="UTF-8" />
		<input type="text" name="q" class="text" id="txtSearchMini" value="<%=ViewData["Searched"] %>" />
		<input type="submit" name="sa" class="button" value="" />
	  </div>
	</form>
</div>