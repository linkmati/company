using System;
using System.Web;
using System.Web.Routing;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.Routing;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.UI
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			var routeConfig = RouteMappingConfiguration.Load();
			RoutingHelper.RegisterRoutes(RouteTable.Routes, routeConfig);
		}

		protected void Session_Start(object sender, EventArgs e)
		{
            //Try to read cookie
//		    try
//		    {
//		        SecurityHelper.ReadMemberCookie(Context.Request.Cookies, Context.Response.Cookies,
//		            new SessionWrapper(new HttpSessionStateWrapper(Context.Session)));
//		    }
//		    catch (Exception)
//		    {
//		        //Do nothing
//		    }
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}