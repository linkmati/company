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

namespace CompanyFeeds.Web.UI.Es.Views.Home
{
	public class Sitemaps : BaseViewPage
	{
		protected override void OnLoad(EventArgs e)
		{
			Response.ContentType = "text/xml";
			base.OnLoad(e);
		}
	}
}
