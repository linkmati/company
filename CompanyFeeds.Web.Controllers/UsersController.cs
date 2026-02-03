using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CompanyFeeds.Services;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Validation;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.State;
using System.IO;
using CompanyFeeds.Web.ActionResults;
using System.Web.Routing;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	public class UsersController : BaseController
	{
		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
        [CheckReadOnly]
		public ActionResult Edit(int id, int? agencyId)
		{
			User user = new User();
			//user.CountryCode = "US"; // default value

			#region Load
			if (id > 0 && User != null && id == User.Id)
			{
				user = UsersService.Get(id);
				user.Password = null;
				ViewData["ShowAdditional"] = true;
			}
			#endregion
			
			ViewData["AgencyId"] = agencyId;
			ViewData["Gender"] = AdaptGenders(user.Gender);

			//ViewData["CountryCode"] = new SelectList(CountriesService.GetCountries(), "CountryCode", "CountryName", user.CountryCode);
			ViewData["Countries"] = CountriesService.GetCountries();
			
			return View(user);
		}

		#region Genders
		private SelectList AdaptGenders(object selectedValue)
		{
			Dictionary<string, string> genders = new Dictionary<string, string>();
			genders.Add("", "");
			genders.Add("1", "Male");
			genders.Add("0", "Female");
			if (selectedValue != null)
			{
				selectedValue = (int)selectedValue;
			}
			SelectList s = new SelectList(genders.ToArray(), "key", "value", selectedValue);
			return s;
		}
		#endregion

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit([Bind(Prefix = "")]User user, string passwordConfirm, int? agencyId)
		{
			try
			{
                // Validate captcha first
			    SecurityHelper.ValidateCaptcha(Request.Form, Request.UserHostAddress);

				if (passwordConfirm != user.Password)
				{
					ModelState.AddModelError("PasswordConfirm", new ValidationError("PasswordConfirm", ValidationErrorType.CompareNotMatch));
				}

				if (GetErrors(ModelState).Count == 0)
				{

					if (user.Id > 0 && User != null && user.Id == User.Id)
					{
						#region Fill in the password
						if (String.IsNullOrEmpty(user.Password))
						{
							user.Password = UsersService.Get(user.Id).Password;
						}
						#endregion

						UsersService.Update(user);

						ViewData["Update"] = true; 
					}
					else
					{
						#region Validate configuration
						if ((!SiteConfiguration.Mail.Subjects.Keys.Contains("Welcome")) || (!SiteConfiguration.Mail.TemplatesPath.Keys.Contains("Welcome")))
						{
							throw new ArgumentException("Template and/or Subject not defined for key 'Welcome'");
						}
						#endregion

						if (agencyId != null)
						{
							user.AgencyId = agencyId.Value;
							user.AgencyValidated = user.AgencyId == Session.NewlyCreatedAgencyId;
						}

						UsersService.AddUser(user);

						//Url to validate email
						string validationUrl = this.Domain + UrlExtensions.GenerateFormattedUrl("ValidateEmail", "Users", new{id = user.Id,guid = user.Guid.ToString("D")}, RouteTable.Routes, this.ControllerContext.RequestContext);

						NotificationsService.SendValidationMail(user, Path.Combine(SiteConfiguration.GetApplicationRootConfigurationPath(), SiteConfiguration.Mail.TemplatesPath["Welcome"]), SiteConfiguration.Mail.AdminMailAddress, SiteConfiguration.Mail.AdminMailName, SiteConfiguration.Mail.Subjects["Welcome"], validationUrl, SiteConfiguration.Mail.SmtpServer, SiteConfiguration.Mail.GetCredentials());

						SecurityHelper.Login(Session, HttpContext.Response.Cookies, user.Id, user.Guid);
						
						//Set a special url for the stat tracker
						ViewData["TrackerUrl"] = "/user/registerdone/";

						return View("RegisterDone");  
					}
				}
			}
			catch (ValidationException ex)
			{
				this.AddErrors(this.ModelState, ex);
			}

			ViewData["Gender"] = AdaptGenders(user.Gender);
			//ViewData["CountryCode"] = new SelectList(CountriesService.GetCountries(), "CountryCode", "CountryName", user.CountryCode);
			ViewData["Countries"] = CountriesService.GetCountries();
			ViewData["ShowAdditional"] = true;


			return View(user);
		} 
		#endregion

		#region Login - Logout
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Login()
		{
			return View();
		}

		public ActionResult Login(string Email, string Password, string returnUrl)
		{
			UserState user = SecurityHelper.Login(Session, HttpContext.Response.Cookies, Email, Password);
			if (user != null)
			{
				//redirect
				if (returnUrl != null)
				{
					return Redirect(returnUrl);
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				//Add the errors
				AddErrors(ModelState, new ValidationException(new List<ValidationError>(){new ValidationError("Email", ValidationErrorType.CompareNotMatch)}));
			}

			return View();
		}

		public ActionResult Logout()
		{
			SecurityHelper.Logout(Session);

			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Detail
		public ActionResult Detail(int id, ResultType type)
		{
			UsersQueries.UsersDetailRow user = UsersService.GetDetail(id);
			if (user == null)
			{
				return NotFoundResult();
			}

			SubscriptionsQueries.SubscriptionsDataTable companies = SubscriptionsService.Get(id);
			PagedList<EntriesQueries.EntriesListRow> entries = null;
			if (companies.Count > 0)
			{
				ViewData["Companies"] = companies;
				entries = new PagedList<EntriesQueries.EntriesListRow>(EntriesService.GetByUser(id, SiteConfiguration.SubscriptionPageSize), 0, SiteConfiguration.SubscriptionPageSize);
				ViewData["Entries"] = entries;
			}
			 
			if (type == ResultType.Rss)
			{
				#region Rss
				StringBuilder description = new StringBuilder();
				description.Append("Press releases for ");
				description.Append(user.UserName);
				description.AppendLine();
				description.Append("(");
				bool first = true;
				foreach (SubscriptionsQueries.SubscriptionsRow company in companies)
				{
					if (!first)
					{
						description.Append(", ");
					}
					description.Append(company.CompanyName);
					first = false;
				}
				description.Append(")");

				return Rss(SiteConfiguration.RssBaseTitle + " - " + user.UserName, Domain, description.ToString(), entries, "EntryTitle", null, "EntryTeaser", "EntryDate", GetEntryDetailUrl);
				#endregion
			}
			else
			{
				#region Latest press releases submitted by user

			    if (UserIsAdmin && Request["show"] == "full")
                {
                    List<Entry> entriesSubmitted = EntriesService.GetByUser(id);
                    if (entriesSubmitted.Count > 0)
                    {
                        ViewData["EntriesSubmitted"] = entriesSubmitted;
                    }    
			    }
				#endregion
			}
			 
			return View(user);
		}
		#endregion

		#region List
		[RequireAuthorization(RequireMailValidation = true)]
		public ActionResult ListUsersWithEmailNotValidated()
		{
			if (UserIsAdmin)
			{
				List<User> users = UsersService.GetByEmailInactive();
				return View(users);
			}
			return NotFoundResult();
		}
		#endregion

		#region ValidateEmail
		public ActionResult ValidateEmail(int id, string guid)
		{
			bool validated = UsersService.ValidateEmail(id, new Guid(guid));
			if (validated)
			{
				SecurityHelper.Login(Session, Response.Cookies, id, new Guid(guid));
			}	
			return View();
		}

		[RequireAuthorization(RequireMailValidation = true)]
		public ActionResult ValidateEmailByAdmin(int id, string guid)
		{
			if (UserIsAdmin)
			{
				UsersService.ValidateEmail(id, new Guid(guid));

				if (Request.UrlReferrer != null)
				{
					return Redirect(Request.UrlReferrer.PathAndQuery);
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				return NotFoundResult();
			}
		}
		#endregion

		#region Spam prevention
		/// <summary>
		/// Retrieves a list of IPs used to submit content
		/// </summary>
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult IpsUsed(int id)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}
			var ipList = UsersService.GetIpsUsed(id);

			return Json(ipList);
		}

		/// <summary>
		/// Retrieves a list of IPs used to submit content
		/// </summary>
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ActivityByIp(string ip)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}
			var entries = EntriesService.GetByIp(ip);
			var companies = CompaniesService.GetByIp(ip);
			return Json(new { entries=entries, companies=companies});
		}

		/// <summary>
		/// Retrieves a list of IPs used to submit content
		/// </summary>
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			if (!UserIsAdmin)
			{
				return NotFoundResult();
			}
			UsersService.Delete(id);
			return Json("OK");
		}
		#endregion
	}
}
