using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Caching;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.State;
using System.Configuration;

namespace CompanyFeeds.Web.ActionFilters
{
	public class IpFilterAttribute : ActionFilterAttribute
	{
		public int Max
		{
			get;
			set;
		}

		public IpFilterAttribute(int max)
		{
			this.Max = max;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string key = filterContext.HttpContext.Request.Url.PathAndQuery;
			if (!CanContinue(filterContext.HttpContext.Request.UserHostAddress, key, new CacheWrapper(filterContext.HttpContext.Cache)))
			{
				filterContext.Result = ResultHelper.NotFoundResult(filterContext.Controller, true);
			}

			base.OnActionExecuting(filterContext);
		}

		public bool CanContinue(string ip, string key, CacheWrapper cache)
		{
			Dictionary<string, Dictionary<string, int>> ipFilters = cache.IpFilters;
			if (ipFilters.ContainsKey(ip))
			{
				if (!ipFilters[ip].ContainsKey(key))
				{
					ipFilters[ip].Add(key, 0);
				}
			}
			else
			{
				ipFilters.Add(ip, new Dictionary<string, int>());
				ipFilters[ip].Add(key, 0);
			}

			ipFilters[ip][key]++;

			if (ipFilters[ip][key] > Max)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
