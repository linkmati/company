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
			//#region Get dummy company
			//CompaniesQueries.CompanyNamesDataTable companies = CompaniesService.GetCompanies();
			//if (companies.Count == 0)
			//{
			//    Assert.Inconclusive();
			//    return;
			//}

			//CompaniesQueries.CompaniesFeedInfoRow company = new CompaniesQueries.CompaniesFeedInfoDataTable().NewCompaniesFeedInfoRow();
			//company.CompanyId = companies[0].CompanyId;
			//company.CompanyName = companies[0].CompanyName; 
			//#endregion

			//FeedClientConfiguration config = FeedClientConfiguration.Load();

			//FeedManager manager = new FeedManager(config);
			//FeedProxy client = new FeedProxy(config);

			//List<string> urls = new List<string>();
			//
			//urls.Add("http://www.agilent.com/about/newsroom/rss/agilent_news.xml");
			//urls.Add("http://media.seagate.com/author/storage-effect/feed/");
			//urls.Add("http://www.facebook.com/feeds/press.php?format=rss20");
			//urls.Add("http://www.blogsouthwest.com/blogsw/feed");
			//urls.Add("http://feeds2.feedburner.com/kodak/1000words?format=xml");
			//urls.Add("http://feeds.feedburner.com/WholeStory?format=xml");

			//foreach (string url in urls)
			//{
			//    Stream stream = client.GetFeed(url);
			//    manager.ParseRss(stream, company);
			//}
		}
	}
}
