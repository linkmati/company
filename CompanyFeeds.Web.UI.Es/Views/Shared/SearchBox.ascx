<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.Es.Views.Shared.SearchBox" %>
<div class="searchBox">
	<h2>Busca empresas y notas de prensa</h2>
	<form action="/search/">
	  <div>
		<input type="hidden" name="cx" value="017167602177516483135:xud0qxwzkqu" />
		<input type="hidden" name="cof" value="FORID:11" />
		<input type="hidden" name="ie" value="UTF-8" />
		<input type="text" name="q" class="text" id="txtSearch" />
		<input type="submit" name="sa" class="button" value="Buscar &gt;&gt;" />
	  </div>
	</form>

<script type="text/javascript" src="http://www.google.com/coop/cse/brand?form=cse-search-box&lang=es"></script>

</div>