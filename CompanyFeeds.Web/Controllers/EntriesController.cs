using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Services;
using CompanyFeeds.Validation;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Web.Helpers;
using System.Web.Routing;

namespace CompanyFeeds.Web.Controllers
{
	public class EntriesController : BaseController
	{
		#region Detail
		public ActionResult Detail(int id, string tag) 
		{ 
			EntriesQueries.EntriesDetailRow entryDetail = EntriesService.GetDetail(id, tag);

			if (entryDetail == null)
			{
				return NotFoundResult();
			}

			CompaniesHelper.RegisterVisit(entryDetail.CompanyId, Cache, Request.UserHostAddress);

			ViewData["ActiveCategoryId"] = entryDetail.CategoryId;
			return View(entryDetail);
		}
		#endregion

		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
		[ValidateInput(false)]
		public ActionResult Edit(int id)
		{
			Entry entry = null;
			if (id == 0)
			{
				//Create
				if (User != null && !User.IsEmailActive)
				{
					return RedirectToAction("Login", "Users", new{returnUrl=Request.Url.PathAndQuery});
				}
			}
			else
			{
				//Edit
				if (User == null)
				{
					return RedirectToAction("Login", "Users", new{returnUrl=Request.Url.PathAndQuery});
				}
				//Load all the data
				entry = EntriesService.Get(id);
			}
			ViewData["Companies"] = CompaniesService.GetCompanies();

			return View(entry);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[RequireAuthorization(RequireMailValidation=true)]
		public ActionResult Edit([Bind(Prefix = "", Exclude = "Source,Tag,ExternalGuid")] Entry entry)
		{
			try
			{
				entry.UserId = User.Id;


				if (entry.Id == 0)
				{
					#region Check if the company exist
					Company company = null;
					if (entry.CompanyName.Trim() != "")
					{
						company = CompaniesService.GetCompany(entry.CompanyName);
						if (company != null)
						{
							entry.CompanyId = company.Id;
						}
					}
					#endregion

					#region Fill in auto fields (teaser and tag)
					if (!String.IsNullOrEmpty(entry.Content))  
					{
						entry.Content = TextUtils.CleanHtml(entry.Content);
						if (String.IsNullOrEmpty(entry.Teaser))
						{
							entry.Teaser = TextUtils.SummarizeHtml(entry.Content, 509, "...");
						}
					}
					entry.Tag = UrlUtils.ToUrlTag(entry.EntryTitle, 128);
					#endregion

					if (company != null || entry.CompanyName.Trim() == "")
					{
						//If: the company exist or not provided
						EntriesService.AddEntry(entry, Request.UserHostAddress); 
						 
						return RedirectToAction("Detail", new
						{
							id = entry.Id,
							companyTag = company.Tag,
							tag = entry.Tag
						});
					}
					else
					{
						//Its a new company
						//Check if the entry is ready to be saved.
						EntriesService.IsPreparedToSave(entry);

						//Add the entry to session
						Session.Entry = entry;

						//Redirect to add company form
						return RedirectToAction("Edit", "Companies", new {companyName = entry.CompanyName});
					}
				}
				else
				{
					//Its an Update

					#region Fill the needed fields
					EntriesQueries.EntriesDetailRow detail = EntriesService.GetDetail(entry.Id, RouteData.Values["tag"].ToString());
					if (detail == null)
					{
						throw new ArgumentException("No entry for given id/tag");
					}
					entry.Tag = detail.EntryTag;
					entry.CompanyName = detail.CompanyName;
					#endregion

					EntriesService.Update(entry, Request.UserHostAddress);
					//Redirect to the entry detail

					RedirectToRouteResult result = RedirectToAction("Detail", new{id = entry.Id,companyTag = detail.CompanyTag,tag = entry.Tag});

					//this.Cache.NotCachedUrls.Add(RouteTable.Routes.GetUrl(ControllerContext.RequestContext, result.RouteValues));

					return result;
				}
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex); 
			}

			#region Adapt some fields for show the same view
			if (this.ViewData.ModelState["Teaser"] == null)
			{
				this.ViewData.ModelState["Teaser"] = new ModelState();
				this.ViewData.ModelState["Teaser"].Value = new ValueProviderResult("", "", System.Threading.Thread.CurrentThread.CurrentCulture);
			}
			else if (this.ViewData.ModelState["Teaser"].Value == null)
			{
				this.ViewData.ModelState["Teaser"].Value = new ValueProviderResult("", "", System.Threading.Thread.CurrentThread.CurrentCulture);
			}
			#endregion

			return View();
		} 
		#endregion

		#region Report abuse
		public JsonResult ReportAbuse(int entryId)
		{
			EntriesService.AddFlag(entryId, User != null ? (int?)User.Id : null);
			return Json(true);
		} 
		#endregion

	}
}
