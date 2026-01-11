using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Collections.Generic;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.Helpers
{
	public static class UrlExtensions
	{

		public static string GetCurrentUrl(ControllerContext context)
		{
			return FormatUrl(context.RouteData.Route.GetVirtualPath(context.RequestContext, context.RouteData.Values).VirtualPath);
		}

		public static string GenerateFormattedUrl(string actionName, string controllerName, object values, RouteCollection routeCollection, RequestContext requestContext)
		{
			return GenerateFormattedUrl(null, actionName, controllerName, new RouteValueDictionary(values), routeCollection, requestContext);
		}

		public static string GenerateFormattedUrl(string routeName, string actionName, string controllerName, RouteValueDictionary valuesDictionary, RouteCollection routeCollection, RequestContext requestContext)
		{
			if (actionName != null)
			{
				if (valuesDictionary.ContainsKey("action"))
				{
					throw new ArgumentException("Values should not contain the action key");
				}
				valuesDictionary.Add("action", actionName);
			}
			if (controllerName != null)
			{
				if (valuesDictionary.ContainsKey("controller"))
				{
					throw new ArgumentException("Values should not contain the controller key");
				}
				valuesDictionary.Add("controller", controllerName);
			}

			#region Work-around to let the routes have the same param names (like page)
			//Copy Route data
			RouteData routeData = new RouteData();
			//Add route values
			foreach (KeyValuePair<string, object> pair in requestContext.RouteData.Values)
			{
				if (valuesDictionary.Keys.Contains(pair.Key))
				{
					routeData.Values.Add(pair.Key, pair.Value);
				}
			}
			//Copy the Request context
			RequestContext clonedContext = new RequestContext(requestContext.HttpContext, routeData);
			#endregion

			return routeCollection.GetUrl(clonedContext, valuesDictionary);
		}

		public static string Url(this HtmlHelper htmlHelper, string actionName, string controllerName, object values)
		{
			return GenerateFormattedUrl(null, actionName, controllerName, new RouteValueDictionary(values), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext);
		}

		public static string GetUrl(this RouteCollection routes, RequestContext context, RouteValueDictionary values)
		{
			return GetUrl(routes, context, values, false);
		}

		public static string GetUrl(this RouteCollection routes, RequestContext context, RouteValueDictionary values, bool throwException)
		{
			string url = null;
			VirtualPathData data = routes.GetVirtualPath(context, values);
			if ((data == null) || string.IsNullOrEmpty(data.VirtualPath))
			{
				if (throwException)
				{
					throw new InvalidOperationException("No route matched.");
				}
			}
			else
			{
				data.VirtualPath = FormatUrl(data.VirtualPath);
				url = data.VirtualPath;
			}
			
			return url;
		}

		public static string FormatUrl(string value)
		{
			if (Regex.IsMatch(value, @"^[^?]+[^/]$"))
			{
				value += "/";
			}

			value = value.ToLower();

			return value;
		}
	}
}
