using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CompanyFeeds.Configuration;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.ActionResults;
using CompanyFeeds.Web.Helpers;
using System.Web.Routing;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	public class HomeController : BaseController    
	{
		#region Index
		public ActionResult Index(int page, ResultType type)
		{
			if (Cache.HomeEntries == null) 
			{
				Cache.HomeEntries = EntriesService.GetRelevant();
			}

			ViewData["Page"] = page;
			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.LogoPath);
			PagedList<EntriesQueries.EntriesListRow> entries = new PagedList<EntriesQueries.EntriesListRow>(Cache.HomeEntries, page, SiteConfiguration.HomePageSize); 

			#region Subscriptions
			if (User != null)
			{
				//If the user is logged in, try to get the subscribed companies. 
				EntriesQueries.EntriesListDataTable dt = EntriesService.GetByUser(User.Id, SiteConfiguration.SubscriptionHomePageSize);
				if (dt.Count > 0)
				{
					ViewData["Subscriptions"] = new PagedList<EntriesQueries.EntriesListRow>(dt, 0, Int32.MaxValue);
				}
			}
			#endregion

			#region Rss
			if (type == ResultType.Rss)
			{
				return Rss(SiteConfiguration.RssBaseTitle, Domain, "Homepage press releases from " + SiteConfiguration.RssBaseTitle, entries, "EntryTitle", null, "EntryTeaser", "EntryDate", GetEntryDetailUrl);
			}
			#endregion

			return View(entries);
		} 
		#endregion

		#region Latest
		public ActionResult Latest(int page, ResultType type)
		{
			//Get the latest entries
			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.LogoPath);
			PagedList<EntriesQueries.EntriesListRow> entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetLatest(SiteConfiguration.LatestMax), page, SiteConfiguration.LatestPageSize);

			#region Rss
			if (type == ResultType.Rss)
			{
				return Rss(SiteConfiguration.RssBaseTitle + " (Latest)", Domain, "Latest press releases from " + SiteConfiguration.RssBaseTitle, entries, "EntryTitle", null, "EntryTeaser", "EntryDate", GetEntryDetailUrl);
			}
			#endregion
			return View(entries);
		} 
		#endregion

		#region Static pages
		public ActionResult About()
		{
			return StaticView("About");
		}

		public ActionResult Terms()
		{
			return StaticView("Terms");
		}
		#endregion

		#region SearchResults
		public ActionResult SearchResults(string q)
		{
			ViewData["Searched"] = q;
			return StaticView("SearchResults");
		}
		#endregion

		#region Sitemaps details
		public ActionResult SitemapsDetail() 
		{
			EntriesQueries.EntriesListDataTable entries = EntriesService.GetLatest(50000);

			return View(entries);
		}
		#endregion

		#region Sitemaps
		public ActionResult Sitemaps()
		{
			return View();
		}
		#endregion

		#region Error
		[RequireAuthorization(RequireMailValidation = true)]
		public ActionResult Error()
		{
			if (UserIsAdmin)
			{
				throw new Exception("Exception forced to be thrown");
			}
			return NotFoundResult();
		}
		#endregion
	}
}
