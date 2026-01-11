using System;
using System.Collections.Generic;
using System.Text;
using CompanyFeeds.DataAccess.Queries.LogsQueriesTableAdapters;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Services
{
	public class LogService
	{
		public static void LogStatus(string title, string message)
		{
			AddLog(Priority.Status, "Status: " + title, new StringBuilder(message), null);
		}

		public static void LogException(string header, Exception ex, string extraInfo, MailConfiguration config)
		{
			try
			{
				switch (ex.GetType().Name)
				{
					case "HttpAntiForgeryException":
					case "HttpRequestValidationException":
						return;
				}
			}
			catch 
			{
 
			
			}

			StringBuilder message = new StringBuilder();
			if (ex.InnerException != null)
			{
				message.AppendLine("-------INNER EXCEPTION----------");
				message.AppendLine(ex.InnerException.Message);
				message.AppendLine(ex.InnerException.StackTrace);
				message.AppendLine("Inner Exception type: " + ex.InnerException.GetType().Name);
				message.AppendLine("-----------EXCEPTION------------");
			}
			message.AppendLine();
			message.AppendLine(ex.Message);
			message.AppendLine();
			message.AppendLine("Exception type: " + ex.GetType().Name);
			message.AppendLine();
			message.AppendLine(ex.StackTrace);
			message.AppendLine();
			if (extraInfo != null)
			{
				message.AppendLine("Extra info");
				message.AppendLine(extraInfo);
			}
			AddLog(Priority.High, header + ": " + ex.GetType().Name, message, config);
		}

		public static void LogException(string header, Exception ex, Uri url, MailConfiguration config)
		{
			LogException(header, ex, url.AbsoluteUri, config);
		}

		internal static void AddLog(Priority priority, string title, StringBuilder message, MailConfiguration config)
		{
			QueriesTableAdapter ta = new QueriesTableAdapter();
			ta.InsertLog((short)priority, title, message.ToString());

			if (priority == Priority.High && config != null)
			{
				NotificationsService.SendMail(config.AdminMailAddress, null, config.LogMailAddress, null, null, title, message, config.SmtpServer, false, config.GetCredentials());
			}
		}

		public enum Priority : short
		{
			Status = 0,
			Low = 1,
			High = 2
		}
	}
}
