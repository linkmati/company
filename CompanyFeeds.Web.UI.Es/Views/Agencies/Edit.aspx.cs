using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;


namespace CompanyFeeds.Web.UI.Es.Views.Agencies
{
	public partial class Edit : BaseViewPage<Agency>
	{
		protected override void OnLoad(EventArgs e)
		{
			EntriesQueries.EntriesDetailDataTable dt = new EntriesQueries.EntriesDetailDataTable();
			ViewData["HideSearchBox"] = true;
			if (Model != null)
			{
				pnlNoMember.Visible = false;
				
			}
			base.OnLoad(e);
		}
	}
}
