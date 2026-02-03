using CompanyFeeds.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds;
using System.Collections.Generic;

using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.Web.Controllers;
using CompanyFeeds.Web.ActionFilters;
using System.Web.Caching;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Tests.Controllers
{
	[TestClass()]
	public class ResultCacheTest
	{


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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		[TestMethod()]
		public void ResultCacheTestMethod()
		{

			EntriesController controller = new EntriesController();
			FakeControllerContext controllerContext = new FakeControllerContext(controller, "http://localhost/hello.aspx");

			ViewResult result = new ViewResult();
			result.ViewName = "/views/sample.aspx";

			ResultCacheAttribute filter = new ResultCacheAttribute();
			filter.Duration = 60;
			filter.OnActionExecuting(new System.Web.Mvc.ActionExecutingContext(controllerContext, new FakeActionDescriptor(), new Dictionary<string, object>()));

			ActionExecutedContext actionExecutedContext = new ActionExecutedContext(controllerContext, new FakeActionDescriptor(), false, null);

			actionExecutedContext.Result = result; 
			filter.OnActionExecuted(actionExecutedContext);

			//test insert from cache
			Assert.AreEqual(controllerContext.HttpContext.Cache[filter.CacheKey], result);


			//Test the GET from cache
			ResultCacheAttribute filter2 = new ResultCacheAttribute();
			ActionExecutingContext actionExecutingContext = new System.Web.Mvc.ActionExecutingContext(controllerContext, new FakeActionDescriptor(), new Dictionary<string, object>());
			filter2.OnActionExecuting(actionExecutingContext);

			Assert.AreEqual(actionExecutingContext.Result, result);

		}
	}
}
