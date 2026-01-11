using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace CompanyFeeds.Web.Routing
{
	public class SiteRoute : Route
	{
		public SiteRoute(string url, IRouteHandler handler)
			: base(url, handler)
		{

		}

		public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
		{
			return base.GetRouteData(httpContext);
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return base.GetVirtualPath(requestContext, values);
		}
	}
}
