using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.State;
using System.Web;
using CompanyFeeds.Services;

namespace CompanyFeeds.Tests.Controllers
{
	/// <summary>
	/// Summary description for CompaniesTest
	/// </summary>
	[TestClass]
	public class CompaniesTest
	{
		public CompaniesTest()
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

//		[TestMethod]
//		public void CompaniesHelperAddVisit_Test()
//		{
//			CacheWrapper cache = new CacheWrapper(HttpRuntime.Cache);
//			//The first time register the visit
//			bool result1 = CompaniesHelper.RegisterVisit(2, cache, "127.0.0.1");
//
//			//The rest of the times the visit must NOT registered.
//			bool result2 = CompaniesHelper.RegisterVisit(2, cache, "127.0.0.1");
//
//			//The rest of the times the visit must NOT registered.
//			bool result3 = CompaniesHelper.RegisterVisit(2, cache, "127.0.0.1");
//
//			Assert.IsTrue(result1);
//			Assert.IsFalse(result2);
//			Assert.IsFalse(result3);
//		}

		[TestMethod]
		public void Companies_Integration_Feeds_IsClosed_Test()
		{
			var companies = CompaniesService.GetCompaniesFeeds();
			if (companies.Count == 0)
			{
				Assert.Inconclusive("No companies with feeds to test");
			}

			foreach (var company in companies)
			{
				Assert.IsTrue(CompaniesHelper.IsClosed(company.Id, 0, false));
				Assert.IsTrue(CompaniesHelper.IsClosed(company.Tag, 0, false));

				Assert.IsFalse(CompaniesHelper.IsClosed(company.Id, 0, true));
			}

			companies = CompaniesService.GetTopCompanies();
			foreach (var c in companies)
			{
				var company = CompaniesService.Get(c.Tag);
				if (String.IsNullOrEmpty(company.FeedUrl) && company.Owner == null)
				{
					Assert.IsFalse(CompaniesHelper.IsClosed(company.Id, 0, false));
				}
				else
				{
					Assert.IsTrue(CompaniesHelper.IsClosed(company.Id, 0, false));
				}
			}
		}
	}
}
