using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Configuration;
using CompanyFeeds.FeedClient;
using System.IO;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using System.Net;

namespace CompanyFeeds.Tests.FeedClient
{
	/// <summary>
	/// Summary description for FeedClientTests
	/// </summary>
	[TestClass]
	public class FeedClientTests
	{
		public FeedClientTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void FeedManager_Test()
		{
			//FeedClientConfiguration config = FeedClientConfiguration.Load();

			//FeedManager manager = new FeedManager(config);
			//int counterSuccess = manager.GetLatest();
		}

		[TestMethod]
		public void FeedManager_GetAndParseRss()
		{
			#region Get dummy company
			var companies = CompaniesService.GetTopCompanies();
			if (companies.Count == 0)
			{
				Assert.Inconclusive();
				return;
			}

			var company = new Company();
			company.Id = companies[0].Id;
			company.Name = companies[0].Name;
			#endregion

			FeedClientConfiguration config = FeedClientConfiguration.Load();

			var manager = new FeedManager(config);
			var client = new FeedProxy(config);
			var parser = new RssParser();

			var urls = new List<string>();

			//urls.Add("http://www.agilent.com/about/newsroom/rss/agilent_news.xml");
			//urls.Add("http://www.blogsouthwest.com/blogsw/feed");
			//urls.Add("http://feeds2.feedburner.com/kodak/1000words?format=xml");
			//urls.Add("http://feeds2.feedburner.com/Hewlett-Packard/News/ES/ES");

			//urls.Add("http://media.seagate.com/author/storage-effect/feed/");
			//urls.Add("http://www.facebook.com/feeds/press.php?format=rss20");
			//urls.Add("http://feeds.feedburner.com/WholeStory?format=xml");
			urls.Add("http://blogs.adobe.com/conversations/blogrss");
			urls.Add("http://prensa.elcorteinglescorporativo.es/show_xml.html?id=21&root=21");

			int totalAdded = 0;
			int totalGet = 0;
			int failedUrls = 0;

			foreach (string url in urls)
			{
				try
				{
					Stream stream = client.GetFeed(url);
					var entriesList = parser.Parse(stream);
					totalAdded += EntriesService.AddFeedEntries(entriesList, company.Id, company.Name, config.DefaultTeaser, null, parser.Encoding);
					totalGet += entriesList.Count;
				}
				catch (WebException)
				{
					failedUrls++;
				}
				catch (Exception)
				{
					throw;
				}
			}

			Assert.IsTrue(totalGet > 0, "total items: " + totalGet + "; total added: " + totalAdded + "; failedUrls: " + failedUrls);
		}

		//[TestMethod]
		//public void FeedManager_ResearchAndMarkets_GetAndParseRss()
		//{
		//    var company = CompaniesService.Get("research-and-markets");

		//    FeedClientConfiguration config = FeedClientConfiguration.Load();

		//    var manager = new FeedManager(config);
		//    var client = new FeedProxy(config);
		//    var parser = new RssParser();
		//    int totalAdded = 0;
		//    int totalGet = 0;
		//    int failedUrls = 0;
		//    Stream stream = client.GetFeed(company.FeedUrl);
		//    var entriesList = parser.Parse(stream);
		//    totalAdded += EntriesService.AddFeedEntries(entriesList, company.Id, company.Name, config.DefaultTeaser, null, parser.Encoding);
		//    totalGet += entriesList.Count;

		//    Assert.IsTrue(totalGet > 0, "total items: " + totalGet + "; total added: " + totalAdded + "; failedUrls: " + failedUrls);
		//}
	}
}
