using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Mvc;

namespace CompanyFeeds.Web.UI.Views.Shared
{
	public class SearchBox : ViewUserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			if (ViewData["HideSearchBox"] != null && Convert.ToBoolean(ViewData["HideSearchBox"]))
			{
				this.Visible = false;
			}
			base.OnLoad(e);
		}
	}
}
