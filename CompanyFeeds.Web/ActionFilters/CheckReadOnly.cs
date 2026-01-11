using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.ActionFilters
{
    public class CheckReadOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cache = new CacheWrapper(filterContext.HttpContext.Cache);
            if (cache.SiteConfiguration.IsReadOnly)
            {
                var view = new ViewResult
                {
                    ViewName = "~/Views/Error/ReadOnly.aspx"
                };
                view.ViewData["TrackerUrl"] = "/error/read-only?requrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.PathAndQuery);
                filterContext.Result = view;

            }
            base.OnActionExecuting(filterContext);
        }
    }
}
