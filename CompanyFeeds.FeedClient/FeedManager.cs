using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Configuration;
using System.Threading;
using CompanyFeeds.Validation;

namespace CompanyFeeds.FeedClient
{
	public class FeedManager
	{
		#region Props
		public FeedClientConfiguration Configuration
		{
			get;
			set;
		}
		#endregion

		public FeedManager(FeedClientConfiguration config)
		{
			Configuration = config;
		}

		#region GetAgencyLatest
		public void GetAgencyLatest()
		{
			//Get all premium agencies with rss (also default company)
			//Foreach agency's rss
				//Parse the rss (with extra fields)
				//For each entry
					//Check if company exist in local cache
					//Check if company exist in db
						//if not create
					//Save entry assigned to the company
			//log


		}
		#endregion

		#region Get Lastest
		public int GetLatest()
		{
			LogService.LogStatus("FeedManager GetLatest() started.", "OK");

			//Get from database all the feeds to be updated. 
			var companies = CompaniesService.GetCompaniesFeeds();
			FeedProxy client = new FeedProxy(Configuration);

			int counterFailed = 0;
			int counterSuccess = 0;

			int totalItems = 0;
			long totalLength = 0;
			var parser = new RssParser();

			foreach (Company company in companies)
			{
				try
				{
					if (Configuration.WaitSeconds > 0)
					{
						Thread.Sleep(Configuration.WaitSeconds * 1000);
					}
					string url = company.FeedUrl;
					using (var stream = client.GetFeed(url))
					{
						totalLength += client.LastStreamLength;
						var entriesList = parser.Parse(stream);
						totalItems += EntriesService.AddFeedEntries(entriesList, company.Id, company.Name, Configuration.DefaultTeaser, company.Owner, parser.Encoding);
					}
					counterSuccess++;
				}
				catch (ValidationException ex)
				{
					LogService.LogException("Exception on FeedClient", ex, "while parsing getting: " + company.FeedUrl + ", model error: " + ex.ValidationErrors[0].FieldName + " - type: " + ex.ValidationErrors[0].Type.ToString(), Configuration.Mail);
					counterFailed++;
				}
				catch (Exception ex)
				{
					LogService.LogException("Exception on FeedClient", ex, "while try getting: " + company.FeedUrl, Configuration.Mail);
					counterFailed++;
				}
			}

			LogService.LogStatus("FeedManager GetLatest() finished. New items: " + totalItems, "Success feed urls retrieved: " + counterSuccess + "; Failed: " + counterFailed + "; Total Kbytes retrieved: " + totalLength / 1024);

			return counterSuccess;
		}

		#endregion

		#region Is valid feed
		public bool IsValidFeed(string url, string schemaPath)
		{
			bool isValid = true;
			FeedProxy feedProxy = new FeedProxy(Configuration);
			Stream stream = feedProxy.GetFeeds(url);

			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			//xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
			xmlReaderSettings.IgnoreComments = true;
			//xmlReaderSettings.ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema;

			xmlReaderSettings.Schemas.Add(null, XmlReader.Create("C:\\Documents and Settings\\JBG\\Desktop\\rss20_v3.xsd"));
			xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(xmlReaderSettings_ValidationEventHandler);

			XmlReader reader = XmlReader.Create(stream, xmlReaderSettings);

			try
			{

				while (reader.Read())
				{
				}
			}
			catch (XmlException)
			{
				isValid = false;
			}
			catch (XmlSchemaException)
			{
				isValid = false;
			}
			catch (Exception)
			{
				isValid = false;
			}
			finally
			{
				stream.Close();
				reader.Close();
			}

			return isValid;
		}

		void xmlReaderSettings_ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			throw e.Exception;
		}
		#endregion
	}
}
