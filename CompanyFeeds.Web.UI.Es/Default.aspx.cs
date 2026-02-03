using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CompanyFeeds.Web.UI.Es
{
	public partial class _Default : Page
	{

		protected override void OnLoad(System.EventArgs e)
		{
			HttpContext.Current.RewritePath(Request.ApplicationPath, false);
			IHttpHandler httpHandler = new MvcHttpHandler();
			httpHandler.ProcessRequest(HttpContext.Current); 

			base.OnLoad(e);
		}
	}
}
