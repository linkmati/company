<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl" %>
<script type="text/javascript">
	var _gaq = _gaq || [];
	_gaq.push(['_setAccount', 'UA-7354927-1']);
	_gaq.push(['_setCustomVar', 1, 'logged-in', <%= this.Session.User != null ? "'Yes'" : "'No'" %>, 2]);
	_gaq.push(['_setCustomVar', 5, 'page-action', '<%=this.ViewContext.RouteData.Values["Controller"] + "." + this.ViewContext.RouteData.Values["Action"]%>', 3]);
</script>