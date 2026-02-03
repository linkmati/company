using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Xml;
using System.Collections;

namespace CompanyFeeds.Web.ActionResults
{
	public class RssResult : ActionResult
	{
		public string Title
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public IEnumerable DataSource
		{
			get;
			set;
		}

		public string ItemTitleProperty
		{
			get;
			set;
		}

		public string ItemUrlProperty
		{
			get;
			set;
		}

		public string ItemDescriptionProperty
		{
			get;
			set;
		}

		public string ItemDateProperty
		{
			get;
			set;
		}

		public UrlGetHandler UrlGetHandler
		{
			get;
			set;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "text/xml";
			

			XmlTextWriter writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8);

			WriteRss(writer);

			writer.Close();
			

			response.Write("                                                                 ");

		}

		public void WriteRss(XmlTextWriter writer)
		{
			writer.Formatting = Formatting.Indented;

			writer.WriteStartDocument();

			writer.WriteStartElement("rss");

			writer.WriteAttributeString("version", "2.0");

			writer.WriteStartElement("channel");
			writer.WriteElementString("title", Title);
			writer.WriteElementString("link", Url);
			writer.WriteElementString("description", Description);

			if (DataSource != null)
			{
				WriteRssItems(writer);
			}

			writer.WriteEndElement();
			writer.WriteWhitespace(Environment.NewLine);

			writer.WriteFullEndElement();


		}

		public void WriteRssItems(XmlTextWriter writer)
		{
			foreach (object item in DataSource)
			{
				string url = null;
				if (ItemUrlProperty != null)
				{
					Uri baseUri = new Uri(this.Url);
					url = baseUri.Scheme + Uri.SchemeDelimiter + baseUri.Host + "/" + ReflectionUtils.GetPropertyValue<object>(item, ItemUrlProperty).ToString();
				}
				else
				{
					url = UrlGetHandler(item);
				}

				AddRSSItem(
					writer,
					ReflectionUtils.GetPropertyValue<string>(item, ItemTitleProperty),
					url,
					ReflectionUtils.GetPropertyValue<string>(item, ItemDescriptionProperty),
					ReflectionUtils.GetPropertyValue<DateTime>(item, ItemDateProperty));
			}
		}

		public void AddRSSItem(XmlTextWriter writer, string title, string url, string description, DateTime date)
		{
			writer.WriteStartElement("item");
			writer.WriteElementString("title", title);
			writer.WriteElementString("link", url);
			writer.WriteElementString("guid", url);
			writer.WriteElementString("description", description);
			writer.WriteElementString("pubDate", date.ToString("r"));
			writer.WriteEndElement();
		}
	}
}
