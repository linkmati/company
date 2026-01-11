using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.State;
using CompanyFeeds.Web.Controllers;
using System.Web.Mvc;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Tests.Fakes;

namespace CompanyFeeds.Tests.Filters
{
	/// <summary>
	/// Summary description for CheckBlacklistedUrlsTest
	/// </summary>
	[TestClass]
	public class CheckBlacklistedUrlsTest
	{
		public CheckBlacklistedUrlsTest()
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
		public void ContainsBlackListedUrls_Test()
		{
			Assert.IsFalse(SpamPreventionHelper.ContainsBlackListedUrls("http://whitelisted.com"));
			Assert.IsTrue(SpamPreventionHelper.ContainsBlackListedUrls("http://blacklisted.com"));
			Assert.IsFalse(SpamPreventionHelper.ContainsBlackListedUrls(
				"<a class='something' \r\n	 href='http://link1.com'> asasas</a><p><a  href='ftp://whatever'> asasas</malformed>"));
			Assert.IsTrue(SpamPreventionHelper.ContainsBlackListedUrls(
				"<a class='something' \r\n	 href='http://link1.com'> asasas</a><p><a  href='http://www.blacklisted.com'> asasas</malformed>"));
			Assert.IsFalse(SpamPreventionHelper.ContainsBlackListedUrls(
				"<a class='something' \r\n	 href='#relative'> asasas</a><p><a  href='/'> asasas</malformed>", "", "http://www.google.es"));
		}

		[TestMethod]
		public void CheckBlackListedUrlsAttribute_Entry_Test()
		{
			var controller = new EntriesController();
			controller.ControllerContext = TestHelper.GetFullControllerContext(controller);

			var whitelistedUrl = "http://whitelisted.com";
			var blacklistedUrl = "http://blacklisted.com";

			var actionParams = new Dictionary<string, object>();

			//Blacklisted
			actionParams["entry"] = new Entry
			{
				Content = "<a href='" + blacklistedUrl + "'>Hello test</a>"
			};
			var filterContext = new ActionExecutingContext(controller.ControllerContext, new FakeActionDescriptor(), actionParams);
			var filter = new CheckBlacklistedUrlsAttribute("entry");
			filter.OnActionExecuting(filterContext);
			Assert.IsNotNull(filterContext.Result);

			//Whitelisted
			actionParams["entry"] = new Entry
			{
				Content = "<a href='" + whitelistedUrl + "'>Hello test</a>"
			};
			filterContext.Result = null;
			filter.OnActionExecuting(filterContext);
			Assert.IsNull(filterContext.Result);

			//Whitelisted
			actionParams["entry"] = new Entry
			{
				Content = null
			};
			filterContext.Result = null;
			filter.OnActionExecuting(filterContext);
			Assert.IsNull(filterContext.Result);
		}

		[TestMethod]
		public void CheckBlackListedUrlsAttribute_Company_Test()
		{
			var controller = new EntriesController();
			controller.ControllerContext = TestHelper.GetFullControllerContext(controller);

			var whitelistedUrl = "http://whitelisted.com";
			var blacklistedUrl = "http://blacklisted.com";

			var actionParams = new Dictionary<string, object>();

			//Blacklisted
			actionParams["company"] = new Company
			{
				Description = "<a href='" + blacklistedUrl + "'>Hello test</a>"
			};
			var filterContext = new ActionExecutingContext(controller.ControllerContext, new FakeActionDescriptor(), actionParams);
			var filter = new CheckBlacklistedUrlsAttribute("company");
			filter.OnActionExecuting(filterContext);
			Assert.IsNotNull(filterContext.Result);

			//Whitelisted
			actionParams["company"] = new Company
			{
				Description = "<a href='" + whitelistedUrl + "'>Hello test</a>"
			};
			filterContext.Result = null;
			filter.OnActionExecuting(filterContext);
			Assert.IsNull(filterContext.Result);

			//Whitelisted
			actionParams["company"] = new Company();
			filterContext.Result = null;
			filter.OnActionExecuting(filterContext);
			Assert.IsNull(filterContext.Result);
		}
	}
}
