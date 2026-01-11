using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CompanyFeeds.DataAccess.Queries;
using System.Xml;
using CompanyFeeds.Services;

namespace CompanyFeeds.FeedClient
{
	public class RssParser
	{
		#region ParseRss
		/// <summary>
		/// Parses the rss stream and returns the total items saved
		/// </summary>
		/// <param name="encoding">The Encoding of the stream, as an Output parameter</param>
		public List<FeedEntry> Parse(Stream feedStream)
		{
			var doc = new XmlDocument();
			var reader = new XmlTextReader(feedStream);
			var entriesList = new List<FeedEntry>();

			int itemDepth = 0;

			while (reader.Read())
			{
				Encoding = reader.Encoding;
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name.Equals("item"))
					{
						itemDepth = reader.Depth;

						var entry = new FeedEntry();

						reader.Read();

						while (itemDepth != reader.Depth) //check greater than
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								if (reader.Name == "title")
								{
									entry.Title = reader.ReadString();
								}
								else if (reader.Name == "guid")
								{
									string tempUrl = reader.ReadString();
									if (Util.ValidateUrl(tempUrl))
									{
										entry.Guid = tempUrl;
									}
								}
								else if (reader.Name == "pubDate")
								{
									try
									{
										entry.Date = W3CDateTime.Parse(reader.ReadString()).LocalDateTime;
									}
									catch
									{
										entry.Date = DateTime.Now;
									}
								}
								else if (reader.Name == "description")
								{
									entry.Description = reader.ReadString();
								}
								else if (reader.Name == "content:encoded")
								{
									entry.Content = reader.ReadString();
								}
								else if (reader.Name == "feedburner:origLink")
								{
									entry.OriginalGuid = reader.ReadString();
								}
								else if (reader.Name == "link" && entry.Guid == null)
								{
									entry.Guid = reader.ReadString();
								}
							}

							//advance the reader one node
							reader.Read();
						}
						entriesList.Add(entry);
					}
				}
			}
			reader.Close();
			feedStream.Close();

			return entriesList;
		}
		#endregion

		/// <summary>
		/// Read encoding
		/// </summary>
		public Encoding Encoding 
		{ 
			get; 
			set; 
		}
	}
}
