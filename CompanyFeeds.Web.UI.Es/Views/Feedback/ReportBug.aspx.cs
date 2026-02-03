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

namespace CompanyFeeds.Web.UI.Es.Views.Feedback
{
	public class ReportBug : ViewPage
	{
		protected override void OnLoad(EventArgs e)
		{
			ViewData["HideSearchBox"] = true;
			base.OnLoad(e);
		}
	}
}
