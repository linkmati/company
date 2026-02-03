using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CompanyFeeds.Validation;
using CompanyFeeds.Services;
using CompanyFeeds.Web.Helpers;

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
		[ValidateAntiForgeryToken]
		public ActionResult Contact(string name, string email, string message)
		{
			try
			{
			    SecurityHelper.ValidateCaptcha(Request.Form, Request.UserHostAddress);

				NotificationsService.SendFeedback(name, email, message, Request.Url.Host, "Contact form", SiteConfiguration.Mail);
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
			ViewData["HideSearchBox"] = true;
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateAntiForgeryToken]
		public ActionResult ReportBug(string name, string email, string message)
		{
			try
			{
			    SecurityHelper.ValidateCaptcha(Request.Form, Request.UserHostAddress);

				NotificationsService.SendFeedback(name, email, message, Request.Url.Host, "Bug report", SiteConfiguration.Mail);
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex);
				ViewData["HideSearchBox"] = true;
				return View();
			}
			return View("MessageSent");
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult ContactPremium()
		{
			ViewData["HideSearchBox"] = true;
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateAntiForgeryToken]
		public ActionResult ContactPremium(string name, string email, string message)
		{
			try
			{
			    SecurityHelper.ValidateCaptcha(Request.Form, Request.UserHostAddress);

				NotificationsService.SendFeedback(name, email, message, Request.Url.Host, "Contact - Premium", SiteConfiguration.Mail);
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex);
				ViewData["HideSearchBox"] = true;
				return View();
			}
			return View("MessageSent");
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult CopyrightInfringements()
		{
			return View();
		}


		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateAntiForgeryToken]
		public ActionResult CopyrightInfringements(string name, string email, string message)
		{
			try
			{
			    SecurityHelper.ValidateCaptcha(Request.Form, Request.UserHostAddress);

				NotificationsService.SendFeedback(name, email, message, Request.Url.Host, "Reporting Copyright Infringements", SiteConfiguration.Mail);
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
