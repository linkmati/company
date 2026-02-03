using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Services;

namespace CompanyFeeds.Web.Controllers
{
	public class AdminController : BaseController
	{

		#region Spam prevention
		/// <summary>
		/// List suspected in rss format
		/// </summary>
		public ActionResult ListSuspectedRss()
		{
			List<Entry> entries = EntriesService.GetSuspected();
			return Rss(this.Domain + ": Suspected entries", this.Domain + Url.Action("Dashboard"), "", entries, "EntryTitle", "Id", "Teaser", "Date", null);
		}

		/// <summary>
		/// List suspected from a certain id (if suplied)
		/// </summary>
		[RequireAuthorization(RequireMailValidation = true)]
		public ActionResult Dashboard(int? id)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}
			if (id == null)
			{
				//Maybe there is a previous id stored in cookies
				id = Cookies.LatestSuspectedEntryId;
			}
			else
			{
				//Set the value in the cookies to avoid re-validate the same suspected entries.
				Cookies.LatestSuspectedEntryId = id;
			}
			var top = 20; //amount of suspected entries to show
			var allEntries = EntriesService.GetSuspected(id, false);
			var entries = allEntries.Take(top);
			if (entries.Count() > 0)
			{
				ViewData["LastId"] = entries.Last().Id;
			}
			if (id != null)
			{
				ViewData["FromId"] = id;
			}
			return View("Dashboard", entries);
		}

		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ExtractLinks(int id)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}
			var entry = EntriesService.Get(id);
			var urls = TextUtils.ExtractLinks(entry.Content);
			return Json(urls);
		}

		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult AddHostsToBlackList(List<string> urls)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}

			List<string> hosts = SpamPreventionHelper.AddHostsToBlackList(urls);

			return Json(hosts);
		}

		#endregion
	}
}
