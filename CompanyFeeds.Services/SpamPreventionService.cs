using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.FlagsQueriesTableAdapters;

namespace CompanyFeeds.Services
{
	public static class SpamPreventionService
	{
		/// <summary>
		/// Gets all flagged entries
		/// </summary>
		/// <returns></returns>
		public static FlagsQueries.EntriesFlagsDataTable GetFlags()
		{
			EntriesFlagsTableAdapter ta = new EntriesFlagsTableAdapter();
			return ta.GetAll();
		}

		/// <summary>
		/// Gets the full list of black listed hosts
		/// </summary>
		/// <returns></returns>
		public static List<string> GetHostBlackList()
		{
			HostBlackListDataAccess da = new HostBlackListDataAccess();
			return da.GetAll();
		}

		/// <summary>
		/// Gets hosts from the urls and add the hosts to black list
		/// </summary>
		public static List<string> AddHostsToBlackList(List<string> urls)
		{
			var hosts = UrlUtils.GetHosts(urls);

			HostBlackListDataAccess da = new HostBlackListDataAccess();
			da.Add(hosts);

			return hosts;
		}
	}
}
