using CompanyFeeds.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.State;
using System.Web;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.TestQueriesTableAdapters;

namespace CompanyFeeds.Tests
{
    
    
    /// <summary>
    ///This is a test class for SecurityHelperPromositeUserTest and is intended
    ///to contain all SecurityHelperPromositeUserTest Unit Tests
    ///</summary>
	[TestClass()]
	public class SecurityHelperPromositeUserTest
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
		///A test for Login
		///</summary>
		[TestMethod()]
		public void LoginTest()
		{
			SessionWrapper session = new SessionWrapper(new FakeHttpSessionState(new System.Web.SessionState.SessionStateItemCollection()));
			
			HttpCookieCollection cookies = new HttpCookieCollection();

			#region Get a dummy user to try login
			UsersTableAdapter ta = new UsersTableAdapter();
			TestQueries.UsersDataTable dt = ta.GetData();
			if (dt.Count == 0)
			{
				Assert.Inconclusive("no data to test.");
				return;
			}
			#endregion

			string email = dt[0].UserEmail; 
			string password = dt[0].UserPassword; 

			UserState user = SecurityHelper.Login(session, cookies, email, password);
			
			//assert login
			Assert.IsNotNull(user);

			//assert writecookies
			Assert.IsTrue(cookies.Count > 0);

			//assert false login
			Assert.IsNull(SecurityHelper.Login(session, new HttpCookieCollection(), "..888399", ".,.,.,-,.-,.,-"));

			//assert ReadMember cookie
			//Assert.IsNotNull(SecurityHelper.ReadMemberCookie(cookies, new HttpCookieCollection(), session));
		}
	}
}
