using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CompanyFeeds.Validation;
using CompanyFeeds.Services;

namespace CompanyFeeds.Web.Controllers
{
	public class FeedbackController : BaseController
	{
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Contact()
		{

			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Contact(string name, string email, string message)
		{
			try
			{
				NotificationsService.SendFeedback(name, email, message, false, SiteConfiguration.Mail);
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex);
				return View();
			}
			return View("MessageSent");
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult ReportBug()
		{

			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ReportBug(string name, string email, string message)
		{
			try
			{
				NotificationsService.SendFeedback(name, email, message, true, SiteConfiguration.Mail);
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex);
				return View();
			}
			return View("MessageSent");
		}
	}
}
