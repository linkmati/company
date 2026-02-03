using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Es.Views.Home
{
	public partial class Index : BaseViewPage<PagedList<EntriesQueries.EntriesListRow>>
	{
		protected int PageIndex
		{
			get
			{
				return Convert.ToInt32(ViewData["Page"]);
			}
		}
	}
}
