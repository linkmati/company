<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.Views.Shared.SearchBox" %>
<div class="searchBox">
	<h2>Search for companies &amp; press releases</h2>
	<form action="/search/" onsubmit="return site.search($('#txtSearch').val());">
	  <div>
		<input type="hidden" name="cx" value="partner-pub-2423614996377514:2140891529" />
		<input type="hidden" name="cof" value="FORID:11" />
		<input type="hidden" name="ie" value="UTF-8" />
		<input type="text" name="q" class="text" id="txtSearch" />
		<input type="submit" name="sa" class="button" value="Search &gt;&gt;" />
	  </div>
	</form>
</div>