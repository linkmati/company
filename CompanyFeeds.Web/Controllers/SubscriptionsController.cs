using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	public class SubscriptionsController : BaseController
	{
		[RequireAuthorization]
		public ActionResult List(string companyName)
		{
			if (companyName != null)
			{
				Add(companyName);
			}

			SubscriptionsQueries.SubscriptionsDataTable companies = SubscriptionsService.Get(User.Id);
			if (companies.Count > 0)
			{
				ViewData["Companies"] = companies;
				ViewData["Entries"] = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByUser(User.Id, SiteConfiguration.SubscriptionPageSize), 0, SiteConfiguration.SubscriptionPageSize);
			}


			return View();
		}

		[RequireAuthorization]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Add(string companyName)
		{
			try
			{
				SubscriptionsService.Add(User.Id, companyName);
			}
			catch
			{
			}
			return Json("");
		}

		[RequireAuthorization]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Remove(string companyName)
		{
			try
			{
				SubscriptionsService.Remove(User.Id, companyName);
			}
			catch
			{
			}
			return Json("");
		}
	}
}
