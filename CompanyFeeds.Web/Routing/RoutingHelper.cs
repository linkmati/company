using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Configuration;
using System.Web.Routing;
using System.Web.Mvc;

namespace CompanyFeeds.Web.Routing
{
	public static class RoutingHelper
	{
		public static void RegisterRoutes(RouteCollection routes, RouteMappingConfiguration config)
		{
			routes.IgnoreRoute("{*staticFile}", new { staticFile = @".*\.(ico|gif|png|css|jpg)" });

			foreach (RouteItem item in config.Routes)
			{
				var route = new SiteRoute(item.Url, new MvcRouteHandler());
				route.Defaults = new RouteValueDictionary(item.Defaults);
				route.Defaults.Add("Controller", item.Controller);
				route.Defaults.Add("Action", item.Action);
				route.Constraints = new RouteValueDictionary(item.Constraints);
				routes.Add(route);
			}
		}
	}
}
