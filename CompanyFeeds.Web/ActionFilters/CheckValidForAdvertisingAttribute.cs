using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using CompanyFeeds.DataAccess.Queries;

namespace CompanyFeeds.Web.ActionFilters
{
	public class CheckValidForAdvertisingAttribute : ActionFilterAttribute
	{
		protected virtual string RegexPattern
		{
			get
			{
				return "sex|escort|\\bcum\\b|drug|porn|video|lady|girl|boy|watch|pleasure|breast";
			}
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var model = filterContext.Controller.ViewData.Model;
			var textList = new List<string>();
			if (model is EntriesQueries.EntriesDetailRow)
			{
				textList.Add(((EntriesQueries.EntriesDetailRow)model).EntryTitle);
			}
			else if (model is CompaniesQueries.CompaniesDetailRow)
			{
				textList.Add(((CompaniesQueries.CompaniesDetailRow)model).CompanyName);
			}

			if (!AreTextsValid(textList))
			{
				filterContext.Controller.ViewData["HideAdvertising"] = true;
			}
			base.OnActionExecuted(filterContext);
		}

		protected virtual bool AreTextsValid(List<string> textList)
		{
			bool isValid = true;
			foreach (var text in textList)
			{
				if (Regex.IsMatch(text, RegexPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase))
				{
					isValid = false;
					break;
				}
			}
			return isValid;
		}
	}
}
