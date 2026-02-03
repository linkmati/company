using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Services;
using System.IO;
using System.Net.Mail;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Tests.Services
{
	/// <summary>
	/// Summary description for NotificationsServiceTest
	/// </summary>
	[TestClass]
	public class NotificationsServiceTest
	{
		public NotificationsServiceTest()
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
		public void ReplaceValuesTest()
		{

			User user = new User();
			user.Name = "Jorge";

			Assert.IsTrue(NotificationsService.ReplaceValues("Hi <!--!Name!-->", user, new string[]{"Name"}) == "Hi Jorge");
		}

		[TestMethod]
		public void ValidationEmail_Test()
		{
			var user = new User() 
			{ 
				Name = "MyUsername"
				, Email = "jbay@ocu.org"
			};
			var config = SiteConfiguration.Load();
			var validationUrl = "http://localhost/validate-url-sample";
			NotificationsService.SendValidationMail(user, Path.Combine(config.GetApplicationRootConfigurationPath(), config.Mail.TemplatesPath["Welcome"]), config.Mail.AdminMailAddress, config.Mail.AdminMailName, config.Mail.Subjects["Welcome"], validationUrl, config.Mail.SmtpServer, config.Mail.GetCredentials());
		}

	    [TestMethod]
	    public void TestingGmail()
	    {
	        var client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = client.Port != 25;
	        client.Send("admin@prsync.es", "jorge.gondra@datastax.com", "Testing 2", "Mail body");
	    }
	}
}
