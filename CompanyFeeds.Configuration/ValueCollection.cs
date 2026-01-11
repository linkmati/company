using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CompanyFeeds.Configuration
{
	public class ValueCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ValueElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ValueElement)(element)).Value;
		}

		public ValueElement this[int idx]
		{
			get
			{
				return (ValueElement)BaseGet(idx);
			}
		}

		public new ValueElement this[string key]
		{
			get
			{
				return (ValueElement)BaseGet(key);
			}
		}
	}
}
