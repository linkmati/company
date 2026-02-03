using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds;
using System.Web.UI;
using System.Web.Extensions;

namespace CompanyFeeds.Web.UI.Views.Users
{
	public partial class Edit : BaseViewPage<User>
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;

			if (ViewData["ShowAdditional"] != null)
			{
				pnlShowAdditionalOnLoad.Visible = true;
			}

			if (Model != null && Model.Id > 0)
			{
				pnlRegister.Visible = false;
				pnlPasswordChange.Visible = true;
				if (ViewData["Update"] != null)
				{
					pnlUpdate.Visible = true;
				}
				if (User.IsPremium)
				{
					pnlPremium.Visible = true;
				}
				else
				{
					
				}
			}
			else
			{
				pnlTerms.Visible = true;
				if (ViewData["AgencyId"] == null)
				{
					this.pnlNoAgency.Visible = true;
				}
				else
				{
					this.pnlAgency.Visible = true;
				}
			}
			base.OnLoad(e);
		}
	}
}
