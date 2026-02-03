using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.SubscriptionsQueriesTableAdapters;

namespace CompanyFeeds.Services
{
	public static class SubscriptionsService
	{
		public static void Add(int userId, int companyId)
		{
			SubscriptionsDataAccess da = new SubscriptionsDataAccess();
			da.Add(userId, companyId);
		}

		public static void Remove(int userId, int companyId)
		{
			SubscriptionsDataAccess da = new SubscriptionsDataAccess();
			da.Remove(userId, companyId);
		}

		public static SubscriptionsQueries.SubscriptionsDataTable Get(int userId)
		{
			SubscriptionsTableAdapter ta = new SubscriptionsTableAdapter();
			return ta.Get(userId);
		}
	}
}
