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
using System.Globalization;
using System.Web;

namespace CompanyFeeds.Web.Controllers
{
	public class EntriesController : BaseController
	{
		#region Detail
		[AcceptVerbs(HttpVerbs.Get)]
		[CheckValidForAdvertising]
		public ActionResult Detail(int id, string tag) 
		{ 
			var entryDetail = EntriesService.GetDetail(id, tag);

			if (entryDetail == null)
			{
				return NotFoundResult();
			}

			//CompaniesHelper.RegisterVisit(entryDetail.CompanyId, Cache, Request.UserHostAddress);

			ViewData["ActiveCategoryId"] = entryDetail.CategoryId;
			if (User != null)
			{
				if (UserIsAdmin || entryDetail.EntryOwner == User.Id)
				{
					ViewData["CanEdit"] = true;
				}
			}

			if (entryDetail.EntryAllowComments)
			{
				ViewData["Comments"] = CommentsService.GetComments(id);
			}
			if (Session.IsNewEntry)
			{
				//Set a special url for the stat tracker
				ViewData["TrackerUrl"] = "/entries/add-done/?requrl=" + HttpUtility.UrlEncode(HttpContext.Request.Url.PathAndQuery);
				Session.IsNewEntry = false;
			}

			return View(entryDetail);
		}
		#endregion

		#region ShortUrl
		public ActionResult GetShortUrl(int id)
		{
			var entry = EntriesService.Get(id);
			if (entry == null)
			{
				return NotFoundResult();
			}
			return this.RedirectToAction("Detail", "Entries", new {id=entry.Id,companyTag=entry.CompanyTag,tag=entry.Tag});
		}
		#endregion

		#region Add
		[AcceptVerbs(HttpVerbs.Get)]
        [PreventFlood(typeof(Entry))]
        [CheckReadOnly]
		public ActionResult Add()
		{
			if (User == null)
			{
				return View("AddPromotional");
			}
			if (User != null && !User.IsEmailActive)
			{
				return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.PathAndQuery });
			}

			return View("Edit");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken(Order = 100)]
		[RequireAuthorization(RequireMailValidation = true, Order = 110)]
		[PreventFlood(typeof(RedirectToRouteResult), typeof(Entry), Order = 120)]
		[CheckBlacklistedUrls("entry", Order=130)]
		[CheckSpam("entry", Order = 200)]
		public ActionResult Add([Bind(Prefix = "", Exclude = "Source,Tag,ExternalGuid")] Entry entry)
		{
			try
			{
				entry.UserId = User.Id;

				//If: the company exist or not provided
				bool companyExists = entry.CompanyId > 0 || entry.CompanyName.Trim() == ""; //if the user did not enter company data

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

				if (companyExists)
				{
					if (CompaniesHelper.IsClosed(entry.CompanyId, User.Id, UserIsAdmin))
					{
						return View("CompanyClosed");
					}
					EntriesService.AddEntry(entry, Request.UserHostAddress);

					Session.IsNewEntry = true;
					return RedirectToAction("GetShortUrl", new
					{
						id = entry.Id
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
					return RedirectToAction("Add", "Companies", new { companyName = entry.CompanyName });
				}
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex);
			}

			#region Adapt some fields for show the same view
			if (ModelState["Teaser"] != null && ModelState["Teaser"].Value == null)
			{
				ModelState["Teaser"].Value = new ValueProviderResult("", "", CultureInfo.CurrentUICulture);
			}
			#endregion

			return View("Edit");
		}
		#endregion

		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
        [RequireAuthorization]
        [CheckReadOnly]
		public ActionResult Edit(int id)
		{
			var entry = EntriesService.Get(id);
			if (entry == null)
			{
				return NotFoundResult();
			}
			return View(entry);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		[RequireAuthorization(RequireMailValidation=true)]
		[ValidateAntiForgeryToken]
		[CheckBlacklistedUrls("entry")]
		public ActionResult Edit([Bind(Prefix = "", Exclude = "Source,Tag,ExternalGuid")] Entry entry)
		{
			try
			{
				entry.UserId = User.Id;

				#region From the previous saved info
				EntriesQueries.EntriesDetailRow detail = EntriesService.GetDetail(entry.Id, RouteData.Values["tag"].ToString());
				if (detail == null)
				{
					throw new ArgumentException("No entry for given id/tag");
				}
				entry.Tag = detail.EntryTag;
				entry.CompanyName = detail.CompanyName;

				#region Check security
				if ((!UserIsAdmin) && User.Id != detail.EntryOwner)
				{
					return ResultHelper.ForbiddenHackingAttemp(this);
				}
				#endregion

				#region Allow comments check
				if (detail.EntryOwner != User.Id)
				{
					entry.AllowComments = null;
				}
				#endregion
				#endregion

				if (CompaniesHelper.IsClosed(entry.CompanyId, User.Id, UserIsAdmin))
				{
					return View("CompanyClosed");
				}

				EntriesService.Update(entry, Request.UserHostAddress);
				//Redirect to the entry detail

				RedirectToRouteResult result = RedirectToAction("Detail", new{id = entry.Id,companyTag = detail.CompanyTag,tag = entry.Tag});

				return result;
			}
			catch (ValidationException ex)
			{
				AddErrors(ModelState, ex); 
			}
			return View(entry);
		}
		#endregion

		#region Delete
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			if (UserIsAdmin)
			{
				EntriesService.Delete(id);

				return Json("OK");
			}
			return NotFoundResult();
		}
		#endregion

		#region Delete by userid
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteByUser(int id)
		{
			if (UserIsAdmin)
			{
				var deleted = EntriesService.DeleteByUser(id);

				return Json(deleted);
			}
			return NotFoundResult();
		}
		#endregion

		#region Report abuse
		public JsonResult ReportAbuse(int entryId)
		{
			EntriesService.AddFlag(entryId, User != null ? (int?)User.Id : null);
			return Json(true);
		} 
		#endregion

		#region Show content
		[RequireAuthorization(RequireMailValidation = true)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ShowContent(int id)
		{
			var entry = EntriesService.Get(id);

			if (entry == null)
			{
				return NotFoundResult();
			}

			return Content(entry.Content);
		} 
		#endregion
	}
}
