using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Mvc;
using System.Web.Routing;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.Helpers
{
	public static class CustomExtensions
	{
		public static string RenderUser(this HtmlHelper htmlHelper, DataRow dr)
		{
			string userName = dr["UserName"].ToString();
			int? agencyId = dr["AgencyId"] == DBNull.Value ? null : (int?)dr["AgencyId"];
			int? userId = dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]; 
			TagBuilder tag = null;

			if (userName == null)
			{
				throw new NullReferenceException("Username can not be null.");
			}
			if (agencyId != null)
			{
				//Its a user from an agency
				string agencyTag = Convert.ToString(dr["AgencyTag"]);
				string agencyName = Convert.ToString(dr["AgencyName"]);
				tag = new TagBuilder("a");
				tag.Attributes.Add("href", UrlExtensions.GenerateFormattedUrl(null, "Detail", "Agencies", new RouteValueDictionary(new{agencyTag = agencyTag}), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext));
				tag.InnerHtml = agencyName;
			}
			else if (userId != null)
			{
				//Its a normal user
				tag = new TagBuilder("a");
				tag.Attributes.Add("href", UrlExtensions.GenerateFormattedUrl(null, "Detail", "Users", new RouteValueDictionary(new{id = userId.Value}), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext));
				tag.Attributes.Add("rel", "nofollow");
				tag.InnerHtml = userName;
			}
			else
			{
				//Its the robot
				tag = new TagBuilder("span");
				tag.InnerHtml = userName.ToString();
			}

			return tag.ToString(TagRenderMode.Normal);
		}

		public static string RenderUser(this HtmlHelper htmlHelper, DataRow dr, string pre)
		{
			if (dr["UserId"] == DBNull.Value || Convert.ToBoolean(dr["HideAuthor"]))
			{
				return null;
			}
			else
			{
				return pre + " " + htmlHelper.RenderUser(dr);
			}
		}

		public static string RenderLogo(this HtmlHelper htmlHelper , string extension, string companyTag, string alt, string logoPath)
		{
			if (extension == null || logoPath == null || logoPath == "")
			{
				return null;
			}
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", String.Format(logoPath, companyTag));
			tag.Attributes.Add("alt", alt); 
			return tag.ToString(TagRenderMode.SelfClosing);
		}

		public static string RenderDate(this HtmlHelper htmlHelper, object value, string todayText, string yesterdayText, string format)
		{
			if (!(value is DateTime))
			{
				throw new ArgumentException("should be a datetime type", "value");
			}
			DateTime date = Convert.ToDateTime(value);

			if (date.Date == DateTime.Today)
			{
				return todayText;
			}
			else if (date.Date == DateTime.Today.AddDays(-1))
			{
				return yesterdayText;
			}
			else
			{
				return date.ToString(format);
			}
		}

		public static string RenderDate(this HtmlHelper htmlHelper, object value)
		{
			return htmlHelper.RenderDate(value, "Today", "Yesterday", "d");
		}
	}
}
