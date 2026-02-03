using System.Web.Caching;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.Validation;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.State;
using System;
using CompanyFeeds.Web.ActionResults;
using System.Collections.Generic;
using System.Collections;
using System.Web.Routing;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	[HandleErrorLog(View = "/Views/Error/500.aspx")]
	public class BaseController : Controller
	{
		#region SessionWrapper
		private SessionWrapper _session;
		public new SessionWrapper Session
		{
			get
			{
				if (_session == null)
				{
					if (HttpContext != null)
					{
						_session = new SessionWrapper(HttpContext.Session);
					}
					else
					{
						throw new NullReferenceException("Controller context not set.");
					}
				}
				return _session;
			}
		}
		#endregion

		#region CacheWrapper
		private CacheWrapper _cache;
		public CacheWrapper Cache
		{
			get
			{
				if (_cache == null)
				{
					if (HttpContext != null)
					{
						_cache = new CacheWrapper(HttpContext.Cache);
					}
					else
					{
						throw new NullReferenceException("Controller context not set.");
					}
				}
				return _cache;
			}
		} 
		#endregion

		#region Domain
		/// <summary>
		/// Gets the application current domain (Host) including Protocol and delimiter. Example: http://www.contoso.com (without slash).
		/// </summary>
		public string Domain
		{
			get
			{
				if (Request == null)
				{
					return "http://www.contoso.com";
				}
				return Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host;
			}
		} 
		#endregion

		#region User
		public new UserState User
		{
			get
			{
				return Session.User;
			}
		}

		public bool UserIsAdmin
		{
			get
			{
				if (User != null && User.Role == UserRole.Admin)
				{
					return true;
				}
				return false;
			}
		}
		#endregion

		#region Configuration
		public SiteConfiguration SiteConfiguration
		{
			get
			{
				return Cache.SiteConfiguration;
			}
		} 
		#endregion

		#region Init
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			base.Initialize(requestContext);

			Init(requestContext);
		}

		public virtual void Init(System.Web.Routing.RequestContext requestContext)
		{
			//Ensure Cached Items
			EnsureCachedItems();

			if (User == null && !Session.CookieRetrieved)
			{
				SecurityHelper.ReadMemberCookie(HttpContext.Request.Cookies, HttpContext.Response.Cookies, Session);
			}
		} 
		#endregion

		#region Model state errors
		/// <summary>
		/// Add the errors to the model state.
		/// </summary>
		protected void AddErrors(ModelStateDictionary modelState, ValidationException ex)
		{
			foreach (ValidationError error in ex.ValidationErrors)
			{
				modelState.AddModelError(error.FieldName, error);
			}
		} 

		protected ModelErrorCollection GetErrors(ModelStateDictionary modelStateDictionary)
		{
			ModelErrorCollection errors = new ModelErrorCollection();
			foreach (KeyValuePair<string, ModelState> pair in modelStateDictionary)
			{
				foreach (ModelError error in pair.Value.Errors)
				{
					errors.Add(error);
				}
			}

			return errors;
		}
		#endregion

		#region Results
		protected virtual ActionResult ForbiddenResult()
		{
			return ResultHelper.ForbiddenResult(this);
		}
		/// <summary>
		/// Responds a 404 status code and the 404 error View.
		/// </summary>
		/// <returns></returns>
		protected virtual ActionResult NotFoundResult()
		{
			return ResultHelper.NotFoundResult(this, false);
		}

		protected virtual JsonCallbackResult Json(string callback, object data)
		{
			return ResultHelper.JsonCallbackResult(this, data, callback);
		}

		protected virtual RssResult Rss(string title, string url, string description, IEnumerable dataSource, string itemTitleProperty, string itemUrlProperty, string itemDescriptionProperty, string itemDateProperty, UrlGetHandler urlGetHandler)
		{
			return ResultHelper.RssResult(title, url, description, dataSource, itemTitleProperty, itemUrlProperty, itemDescriptionProperty, itemDateProperty, GetEntryDetailUrl);
		}

		protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary values)
		{
			return ResultHelper.RedirectToAction(actionName, controllerName, values, this.RouteData);
		}

		#region StaticView
		public ActionResult StaticView(string key)
		{
			ViewData["HideSearchBox"] = true;
			return View("~/Views/Static/" + key + ".aspx");
		}
		#endregion

		#region EntryDetailUrl
		/// <summary>
		/// Gets full url of the detail of an entry. Used for callback in rss for getting urls.
		/// </summary>
		/// <param name="value">Typically a EntryListRow</param>
		/// <returns></returns>
		public string GetEntryDetailUrl(object value)
		{
			int id = ReflectionUtils.GetPropertyValue<int>(value, "EntryId");
			string companyTag = ReflectionUtils.GetPropertyValue<string>(value, "CompanyTag");
			string tag = ReflectionUtils.GetPropertyValue<string>(value, "EntryTag");
			return Domain + UrlExtensions.GenerateFormattedUrl("Detail", "Entries", new{id = id,companyTag = companyTag,tag = tag},  RouteTable.Routes, ControllerContext.RequestContext);
		}
		#endregion
		#endregion

		#region EnsureCachedItems
		public virtual void EnsureCachedItems()
		{
			//for every cache item that can be used, check if its null and load it.
			if (Cache.Categories == null)
			{
				Cache.Categories = CategoriesService.GetCategories();
			}

			if (Cache.SiteConfiguration == null)
			{
				Cache.SiteConfiguration = SiteConfiguration.Load();
			}
		} 
		#endregion

		#region ToWebUrl
		/// <summary>
		/// Converts backslashes to slashes.
		/// </summary>
		/// <param name="physicalRelativePath"></param>
		/// <returns></returns>
		public string ToWebUrl(string physicalRelativePath)
		{
			if (physicalRelativePath != null)
			{
				physicalRelativePath = physicalRelativePath.Replace("\\", "/");
				if (!physicalRelativePath.StartsWith("\\"))
				{
					physicalRelativePath = "/" + physicalRelativePath;
				}
			}

			return physicalRelativePath;
		}
		#endregion

	}
}
