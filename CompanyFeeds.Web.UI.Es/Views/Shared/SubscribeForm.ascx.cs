using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyFeeds.Web.UI.Es.Views.Shared
{
	public partial class SubscribeForm : System.Web.Mvc.ViewUserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			if (ViewData["Companies"] != null)
			{
				repCompanies.DataSource = ViewData["Companies"];
				repCompanies.DataBind();
			}
			base.OnLoad(e);
		}
	}
}
