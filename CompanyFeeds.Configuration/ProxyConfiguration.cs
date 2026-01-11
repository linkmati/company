using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompanyFeeds.Configuration
{
	public class ProxyConfiguration
	{
		[XmlAttribute]
		public bool UseProxy
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public string Domain
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}
	}
}
