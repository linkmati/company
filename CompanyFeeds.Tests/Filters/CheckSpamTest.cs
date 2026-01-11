using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.Web.Controllers;
using System.Web.Mvc;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.State;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Tests.Controllers
{
	[TestClass]
	public class CheckSpamTest
	{
		[TestMethod]
		public void Check_Entry_Spam()
		{
			var controller = new EntriesController();
			controller.ControllerContext = TestHelper.GetFullControllerContext(controller);
			var cache = new CacheWrapper(controller.HttpContext);
			var session = new SessionWrapper(controller.HttpContext.Session);

			cache.SiteConfiguration = SiteConfiguration.Load();
			session.User = new UserState() 
			{ 
				Email = "viagra-spammer@viagra.com",
				Name = "viagra-test-123"
			};

			var actionParams = new Dictionary<string, object>();
			var entry = new Entry()
			{
				Content = "viagra-test-123"
			};
			actionParams["entry"] = entry;
			var filterContext = new ActionExecutingContext(controller.ControllerContext, new FakeActionDescriptor(), actionParams);
			var filter = new CheckSpamAttribute("entry");
			filter.IsTest = true;
			filter.OnActionExecuting(filterContext);
			Assert.IsTrue(filterContext.Result != null);

			session.User.Name = "Legal man";
			session.User.Email = "legal@googlemail.com";
			entry.Content = "Super good!";
			filterContext.Result = null;
			filter.OnActionExecuting(filterContext);
			Assert.IsTrue(filterContext.Result == null);
		}

		[TestMethod]
		public void Check_Company_Spam()
		{
			var controller = new EntriesController();
			controller.ControllerContext = TestHelper.GetFullControllerContext(controller);
			var cache = new CacheWrapper(controller.HttpContext);
			var session = new SessionWrapper(controller.HttpContext.Session);

			cache.SiteConfiguration = SiteConfiguration.Load();
			session.User = new UserState()
			{
				Email = "viagra-spammer@viagra.com",
				Name = "viagra-test-123"
			};

			var actionParams = new Dictionary<string, object>();
			var company = new Company()
			{
				Description = "Company to get viagra",
				Url = "http://www.mediaindonesia.com"
			};
			actionParams["company"] = company;
			var filterContext = new ActionExecutingContext(controller.ControllerContext, new FakeActionDescriptor(), actionParams);
			var filter = new CheckSpamAttribute("company");
			filter.IsTest = true;
			filter.OnActionExecuting(filterContext);
			Assert.IsTrue(filterContext.Result != null);
		}
	}
}
