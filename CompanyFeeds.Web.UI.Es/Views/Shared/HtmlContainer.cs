using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CompanyFeeds.Web.UI.Es.Views.Shared
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:HtmlContainer runat=server></{0}:HtmlContainer>")]
	public class HtmlContainer : HtmlGenericControl
	{
		public HtmlContainer(string tag)
			: base(tag)
		{

		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{

		}

		protected override void RenderEndTag(HtmlTextWriter writer)
		{

		}
	}
}
