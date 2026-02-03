using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CompanyFeeds.Web.Controllers
{
	public class CommandsController : BaseController
	{
		public ActionResult CacheClear()
		{
			if (UserIsAdmin)
			{
				//Clear the cache
				Cache.Clear();

				return RedirectToAction("Index", "Home");
			}
			return NotFoundResult();
		}

		public ActionResult CacheStatus()
		{
			if (UserIsAdmin)
			{
				string result = "Cache Percentage Max: " + Cache.Cache.EffectivePercentagePhysicalMemoryLimit;
				result += "\r\nCache bytes limit: " + Cache.Cache.EffectivePrivateBytesLimit;


				return Content(result);
			}
			return NotFoundResult();
		}
	}
}
