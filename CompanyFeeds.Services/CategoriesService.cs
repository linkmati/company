using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.CategoriesQueriesTableAdapters;

namespace CompanyFeeds.Services
{
	public class CategoriesService
	{
		public static CategoriesQueries.CategoriesRow GetCategory(string tag)
		{
			CategoriesTableAdapter ta = new CategoriesTableAdapter();
			CategoriesQueries.CategoriesDataTable dt = ta.GetByTag(tag);
			if (dt.Count > 0)
			{
				return dt[0];
			}
			else
			{
				return null;
			}
		}

		public static CategoriesQueries.CategoriesDataTable GetCategories()
		{
			CategoriesTableAdapter ta = new CategoriesTableAdapter();
			return ta.GetAll();
		}
	}
}
