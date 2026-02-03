using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace CompanyFeeds.Configuration
{
	public class RedirectorConfiguration
	{
		public RedirectorUrlGroup[] UrlGroups { get; set; }

		public string IgnoreRegex { get; set; } 

		public RedirectorConfiguration()
		{

		}

		#region Load
		/// <summary>
		/// Loads the RedirectorConfiguration from the default path. 
		/// </summary>
		/// <returns></returns>
		public static RedirectorConfiguration Load()
		{
			string fileName = ConfigurationManager.AppSettings["RedirectorConfiguration"];
			if (fileName == null)
			{
				fileName = "Config\\Redirector.config";
			}
			
			//System.Configuration.Configuration applicationConfiguration = ConfigurationManager.OpenExeConfiguration("");
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			 
			string filePath = Path.Combine(Path.GetDirectoryName(basePath), fileName);
			return Load(filePath);

			//RedirectorConfiguration config = new RedirectorConfiguration();
			//config.MatchGroups = new RedirectorMatchGroup[] {new RedirectorMatchGroup(){Regex="/rss/", ReplaceItems=new RedirectorReplaceItem[]{new RedirectorReplaceItem(){Regex="/rss/",Value="http://feeds.feedburner.com"}}}};
		}

		private static object lockObject = new object();
		/// <summary>
		/// Loads the RedirectorConfiguration from a file
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static RedirectorConfiguration Load(string filePath)
		{

			lock (lockObject)
			{
				RedirectorConfiguration config = new RedirectorConfiguration();
				#region Read config
				XmlSerializer serializer = new XmlSerializer(typeof(RedirectorConfiguration));
				FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				try
				{
					config = (RedirectorConfiguration)serializer.Deserialize(stream);
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
