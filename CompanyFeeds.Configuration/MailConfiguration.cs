using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CompanyFeeds.Configuration
{
	public class MailConfiguration
	{
		public string SmtpServer { get; set; }

		public string AdminMailAddress { get; set; }

		public string AdminMailName { get; set; }

		public string LogMailAddress
		{
			get;
			set;
		}

		public string CredentialsUserName { get; set; }

		public string CredentialsPassword { get; set; }

		public NetworkCredential GetCredentials()
		{
			if (String.IsNullOrEmpty(CredentialsUserName))
			{
				return null;
			}
			return new NetworkCredential(CredentialsUserName, CredentialsPassword);
		}

		public string FeedbackMailAddress
		{
			get;
			set;
		}

		public SerializableDictionary<string, string> TemplatesPath
		{
			get;
			set;
		}

		public SerializableDictionary<string, string> Subjects
		{
			get;
			set;
		}
	}
}
