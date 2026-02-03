using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyFeeds.Web.UI.Views.Shared
{
	public partial class SubscribeForm : BaseViewUserControl
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
