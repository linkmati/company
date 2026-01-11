using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CompanyFeeds.Web.State
{
	public class CookiesHelper
	{
		private HttpCookieCollection _requestCookies;
		private HttpCookieCollection _responseCookies;

		public CookiesHelper(HttpRequestBase request, HttpResponseBase response)
		{
			_requestCookies = request.Cookies;
			_responseCookies = response.Cookies;
		}

		public CookiesHelper(HttpContextBase context)
			: this(context.Request, context.Response)
		{

		}

		/// <summary>
		/// Id of the latest checked entry id of suspected entries
		/// </summary>
		public int? LatestSuspectedEntryId
		{
			get
			{
				int? id = null;
				if (_requestCookies["spamid"] != null)
				{
					id = Convert.ToInt32(_requestCookies["spamid"].Value);
				}
				return id;
			}
			set
			{
				_responseCookies.Add(new HttpCookie("spamid", Convert.ToString(value)) { Expires = DateTime.Now.AddDays(365)});
			}
		}
	}
}
