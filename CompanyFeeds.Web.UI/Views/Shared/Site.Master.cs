using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.UI.Views.Shared
{
	public partial class Site : BaseViewMasterPage
	{
		protected override void OnLoad(EventArgs e)
		{
			repCategories.DataSource = Cache.Categories;
			repCategories.DataBind();
			if (User != null)
			{
				pnlHeaderNotMember.Visible = false;
				pnlHeaderMember.Visible = true;

				if (User.Role == UserRole.Admin)
				{
					//Show extra options for admin
					pnlHeaderAdmin.Visible = true;
					//Hide viglink
					pnlVigLink.Visible = false;
				}

				if (User.AgencyTag != null)
				{
					linkAgency.Visible = true;
				}
			}
			base.OnLoad(e);
		}
	}
}
