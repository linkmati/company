using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Es.Views.Companies
{
    public partial class Detail : BaseViewPage<CompaniesQueries.CompaniesDetailRow>
    {
		protected override void OnLoad(EventArgs e)
		{
			if (Convert.ToInt32(ViewData["Page"]) > 0)
			{
				pnlInfo.Visible = false;
			}
			if (!Model.IsCompanyLogoNull())
			{
				imgContainer.Visible = true;
			}

			if (User != null)
			{
				linkEdit.Visible = true;
				if (UserIsAdmin)
				{
					linkNoRevence.Visible = true;
					linkDelete.Visible = true;
				}
			}

			base.OnLoad(e);
		}
    }
}
