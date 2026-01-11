using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System;
using System.Web;
using CompanyFeeds.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;

namespace CompanyFeeds.Web.Helpers
{
	public static class HtmlExtensions
	{
		public static string ActionLinkFormatted(this HtmlHelper htmlHelper, string linkText, string actionName)
		{
			string controllerName = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			return htmlHelper.ActionLinkFormatted(linkText, null, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
		}

		public static string ActionLinkFormatted(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object values, object htmlAttributes)
		{
			return htmlHelper.ActionLinkFormatted(linkText, null, actionName, controllerName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
		}

		public static string ActionLinkFormatted(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
		{
			return htmlHelper.ActionLinkFormatted(linkText, null, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
		}

		public static string ActionLinkFormatted(this HtmlHelper htmlHelper, string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary values, RouteValueDictionary htmlAttributes)
		{
			string str = UrlExtensions.GenerateFormattedUrl(routeName, actionName, controllerName, values, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext);
			TagBuilder builder2 = new TagBuilder("a");
			builder2.InnerHtml = htmlHelper.Encode(linkText);
			TagBuilder builder = builder2;
			builder.MergeAttributes<string, object>(htmlAttributes);
			builder.MergeAttribute("href", str);
			return builder.ToString(TagRenderMode.Normal);
		}

		public static string ImageStatic(this HtmlHelper htmlHelper, string src, string css)
		{
			if (src == null)
			{
				throw new NullReferenceException("The src cannot be null.");
			}
			else if (!src.StartsWith("/"))
			{
				throw new ArgumentException("The src should be absolute (starting with /)");
			}
			TagBuilder builder = new TagBuilder("img");
			if (css != null)
			{
				builder.AddCssClass(css);
			}
			builder.Attributes.Add("src", src);

			return builder.ToString();
		}

		public static string TextBox(this HtmlHelper htmlHelper, string name, string format, Type type, object htmlAttributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("Name cannot be null.");
			}
			string value = htmlHelper.GetModelAttemptedValue(name);
			if (value == null)
			{
				value = htmlHelper.EvalString(name);
			}
			if (!String.IsNullOrEmpty(value)) 
			{
				try
				{
					value = String.Format(format, Convert.ChangeType(value, type));
				}
				catch
				{
				}
			}

			return htmlHelper.TextBox(name, value, new RouteValueDictionary(htmlAttributes));
		}

		public static string GetModelAttemptedValue(this HtmlHelper htmlHelper, string key)
		{
			ModelState state;
			if (htmlHelper.ViewData.ModelState.TryGetValue(key, out state))
			{
				return state.Value.AttemptedValue;
			}
			return null;
		}

		public static string EvalString(this HtmlHelper htmlHelper, string key)
		{
			return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
		}

		public static string MetaDescription(this HtmlHelper htmlHelper, string content)
		{
			if (content != null)
			{
				TagBuilder builder = new TagBuilder("meta");
				builder.Attributes.Add("content", Regex.Replace(SecurityElement.Escape(content), "\r|\n", ""));
				builder.Attributes.Add("name", "description");

				return builder.ToString(TagRenderMode.SelfClosing);
			}
			else
			{
				return null;
			}
		}

		public static string DropDownListDefault(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object defaultValue, string defaultText)
		{
			List<SelectListItem> list = new List<SelectListItem>(selectList);
			SelectListItem item = new SelectListItem();
			item.Text = defaultText;
			item.Value = defaultValue.ToString();
			list.Insert(0, item);
			return htmlHelper.DropDownList(name, list);
		}

		public static void RenderPartialIf(this HtmlHelper htmlHelper, bool condition, string partialViewName, object model)
		{
			if (condition)
			{
				htmlHelper.RenderPartial(partialViewName, model);
			}
		}

		public static void RenderPartialIf(this HtmlHelper htmlHelper, bool condition, string partialViewName)
		{
			htmlHelper.RenderPartialIf(condition, partialViewName, null);
		}
	}
}
