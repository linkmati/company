using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using CompanyFeeds.Web.ActionFilters;
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

		[TestMethod]
		public void Akismet_NotSpam_Test()
		{
			var api = new AkismetHandler("6fdb57fbab0e", "http://localhost", "Test/1.0");
			bool isSpam = false;
			var post = GetComment();
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);

			post = GetComment();
			//Whitelisted url
			post.CommentAuthorUrl = "http://nearforums.com";
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);

			post = GetComment();
			post.CommentAuthorUrl = "http://oracle.com";
			post.CommentContent = "<p>YES! I love the sheer cut-to-the-chase instant beauty and usefulness of my <a href=\"https://itunes.apple.com/us/app/clear/id493136154?mt=8\">Clear</a> app on iOS. <a href=\"https://www.dropbox.com/\">Dropbox</a> really does simplify my ICT world, if not my life. I use those apps every day: on mobile, desktop or web. </p><p></p><center><br><img src=\"https://blogs.oracle.com/userassistance/resource/simple/clear.png\" title=\"Clear App\" alt=\"Clear App\" border=\"1\"><br></center><br><center><small>Clear app</small><br></center><p></p><p>In the enterprise apps world, you'll love what Oracle Applications User Experience team is doing with our <a href=\"https://blogs.oracle.com/VOX/entry/user_experience_roadmap_for_oracle\" title=\"User Experience Roadmap for Oracle Applications: Direct from Jeremy Ashley\">roadmap to simple and modern user experience</a> with Oracle Fusion Applications built with 100% Oracle Application Development Framework (ADF).</p><p>Beautiful. Simplicity, it's all part of the <abbr title=\"Bring Your Own Device\">BYOD</abbr> and <abbr title=\"Consumerization of IT\">COIT phenomenon that enterprises need to embrace rather than tolerate or ignore.</abbr></p><p>So, introduce yourself to the new face of Oracle Fusion Applications. More on the <a href=\"https://blogs.oracle.com/VOX/entry/introducing_the_new_face_of\" title=\"Introducing the New Face of Fusion Applications\">Voice of User Experience</a>  for Oracle Applications blog.</p>";
			post.CommentAuthor = "Whatever";
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);

			post = GetComment();
			post.UserIp = "183.87.250.51";
			post.CommentAuthorUrl = "http://www.deliver2inbox.com/index.php";
			post.CommentContent = "<p>&nbsp;Earlier, just in case you possessed to promote your products or services, you possessed to make the most of lots of traditional marketing methods enjoy printing literature, posters, and so on. Very good has changed completely. &nbsp;People was now scrambling for the new and advanced trend of <a href=\"http://deliver2inbox.com/smtp.php\">Smtp service</a>. This really is frequently that kind of belief that may be used via everybody and what is the benefit, just in case you request. The benefit might be the price effective character in the medium. You may even target more clients than that can be done with regular marketing. That is really a substantial improvement.bulk e-mail marketing might be a considerable type of marketing technique that's quite broadly used nowadays. There ended up being also numerous kinds of programs and application being made to appreciate this a much better platform for marketing person's business. Once the involves bulk e-mail marketing, there ended up making sure stuff that you need to looked into. It's because bulk e-mail marketing might be confused via junk emails. There's a massive distinction including. Because the former is exclusively based on ethical business tactics, second is really a genuine 'looked lower on' area. Creating a great bulk e-mail program getting a sizable email database of clients brings a larger yielding response measure. A great traffic specific within the website. Now enables enjoy phone numerous rules and rules that should be adopted for <a href=\"http://deliver2inbox.com/smtp.php\">Smtp plan</a> . A manufacturer, a supplier or simply a entrepreneur nowadays must learn how to market products the easiest and greatest possible way. Since the web has become an important place to acquire all this information, due to its magnificent cheap marketing options, finding the easiest method to send bulk email can generate a better place. Mass emails are important if marketing clients are essential reason. You'll be capable of choose bulk email service companies who gives you the appropriate tools to produce the professionalcess effective. Realize that the very best bulk email software is certainly not certain you'll get totally free just one which provides you all the needed options to make your objectives effective. Free bulk email services may limit your options and also on one account, you don't have the ability to achieve your main goal. Nowadays, you'll be capable of send bulk email while using the <a href=\"http://deliver2inbox.com/smtp.php\">Smtp dedicated services</a> functions inside your email. The benefits of while using the option is certainly send, forward or perhaps attach contents, which will later be shipped to multiple site visitors by usage of the majority email functions. It is also simple to create groups exactly the same emails may be shipped to. Bear in mind the BCC together with the CC functions in your normal email account, which allows you to definitely certainly certainly send multiple emails to many groups of individuals, differ greatly within the bulk direct emailer. Paid out out services will generally provip you using the sender getting plenty of choices, including list management, smtp in addition to monitoring send emails and came back emails correspondingly among other several options.</p>";
			post.CommentAuthor = "Whatever";
			post.CommentAuthorEmail = "samer12552@yahoo.com";
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);

			post = GetComment();
			post.UserIp = "115.119.157.50";
			post.CommentAuthorUrl = "http://www.infibeam.com";
			post.CommentContent = "<p>Ahmedabad, India; October 29: Neelima Kota through this book brings to you the teachings of Tirupati. It gives a detailed analysis of a guide to life and the philosophy that surrounds the God of Tirupati to the tests and tragedies of our life. There are some issues that bother us in our everyday life that have been discussed here. Issues like love, pain, jealousy, ambition, life, death, etc. all of these have been discussed in short chapters.<br><br>She has given a clear understanding of how we would approach each of them to get complete joy and happiness in our life. To achieve complete happiness and self-fulfillment in our lives what should we do? Why do people celebrate Trimula and what is the significance of it in our lives. She has taken us back to the history and has given the details of:<br><br>The daily puja and slokas to be done and in which way we should perform the puja.<br><br>The list of the festivals celebrated of Tirupati.<br><br>Which were the different vehicles of the Gods that were used in each festival?<br><br>The places where the lamps are lit what is the importance and why are lamps lit there.<br><br>Which slokas need to read before climbing the hill of Tirumala and why should they be read?<br><br>All the slokas that are read in the temple to wake the Lord are given in this book.<br><br>Each of the information that you will find in this book is genuine and has also been confirmed in the foreword by the head priest, saying that this book is a gem because this book will make you understand the real meaning of life through the insight of Tirupati.<br><a href=\"http:// http://www.infibeam.com/Books/tirupatis-guide-life-neelima-kota/9788184001983.html\"><br>http://www.infibeam.com/Books/tirupatis-guide-life-neelima-kota/9788184001983.html</a><br><br>###<br><strong><br>About the Author:</strong><br><br>Neelima has 17 years of experience of working as a journalist in top media brands like The Indian Express, Mint, The Sunday, etc. She has also written two other novels Riverstones and Death of Moneylender. She is currently working as the Political Editor for The Sunday Guardian in New Delhi, and a Research Fellow for South Asia Studies at the Paul H. Nitze School of Advanced International Studies. She has also held four solo exhibitions of her impression abstract paintings. Live in New Delhi and Washington DC. This is her third book.</p>";
			post.CommentAuthor = "Kislay Prajapati";
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);

			post = GetComment();
			post.UserIp = "122.164.72.147";
			post.CommentAuthorUrl = "http://www.opentohope.com";
			post.CommentContent = "<p>New York, NY ( prsync ) October 25, 2012 - Master Charles Cannon is a leader in the  field of modern spirituality, a visionary and pioneer in the evolution of human  consciousness. His latest book Forgiving the Unforgivable describes the 2008  Pakistani Muslim terrorist attacks on Mombai a 45-hour siege where four in his  group were wounded and two were killed.<br><br>To listen to this inspirational  show, go to <a href=\"http://www.opentohope.com/\">www.opentohope.com</a>.<br><br>About Dr. Heidi  Horsley<br><br>Dr. Heidi Horsley, PsyD, LMSW, MS, is a bereaved sibling and a  licensed psychologist and social worker. Dr. Heidi is the Co-Founder and  Executive Director of the Open to Hope Foundation and an adjunct professor at  Columbia University. She has a private practice in Manhattan, NY specializing in  grief and loss. She serves as a member of the board of directors for the  Compassionate Friends, <a href=\"http://www.compassionatefriends.org/\">www.compassionatefriends.org</a> and  on the advisory board for TAPS, Tragedy Assistance Program for Survivors of  those who died in the military. She blogs for the Huffington Post and Maria  Shriver. Dr. Heidi is also author of \"Open to Hope: Inspirational Stories of  Healing After Loss,\" \"Teen Grief Relief\" and \"Real Men Do Cry\". She holds  graduate degrees from the University of San Francisco, Columbia University, and  Loyola University.<br><br>About Dr. Gloria Horsley<br><br>Dr. Gloria C. Horsley,  PHD, MFT, CNS, is a bereaved parent, and has worked in the field of family  therapy for over thirty years. Dr. Gloria is Founder and President of the Open  to Hope Foundation. She serves on the national advisory board for The  Compassionate Friends. She blogs for the Huffington Post and Maria Shriver. Dr.  Heidi is co-author of \"Open to Hope: Inspirational Stories of Healing After  Loss,\" \"Teen Grief Relief\" and \"Real Men Do Cry.\" She holds graduate degrees  from the University of Rochester, Syracuse, Greenwich and Holos  Universities.<br><br>Dr. Heidi Horsley<br>125 West 72nd Street, Suite 6F<br>NY  NY 10023<br>(646) 269-1664<br><br>###</p>";
			post.CommentAuthor = "opentoho";
			post.CommentAuthorEmail = "opentoho@gmail.com";
			isSpam = api.CommentCheck(post);
			Assert.IsFalse(isSpam);
		}



	    [TestMethod]
	    public void Akismet_NotSpam_Test2()
	    {
	        var api = new AkismetHandler("6fdb57fbab0e", "http://localhost", "Test/1.0");
	        bool isSpam = false;
	        var post = GetComment();
	        isSpam = api.CommentCheck(post);
	        Assert.IsFalse(isSpam);

	        post = GetComment();
	        //Whitelisted url
	        post.CommentAuthorUrl = "http://nearforums.com";
	        isSpam = api.CommentCheck(post);
	        Assert.IsFalse(isSpam);

	        post = GetComment();
	        post.CommentAuthorUrl = "http://oracle.com";
	        post.CommentContent = "<p>YES! I love the sheer cut-to-the-chase instant beauty and usefulness of my <a href=\"https://itunes.apple.com/us/app/clear/id493136154?mt=8\">Clear</a> app on iOS. <a href=\"https://www.dropbox.com/\">Dropbox</a> really does simplify my ICT world, if not my life. I use those apps every day: on mobile, desktop or web. </p><p></p><center><br><img src=\"https://blogs.oracle.com/userassistance/resource/simple/clear.png\" title=\"Clear App\" alt=\"Clear App\" border=\"1\"><br></center><br><center><small>Clear app</small><br></center><p></p><p>In the enterprise apps world, you'll love what Oracle Applications User Experience team is doing with our <a href=\"https://blogs.oracle.com/VOX/entry/user_experience_roadmap_for_oracle\" title=\"User Experience Roadmap for Oracle Applications: Direct from Jeremy Ashley\">roadmap to simple and modern user experience</a> with Oracle Fusion Applications built with 100% Oracle Application Development Framework (ADF).</p><p>Beautiful. Simplicity, it's all part of the <abbr title=\"Bring Your Own Device\">BYOD</abbr> and <abbr title=\"Consumerization of IT\">COIT phenomenon that enterprises need to embrace rather than tolerate or ignore.</abbr></p><p>So, introduce yourself to the new face of Oracle Fusion Applications. More on the <a href=\"https://blogs.oracle.com/VOX/entry/introducing_the_new_face_of\" title=\"Introducing the New Face of Fusion Applications\">Voice of User Experience</a>  for Oracle Applications blog.</p>";
	        post.CommentAuthor = "Whatever";
	        isSpam = api.CommentCheck(post);
	        Assert.IsFalse(isSpam);

	        post = GetComment();
	        post.UserIp = "122.164.72.147";
	        post.CommentAuthorUrl = "http://www.opentohope.com";
	        post.CommentContent = "<p>New York, NY ( prsync ) October 25, 2012 - Master Charles Cannon is a leader in the  field of modern spirituality, a visionary and pioneer in the evolution of human  consciousness. His latest book Forgiving the Unforgivable describes the 2008  Pakistani Muslim terrorist attacks on Mombai a 45-hour siege where four in his  group were wounded and two were killed.<br><br>To listen to this inspirational  show, go to <a href=\"http://www.opentohope.com/\">www.opentohope.com</a>.<br><br>About Dr. Heidi  Horsley<br><br>Dr. Heidi Horsley, PsyD, LMSW, MS, is a bereaved sibling and a  licensed psychologist and social worker. Dr. Heidi is the Co-Founder and  Executive Director of the Open to Hope Foundation and an adjunct professor at  Columbia University. She has a private practice in Manhattan, NY specializing in  grief and loss. She serves as a member of the board of directors for the  Compassionate Friends, <a href=\"http://www.compassionatefriends.org/\">www.compassionatefriends.org</a> and  on the advisory board for TAPS, Tragedy Assistance Program for Survivors of  those who died in the military. She blogs for the Huffington Post and Maria  Shriver. Dr. Heidi is also author of \"Open to Hope: Inspirational Stories of  Healing After Loss,\" \"Teen Grief Relief\" and \"Real Men Do Cry\". She holds  graduate degrees from the University of San Francisco, Columbia University, and  Loyola University.<br><br>About Dr. Gloria Horsley<br><br>Dr. Gloria C. Horsley,  PHD, MFT, CNS, is a bereaved parent, and has worked in the field of family  therapy for over thirty years. Dr. Gloria is Founder and President of the Open  to Hope Foundation. She serves on the national advisory board for The  Compassionate Friends. She blogs for the Huffington Post and Maria Shriver. Dr.  Heidi is co-author of \"Open to Hope: Inspirational Stories of Healing After  Loss,\" \"Teen Grief Relief\" and \"Real Men Do Cry.\" She holds graduate degrees  from the University of Rochester, Syracuse, Greenwich and Holos  Universities.<br><br>Dr. Heidi Horsley<br>125 West 72nd Street, Suite 6F<br>NY  NY 10023<br>(646) 269-1664<br><br>###</p>";
	        post.CommentAuthor = "opentoho";
	        post.CommentAuthorEmail = "opentoho@gmail.com";
	        isSpam = api.CommentCheck(post);
	        Assert.IsFalse(isSpam);
	    }

		[TestMethod]
		public void Akismet_Spam_Test()
		{
			var api = new AkismetHandler("6fdb57fbab0e", "http://localhost", "Test/1.0");
			bool isSpam = false;
			var post = GetComment();

			post = GetComment();
			//Blacklisted url
			post.CommentAuthorUrl = "http://revenuexl.com";
			isSpam = api.CommentCheck(post);
			Assert.IsTrue(isSpam);

			post = GetComment();
			post.UserIp = "184.75.216.49";
			post.CommentAuthorUrl = "http://www.royalcourt.com.au";
			post.CommentContent = "<p>&nbsp;</p><p><b>Sydney, New South Wales - </b>All the women at Royal Court can give a taste of Australia's amazing night life to all the high-class travelers with their sizzling acts. These girls are carefully chosen according to their looks and sensuous bodies to suit the preferences of travelers who love to be in company of the most gorgeous women. Going around in Australia alone can be very boring and all the tourists would long for someone who could accompany them to the most exciting locations. The sensuous <u><a href=\"http://www.royalcourt.com.au/sydney_escorts.html\"><b>Sydney escorts</b></a></u> not only provide great company, but also give these travelers some memorable moments that would make them come back for more.</p><p>Women at Royal Court are picked from the most exotic locations across the world and have just the right stuff that their clients would want in them. All of them are placed into different clubs such as Gold, Platinum, Diamond, Connoisseur and Royal according to the rates that they charge per hour. The clients can be assured of complete satisfaction when they hire these escorts. These lovely ladies would make their clients' days pleasant and nights exciting with their seductive ways. People who hire the services of these <u><a href=\"http://www.royalcourt.com.au/australian_escort.html\"><b>Australian escorts</b></a></u> would be hooked on to their beautifully carved bodies and pleasant conversations.</p><p>Royal Court offers different types of packages for all the travelers who come to Australia for an exciting time. Limousines and private air travel can be arranged for the transportation of clients.  There many other amazing stuff to offer such as hot weekend getaways, vacation on a tropical island and romantic cruises. Whatever the service offered, <u><a href=\"http://www.royalcourt.com.au/\" ><b>Royal Sydney escorts</b></a></u> would always accompany their clients wherever they go. These sensuous companions can keep anyone entertained with all their charm. This would make the travelers forget all the worries of their life and enjoy the company of these gorgeous babes.</p><p>There is plenty of useful information about Australian travel and beautiful courtesans offered on the website of Royal Court. There are many independent <b>Sydney escorts</b> whose links are provided on this website. All the high-class travelers can find these women very interesting and would do anything to be in their company. Individuals who are interested in these delicious females can follow these links and know everything about them. Clients can also browse the galleries to explore their curvy bodies and know all the other details about them. For more information about the website and all the services offered, visit <u><a href=\"http://www.royalcourt.com.au/\">http://www.royalcourt.com.au/</a></u></p>";
			post.CommentAuthorEmail = "sroyalcourt6@yahoo.com";
			post.CommentAuthor = "Royal Court";
			isSpam = api.CommentCheck(post);
			Assert.IsTrue(isSpam); //Must be true
		}

		public void Akismet_Spam_Failed_Test()
		{
			var api = new AkismetHandler("6fdb57fbab0e", "http://localhost", "Test/1.0");
			bool isSpam = false;
			var post = GetComment();

			post = GetComment();
			post.UserIp = "103.9.151.144";
			post.CommentAuthorUrl = "http://sexysydneyescorts.com/";
			post.CommentContent = " Pyrmont, New South Wales - Among all the attractions in Australia, what excites most of the travelers and residents is the opportunity to spend time with Sexy Sydney Escorts and bask in their seductive acts. The agency has access to some of the most beautiful women on earth offering to show customers a wonderful time in Sydney. ";
			post.CommentAuthorEmail = "sexysydney4@yahoo.com";
			post.CommentAuthor = "Sexy Sydney Escorts";
			isSpam = api.CommentCheck(post);
			Assert.IsTrue(isSpam); //Must be true
		}

		public AkismetHandlerComment GetComment()
		{
            var comment = new AkismetHandlerComment()
			{
				UserIp = "213.246.236.94",
				UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:13.0) Gecko/20100101 Firefox/13.0.1",
				CommentContent = "Clean",
				CommentAuthorEmail = "charly.team@gmail.com",
				CommentAuthorUrl = "http://blogger.com",
                Blog = "prsync.com"
			};
			return comment;
		}
	}
}
