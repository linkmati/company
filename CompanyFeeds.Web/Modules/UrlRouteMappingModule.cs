using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.IO;
using CompanyFeeds.Configuration;
using CompanyFeeds.Web.Routing;

namespace CompanyFeeds.Web.Modules
{
	public class UrlRouteMappingModule : IHttpModule
	{
		#region IHttpModule Members
		public void Dispose()
		{

		}

		static object lockObject = new object();
		static bool initialized = false;

		public void Init(HttpApplication context)   
		{
			//prevent init calls in multiple HttpApplications
			if (!initialized)
			{
				lock (lockObject)
				{
					if( !initialized)
					{

						RouteMappingConfiguration config = RouteMappingConfiguration.Load();
						using (RouteTable.Routes.GetWriteLock())
						{
							RoutingHelper.RegisterRoutes(RouteTable.Routes, config);
						}
						initialized = true;
					}
				}
			}
		}
		#endregion
	}
}
