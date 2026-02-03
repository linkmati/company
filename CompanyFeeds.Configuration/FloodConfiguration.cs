using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompanyFeeds.Configuration
{
	public class FloodConfiguration
	{
		/// <summary>
		/// Max amount of entries that can be submitted
		/// </summary>
		[XmlAttribute]
		public int MaxEntries
		{
			get;
			set;
		}

		/// <summary>
		/// Max amount of companies that can be submitted
		/// </summary>
		[XmlAttribute]
		public int MaxCompanies
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if a paywall is shown when flooding, if false a forbidden page is shown.
		/// </summary>
		[XmlAttribute]
		public bool ShowPaywall
		{
			get;
			set;
		}
	}
}
