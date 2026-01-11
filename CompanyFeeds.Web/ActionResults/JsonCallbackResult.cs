using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace CompanyFeeds.Web.ActionResults
{
	public class JsonCallbackResult : JsonResult
	{
		public string Callback
		{
			get;
			set;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			HttpResponseBase response = context.HttpContext.Response;
			if (Data != null)
			{
				response.Write(Callback + "(");
			}

			base.ExecuteResult(context);

			if (Data != null)
			{
				response.Write(")");
			}
		}
	}
}
