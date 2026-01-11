using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Configuration;

namespace CompanyFeeds.Configuration
{
	public class RouteMappingConfiguration
	{
		private RouteMappingConfiguration()
		{

		}

		#region Props
		public RouteItem[] Routes
		{
			get;
			set;
		}

		public RouteItem[] IgnoreRoutes
		{
			get;
			set;
		}
		#endregion

		#region Load
		/// <summary>
		/// Loads from file using the ConfigurationManager filePath and fileName (AppSettings->RouteMapping)
		/// </summary>
		/// <returns></returns>
		public static RouteMappingConfiguration Load()
		{
			string fileName = ConfigurationManager.AppSettings["RouteMapping"];
			if (fileName == null)
			{
				fileName = "Config\\RouteMapping.config";
			}
			//System.Configuration.Configuration applicationConfiguration = ConfigurationManager.OpenExeConfiguration("");
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			string filePath = Path.Combine(Path.GetDirectoryName(basePath), fileName);
			return Load(filePath);
		}

		private static object lockObject = new object();
		public static RouteMappingConfiguration Load(string filePath)
		{
			lock (lockObject)
			{
				RouteMappingConfiguration config = new RouteMappingConfiguration();

				#region Read config
				XmlSerializer deserializer = new XmlSerializer(typeof(RouteMappingConfiguration));
				FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				try
				{
					config = (RouteMappingConfiguration)deserializer.Deserialize(stream);
				}
				finally
				{
					stream.Close();
				}
				#endregion

				return config;
			}
		}
		#endregion
	}
}
