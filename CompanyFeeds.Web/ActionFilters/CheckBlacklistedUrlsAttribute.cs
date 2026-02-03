using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.ActionFilters
{
	public class CheckBlacklistedUrlsAttribute : ActionFilterAttribute
	{
		public string ParamName { get; set; }

		public CheckBlacklistedUrlsAttribute(string paramName)
		{
			ParamName = paramName;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (ContainsBlacklistedUrls(filterContext))
			{
				OnBlacklistedUrlsDetected(filterContext);
			}
			base.OnActionExecuting(filterContext);
		}

		protected virtual void OnBlacklistedUrlsDetected(ActionExecutingContext filterContext)
		{
			filterContext.Result = ResultHelper.ForbiddenSpamResult(filterContext.Controller);
		}

		protected virtual bool ContainsBlacklistedUrls(ActionExecutingContext filterContext)
		{
			if (filterContext.Result != null)
			{
				return false;
			}
			List<string> values = GetTextToCheck(filterContext);
			return SpamPreventionHelper.ContainsBlackListedUrls(values.ToArray());
		}

		private List<string> GetTextToCheck(ActionExecutingContext filterContext)
		{
			if (!filterContext.ActionParameters.ContainsKey(ParamName))
			{
				throw new ArgumentException("Argument was not present at the action");
			}
			var texts = new List<string>();
			var model = filterContext.ActionParameters[ParamName];
			if (model != null)
			{
				if (model is Entry)
				{
					var entry = (Entry)model;
					texts.Add(entry.Content);
					texts.Add(entry.ContactInfo);
				}
				else if (model is Company)
				{
					var company = (Company)model;
					texts.Add(company.Description);
					texts.Add(company.Url);
				}
			}
			return texts;
		}
	}
}
