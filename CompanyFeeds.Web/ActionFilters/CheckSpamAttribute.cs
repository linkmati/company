using System;
using System.Web.Mvc;
using CompanyFeeds.Web.State;
using System.Net;
using CompanyFeeds.Services;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.ActionFilters
{
	public class CheckSpamAttribute : ActionFilterAttribute
	{
		public string ParamName { get; set; }

		public bool IsTest { get; set; }

		public CheckSpamAttribute(string paramName)
		{
			ParamName = paramName;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (IsSpam(filterContext) == true)
			{
				OnSpamDetected(filterContext);
			}
			base.OnActionExecuting(filterContext);
		}

		/// <summary>
		/// Checks if the content is considered spam
		/// </summary>
		/// <param name="filterContext"></param>
		/// <returns>true/false or null if there is a network error or must not be validated.</returns>
		protected virtual bool? IsSpam(ActionExecutingContext filterContext)
		{
			bool? result = null;
			var session = new SessionWrapper(filterContext.HttpContext.Session);
			var cache = new CacheWrapper(filterContext.HttpContext.Cache);
			var request = filterContext.HttpContext.Request;
			var config = cache.SiteConfiguration;
			if (config == null)
			{
				throw new ArgumentException("Configuration can not be null");
			}
			try
			{
				if (filterContext.Result != null || String.IsNullOrEmpty(config.AkismetKey) || session.User == null || session.User.Role == UserRole.Admin || session.User.IsPremium)
				{
                    // Posts from premium and admin users are not checked
					return null;
				}

				if (!filterContext.ActionParameters.ContainsKey(ParamName))
				{
					throw new ArgumentException("Argument was not present at the action");
				}

				string content = null;
				string url = null;
				var model = filterContext.ActionParameters[ParamName];
				if (model == null || !filterContext.Controller.ViewData.ModelState.IsValid)
				{
					return null;
				}
				if (model is Entry)
				{
					var entry = (Entry)model;
					content = entry.Content;
					if (entry.CompanyId > 0)
					{
						var company = CompaniesService.Get(entry.CompanyId);
						url = company.Url;
					}
				}
				else if (model is Company)
				{
					var company = (Company)model;
					content = company.Description;
					url = company.Url;
				}
				else
				{
					throw new NotSupportedException("Type " + model.GetType().FullName + " not supported.");
				}
				if (String.IsNullOrEmpty(content))
				{
					return null;
				}
                var api = new AkismetHandler(config.AkismetKey, request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host, null);
				//api.IsTest = IsTest ? true : config.AkismetIsTest;
                var post = new AkismetHandlerComment()
				{
					UserIp = request.UserHostAddress,
					UserAgent = request.UserAgent,
                    Blog = "prsync.com"
				};
				post.CommentContent = content;
				post.CommentAuthorEmail = session.User.Email;
				post.CommentAuthor = session.User.Name;
				post.CommentAuthorUrl = url;

				result = api.CommentCheck(post);
            }
            catch (WebException ex)
            {
                LogService.LogException("Exception on Spam filter", ex, (string)null, config.Mail);
            }
            catch (FormatException ex)
            {
                LogService.LogException("Exception on Spam filter", ex, (string)null, config.Mail);
            }
			return result;
		}

		protected virtual void OnSpamDetected(ActionExecutingContext filterContext)
		{
			filterContext.Result = ResultHelper.ForbiddenSpamSemantics(filterContext.Controller);
		}
	}
}
