using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CompanyFeeds.Web.ActionFilters;
using CompanyFeeds.Services;
using CompanyFeeds.Web.Helpers;
using System.Web;

namespace CompanyFeeds.Web.Controllers
{
	public class CommentsController : BaseController
	{
		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(true)]
		public JsonResult Add(int entryId, string value, bool notify, string name, string email, bool? sync)
		{
			User user = null;
			if (this.User != null)
			{
				user = new User(this.User.Id, null, this.User.Email);
			}
			else
			{
				user = new User(0, name, email);
			}

			Comment comment = new Comment(0, TextUtils.TextToHtmlFragment(value), user, notify);
			CommentsService.Add(comment, entryId, Request.UserHostAddress);

			#region Notify users
			Entry entry = EntriesService.Get(entryId);
			string url = Domain + UrlExtensions.GenerateFormattedUrl("Detail", "Entries", new {id = entry.Id,tag = entry.Tag,companyTag=entry.CompanyTag}, this.Url.RouteCollection, this.ControllerContext.RequestContext);

			string unsubscribeUrl = this.Domain + HttpUtility.UrlDecode(this.Url.RouteUrl(new {action = "Unsubscribe", controller = "Comments", id = "{0}", uid = "{1}", email = "{2}"}));

			if (sync == true)
			{
				NotificationsService.NotifyComments(entry, user, url, unsubscribeUrl, SiteConfiguration);
			}
			else
			{
				NotificationsService.NotifyCommentsAsync(entry, user, url, unsubscribeUrl, SiteConfiguration);
			} 
			#endregion

			return Json("OK");
		}

		[ValidateInput(true)]
		public ActionResult Unsubscribe(int id, string email, int uid)
		{
			CommentsService.Unsubscribe(id, email, uid);
			Entry entry = EntriesService.Get(id);
			return View(entry);
		}

		public ActionResult ListLatest()
		{
			List<Comment> comments = CommentsService.GetLatest();
			return Rss("Last comments " + this.Domain, this.Domain, "", comments, "User.Email", "Entry.Id", "Value", "Date", null);
		}
	}
}
