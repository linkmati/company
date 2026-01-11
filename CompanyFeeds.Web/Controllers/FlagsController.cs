using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.Validation;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.Helpers;
using System.Web.Routing;

namespace CompanyFeeds.Web.Controllers
{
	public class FlagsController : BaseController
	{
		#region List
		public ActionResult List()
		{
			return Rss("Flags", Domain, "Flags", FlagsService.GetAll(), "FlagName", null, "UserName", "EntryDate", GetEntryDetailUrl);
		}
		#endregion
	}
}
