using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.Controllers;
using System.Web.Mvc;
using CompanyFeeds.Tests.Fakes;

namespace CompanyFeeds.Tests.Controllers
{
	[TestClass]
	public class FeedbackControllerTest
	{
		[TestMethod]
		public void Contact_Test()
		{
			var controller = new FeedbackController();
			controller.ControllerContext = new FakeControllerContext(controller, "http://localhost/contact", "NAME", new string[]{}, new System.Collections.Specialized.NameValueCollection(), new System.Collections.Specialized.NameValueCollection(), new System.Web.HttpCookieCollection(), new System.Web.SessionState.SessionStateItemCollection());
			controller.Init(new System.Web.Routing.RequestContext(controller.HttpContext, new System.Web.Routing.RouteData()));
			var result = controller.Contact("Unit test guy", "dummyunittest@email.com", "UNIT TEST MESSAGE");

			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}
	}
}
