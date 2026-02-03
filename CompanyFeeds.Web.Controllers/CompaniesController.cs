using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Services;
using System.Web;
using CompanyFeeds.Validation;
using System.IO;
using System.Web.Script.Serialization;
using CompanyFeeds.Web.ActionFilters;
using System.Net;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Web.ActionResults;
using CompanyFeeds.Web.State;
using CompanyFeeds;
using System.Web.UI;

namespace CompanyFeeds.Web.Controllers
{
	public class CompaniesController : BaseController  
	{
		#region Detail
		[CheckValidForAdvertising]
		public ActionResult Detail(string companyTag, int page)
		{
			if (companyTag == null)
			{
				throw new ArgumentNullException("companyTag");
			}
			var company = CompaniesService.GetDetailByTag(companyTag, page, SiteConfiguration);
			if (company == null)
			{
				return NotFoundResult();
			}
			
            //CompaniesHelper.RegisterVisit(company.CompanyId, Cache, Request.UserHostAddress);

			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.LogoPath);
			ViewData["page"] = page;
			ViewData["ActiveCategoryId"] = company.CategoryId;

			if (Session.IsNewCompany)
			{
				//Set a special url for the stat tracker
				ViewData["TrackerUrl"] = "/companies/add-done/?requrl=" + HttpUtility.UrlEncode(HttpContext.Request.Url.PathAndQuery);
				Session.IsNewCompany = false;
			}

			return View(company);
		}
		#endregion

