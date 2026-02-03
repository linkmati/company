using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompanyFeeds
{
	public static class ReflectionUtils
	{
		/// <summary>
		/// Gets a property value from an object
		/// </summary>
		/// <param name="container">Container to extract the value from.</param>
		/// <param name="propName">Property name.</param>
		public static object GetPropertyValue(object container, string propName)
		{
			PropertyDescriptor descriptor = null;
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (string.IsNullOrEmpty(propName))
			{
				throw new ArgumentNullException("propName");
			}
			if (!propName.Contains('.'))
			{
				descriptor = TypeDescriptor.GetProperties(container).Find(propName, true);
			}
			else
			{
				string[] properties = propName.Split('.');
				container = GetPropertyValue(container, properties[0]);
				descriptor = TypeDescriptor.GetProperties(container).Find(properties[1], true);
			}
			if (descriptor == null)
			{
				throw new ArgumentException(propName + " property not found");
			}
			return descriptor.GetValue(container);
		}

		/// <summary>
		/// Gets a property value from an object
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="container">Container to extract the value from.</param>
		/// <param name="propName">Property name.</param>
		/// <returns></returns>
		public static T GetPropertyValue<T>(object container, string propName)
		{
			return (T)GetPropertyValue(container, propName);
		}
	}
}
