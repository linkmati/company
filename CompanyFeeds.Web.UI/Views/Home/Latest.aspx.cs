using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;


namespace CompanyFeeds.Web.UI.Views.Home
{
	public partial class Latest : BaseViewPage<PagedList<EntriesQueries.EntriesListRow>>
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
