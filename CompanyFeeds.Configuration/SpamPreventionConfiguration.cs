using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CompanyFeeds.Configuration
{
	public class SpamPreventionConfiguration : ConfigurationSection
	{
		#region Current
		/// <summary>
		/// Gets the current configuration. If not set it returns null
		/// </summary>
		public static SpamPreventionConfiguration Current
		{
			get
			{
				var config = (SpamPreventionConfiguration)ConfigurationManager.GetSection("spamPrevention");
				return config;
			}
		}
		#endregion

		[ConfigurationProperty("ipBlackList", IsRequired = true)]
		public ValueCollection IpBlackList
		{
			get
			{
				return (ValueCollection)this["ipBlackList"];
			}
			set
			{
				this["ipBlackList"] = value;
			}
		}

		[ConfigurationProperty("hostBlackList", IsRequired = false)]
		public ValueCollection HostBlackList
		{
			get
			{
				return (ValueCollection)this["hostBlackList"];
			}
			set
			{
				this["hostBlackList"] = value;
			}
		}
	}
}
