using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CompanyFeeds.DataAccess;

namespace CompanyFeeds.Web.UI
{
	public partial class _Default : Page
	{
		public void Page_Load(object sender, System.EventArgs e)
		{
			HttpContext.Current.RewritePath(Request.ApplicationPath);
			IHttpHandler httpHandler = new MvcHttpHandler();
			httpHandler.ProcessRequest(HttpContext.Current);
		}
	}
}
