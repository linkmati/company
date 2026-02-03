using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.State;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.ActionFilters
{
	/// <summary>
	/// Prevents a user (determined by the ip) to post unlimited times on an action.
	/// Checks that the user posted less than max amount of times. If the user exceeds the max, it gives an forbidden result
	/// </summary>
	public class PreventFloodAttribute : ActionFilterAttribute
	{
		#region Constructor, Field and Props

		/// <summary>
		/// Determines the type of the action result in case of a success. If null, means that it is a get request
		/// </summary>
		public Type SuccessResultType
		{
			get;
			set;
		}

		public Type EntityType
		{
			get;
			set;
		}

		protected int Max
		{
			get;
			set;
		}

		public PreventFloodAttribute(Type entityType) : this(null, entityType)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="successResultType"></param>
		/// <param name="max">Max amount of submittions</param>
		public PreventFloodAttribute(Type successResultType, Type entityType)
		{
			this.SuccessResultType = successResultType;
		}
		#endregion

		#region Before action execution
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SetMaxSubmittions(filterContext);
			var session = new SessionWrapper(filterContext.HttpContext.Session);
			if (session.User != null && session.User.Role != UserRole.Admin && !session.User.IsPremium)
			{
				//Checks if the user is flooding, show captcha
				var isFlooding = IsFlooding(filterContext, filterContext.ActionDescriptor);
				if (isFlooding)
				{
					OnFlooding(filterContext);
				}
			}
			base.OnActionExecuting(filterContext);
		}

		protected virtual void OnFlooding(ActionExecutingContext filterContext)
		{
			var cache = new CacheWrapper(filterContext.HttpContext);
			if (cache.SiteConfiguration.Flood.ShowPaywall)
			{
				filterContext.Result = ResultHelper.PremiumWall(filterContext.Controller);
			}
			else
			{
				filterContext.Result = ResultHelper.ForbiddenFloodResult(filterContext.Controller);
			}
		}

		/// <summary>
		/// Determines the max amount of submittions allowed
		/// </summary>
		/// <param name="filterContext"></param>
		protected virtual void SetMaxSubmittions(ActionExecutingContext filterContext)
		{
			var cache = new CacheWrapper(filterContext.HttpContext);
			if (EntityType == typeof(Entry))
			{
				Max = cache.SiteConfiguration.Flood.MaxEntries;
			}
			else
			{
				Max = cache.SiteConfiguration.Flood.MaxCompanies;
			}

			if (Max <= 0)
			{
				Max = 1;
			}
		}
		#endregion

		#region After action execution
		/// <summary>
		/// Called after the action method executes
		/// </summary>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}
			var session = new SessionWrapper(filterContext.HttpContext.Session);
			if (session.User != null && session.User.Role != UserRole.Admin && !session.User.IsPremium)
			{
				//If the action was successful
				if (IsSuccess(filterContext.Result))
				{
					ActionExecutedSuccess(filterContext, filterContext.ActionDescriptor);
				}
			}
			base.OnActionExecuted(filterContext);
		}
		#endregion

		#region Is / Set Flooding
		/// <summary>
		/// Checks if the user is flooding
		/// </summary>
		protected virtual bool IsFlooding(ControllerContext context, ActionDescriptor action)
		{
			bool isFlooding = false;
			var cache = new CacheWrapper(context.HttpContext);
			var actionCount = cache.GetUserActionCount(context.HttpContext.Request.UserHostAddress, GetActionKey(action));

			if (actionCount >= Max)
			{
				isFlooding = true;
			}
			return isFlooding;
		}

		/// <summary>
		/// Stores that the action was executed
		/// </summary>
		protected virtual void ActionExecutedSuccess(ControllerContext context, ActionDescriptor action)
		{
			var cache = new CacheWrapper(context.HttpContext);
			cache.SetUserActionCount(context.HttpContext.Request.UserHostAddress, GetActionKey(action));

		}

		protected virtual string GetActionKey(ActionDescriptor action)
		{
			return action.ControllerDescriptor.ControllerName + "." + action.ActionName;
		}
		#endregion

		#region Is Success
		/// <summary>
		/// Determines if the action execution was successful. ie: Redirection after save, 
		/// </summary>
		protected virtual bool IsSuccess(ActionResult actionResult)
		{
			return SuccessResultType != null && actionResult != null && actionResult.GetType().IsSubclassOf(SuccessResultType);
		} 
		#endregion
	}
}
