using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace CompanyFeeds.Web.ActionFilters
{
	public class ClientCacheAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Expiration days
		/// </summary>
		public int ExpirationDays
		{
			get;
			set;
		}

		/// <param name="expirationDays">Days from now when the content expires</param>
		public ClientCacheAttribute(int expirationDays)
		{
			ExpirationDays = expirationDays;
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			HttpResponseBase response = filterContext.HttpContext.Response;
			response.Cache.SetCacheability(HttpCacheability.Public);
			response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
			response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(ExpirationDays)));

			base.OnResultExecuted(filterContext);
		}
	}
}
