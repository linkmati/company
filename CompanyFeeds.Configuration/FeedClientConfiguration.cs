using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace CompanyFeeds.Configuration
{
	public class FeedClientConfiguration
	{
		public ProxyConfiguration Proxy
		{
			get;
			set;
		}

		public MailConfiguration Mail
		{
			get;
			set;
		}

		public string AcceptLanguage
		{
			get;
			set;
		}

		public string UserAgent
		{
			get;
			set;
		}

		public int WaitSeconds
		{
			get;
			set;
		}

		public string DefaultTeaser
		{
			get;
			set;
		}

		public int[] Hours
		{
			get;
			set;
		}

		#region Load
		/// <summary>
		/// Loads the RedirectorConfiguration from the default path. 
		/// </summary>
		/// <returns></returns>
		public static FeedClientConfiguration Load()
		{
			string fileName = ConfigurationManager.AppSettings["FeedClientConfiguration"];
			if (fileName == null)
			{
				fileName = "Config\\FeedClient.config";
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
		public static FeedClientConfiguration Load(string filePath)
		{
			lock (lockObject)
			{
				FeedClientConfiguration config = null;
				#region Read config
				XmlSerializer serializer = new XmlSerializer(typeof(FeedClientConfiguration));
				FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				try
				{
					config = (FeedClientConfiguration)serializer.Deserialize(stream);
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
