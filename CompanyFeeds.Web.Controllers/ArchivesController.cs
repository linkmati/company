using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Services;
using CompanyFeeds.Web.ActionFilters;
using System.Linq;

namespace CompanyFeeds.Web.Controllers
{
	public class ArchivesController : BaseController
	{
		public ActionResult ListDates()
		{
			return View();
		}

		public ActionResult Detail(int year, int month, int day)
		{
			DateTime date = new DateTime(year, month, day);
			var entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByDate(date), 0, Int32.MaxValue);
			ViewData["date"] = date;
			return View(entries); 
		}

		/// <summary>
		/// Displays a list of most viewed entries per year
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public ActionResult MostViewed(int year)
		{
			if (!SiteConfiguration.MostViewed.ContainsKey(year.ToString()))
			{
				return NotFoundResult();
			}
			var ids = SiteConfiguration.MostViewed[year.ToString()];
			var entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByIdList(ids), 0, Int32.MaxValue);
			return View(entries);
		}
	}
}
