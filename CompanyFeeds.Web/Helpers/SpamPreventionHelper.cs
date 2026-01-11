using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Configuration;
using System.Text.RegularExpressions;
using CompanyFeeds.Web.State;
using System.Web;
using CompanyFeeds.Services;

namespace CompanyFeeds.Web.Helpers
{
	public static class SpamPreventionHelper
	{
		private static HashSet<string> HostBlackList
		{
			get
			{
				var cacheWrapper = new CacheWrapper(HttpRuntime.Cache);
				if (cacheWrapper.HostBlackList == null)
				{
					cacheWrapper.HostBlackList = new HashSet<string>(SpamPreventionService.GetHostBlackList());
				}
				return cacheWrapper.HostBlackList;
			}
		}

		/// <summary>
		/// Clears the cached list to force refresh
		/// </summary>
		private static void ClearHostBlackList()
		{
			var cacheWrapper = new CacheWrapper(HttpRuntime.Cache);
			cacheWrapper.HostBlackList = null;
		}

		/// <summary>
		/// Verifies if any of the values provided contains black listed urls
		/// </summary>
		/// <param name="values">Can be urls or html</param>
		/// <returns></returns>
		public static bool ContainsBlackListedUrls(params string[] values)
		{
			bool contains = false;
			var blackList = HostBlackList;
			foreach (string v in values)
			{
				if (!String.IsNullOrEmpty(v))
				{
					var urls = new List<string>();
					if (TextUtils.ContainsHtmlMarkup(v))
					{
						urls = TextUtils.ExtractLinks(v);
					}
					else
					{
						urls.Add(v);
					}
					var hostList = UrlUtils.GetHosts(urls);
					foreach (string host in hostList)
					{
						if (blackList.Contains(host))
						{
							contains = true;
							break;
						}
					}
					if (contains)
					{
						break;
					}
				}
			}
			return contains;
		}

		/// <summary>
		/// Gets hosts from the urls and add the hosts to black list
		/// </summary>
		public static List<string> AddHostsToBlackList(List<string> urls)
		{
			var hosts = SpamPreventionService.AddHostsToBlackList(urls);
			if (hosts.Count > 0)
			{
				ClearHostBlackList();
			}

			return hosts;
		}
	}
}
