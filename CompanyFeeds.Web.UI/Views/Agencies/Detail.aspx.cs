using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Views.Agencies
{
	public partial class Detail : BaseViewPage<Agency>
	{
		protected override void OnLoad(EventArgs e)
		{
			if (Model.Logo != null)
			{
				imgContainer.Visible = true;
			}
			if (ViewData["EntriesSubmitted"] != null)
			{
				repSubmits.DataBind();
			}
			base.OnLoad(e);
		}
	}
}
