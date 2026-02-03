using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.ActionResults;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	public class CategoriesController : BaseController  
	{
		[ResultCache(Duration=120)] 
		public ActionResult Detail(string categoryTag, int page, ResultType type)
		{
			CategoriesQueries.CategoriesRow category = CategoriesService.GetCategory(categoryTag);
			if (category == null)    
			{ 
			 	throw new ArgumentException("No category for given tag.");
			}
			ViewData["CategoryName"] = category.CategoryName;
			ViewData["Description"] = category.CategoryDescription;
			ViewData["CategoryTag"] = categoryTag;
			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.LogoPath);
			ViewData["ActiveCategoryId"] = category.CategoryId;

			PagedList<EntriesQueries.EntriesListRow> entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByCategory(category.CategoryId), page, SiteConfiguration.CategoryPageSize);
			
			#region Rss
			if (type == ResultType.Rss)
			{
				return Rss(SiteConfiguration.RssBaseTitle + " - " + category.CategoryName, Domain, "Press releases from " + SiteConfiguration.RssBaseTitle, entries, "EntryTitle", null, "EntryTeaser", "EntryDate", GetEntryDetailUrl);
			}
			#endregion

			return View(entries); 
		}
	}
}
