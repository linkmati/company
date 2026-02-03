using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompanyFeeds.Configuration
{
	public class RedirectorUrl
	{
		[XmlAttribute]
		public string Regex { get; set; }

		[XmlAttribute]
		public string Replacement { get; set; }

		[XmlAttribute]
		public string HeaderRegex { get; set; }

		[XmlAttribute]
		public int ResponseStatus { get; set; }

		public RedirectorUrl()
		{
			//Moved permanently by default
			ResponseStatus = 301;
		}

		public RedirectorUrl(string regex, string replacement) : this()
		{
			Regex = regex;
			Replacement = replacement;
		}
	}
}
