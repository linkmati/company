using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.State;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Web.UI
{
	public class BaseViewUserControl<TModel> : ViewUserControl<TModel> where TModel : class
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

		/// <summary>
		/// Current cached configuration
		/// </summary>
		public SiteConfiguration Config
		{
			get
			{
				return Cache.SiteConfiguration;
			}
		}

		#region User
		public UserState User
		{
			get
			{
				return Session.User;
			}
		}

		public bool UserIsAdmin
		{
			get
			{
				if (User != null && User.Role == UserRole.Admin)
				{
					return true;
				}
				return false;
			}
		}
		#endregion

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

	public class BaseViewUserControl : BaseViewUserControl<object>
	{

	}
}
