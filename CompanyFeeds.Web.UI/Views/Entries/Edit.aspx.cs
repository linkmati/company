using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CompanyFeeds.Web.UI.Views.Entries
{
	public partial class Edit : BaseViewPage<Entry>
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;
			
			if (Model != null)
			{
				//If its not the owner -> do not allow to edit the allow comments control.
				if (Model.Owner != User.Id)
				{
					pnlAllowComments.Visible = false;
				}
			}
			base.OnLoad(e);
		}
	}
}
