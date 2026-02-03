using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Es.Views.Users
{
	public partial class Login : BaseViewPage
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;
			if (User != null && !User.IsEmailActive)
			{
				pnlForm.Visible = false;
				pnlValidateEmail.Visible = true;
			}
			base.OnLoad(e);
		}
	}
}
