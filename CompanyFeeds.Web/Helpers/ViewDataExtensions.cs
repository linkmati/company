using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;

namespace CompanyFeeds.Web.Helpers
{
	public static class ViewDataExtensions
	{
		public static T Get<T>(this ViewDataDictionary viewData, string key, T valueIfNull)
		{
			if (viewData[key] == null)
			{
				return valueIfNull;
			}
			else
			{
				return (T)viewData[key];
			}
		}


		public static ViewDataDictionary Create(this ViewDataDictionary viewData, object values)
		{
			var viewDataEmpty = new ViewDataDictionary();
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
			{
				object value = descriptor.GetValue(values);
				viewDataEmpty.Add(descriptor.Name, value);
			}
			return viewDataEmpty;
		}
	}
}
