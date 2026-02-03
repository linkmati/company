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

namespace CompanyFeeds.Web.Controllers
{
	public class CompaniesController : BaseController  
	{
		#region Detail
		public ActionResult Detail(string companyTag, int page)
		{
			if (companyTag == null)
			{
				throw new ArgumentNullException("companyTag");
			}
			CompaniesQueries.CompaniesDetailRow company = CompaniesService.GetByTag(companyTag);
			if (company == null)
			{
				return NotFoundResult();
			}
			CompaniesHelper.RegisterVisit(company.CompanyId, Cache, Request.UserHostAddress);

			EntriesQueries.EntriesListBasicDataTable dt = EntriesService.GetByCompany(company.CompanyId);

			ViewData["List"] = new PagedList<EntriesQueries.EntriesListBasicRow>(dt, page, SiteConfiguration.CompanyDetailPageSize);
			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.LogoPath);

			ViewData["page"] = page;
			ViewData["ActiveCategoryId"] = company.CategoryId;

			return View(company);
		} 
		#endregion

		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
		[RequireAuthorization(RequireMailValidation=true)]
		public ActionResult Edit(string companyTag, string companyName)
		{
			if (companyTag == null)
			{
				ViewData["Name"] = companyName;
			}
			else
			{
				Company company = CompaniesService.Get(companyTag);
				ViewData.Model = company;
			}
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[RequireAuthorization(RequireMailValidation=true)]
		public ActionResult Edit(string companyTag, [Bind(Prefix = "", Exclude="Logo")] Company company, HttpPostedFileBase Logo)
		{
			try
			{
				#region Arrange fields (tag, logo, feedurl)
				company.Tag = UrlUtils.ToUrlTag(company.Name, 50);
				if (Logo != null && Logo.ContentLength > 0)
				{
					company.Logo = Path.GetExtension(Logo.FileName);
					if (company.Logo != null)
					{
						company.Logo = company.Logo.Substring(1).ToLower();
					}
				}
				if (company.FeedUrl != null && company.FeedUrl != "" && !company.FeedUrl.StartsWith("http://"))
				{
					company.FeedUrl = "http://" + company.FeedUrl;
				}
				if (company.Url != null && company.Url != "" && !company.Url.StartsWith("http://"))
				{
					company.Url = "http://" + company.Url;
				}
				#endregion

				if (String.IsNullOrEmpty(companyTag))
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
				}
				else
				{
					company.Tag = companyTag;

					CompaniesService.Update(company, Request.UserHostAddress, User.Id);
				}

				#region Save File
				if (company.Logo != null)
				{
					ImageHelper.ResizeByWith(Logo.InputStream, SiteConfiguration.LogoMaxWidth, SiteConfiguration.GetApplicationRootConfigurationPath(String.Format(SiteConfiguration.LogoPath, company.Tag)));
				}
				#endregion


				return RedirectToAction("Detail", "Companies", new{companyTag=company.Tag});
			}
			catch (ValidationException ex)
			{
				this.AddErrors(ModelState, ex);
			}
			return View();
		}
		#endregion

		#region Search
		[IpFilter(30)]
		public ActionResult Search(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("Company name can not be null or empty.");
			}
			Company company = CompaniesService.GetCompany(name.Trim());
			if (company == null)
			{
				return NotFoundResult();
			}
			return RedirectToAction("Detail", "Companies", new{companyTag=company.Tag});
		}
		#endregion

		#region Get by Name
		[IpFilter(30)]
		public JsonResult Get(string companyName)
		{
			if (String.IsNullOrEmpty(companyName))
			{
				throw new ArgumentNullException("Company name can not be null or empty.");
			}
			return Json(CompaniesService.GetCompany(companyName.Trim()));
		}
		#endregion

		#region ListNames
		[AcceptVerbs(HttpVerbs.Get)]
		[ResultCache(Duration = 120)]
		public ViewResult ListNames()
		{
			return View(CompaniesService.GetCompanies());
		} 
		#endregion

		#region List All
		[ResultCache(Duration=120)]
		public ActionResult ListAll()
		{
			CompaniesQueries.CompanyNamesDataTable companies = CompaniesService.GetCompanies();	
			return View(companies);
		}
		#endregion
	}
}
