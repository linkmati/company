using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.ActionResults;
using System.Collections;
using System.Web.Routing;
using System.Web;

namespace CompanyFeeds.Web.Helpers
{
	public class ResultHelper
	{
		/// <summary>
		/// Returns a status 404 to the client and the error 404 view.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="emptyBody">false: the response ends</param>
		/// <returns></returns>
		public static ActionResult NotFoundResult(ControllerBase controller, bool emptyBody)
		{
			controller.ControllerContext.HttpContext.Response.StatusCode = 404;
			if (emptyBody)
			{
				controller.ControllerContext.HttpContext.Response.End();
			}

			ViewResult viewResult = new ViewResult();
			viewResult.ViewName = "/Views/Error/404.aspx";
			viewResult.ViewData["TrackerUrl"] = "/error/404?requrl=" + HttpUtility.UrlEncode(controller.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return viewResult;
		}

		public static ActionResult NotFoundResult(ControllerBase controller)
		{
			return NotFoundResult(controller, false);
		}

		public static ViewResult ForbiddenResult(ControllerBase controller)
		{
			return ForbiddenResult(controller, false);
		}

		public static ViewResult ForbiddenResult(ControllerBase controller, bool emptyBody)
		{
			controller.ControllerContext.HttpContext.Response.StatusCode = 403;
			if (emptyBody)
			{
				controller.ControllerContext.HttpContext.Response.End();
			}

			ViewResult viewResult = new ViewResult();
			viewResult.ViewName = "~/Views/Error/403.aspx";
			viewResult.ViewData = new ViewDataDictionary();
			viewResult.ViewData["TrackerUrl"] = "/error/403?requrl=" + HttpUtility.UrlEncode(controller.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return viewResult;
		}

		public static JsonCallbackResult JsonCallbackResult(ControllerBase controller, object data, string callback)
		{
			JsonCallbackResult result = new JsonCallbackResult();
			result.Data = data;
			result.Callback = callback;
			return result;
		}

		public static RssResult RssResult(string title, string url, string description, IEnumerable dataSource, string itemTitleProperty, string itemUrlProperty, string itemDescriptionProperty, string itemDateProperty, UrlGetHandler urlGetHandler)
		{
			RssResult result = new RssResult();
			result.Title = title;
			result.Url = url;
			result.UrlGetHandler = urlGetHandler;
			result.Description = description;
			result.DataSource = dataSource;
			result.ItemTitleProperty = itemTitleProperty;
			result.ItemUrlProperty = itemUrlProperty;
			result.ItemDateProperty = itemDateProperty;
			result.ItemDescriptionProperty = itemDescriptionProperty;

			return result;
		}

		public static RedirectFormattedResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary values, RouteData routeData)
		{
			if (string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentException("Cannot be null or empty {0}", "actionName");
			}
			RouteValueDictionary valuesComplete = new RouteValueDictionary();
			valuesComplete["action"] = actionName;
			if (!string.IsNullOrEmpty(controllerName))
			{
				valuesComplete["controller"] = controllerName;
			}
			if ((!valuesComplete.ContainsKey("controller") && (routeData != null)) && routeData.Values.ContainsKey("controller"))
			{
				valuesComplete["controller"] = routeData.Values["controller"];
			}
			if (values != null)
			{
				foreach (KeyValuePair<string, object> pair in values)
				{
					valuesComplete[pair.Key] = pair.Value;
				}
			}


			return new RedirectFormattedResult(null, valuesComplete);
		}

		public static ActionResult PremiumWall(ControllerBase controllerBase)
		{
			var viewResult = new ViewResult();
			viewResult.ViewName = "~/Views/Accounts/PremiumWall.aspx";
			viewResult.ViewData = new ViewDataDictionary();
			viewResult.ViewData["HideSearchBox"] = true;
			viewResult.ViewData["TrackerUrl"] = "/premium-wall?requrl=" + HttpUtility.UrlEncode(controllerBase.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return viewResult;
		}

		public static ActionResult ForbiddenFloodResult(ControllerBase controllerBase)
		{
			var result = ForbiddenResult(controllerBase);
			result.ViewName = "~/Views/Error/403-Flood.aspx";
			result.ViewData["TrackerUrl"] = "/error/403-flood?requrl=" + HttpUtility.UrlEncode(controllerBase.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return result;
		}

		public static ActionResult ForbiddenSpamSemantics(ControllerBase controllerBase)
		{
			var result = ForbiddenResult(controllerBase);
			result.ViewName = "~/Views/Error/403-SpamSemantics.aspx";
			result.ViewData["TrackerUrl"] = "/error/403-spam-semantics?requrl=" + HttpUtility.UrlEncode(controllerBase.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return result;
		}

		public static ActionResult ForbiddenSpamResult(ControllerBase controllerBase)
		{
			var result = ForbiddenResult(controllerBase);
			result.ViewName = "~/Views/Error/403.aspx";
			result.ViewData["TrackerUrl"] = "/error/403-spam?requrl=" + HttpUtility.UrlEncode(controllerBase.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return result;
		}

		/// <summary>
		/// Result to be used when the user is trying to do complex hacking/posting. 
		/// Returns a 403 http status and sets stats as 403-hack-attemp
		/// </summary>
		/// <returns></returns>
		public static ActionResult ForbiddenHackingAttemp(ControllerBase controllerBase)
		{
			var result = ForbiddenResult(controllerBase);
			result.ViewName = "~/Views/Error/403.aspx";
			result.ViewData["TrackerUrl"] = "/error/403-hack-attemp?requrl=" + HttpUtility.UrlEncode(controllerBase.ControllerContext.HttpContext.Request.Url.PathAndQuery);
			return result;
		}
	}
}
