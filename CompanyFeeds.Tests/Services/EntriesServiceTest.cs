using CompanyFeeds.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds;

namespace CompanyFeeds.Tests.Services
{
    
    
    /// <summary>
    ///This is a test class for EntriesServicePromositeUserTest and is intended
    ///to contain all EntriesServicePromositeUserTest Unit Tests
    ///</summary>
	[TestClass()]
	public class EntriesServiceTest
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


		/// <summary>
		///A test for AddEntry
		///</summary>
		[TestMethod()]
		public void AddEntryTest()
		{
			Entry entry = new Entry();
			entry.CompanyId = CompaniesService.GetCompanies()[0].CompanyId;
			entry.CompanyName = "Dummy company";
			entry.Content = "<p>Sample content</p>";
			entry.EntryTitle = "Submitted by test project";
			entry.Tag = "submitted-by-test-project";
			entry.Teaser = "Sample Teaser";

			EntriesService.AddEntry(entry, "127.0.0.1");
			Assert.IsTrue(entry.Id > 0);
		}

		/// <summary>
		///A test for GetByCategory
		///</summary>
		[TestMethod()]
		public void GetByCategoryTest()
		{
			int categoryId = 1;
			EntriesQueries.EntriesListDataTable entries;
			entries = EntriesService.GetByCategory(categoryId);
			Assert.IsNotNull(entries);
			Assert.IsTrue(entries.Count > 0);
		}

		[TestMethod()]
		public void Get_Test()
		{
			int categoryId = 1;
			EntriesQueries.EntriesListDataTable entries;
			entries = EntriesService.GetByCategory(categoryId);
			if (entries.Count == 0)
			{
				Assert.Inconclusive("Empty db");
			}
			else
			{
				Entry entry = EntriesService.Get(entries[0].EntryId);
				Assert.IsNotNull(entry);
			}
		}

		[TestMethod()]
		public void Update_Test()
		{
			int categoryId = 1;
			EntriesQueries.EntriesListDataTable entries;
			entries = EntriesService.GetByCategory(categoryId);
			if (entries.Count == 0)
			{
				Assert.Inconclusive("Empty db");
			}
			else
			{
				Entry entry = EntriesService.Get(entries[0].EntryId);
				if (entry == null)
				{
					Assert.Inconclusive("Empty db?");
				}
				else
				{
					EntriesService.Update(entry, "127.0.0.1");
				}
			}
		}
	}
}
