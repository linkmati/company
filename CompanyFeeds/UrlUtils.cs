using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompanyFeeds
{
	public class UrlUtils
	{
		public static string ToUrlTag(string value, int maxLength)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			value = value.ToLowerInvariant();
			value = Regex.Replace(value, @"[^a-z- ]+", "");
			value = Regex.Replace(value, @" ", "-");
			if (value.Length > maxLength)
			{
				value = value.Substring(0, maxLength);
			}
			return value;
		}

		/// <summary>
		/// Removes the protocol and the www if appears
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string RemovePrefix(string url)
		{
			return Regex.Replace(url, @"https?://(?:www.)?(.*)", "$1");
		}

		public static bool IsValid(string value)
		{
			string pattern = @"^https?:((//)+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";
			return Regex.IsMatch(value, pattern);
		}

		/// <summary>
		/// Get the host from the url
		/// </summary>
		public static string GetHost(string url)
		{
			string host = null;
			string pattern = "^((http[s]?|ftp):\\/)?\\/?([^/]*?)([^./]+\\.([a-z\\.]){2,6})(:\\d{1,6})?(/.*)?$";
			url = url.ToLower();
			var match = Regex.Match(url, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			if (match != null && match.Groups.Count >= 4)
			{
				host = match.Groups[4].Value;
			}
			return host;
		}

		/// <summary>
		/// Gets a list of host out of a list of urls
		/// </summary>
		public static List<string> GetHosts(List<string> urls)
		{
			var hostList = new List<string>();
			foreach (var url in urls)
			{
				string host = GetHost(url);
				if (host != null && !hostList.Contains(host))
				{
					hostList.Add(host);
				}
			}
			return hostList;
		}
	}
}
