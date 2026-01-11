using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using CompanyFeeds.Services;
using CompanyFeeds.Web.Helpers;
using CompanyFeeds.Validation;
using System.IO;
using CompanyFeeds.Web.ActionFilters;

namespace CompanyFeeds.Web.Controllers
{
	public class AgenciesController : BaseController
	{
		#region Detail
		public ActionResult Detail(string agencyTag)
		{
			Agency agency = AgenciesService.Get(agencyTag);
			if (agency == null)
			{
				return NotFoundResult();
			}
			ViewData["LogoUrl"] = ToWebUrl(SiteConfiguration.AgencyLogoPath);
			
			//Latest press releases submitted by the agency
			List<Entry> entriesSubmitted = EntriesService.GetByAgency(agency.Id);
			if (entriesSubmitted.Count > 0)
			{
				ViewData["EntriesSubmitted"] = entriesSubmitted;
			}
			return View(agency);
		} 
		#endregion

		#region Edit
        [AcceptVerbs(HttpVerbs.Get)]
        [CheckReadOnly]
		public ActionResult Edit(string agencyTag)
		{
			Agency agency = null;
			if (agencyTag != null)
			{
				if (User == null || agencyTag != User.AgencyTag)
				{
					return ForbiddenResult();
				}

				agency = AgenciesService.Get(agencyTag);
				ViewData["AgencyTag"] = agencyTag;
			}
			return View(agency);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public ActionResult Edit(string agencyTag, [Bind(Prefix = "", Exclude = "Logo")] Agency agency)
		{
			try
			{
				#region Arrange fields (tag, logo, url)
				agency.Tag = UrlUtils.ToUrlTag(agency.Name, 50);

                if (agency.Url != null && agency.Url != "" && !agency.Url.StartsWith("http://") && !agency.Url.StartsWith("https://"))
				{
					agency.Url = "http://" + agency.Url;
				}
				#endregion

				if (agencyTag == null)
				{
					AgenciesService.Add(agency);

					Session.NewlyCreatedAgencyId = agency.Id;

					return RedirectToAction("Edit", "Users", new
					{
						agencyId = agency.Id
					});
				}
				else
				{
					#region Check security
					if (User == null || agencyTag != User.AgencyTag)
					{
						return ForbiddenResult();
					}
					#endregion

					agency.Tag = agencyTag;

					AgenciesService.Update(agency);

					return RedirectToAction("Detail", "Agencies", new
					{
						agencyTag = agency.Tag
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

		#region About
		public ActionResult About()
		{
			ViewData["HideSearchBox"] = true;
			return View();
		}
		#endregion
	}
}
