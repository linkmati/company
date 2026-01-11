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
			return View(agency);
		} 
		#endregion

		#region Edit
		[AcceptVerbs(HttpVerbs.Get)]
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
		public ActionResult Edit(string agencyTag, [Bind(Prefix = "", Exclude = "Logo")] Agency agency, HttpPostedFileBase Logo)
		{
			try
			{
				#region Arrange fields (tag, logo, url)
				agency.Tag = UrlUtils.ToUrlTag(agency.Name, 50);
				if (Logo != null && Logo.ContentLength > 0)
				{
					agency.Logo = Path.GetExtension(Logo.FileName);
					if (agency.Logo != null)
					{
						agency.Logo = agency.Logo.Substring(1).ToLower();
					}
				}
				if (agency.Url != null && agency.Url != "" && !agency.Url.StartsWith("http://"))
				{
					agency.Url = "http://" + agency.Url;
				}
				#endregion

				if (agencyTag == null)
				{
					AgenciesService.Add(agency);

					Session.NewlyCreatedAgencyId = agency.Id;

					#region Save File
					if (agency.Logo != null)
					{
						ImageHelper.ResizeByWith(Logo.InputStream, SiteConfiguration.LogoMaxWidth, SiteConfiguration.GetApplicationRootConfigurationPath(String.Format(SiteConfiguration.AgencyLogoPath, agency.Tag)));
					}
					#endregion

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

					#region Save File
					if (agency.Logo != null)
					{
						ImageHelper.ResizeByWith(Logo.InputStream, SiteConfiguration.LogoMaxWidth, SiteConfiguration.GetApplicationRootConfigurationPath(String.Format(SiteConfiguration.AgencyLogoPath, agency.Tag)));
					}
					#endregion

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
	}
}
