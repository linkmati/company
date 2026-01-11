using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CompanyFeeds.Configuration
{
	public class RedirectorUrlGroup
	{
		[XmlAttribute]
		public string Regex { get; set; }

		public RedirectorUrl[] Urls { get; set; }

		public RedirectorUrlGroup()
		{

		}

		public RedirectorUrlGroup(string regexGroup, string regex, string replacement)
		{
			Regex = regexGroup;
			Urls = new RedirectorUrl[] { new RedirectorUrl(regex, replacement) };
		}
	}
}
