using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.Helpers;

namespace CompanyFeeds.Web.Controllers
{
	public class AccountsController : BaseController
	{
		/// <summary>
		/// Used to see the premium wall without re-posting
		/// </summary>
		/// <returns></returns>
		public ActionResult PremiumWall()
		{
			ViewData["HideSearchBox"] = true;
			return View();
		}

		/// <summary>
		/// Display the benefits of having a premium account and displays the payment button
		/// </summary>
		/// <returns></returns>
		[RequireAuthorization]
		public ActionResult Premium()
		{
			if (User.IsPremium)
			{
				// the user is already a premium user: do not ask to pay again!
				return RedirectToAction("Edit", "Users", new {id=User.Id});
			}
			ViewData["HideSearchBox"] = true;
			return View();
		}

		/// <summary>
		/// Action that handles Paypal's IPN 
		/// </summary>
		/// <returns></returns>
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult PaymentNotification()
		{
			PaymentService.ProcessPaymentNotification(Request.Form, SiteConfiguration);
			return new EmptyResult();
		}

		/// <summary>
		/// Action that shows the user the "Thanks page"
		/// </summary>
		/// <param name="subscr_id"></param>
		/// <param name="payer_email"></param>
		/// <returns></returns>
		[RequireAuthorization]
		public ActionResult PaymentDone()
		{
			//check if payment notification is done
			//update the session with premium data
			ViewData["HideSearchBox"] = true;
			SecurityHelper.Login(Session, Response.Cookies, User.Id, User.Guid);
			if (!User.IsPremium)
			{
				//if not generate a link (repost) for the user to click to refresh
				return View("PaymentWaiting");
			}
			return View();
		}
	}
}
