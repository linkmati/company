using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Caching;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.ActionFilters
{
	public class ResultCacheAttribute : ActionFilterAttribute
	{
		public ResultCacheAttribute()
		{

		}

		public string CacheKey
		{
			get;
			private set;
		}

		public CacheWrapper Cache
		{
			get;
			private set;
		}

		public CacheDependency Dependency
		{
			get;
			set;
		}

		private CacheItemPriority _priority = CacheItemPriority.Default;
		public CacheItemPriority Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
			}
		}

		/// <summary>
		/// Duration in seconds of the cached values before expiring.
		/// </summary>
		public int Duration
		{
			get;
			set;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string url = filterContext.HttpContext.Request.Url.PathAndQuery;
			this.CacheKey = "ResultCache-" + url;
			this.Cache = new CacheWrapper(filterContext.HttpContext.Cache);

			if (!this.Cache.NotCachedUrls.Contains(url))   
			{
				if (filterContext.HttpContext.Cache[this.CacheKey] != null)
				{
					filterContext.Result = (ActionResult)filterContext.HttpContext.Cache[this.CacheKey];
				}
			}
			else
			{
				this.Cache.NotCachedUrls.Remove(url);
			}

			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewData["CachedStamp"] = DateTime.Now;
            filterContext.HttpContext.Cache.Add(this.CacheKey, filterContext.Result, Dependency, DateTime.Now.AddSeconds(Duration), System.Web.Caching.Cache.NoSlidingExpiration, Priority, null);
		
			base.OnActionExecuted(filterContext);
		}
	}
}
