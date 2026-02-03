<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Search results</title>
	<script src="/scripts/jquery.autocomplete.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Search results</h1>
	<script type="text/javascript">
	$(document).ready(function()
		{
			$("#txtSearch").autocompleteArray(
				companyNamesList
				,{delay:40,selectOnly:true,onItemSelect: function(){$("#txtSearch").focus();}});
		});	
	</script>
	<div class="searchBox">
		<form action="/search/" onsubmit="return site.search($('#txtSearch').val());">
		  <div>
			<input type="hidden" name="cx" value="partner-pub-2423614996377514:2140891529" />
			<input type="hidden" name="cof" value="FORID:11" />
			<input type="hidden" name="ie" value="UTF-8" />
			<input type="text" name="q" class="text" id="txtSearch" value="<%=ViewData["Searched"] %>" />
			<input type="submit" name="sa" class="button" value="Search &gt;&gt;" />
		  </div>
		</form>
	</div>
	<br />
	<div style="width: 710px;overflow:hidden;">
		<div id="cse-search-results"></div>
		<script type="text/javascript">
		  var googleSearchIframeName = "cse-search-results";
		  var googleSearchFormName = "cse-search-box";
		  var googleSearchFrameWidth = 710;
		  var googleSearchFrameborder = 0 ;
		  var googleSearchDomain = "www.google.com";
		  var googleSearchPath = "/cse";
		</script>
		<script type="text/javascript" src="http://www.google.com/afsonline/show_afs_search.js"></script>
	</div>
<!--
<div id="cse-search-results"></div>
<script type="text/javascript">
  var googleSearchIframeName = "cse-search-results";
  var googleSearchFormName = "cse-search-box";
  var googleSearchFrameWidth = 800;
  var googleSearchDomain = "www.google.com";
  var googleSearchPath = "/cse";
</script>
<script type="text/javascript" src="http://www.google.com/afsonline/show_afs_search.js"></script>

-->	
</asp:Content>
