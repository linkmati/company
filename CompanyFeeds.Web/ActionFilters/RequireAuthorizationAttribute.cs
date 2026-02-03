using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.State;
using System.Web;
using System.Web.Security;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.ActionFilters
{
	public class RequireAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
	{
		public bool RequireMailValidation
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if the filter must return forbidden status in the case the user is not logged in
		/// </summary>
		public bool RefuseOnFail
		{
			get;
			set;
		}

		#region IAuthorizationFilter Members
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			
			SessionWrapper session = new SessionWrapper(filterContext.HttpContext.Session);
			if (IsAuthorized(session.User, RequireMailValidation))
			{
				HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
				cachePolicy.SetProxyMaxAge(new TimeSpan(0));
			}
			else
			{
				//Authorization failed
				if (RefuseOnFail)
				{
					filterContext.Result = ResultHelper.ForbiddenResult(filterContext.Controller);
					return;
				}
				string redirectOnSuccess = HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.PathAndQuery);

				//send them off to the login page
				string redirectUrl = string.Format("?returnUrl={0}", redirectOnSuccess);
				string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
				filterContext.HttpContext.Response.Redirect(loginUrl, true);
			}

		}

		#endregion

		public virtual bool IsAuthorized(UserState user, bool requireMailValidation)
		{
			if (user == null)
			{
				return false;
			}
			if (requireMailValidation && !user.IsEmailActive)
			{
				return false;
			}
			return true;
		}
	}
}
