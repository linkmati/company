using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace CompanyFeeds.FeedClient
{
	//Generates an specific event when at specific hours
	public class HourScheduler
	{
		public event ElapsedEventHandler Elapsed;

		public int[] Hours
		{
			get;
			set;
		}

		public HourScheduler(int[] hours)
		{
			Hours = hours;
		}

		public void Start()
		{
			if (Hours != null)
			{
				foreach (int hour in Hours) 
				{
					DateTime date = DateTime.Today.AddHours(hour);
					if (date <= DateTime.Now)
					{
						date = date.AddDays(1);
					}
					double milliseconds = date.Subtract(DateTime.Now).TotalMilliseconds;

					Timer timer = new Timer(milliseconds);
					timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
					timer.Enabled = true;
					timer.Start(); 
				}
			}
		}

		void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			((Timer)sender).Enabled = false;
			if (Elapsed != null)
			{
				Elapsed(this, e);
			}
		}

	}
}
