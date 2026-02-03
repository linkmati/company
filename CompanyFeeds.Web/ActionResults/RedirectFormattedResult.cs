using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.ActionResults
{
	public class RedirectFormattedResult : RedirectToRouteResult
	{
		public new string RouteName
		{
			get;
			set;
		}

		public new RouteValueDictionary RouteValues
		{
			get;
			set;
		}

		public RedirectFormattedResult(string routeName, RouteValueDictionary values) : base(routeName, values)
		{
			this.RouteName = routeName ?? string.Empty;
			this.RouteValues = values ?? new RouteValueDictionary();
		}

		public RedirectFormattedResult(RouteValueDictionary values) : this(null, values)
		{
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			string virtualPath = this.Routes.GetUrl(context.RequestContext, this.RouteValues);
			context.HttpContext.Response.Redirect(virtualPath, false);
		}

		#region Routes
		private RouteCollection _routes;
		internal RouteCollection Routes
		{
			get
			{
				if (this._routes == null)
				{
					this._routes = RouteTable.Routes;
				}
				return this._routes;
			}
			set
			{
				this._routes = value;
			}
		} 
		#endregion
	}
}
