using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Views.Agencies
{
	public partial class Edit : BaseViewPage<Agency>
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;
			base.OnLoad(e);
		}
	}
}
