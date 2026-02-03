using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Es.Views.Subscriptions
{
	public partial class List : BaseViewPage
	{
		protected override void OnLoad(EventArgs e)
		{
			if (ViewData["Entries"] == null)
			{
				pnlCompanies.Visible = false;
				pnlNoRecords.Visible = true;
			}
			else
			{
				this.DataBind();
			}
			base.OnLoad(e);
		}
	}
}
