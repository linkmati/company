using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Views.Categories
{
    public partial class Detail : BaseViewPage<PagedList<EntriesQueries.EntriesListRow>>
    {
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
    }
}
