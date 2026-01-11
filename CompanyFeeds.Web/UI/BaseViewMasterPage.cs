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
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.UI
{
	public class BaseViewMasterPage : ViewMasterPage
	{
		public new CacheWrapper Cache
		{
			get;
			set;
		}

		public new SessionWrapper Session
		{
			get;
			set;
		}

		public UserState User
		{
			get
			{
				return Session.User;
			}
		}

		#region Domain
		/// <summary>
		/// Gets the application current domain (Host) including Protocol and delimiter. Example: http://www.contoso.com (without slash).
		/// </summary>
		public string Domain
		{
			get
			{
				if (this.ViewContext == null)
				{
					return "http://www.contoso.com";
				}
				return this.ViewContext.HttpContext.Request.Url.Scheme + Uri.SchemeDelimiter + this.ViewContext.HttpContext.Request.Url.Host;
			}
		}
		#endregion


		protected override void OnInit(EventArgs e)
		{
			this.Cache = new CacheWrapper(this.ViewContext.HttpContext.Cache);
			this.Session = new SessionWrapper(this.ViewContext.HttpContext.Session);
			base.OnInit(e);
		}
	}
}
