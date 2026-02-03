using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyFeeds.Web.State;
using System.Web;
using CompanyFeeds.Tests.Fakes;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.TestQueriesTableAdapters;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web.SessionState;

namespace CompanyFeeds.Tests
{
	public static class TestHelper
	{
		/// <summary>
		/// Gets a non admin user
		/// </summary>
		/// <returns></returns>
		public static User GetAnyUser()
		{
			User user = null;
			var ta = new UsersTableAdapter();
			var userList = ta.GetData();
			
			foreach (var u in userList)
			{
				if (u.UserUpdates >= 0)
				{
					//not admin
					user = new User(u.UserId, u.UserName, u.UserEmail);
					break;
				}
			}

			if (user == null)
			{
				Assert.Inconclusive("No user to perform test");
			}
			return user;
		}

		public static ControllerContext GetFullControllerContext(IController controller)
		{
			var context = new FakeControllerContext(controller, "http://localhost/", "Dummy", new string[]{}, new NameValueCollection(), new NameValueCollection(), new HttpCookieCollection(), new SessionStateItemCollection());
			return context;
		}
	}
}
