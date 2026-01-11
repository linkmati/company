using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Services;

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
			PagedList<EntriesQueries.EntriesListRow> entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByDate(date), 0, Int32.MaxValue);
			ViewData["date"] = date;
			return View(entries); 
		}
	}
}
