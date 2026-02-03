using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Views.Companies
{
	public partial class Edit : BaseViewPage<Company>
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;
			base.OnLoad(e);
		}
	}
}
