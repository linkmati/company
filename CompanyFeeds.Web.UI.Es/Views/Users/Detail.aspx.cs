using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;


namespace CompanyFeeds.Web.UI.Es.Views.Users
{
	public partial class Detail : BaseViewPage<UsersQueries.UsersDetailRow>
	{
		protected override void OnLoad(EventArgs e)
		{
			if (ViewData["Companies"] != null)
			{
				pnlSubscriptions.Visible = true;
				repCompanies.DataBind();
			}
			if (ViewData["EntriesSubmitted"] != null)
			{
				repSubmits.DataBind();
			}
			if (this.User != null && User.Role == CompanyFeeds.Web.State.UserRole.Admin)
			{
				pnlAdmin.Visible = true;
			}
			base.OnLoad(e);
		}
	}
}
