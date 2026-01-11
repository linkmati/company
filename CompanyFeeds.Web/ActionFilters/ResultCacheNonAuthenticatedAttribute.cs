using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.State;
using System.Web.UI;
using System.Web;
using System.Security.Principal;

namespace CompanyFeeds.Web.ActionFilters
{
	public class ResultCacheNonAuthenticatedAttribute : ResultCacheAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var session = new SessionWrapper(filterContext.HttpContext.Session);
			if (session.User == null)
			{
				base.OnActionExecuted(filterContext);
			}
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var session = new SessionWrapper(filterContext.HttpContext.Session);
			if (session.User == null)
			{
				base.OnActionExecuting(filterContext);
			}
		}
	}
}
