using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.Web.Modules;
using CompanyFeeds.Web.Controllers;
using CompanyFeeds.Configuration;
using System.Web;

namespace CompanyFeeds.Tests.Modules
{
	/// <summary>
	/// Summary description for RedirectorModuleTest
	/// </summary>
	[TestClass]
	public class RedirectorModuleTest
	{
		public RedirectorModuleTest()
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
		public void Redirector_Urls_Test()
		{
			EntriesController dummyController = new EntriesController();
			FakeControllerContext controllerContext = new FakeControllerContext(dummyController, "http://localhost/");

			RedirectorConfiguration config = new RedirectorConfiguration();
			
			#region UrlGroups
			RedirectorUrlGroup group1 = new RedirectorUrlGroup(@"^(https?)://[^/]+/testing.*$", @"/testing/$", @"/binarycontent/_rights.txt");


			config.UrlGroups = new RedirectorUrlGroup[] { group1};
			config.IgnoreRegex = @".*(\.css|\.txt|\.js|\.gif|\.jpg|\.png)";
			#endregion

			RedirectorModule module = new RedirectorModule();

			Dictionary<string, string> urlList = new Dictionary<string, string>();
			urlList.Add("http://localhost/testing/", ""); //Valores esperados??

			foreach (string url in urlList.Keys)
			{
				controllerContext.SetUri(url);
				module.RedirectRequest(controllerContext.HttpContext, config);
				Assert.IsTrue(controllerContext.HttpContext.Response.StatusCode == 301);
				controllerContext.HttpContext.Response.Clear();
			}

		}
	}
}
