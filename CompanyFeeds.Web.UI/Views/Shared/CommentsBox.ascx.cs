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
using System.Collections.Generic;
using CompanyFeeds.Web.State;

namespace CompanyFeeds.Web.UI.Views.Shared
{
	public class CommentsBox : BaseViewUserControl<List<Comment>>
	{
		protected HtmlContainer pnlForm;
		protected HtmlContainer pnlNoMember;
		protected Repeater rep;

		protected override void OnLoad(EventArgs e)
		{
			if (this.Model != null && this.Model.Count > 0)
			{
				rep.DataSource = this.Model;
				rep.DataBind();
			}


			if (Session.User == null)
			{
				pnlNoMember.Visible = true;
			}

			base.OnLoad(e);
		}
	}
}
