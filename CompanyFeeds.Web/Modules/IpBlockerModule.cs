using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Web.Modules
{
	public class IpBlockerModule : IHttpModule	
	{
		#region IHttpModule Members
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

			var config = SpamPreventionConfiguration.Current;
			if (config != null)
			{
				if (IsIpBlacklisted(context, config))
				{
					context.Response.StatusCode = 404;
					context.Response.SuppressContent = true;
					context.Response.End();
				}
			}
		}
		#endregion

		private bool IsIpBlacklisted(HttpContextBase context, SpamPreventionConfiguration config)
		{
			bool blackListed = false;
			var ip = context.Request.UserHostAddress;
			if (config.IpBlackList[ip] != null)
			{
				blackListed = true;
			}
			else
			{
				//Check if the ip is in a wildcard range like 120.120.10.*
				if (ip.Contains("."))
				{
					var ipRange = ip.Substring(0, ip.LastIndexOf('.')) + ".*";
					if (config.IpBlackList[ipRange] != null)
					{
						blackListed = true;
					}
				}
			}

			return blackListed;
		}
	}
}
