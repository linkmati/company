<%@ Page Language="C#" Inherits="CompanyFeeds.Web.UI.Es.Views.Home.SitemapsDetail" %><?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/0.9
			    http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd">
	<asp:Repeater runat="server" id="rep">
		<ItemTemplate>
		   <url>
			  <loc><%#Domain %><%# Html.Url("Detail", "Entries", new{companyTag=Eval("CompanyTag"), tag=Eval("EntryTag"), id=Eval("EntryId")}) %></loc>
			  <changefreq>weekly</changefreq>
		   </url>
		</ItemTemplate>
	</asp:Repeater>
</urlset> 