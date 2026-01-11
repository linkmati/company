using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.Controllers
{
	public class SubscriptionsController : BaseController
	{
		[RequireAuthorization]
		public ActionResult List(string companyName)
		{
			if (companyName != null)
			{
				Add(companyName, null);
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
		public ActionResult Add(string companyName, int? companyId)
		{
			if (companyId == null)
			{
				companyId = CompaniesHelper.GetCompanyId(companyName);
			}
			if (companyId != 0)
			{
				SubscriptionsService.Add(User.Id, companyId.Value);
				return Json(companyId.Value);
			}
			return Json(null);
		}

		[RequireAuthorization]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Remove(int companyId)
		{
			SubscriptionsService.Remove(User.Id, companyId);
			return Json("OK");
		}
	}
}
