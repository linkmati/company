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
	public class SiteConfiguration
	{
		public SiteConfiguration()
		{

		}

		#region Props
		/// <summary>
		/// Amount of entries on a category page.
		/// </summary>
		public int CategoryPageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Amount of entries on a company detail page.
		/// </summary>
		public int CompanyDetailPageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Amount of entries on the home page
		/// </summary>
		public int HomePageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum last entries fetched.
		/// </summary>
		public int LatestMax
		{
			get;
			set;
		}

		/// <summary>
		/// Latest page, entries page size
		/// </summary>
		public int LatestPageSize
		{
			get;
			set;
		}

		/// <summary>
		/// On my subscription page, entries page size
		/// </summary>
		public int SubscriptionPageSize
		{
			get;
			set;
		}

		/// <summary>
		/// On home page, entries page size
		/// </summary>
		public int SubscriptionHomePageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the formated logo path
		/// </summary>
		public string LogoPath
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the formated agency logo path
		/// </summary>
		public string AgencyLogoPath
		{
			get;
			set;
		}

		public int LogoMaxWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Mailer configuration
		/// </summary>
		public MailConfiguration Mail 
		{ 
			get; 
			set;
		}

		public PaymentConfiguration Payment
		{
			get;
			set;
		}

		public FloodConfiguration Flood
		{
			get;
			set;
		}

		/// <summary>
		/// Title used as base for all rss documents.
		/// </summary>
		public string RssBaseTitle
		{
			get;
			set;
		}

		public DateTime LaunchDate
		{
			get;
			set;
		}

		public string AkismetKey
		{
			get;
			set;
		}

		public bool AkismetIsTest
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets a list of most viewed press releases
		/// </summary>
		public SerializableDictionary<string, string> MostViewed
		{
			get;
			set;
		}

        /// <summary>
        /// Determines if the site is currently in read only mode
        /// </summary>
	    public bool IsReadOnly { get; set; }
		#endregion

		#region Load
		public static SiteConfiguration Load()
		{
			string fileName = ConfigurationManager.AppSettings["Site"];
			if (fileName == null)
			{
				fileName = "Config\\Site.config";
			}
			//System.Configuration.Configuration applicationConfiguration = ConfigurationManager.OpenExeConfiguration("");
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			string filePath = Path.Combine(Path.GetDirectoryName(basePath), fileName);
			return Load(filePath);
		}

		private static object lockObject = new object();
		public static SiteConfiguration Load(string fileName)
		{
			lock (lockObject)
			{
				SiteConfiguration config = new SiteConfiguration();
				#region Read config
				XmlSerializer serializer = new XmlSerializer(typeof(SiteConfiguration));
				FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				try
				{
					config = (SiteConfiguration)serializer.Deserialize(stream);
				}
				finally
				{
					stream.Close();
				}
				#endregion

				#region Write config
				//TODO: Asign values to config
				//System.IO.FileStream file = new System.IO.FileStream(@"C:\Inetpub\wwwroot\Tagpoint\CompanyFeeds\Solution\CompanyFeeds.Web.UI\Config\Site.config", System.IO.FileMode.Create);

				//XmlSerializer serializer = new XmlSerializer(typeof(SiteConfiguration));
				//serializer.Serialize(file, config);

				//file.Close();
				#endregion

				return config;
			}
		} 
		#endregion

		#region Methods
		#region Application path
		/// <summary>
		/// Gets the directory path of the application's root configuration (web.config or app.config)
		/// </summary>
		protected string _applicationRootConfigurationPath = null;
		public virtual string GetApplicationRootConfigurationPath()
		{
			if (_applicationRootConfigurationPath == null)
			{
				string localConfig = TextUtils.NullToEmpty(ConfigurationManager.AppSettings["ApplicationRoot"]);
				//System.Configuration.Configuration applicationConfiguration = ConfigurationManager.OpenExeConfiguration("");
				var basePath = AppDomain.CurrentDomain.BaseDirectory;
				_applicationRootConfigurationPath = Path.Combine(Path.GetDirectoryName(basePath), localConfig);
			}
			return _applicationRootConfigurationPath;
		}

		/// <summary>
		/// Gets the directory path of the application's root configuration (web.config or app.config) and combines to the path given.
		/// </summary>
		/// <param name="pathToCombine">relative path to combine the path of the local configuration.</param>
		/// <returns></returns>
		public virtual string GetApplicationRootConfigurationPath(string pathToCombine)
		{
			return Path.Combine(GetApplicationRootConfigurationPath(), pathToCombine);
		}
		#endregion 
		#endregion
	}
}
