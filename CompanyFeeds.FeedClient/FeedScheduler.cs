using System;
using System.Collections.Generic;
using System.Text;
using CompanyFeeds;
using CompanyFeeds.Configuration;
using System.Threading;
using System.Timers;

namespace CompanyFeeds.FeedClient
{
	public static class FeedScheduler
	{
		public static FeedClientConfiguration Configuration
		{
			get;
			set;
		}

		public static void StartScheduler(object values)
		{
			Configuration = (FeedClientConfiguration)values;

			HourScheduler sheduler = new HourScheduler(Configuration.Hours);
			sheduler.Elapsed += new ElapsedEventHandler(sheduler_Elapsed);
			sheduler.Start();
			//If its development environment, launch now!
			if (DateTime.Now < new DateTime(2013, 12, 31) || (Environment.MachineName != null && Environment.MachineName.ToLower() == "spa-web415"))
			{
				sheduler_Elapsed(null, null);
			}
		}

		static void sheduler_Elapsed(object sender, ElapsedEventArgs e)
		{
			FeedManager manager = new FeedManager(Configuration);
			manager.GetLatest();
		}
	}
}
