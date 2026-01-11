using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.Controllers;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.Web.ActionResults;
using System.Collections;

namespace CompanyFeeds.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTests
	{
		[TestMethod]
		public void Home_Index_Test()
		{
			var controller = new HomeController();
			controller.ControllerContext = new FakeControllerContext(controller);
			controller.SiteConfiguration = new CompanyFeeds.Configuration.SiteConfiguration() { LogoPath = "/", HomePageSize = 10};
			controller.Index(0, ResultType.Xhtml);

			var list = controller.ViewData.Model as ICollection;
			Assert.IsTrue(list.Count > 0);
		}
	}
}
