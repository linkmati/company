<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="searchBoxMini">
	<form action="/search/">
	  <div>
		<input type="hidden" name="cx" value="017167602177516483135:xud0qxwzkqu" />
		<input type="hidden" name="cof" value="FORID:11" />
		<input type="hidden" name="ie" value="UTF-8" />
		<input type="text" name="q" class="text" id="txtSearchMini" value="<%=ViewData["Searched"] %>" />
		<input type="submit" name="sa" class="button" value="" />
	  </div>
	</form>
</div>