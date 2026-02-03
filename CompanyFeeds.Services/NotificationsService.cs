using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;
using System.IO;
using CompanyFeeds.Configuration;
using CompanyFeeds.Validation;
using System.Net;

namespace CompanyFeeds.Services
{

	public class NotificationsService
	{
		private delegate void NotifyCommentsHandler(Entry entry, User sender, string url, string unsubscribeUrl, SiteConfiguration config);

		/// <summary>
		/// Sends a mail to the user in order to validate email address.
		/// </summary>
		public static void SendValidationMail(User user, string path, string from, string fromName, string subject, string validationUrl, string smtpHost, NetworkCredential credentials)
		{
			SendMail(
				from,
				fromName,
				user.Email,
				user.Name,
				null,
				subject,
				path,
				user,
				new string[] { "Name" },
				new Dictionary<string, string>() { { "Url", validationUrl } },
				smtpHost,
				credentials);

		}

		/// <summary>
		/// Notify users that a new comment has been made.
		/// </summary>
		public static void NotifyComments(Entry entry, User sender, string url, string unsubscribeUrl, SiteConfiguration config)
		{
			List<User> users = CommentsService.GetUsersToNotify(entry.Id);

			//foreach user to notify
			foreach (User u in users)
			{
				if ((u.Id != 0 && u.Id == sender.Id) || u.Email.ToLower() == sender.Email.ToLower())
				{
					//The sender is one of the users to notify
					//do nothing
				}
				else
				{
					string replacedUnsub = String.Format(unsubscribeUrl, entry.Id, u.Id, u.Email);
					//Replace the email template
					SendMail(
						config.Mail.AdminMailAddress,
						config.Mail.AdminMailName,
						u.Email,
						u.Name,
						null,
						"RE: " + entry.EntryTitle,
						config.GetApplicationRootConfigurationPath(config.Mail.TemplatesPath["NewComment"]),
						null,
						null,
						new Dictionary<string, string> { { "Name", u.Name }, { "Url", url }, { "EntryTitle", entry.EntryTitle }, { "UnsubscribeUrl", replacedUnsub } },
						config.Mail.SmtpServer,
						config.Mail.GetCredentials());
				}
			}
		}

		/// <summary>
		/// Notify users that a new comment has been made, asynchronously.
		/// </summary>
		public static void NotifyCommentsAsync(Entry entry, User sender, string url, string unsubscribeUrl, SiteConfiguration config)
		{
			NotifyCommentsHandler handler = new NotifyCommentsHandler(NotifyComments);
			handler.BeginInvoke(entry, sender, url, unsubscribeUrl, config, null, null);
		}

		/// <summary>
		/// Send a contact / report bugs / etc.
		/// </summary>
		/// <exception cref="ValidationException">Throws this exception when the fields are not properly set.</exception>  
		public static void SendFeedback(string name, string email, string message, string host, string subject, MailConfiguration config)
		{
			#region Validation
			ValidationException ex = new ValidationException(new List<ValidationError>());
			if (TextUtils.IsNullOrEmpty(name))
			{
				ex.ValidationErrors.Add(new ValidationError("name", ValidationErrorType.NullOrEmpty));
			}
			if (TextUtils.IsNullOrEmpty(email))
			{
				ex.ValidationErrors.Add(new ValidationError("email", ValidationErrorType.NullOrEmpty));
			}
			else if (!TextUtils.IsValidEmailAddress(email))
			{
				ex.ValidationErrors.Add(new ValidationError("email", ValidationErrorType.Format));
			}
			if (TextUtils.IsNullOrEmpty(message))
			{
				ex.ValidationErrors.Add(new ValidationError("message", ValidationErrorType.NullOrEmpty));
			}
			if (ex.ValidationErrors.Count > 0)
			{
				throw ex;
			}
			#endregion

			SendMail(config.AdminMailAddress, config.AdminMailName, config.FeedbackMailAddress, null, email, subject + " - " + host, new StringBuilder(message), config.SmtpServer, false, config.GetCredentials());
		}

		/// <summary>
		/// Sends a mail using the template and the values from the properties
		/// </summary>
		/// <param name="templatePath">Complete path of the file to use as a template</param>
		/// <param name="value">object to extract the values of and replaced from the template. If null, no values will be replaced.</param>
		/// <param name="propertyNames">name of the properties with the values.</param>
		/// <param name="extraValues">Dictionary of key/value pairs to replace.</param>
		public static void SendMail(string from, string fromName, string to, string toName, string replyTo, string subject, string templatePath, object value, string[] propertyNames, Dictionary<string,string> extraValues, string smtpHost, NetworkCredential credentials)
		{
			string body = File.ReadAllText(templatePath, Encoding.UTF8);

			body = ReplaceValues(body, value, propertyNames);

			body = ReplaceValues(body, extraValues);

			SendMail(from, fromName, to, toName, replyTo, subject, new StringBuilder(body), smtpHost, true, credentials);
		}

		internal static string ReplaceValues(string body, object value, string[] propertyNames)
		{
			if (value != null)
			{
				//replace all the template values with the object values.
				foreach (string name in propertyNames)
				{
					body = body.Replace("<!--!" + name + "!-->", ReflectionUtils.GetPropertyValue<string>(value, name));
				}
			}
			return body;
		}

		internal static string ReplaceValues(string body, Dictionary<string,string> values)
		{
			if (values != null)
			{
				//replace all the template values with the object values.
				foreach (KeyValuePair<string,string> pair in values)
				{
					body = body.Replace("<!--!" + pair.Key + "!-->", pair.Value);
				}
			}
			return body;
		}

		/// <summary>
		/// Sends an email
		/// </summary>
		public static void SendMail(string from, string fromName, string to, string toName, string replyTo, string subject, StringBuilder body, string smtpHost, bool isBodyHtml, NetworkCredential credentials)
		{
			MailMessage message = new MailMessage();
			#region From & To
			if (fromName != null)
			{
				message.From = new MailAddress(from, fromName);
			}
			else
			{
				message.From = new MailAddress(from);
			}
			if (toName != null)
			{
				message.To.Add(new MailAddress(to, toName));
			}
			else
			{
				message.To.Add(new MailAddress(to));
			} 
			#endregion
			if (replyTo != null)
			{
				message.ReplyTo = new MailAddress(replyTo);
			}

			message.Subject = subject;
			message.BodyEncoding = Encoding.UTF8;
			message.Body = body.ToString();
			message.IsBodyHtml = isBodyHtml;

			SmtpClient smtp = new SmtpClient(smtpHost);
			if (credentials != null)
			{
				smtp.Credentials = credentials;
			}
		    smtp.EnableSsl = smtp.Port != 25;
			smtp.Send(message);
		}

 

	}
}
