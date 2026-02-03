using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Tests.Utils
{
	/// <summary>
	/// Summary description for UtilsTests
	/// </summary>
	[TestClass]
	public class UtilsTests
	{
		public UtilsTests()
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


		[TestMethod()]
		public void Summarize_Test()
		{
			string value01 = "Google today launched On-Demand Indexing, a new feature for Google Site Search that allows businesses to quickly incorporate new pages and important site updates into search results on their websites. On-Demand Indexing ensures that site visitors have access to a site's freshest content, and that businesses have the flexibility to share news, product releases and promotions as they happen. With something to displa";
			string value02 = "Google today launched On-Demand Indexing, a new feature for Google Site Search that allows businesses to quickly incorporate new pages and important site updates into search results on their websites. On-Demand Indexing ensures that site visitors have access to a site's freshest content, and that businesses have the flexibility to share news, product releases and promotions as they happen. With something to...";

			Assert.AreEqual(TextUtils.Summarize(value01, value01.Length - 1, "..."), value02);
		}

		[TestMethod()]
		public void CleanHtml_Test()
		{
			string value01 = " <!--[if !mso]>\nst1\\:*{behavior:url(#ieooui) }\n<![endif]--><div style=\"font-family: Arial;\">Ar<object></object>go</div>";
			string value02 = "<div>Argo</div>";

			Assert.AreEqual(TextUtils.CleanHtml(value01).Trim(), value02);
		}

		[TestMethod()]
		public void RemoveTags_Test()
		{
			string value01 = "<SCRIPT>\r\nfunction dummy{}</SCRIPT> <!--[if !mso]>\nst1\\:*{behavior:url(#ieooui) }\n<![endif]--><div style=\"font-family: Arial;\">Ar<object></object>go</div>";
			string value02 = "Argo";

			Assert.AreEqual(TextUtils.RemoveTags(value01).Trim(), value02);
		}

		[TestMethod]
		public void TestGetProperty()
		{
			User user = new User();
			user.Name = "Jorge";

			Assert.IsTrue(ReflectionUtils.GetPropertyValue<string>(user, "Name") == "Jorge");
		}

		[TestMethod()]
		public void IsHtmlFragmentTest()
		{
			string value01 = "<div style=\"font-family: Arial;\">Ar<object></object>go</div>";
			string value02 = "<p>Argo</p>";
			string value03 = "Argo yada< yada<a href=\"\">Hello</a> hi";

			Assert.IsTrue(TextUtils.IsHtmlFragment(value01));
			Assert.IsTrue(TextUtils.IsHtmlFragment(value02));
			Assert.IsFalse(TextUtils.IsHtmlFragment(value03));
		}


		[TestMethod()]
		public void TextToHtmlFragment_Test()
		{
			string value = "First line\r\nThis is a Url: http://www.dosporcuatro.com";
			//string expected
			value = TextUtils.TextToHtmlFragment(value);
		}

		[TestMethod()]
		public void MetaDescriptionHelper_Test()
		{
			//string expected = "<meta content=\"hello&amp;quot;\r\n \" name=\"description\" />";
			string result = HtmlExtensions.MetaDescription(null, "hello\"\r\n ");
		}

		[TestMethod()]
		public void CleanHtmlComments_Test()
		{
			string value = "<p>Hola</p><!--[if gte mso 9]><xml>\n<w:LatentStyles DefLockedState=\"false\" LatentStyleCount=\"156\">\n</w:LatentStyles>\n</xml><![endif]--><!--[if !mso]>\\nst1\\:*{behavior:url(#ieooui) }\n<![endif]-->";
			Assert.AreEqual(TextUtils.CleanHtmlComments(value).Trim(), "<p>Hola</p>");
		}


		[TestMethod()]
		public void SortedList_Performance()
		{
			DateTime date1 = DateTime.Now;
			SortedList<string, string> list = new SortedList<string, string>();
			for (int i = 1; i < 100000; i++)
			{
				list.Add(String.Format("{0:000000}", i), String.Concat("value-", i.ToString()));
			}
			DateTime date2 = DateTime.Now;
			var filtered = list.Where(x => x.Key.StartsWith("0000")).ToList();
			
			DateTime date3 = DateTime.Now;
			var filteredKeys = list.Keys.Where(x => x.StartsWith("0000")).ToList();

			DateTime date4 = DateTime.Now;

			var time1 = date2.Subtract(date1);
			var time2 = date3.Subtract(date2);
			var time3 = date4.Subtract(date3);


		}

		[TestMethod]
		public void ExtractLinks_Test()
		{
			var html = "";
			html = "<a	 href='http://link1.com'>Link text</a>";
			Assert.IsTrue(TextUtils.ExtractLinks(html).Count == 1);
			Assert.IsTrue(TextUtils.ExtractLinks(html)[0] == "http://link1.com");
			html = "<a class='something' \r\n	 href='http://link1.com'> asasas</a>";
			Assert.IsTrue(TextUtils.ExtractLinks(html).Count == 1);
			html = "<a class='something' \r\n	 href='http://link1.com'> asasas</a><p><a  href='ftp://whatever'> asasas</malformed>";
			Assert.IsTrue(TextUtils.ExtractLinks(html).Count == 2);
		}
	}
}
