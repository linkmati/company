using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Views.Home
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

		protected override void OnLoad(EventArgs e)
		{
			if (PageIndex == 0)
			{
				if (User != null)
				{
					pnlMembers.Visible = true;
					if (ViewData["Subscriptions"] != null)
					{
						pnlSubscriptions.Visible = true;
						pnlNoSubscriptions.Visible = false;
					}
				}
				else
				{
					pnlNotMembers.Visible = true;
				}
			}
			base.OnLoad(e);
		}
	}
}
