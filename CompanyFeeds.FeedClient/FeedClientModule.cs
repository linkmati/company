using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.FeedClient
{
	public class FeedClientModule : IHttpModule
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
						initialized = true;

						#region Get Config
						FeedClientConfiguration config = FeedClientConfiguration.Load();

						#endregion

						Thread thread = new Thread(new ParameterizedThreadStart(FeedScheduler.StartScheduler));
						thread.Priority = ThreadPriority.Lowest;
						thread.Start(config);

					}
				} 
			}
		}

		#endregion
	}
}
