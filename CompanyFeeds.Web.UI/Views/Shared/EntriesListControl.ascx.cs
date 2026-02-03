using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.UI.Views.Shared
{
	public partial class EntriesListControl : System.Web.Mvc.ViewUserControl<PagedList<EntriesQueries.EntriesListRow>>
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			if (ViewData.Model != null && ViewData.Model.Count > 0)
			{
				if (rep != null)
				{
					rep.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rep_ItemDataBound);
					rep.DataSource = ViewData.Model;
				}
				this.DataBind();
			}
			base.OnLoad(e);
		}

		void rep_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (ViewData.Get<bool>("ShowAdvertising", false))
			{
				if (e.Item.ItemIndex == 1 || e.Item.ItemIndex == 3)
				{
					var control = e.Item.FindControl("ad");
					if (control != null)
					{
						control.Visible = true;
					}
				}
			}
		}
	}
}
