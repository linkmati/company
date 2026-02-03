using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Services;
using CompanyFeeds.Web.Controllers;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Tests.Fakes;

namespace CompanyFeeds.Tests.Controllers
{
	[TestClass]
	public class EntriesControllerTest
	{
		[TestMethod]
		public void Entries_Integration_Detail_Test()
		{
			int entriesCount = 0;
			var controllerContext = new FakeControllerContext(new BaseController());
			for (var categoryId = 1; categoryId < 4; categoryId++)
			{
				var entries = EntriesService.GetByCategory(categoryId);
				foreach (var e in entries)
				{
					var entriesController = new EntriesController();
					entriesController.ControllerContext = controllerContext;
					var result = entriesController.Detail(e.EntryId, e.EntryTag);
					var entry = entriesController.ViewData.Model as EntriesQueries.EntriesDetailRow;
					Assert.IsNotNull(entry);
					Assert.IsTrue(entry.CategoryId == categoryId);
					//try to check dbnulls
					var dummyInt = entry.EntryOwner;
					var dummyBool = entry.HideAuthor;
					entriesCount++;
				}
			}
		}
	}
}
