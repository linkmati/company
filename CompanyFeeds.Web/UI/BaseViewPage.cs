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
using System.Web.Caching;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Web.UI
{
	public class BaseViewPage : BaseViewPage<object>
	{
		
	}

	public class BaseViewPage<TModel> : ViewPage<TModel> where TModel : class
	{
		public string PageTitle
		{
			get;
			set;
		}

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
		public new UserState User
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

		public new ViewMasterPage Master
		{
			get
			{
				return (ViewMasterPage)this.Master;
			}
		}

		public ModelStateDictionary ModelState
		{
			get
			{
				return ViewData.ModelState;
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

		#region Write
		public void Write(object value)
		{
			Response.Write(value);
		}
		#endregion

		protected override void OnInit(EventArgs e)
		{
			this.Cache = new CacheWrapper(base.Cache);
			this.Session = new SessionWrapper(this.ViewContext.HttpContext.Session);
			base.OnInit(e);
		}
	}
}
