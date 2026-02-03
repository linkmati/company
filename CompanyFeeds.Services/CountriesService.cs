using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.CountriesQueriesTableAdapters;

namespace CompanyFeeds.Services
{
	public class CountriesService
	{
		public static CountriesQueries.CountriesDataTable GetCountries()
		{
			CountriesTableAdapter ta = new CountriesTableAdapter();
			return ta.Get();
		}
	}
}
