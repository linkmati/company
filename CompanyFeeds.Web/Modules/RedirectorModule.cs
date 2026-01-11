using System;
using System.Collections.Generic;
using System.Web;
using CompanyFeeds.Configuration;
using System.Text.RegularExpressions;

namespace CompanyFeeds.Web.Modules
{
	public class RedirectorModule : IHttpModule
	{
		private const string configCacheKey = "RedirectorConfiguration";
		public void Dispose()
		{

		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
		}

		void context_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			HttpContextBase context = new HttpContextWrapper(application.Context);

			RedirectorConfiguration config = GetConfiguration(context); 

			RedirectRequest(context, config);
		}

		#region Configuration
		private RedirectorConfiguration GetConfiguration(HttpContextBase context)
		{
			//Cache the configuration;
			if (context.Cache[configCacheKey] == null)
			{
				context.Cache[configCacheKey] = RedirectorConfiguration.Load();
			}

			return (RedirectorConfiguration)context.Cache[configCacheKey];
		} 
		#endregion

		#region Redirect Request
		public void RedirectRequest(HttpContextBase context, RedirectorConfiguration config)
		{
			HttpRequestBase request = context.Request;
			HttpResponseBase response = context.Response;

			string rawUrl = request.Url.AbsoluteUri;

			if (request.HttpMethod.ToUpper() != "GET" && request.HttpMethod.ToUpper() != "HEAD")
			{
				return;
			}

			#region Ignores
			if (config.IgnoreRegex != null)
			{
				if (Regex.IsMatch(rawUrl, config.IgnoreRegex))
				{
					return;
				}
			}
			#endregion

			foreach (RedirectorUrlGroup group in config.UrlGroups)
			{
				//For each group of regular expression, check if the url matches
				if (Regex.IsMatch(rawUrl, group.Regex))
				{
				    foreach (RedirectorUrl url in group.Urls)  
				    {
				        //For each regular expression in the group check if it matches
				        if (Regex.IsMatch(rawUrl, url.Regex))
				        {
							string headers = HttpUtility.UrlDecode(request.Headers.ToString()); 
							if (url.HeaderRegex == null || Regex.IsMatch(headers, url.HeaderRegex, RegexOptions.IgnoreCase))
							{
								string urlResult = Regex.Replace(rawUrl, url.Regex, url.Replacement);
								response.StatusCode = url.ResponseStatus;
								response.AddHeader("Location", urlResult);
								response.End();
								break;
							}
				        }
				    }
				    break;
				}
			}
		} 
		#endregion
	}
}
