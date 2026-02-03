<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="false" CodeBehind="Detail.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Entries.Detail" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		function reportAbuse(id)
		{
			$.post("/entries/reportabuse/", {EntryId:id}, reportAbuseCallback);
		}
		
		function reportAbuseCallback(result)
		{
			alert("Thank you for informing us about an abuse on this post.\nAn administrator will review the post shortly.");
		}
		
		function callbackFail() { }
		$(document).ready(function () {
		    if (!site.regv) return;
		    site.regv(<%=Model.CompanyId%>);
		});
	</script>
	<title><%=Model.EntryTitle%></title>
	<%=Html.MetaDescription(Model.EntryTeaser) %>
    <link rel="canonical" href="<%=Html.Url("Detail", "Entries", new { tag = Model.EntryTag, id = Model.EntryId, companyTag = Model.CompanyTag }) %>" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<ul class="path">
		<li><a href="/">Press releases</a></li>
		<li><%= Html.ActionLinkFormatted(Model.CategoryName, "Detail", "Categories", new{categoryTag=Model.CategoryTag}, null) %></li>
		<li><%= Html.ActionLinkFormatted(Model.CompanyName, "Detail", "Companies", new{companyTag=Model.CompanyTag}, null ) %></li>
	</ul>
	<div id="detail">
		<h1><%= Model.EntryTitle%></h1>
		<div class="floatContainer">
		
			<div class="like">
				<script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like href="<%= this.Domain +  Request.Url.PathAndQuery %>" show_faces="false" width="80" font=""></fb:like>
			</div>
			<h2>
				<%= Html.ActionLinkFormatted(Model.CompanyName, "Detail", "Companies", new{companyTag=Model.CompanyTag}, null ) %> - <%=Model.EntryDate.ToLongDateString() %>.
				<asp:Label runat="server" id="lblSubmitted"><br />Submitted by <%=Html.RenderUser(Model) %>.</asp:Label>
			</h2>
		</div>
		<div class="content">
			<% Html.RenderPartialIf((!Model.IsPremium) && DateTime.Now.Subtract(Model.EntryDate) > new TimeSpan(2, 0, 0, 0), "AdslotConditional"); %>
			<asp:Panel runat="server" ID="pnlContent" Visible="false">
				<%=Model.EntryContent %>
			</asp:Panel>
			<asp:Panel runat="server" ID="pnlOriginalSource" Visible="false">
				<p><%=Model.EntryTeaser %></p>
				<br />
				<p>Continue reading at the <a href="<%= Model.EntrySource %>" target="_blank" rel="nofollow" onclick="site.trackOutboundLink('original-source', this.href);return false;">original source &gt;&gt;</a></p>
			</asp:Panel>
			<% Html.RenderPartialIf(!Model.IsPremium, "Adslot", new{SlotId="1516360456", Width=728, Height=90});%>
		</div>
		<h3>About <%= Model.CompanyName  %></h3>
		<%= Model.CompanyDescription %>
		<p class="moreInfo"><%= Html.ActionLinkFormatted("More about " + Model.CompanyName, "Detail", "Companies", new{companyTag=Model.CompanyTag}, null)%></p>
		<asp:Panel runat="server" ID="pnlContactInfo" Visible="false">
			<h3>Contact info</h3>
			<p><%=ViewData.Model.EntryContactInfo %></p>
		</asp:Panel>
		<% Html.RenderPartial("Toolbar"); %>
		<%//Only if entry allows comments
		Html.RenderPartialIf(ViewData["Comments"] != null, "CommentsBox", ViewData["Comments"]);
		%>
	</div>
	<% Html.RenderPartial("StatsTrackingCustomVars", new CompanyFeeds.Web.StatsTrackingCustomVar(2, "company", this.Model.CompanyName)); %>
	<% Html.RenderPartial("StatsTrackingCustomVars", new CompanyFeeds.Web.StatsTrackingCustomVar(3, "age", DateTime.Now.Subtract(this.Model.EntryDate).Days.ToString())); %>
	<% Html.RenderPartial("StatsTrackingCustomVars", new CompanyFeeds.Web.StatsTrackingCustomVar(4, "entry-content", Model.IsEntrySourceNull() ? "Full" : (Model.IsEntryContentNull() ? "External-Min" : "External-Full"))); %>
	<input type="hidden" id="entryId" value="<%=Model.EntryId %>" />
	<script type="text/javascript" src="http://s7.addthis.com/js/152/addthis_widget.js" defer="defer"></script>
</asp:Content>
