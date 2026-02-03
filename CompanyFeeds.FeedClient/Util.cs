using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CompanyFeeds.FeedClient
{
	public class Util
	{
		public static bool ValidateUrl(string value)
		{
			string pattern = @"^https?:((//)+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";
			return Regex.IsMatch(value, pattern);
 		}
	}
}
