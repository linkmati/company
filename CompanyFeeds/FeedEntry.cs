using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyFeeds
{
	public class FeedEntry
	{
		public FeedEntry()
		{
			Date = DateTime.Now;
		}
		public string Description 
		{ 
			get; 
			set; 
		}
		public string Title
		{
			get;
			set;
		}
		public DateTime Date
		{
			get;
			set;
		}
		public string Content
		{
			get;
			set;
		}
		public string Guid
		{
			get;
			set;
		}
		public string OriginalGuid
		{
			get;
			set;
		}
	}
}
