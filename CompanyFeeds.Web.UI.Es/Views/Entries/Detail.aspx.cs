using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.UI.Es.Views.Entries
{
	public partial class Detail : BaseViewPage<EntriesQueries.EntriesDetailRow>
    {
		protected override void OnLoad(EventArgs e)
		{
			if (Model.IsEntryContentNull())
			{
				pnlOriginalSource.Visible = true; 
			}
			else
			{
				pnlContent.Visible = true; 
			}

			if (Model.IsUserIdNull() || Model.HideAuthor)
			{
				lblSubmitted.Visible = false;
			}

			if (!Model.IsEntryContactInfoNull()) 
			{
				pnlContactInfo.Visible = true;
			}

			base.OnLoad(e);
		}
    }
}
