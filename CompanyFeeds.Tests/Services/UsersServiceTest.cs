using CompanyFeeds.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace CompanyFeeds.Tests.Services
{
    
    
    /// <summary>
    ///This is a test class for UsersServicePromositeUserTest and is intended
    ///to contain all UsersServicePromositeUserTest Unit Tests
    ///</summary>
	[TestClass()]
	public class UsersServiceTest
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
		///A test for RegisterUser
		///</summary>
		[TestMethod()]
		public void RegisterUserTest()
		{
			User user = new User();
			user.Birthday = new DateTime(1960, 1, 1);
			user.CountryCode = "ES";
			user.Email = "qwe" + Convert.ToInt32(new Random().Next(1000000)) + "@ocu.org";
			user.Gender = Gender.Female;
			user.Name = "TestProyect";
			user.Password = "12345678";

			UsersService.AddUser(user);

			Assert.IsTrue(user.Id > 0);
		}

		/// <summary>
		///A test for IsEmailUnique
		///</summary>
		[TestMethod()]
		public void IsEmailUniqueTest()
		{
			User user = new User();
			user.Id = -2000;
			user.Email = "imposibletohaveamail@email.com";

			Assert.IsTrue(UsersService.IsEmailUnique(user));
		}
	}
}
