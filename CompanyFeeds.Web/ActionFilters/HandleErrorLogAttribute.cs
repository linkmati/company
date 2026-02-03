using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.ActionFilters
{
	public class HandleErrorLogAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			if (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled)
			{
				var extraInfo = new StringBuilder();
				extraInfo.Append("url: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.Url.AbsoluteUri);
				extraInfo.Append("querystring: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.QueryString.ToString());
				extraInfo.Append("form: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.Form.ToString());
				extraInfo.Append("request headers: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.Headers.ToString()); 
				extraInfo.Append("IP: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.UserHostAddress);
				extraInfo.Append("User host name: ");
				extraInfo.AppendLine(filterContext.HttpContext.Request.UserHostName);
				LogService.LogException("Exception on Controller", filterContext.Exception, extraInfo.ToString(), new CacheWrapper(filterContext.HttpContext.Cache).SiteConfiguration.Mail);
				base.OnException(filterContext);
			}
		}
	}
}