		#region Add
		[AcceptVerbs(HttpVerbs.Get)]
		[RequireAuthorization(RequireMailValidation = true)]
        [PreventFlood(typeof(Company))]
        [CheckReadOnly]
		public ActionResult Add(string companyName)
		{
			ViewData["Name"] = companyName.UppercaseFirst();
			return View("Edit");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken(Order = 100)]
		[RequireAuthorization(RequireMailValidation = true, Order = 110)]
		[CheckBlacklistedUrls("company", Order = 120)]
		[PreventFlood(typeof(RedirectToRouteResult), typeof(Company), Order = 130)]
		[CheckSpam("company", Order = 200)]
		public ActionResult Add([Bind(Prefix = "", Exclude = "Logo")] Company company)
		{
			try
			{
				#region Arrange fields (tag, logo, feedurl)
				company.Tag = UrlUtils.ToUrlTag(company.Name, 50);

				if (!String.IsNullOrEmpty(company.FeedUrl))
				{
					if (this.User.Role == UserRole.Admin)
					{
						if ((!company.FeedUrl.StartsWith("http://")) && (!company.FeedUrl.StartsWith("https://")))
						{
							company.FeedUrl = "http://" + company.FeedUrl;
						}
					}
					else
					{
						company.FeedUrl = null;
					}
				}
				if (company.Url != null && company.Url != "" && (!company.Url.StartsWith("http://")) && (!company.Url.StartsWith("https://")))
				{
					company.Url = "http://" + company.Url;
				}
				#endregion

				if (GetErrors(ModelState).Count == 0)
				{
					CompaniesService.Add(company, Request.UserHostAddress, User.Id);

					#region Check if an entry has to be saved
					if (Session.Entry != null)
					{
						Session.Entry.CompanyId = company.Id;
						//Save the Entry
						EntriesService.AddEntry(Session.Entry, Request.UserHostAddress);

						//Remove it from session
						Session.Entry = null;
					}
					#endregion

					CompaniesHelper.RefreshFullList();

					Session.IsNewCompany = true;
					return RedirectToAction("Detail", "Companies", new
					{
						companyTag = company.Tag
					});
				}
			}
			catch (ValidationException ex)
			{
				this.AddErrors(ModelState, ex);
			}
			return View("Edit");
		} 
		#endregion

		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
        [RequireAuthorization(RequireMailValidation = true)]
        [CheckReadOnly]
		public ActionResult Edit(string companyTag, string companyName)
		{
			if (CompaniesHelper.IsClosed(companyTag, User.Id, UserIsAdmin))
			{
				return View("CompanyClosed");
			}
			var company = CompaniesService.Get(companyTag);
			return View(company);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[RequireAuthorization(RequireMailValidation=true)]
		[ValidateAntiForgeryToken]
		[CheckBlacklistedUrls("company")]
		[CheckSpam("company", Order = 200)]
		public ActionResult Edit(string companyTag, [Bind(Prefix = "", Exclude="Logo")] Company company, HttpPostedFileBase Logo)
		{
			try
			{
				if (CompaniesHelper.IsClosed(companyTag, User.Id, UserIsAdmin))
				{
					return View("CompanyClosed");
				}

				#region Arrange fields (logo, feedurl)
				var storedCompany = CompaniesService.Get(companyTag);
				if (Logo != null && Logo.ContentLength > 0)
				{
					company.Logo = Path.GetExtension(Logo.FileName);
					if (company.Logo != null)
					{
						company.Logo = company.Logo.Substring(1).ToLower();
					}
				}
				if (this.User.Role == UserRole.Admin && !String.IsNullOrEmpty(company.FeedUrl))
				{
					if ((!company.FeedUrl.StartsWith("http://")) && (!company.FeedUrl.StartsWith("https://")))
					{
						company.FeedUrl = "http://" + company.FeedUrl;
					}
				}
				else
				{
					company.FeedUrl = storedCompany.FeedUrl;
				}
				if (company.Url != null && company.Url != "" && (!company.Url.StartsWith("http://")) && (!company.Url.StartsWith("https://")))
				{
					company.Url = "http://" + company.Url;
				}
				#endregion

				if (GetErrors(ModelState).Count == 0)
				{
					company.Tag = companyTag;

					CompaniesService.Update(company, Request.UserHostAddress, User.Id);

					#region Save File
					if (company.Logo != null)
					{
						ImageHelper.ResizeByWith(Logo.InputStream, SiteConfiguration.LogoMaxWidth, SiteConfiguration.GetApplicationRootConfigurationPath(String.Format(SiteConfiguration.LogoPath, company.Tag)));
					}
					#endregion

					CompaniesHelper.RefreshFullList();

					return RedirectToAction("Detail", "Companies", new
					{
						companyTag = company.Tag
					});
				}
			}
			catch (ValidationException ex)
			{
				this.AddErrors(ModelState, ex);
			}
			return View();
		}
		#endregion

		#region Search
		public JsonResult Search(string name)
		{
			var companies = CompaniesHelper.SearchByName(name);

			return Json(companies);
		}
		#endregion

		#region List All
		/// <summary>
		/// Gets top companies 
		/// </summary>
		[ResultCache(Duration=86400)]
		public ActionResult ListAll()
		{
			return View(CompaniesService.GetTopCompanies());
		}
		#endregion

		#region Get by Name
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Get(string companyName)
		{
			if (String.IsNullOrEmpty(companyName))
			{
				throw new ArgumentNullException("Company name can not be null or empty.");
			}
			return Json(CompaniesHelper.GetCompanyId(companyName));
		}
		#endregion

		#region Set no relevance
		[RequireAuthorization(RequireMailValidation = true)]
		public ActionResult SetNoRelevance(int id)
		{
			if (UserIsAdmin)
			{
				CompaniesService.SetNoVisits(id);
				return Redirect("/");
			}
			return NotFoundResult();
		} 
		#endregion

		#region Delete
		/// <summary>
		/// Removes the company and all of its entries
		/// </summary>
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			if (UserIsAdmin)
			{
				CompaniesService.Delete(id);
				return Json("OK");
			}
			return NotFoundResult();
		}
		#endregion

        [AcceptVerbs(HttpVerbs.Post)]
	    public ActionResult RegisterVisit(int id)
	    {
            if (Request.Cookies.Get("regv") != null)
            {
                return Content("OK0" + id);
            }
            var cookie = new HttpCookie("regv", "set")
            {
                Path = "/",
                HttpOnly = false,
                Expires = DateTime.Now.AddYears(1)
            };
            Response.Cookies.Add(cookie);
            CompaniesService.AddVisit(id);
	        return Content("OK1" + id);
	    }
	}
}
