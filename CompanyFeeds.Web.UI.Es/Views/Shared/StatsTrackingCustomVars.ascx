<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl<CompanyFeeds.Web.StatsTrackingCustomVar>" %>
<script type="text/javascript">
	_gaq.push(['_setCustomVar', <%= this.Model.Index %>, '<%= this.Model.Name%>', '<%= this.Model.Value %>', <%= this.Model.Scope%>]);
</script>
