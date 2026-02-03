using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.Controllers;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.Services;

namespace CompanyFeeds.Tests.Controllers
{
	/// <summary>
	/// Summary description for CommentsControllerTest
	/// </summary>
	[TestClass]
	public class CommentsControllerTest
	{
		public CommentsControllerTest()
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
		public void CommentAdd_Test()
		{
			CommentsController controller = new CommentsController();
			controller.ControllerContext = new FakeControllerContext(controller, "http://localhost/comments/add/", "Dummy", null, new NameValueCollection(), new NameValueCollection(), new HttpCookieCollection(), new SessionStateItemCollection());
			controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);
			controller.Init(controller.ControllerContext.RequestContext);

			controller.Add(1025, "Message from test project", true, "TESTPROJECT", "test@test.com", true);
			controller.Add(1025, "Message from test project", true, "TESTPROJECT", "jbg@conseur.org", true);
		}
	}
}
