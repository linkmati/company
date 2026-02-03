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
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Es.Views.Home
{
	public class SitemapsDetail : BaseViewPage<EntriesQueries.EntriesListDataTable>
	{

		protected Repeater rep;

		protected override void OnLoad(EventArgs e)
		{
			Response.ContentType = "text/xml";
			rep.DataSource = this.Model;
			rep.DataBind();
			base.OnLoad(e);
		}
	}
}
