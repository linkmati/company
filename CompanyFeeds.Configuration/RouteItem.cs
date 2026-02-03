using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace CompanyFeeds.Configuration
{
	public class RouteItem
	{
		public RouteItem()
		{
			this.Defaults = new SerializableDictionary<string, object>();
			this.Constraints = new SerializableDictionary<string, object>();
		}

		[XmlAttribute]
		public string Url
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Controller
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Action
		{
			get;
			set;
		}

		public SerializableDictionary<string, object> Defaults
		{
			get;
			set;
		}

		public SerializableDictionary<string, object> Constraints
		{
			get;
			set;
		}
	}
}
