using System;
using System.Collections.Generic;
using System.Linq;
using CompanyFeeds.DataAccess.Queries.EntriesQueriesTableAdapters;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess;
using System.Text;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Services
{
	public static class EntriesService
	{

		public static EntriesQueries.EntriesListDataTable GetByCategory(int categoryId)
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetByCategory(categoryId);
		}

		public static EntriesQueries.EntriesListDataTable GetByUser(int userId, int top)
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetByUser(userId, top);
		}

		/// <summary>
		/// Gets entries by company, paged results.
		/// </summary>
		/// <returns>Returns null if the page is greater than total pages</returns>
		public static EntriesQueries.EntriesListBasicDataTable GetByCompany(int companyId, int page, SiteConfiguration config)
		{
			var ta = new EntriesListBasicTableAdapter();
			var rowNumberFrom = page * config.CompanyDetailPageSize + 1;
			var rowNumberTo = (page+1) * config.CompanyDetailPageSize;
			int? totalItems = null;
			var dt = ta.GetByCompany(companyId, rowNumberFrom, rowNumberTo, ref totalItems);
			if (totalItems != null)
			{
				dt.TotalCount = totalItems.Value;
			}
			//check if the page is greater than total pages
			if (page > 0 && dt.TotalCount <= page * config.CompanyDetailPageSize)
			{
				dt = null;
			}

			return dt;
		}

		/// <summary>
		/// Gets a full detail of an entry
		/// </summary>
		public static EntriesQueries.EntriesDetailRow GetDetail(int entryId, string tag)
		{
			EntriesDetailTableAdapter ta = new EntriesDetailTableAdapter();
			EntriesQueries.EntriesDetailDataTable dt = ta.GetByEntry(entryId, tag);
			if (dt.Count > 0)
			{
				return dt[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets an entry
		/// </summary>
		public static Entry Get(int id)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.Get(id);
		}

		/// <summary>
		/// Adds a new entry
		/// </summary>
		/// <exception cref="ValidationException">Throws this exception when the fields are not properly set in order to save.</exception>  
		public static void AddEntry(Entry entry, string ipAddress)
		{
			entry.ValidateFields();

			var da = new EntriesDataAccess();
			da.AddEntry(entry, ipAddress);
		}

		/// <summary>
		/// Add a flag (report abuse) to a given entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="userId"></param>
		public static void AddFlag(int entryId, int? userId)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			da.AddFlag(entryId, userId);
		}

		public static void Update(Entry entry, string ipAddress)
		{
			entry.ValidateFields();

			EntriesDataAccess da = new EntriesDataAccess();
			da.Update(entry, ipAddress);
		}

		/// <summary>
		/// Determines if the entry is ready to be saved.
		/// </summary>
		/// <exception cref="ValidationException">Throws this exception when the fields are not properly set in order to save.</exception>  
		public static void IsPreparedToSave(Entry entry)
		{
			entry.ValidateFields();
		}

		/// <summary>
		/// Get the latest entries
		/// </summary>
		/// <param name="max">Max number of entries</param>
		public static EntriesQueries.EntriesListDataTable GetLatest(int max)
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetLatest(max);
		}

		/// <summary>
		/// Gets latest entries by User
		/// </summary>
		/// <returns></returns>
		public static List<Entry> GetByUser(int userId)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.GetByUser(userId);
		}

		/// <summary>
		/// Gets a list of entries that match word blacklist
		/// </summary>
		/// <returns></returns>
		public static List<Entry> GetSuspected()
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.GetSuspected();
		}

		/// <summary>
		/// Gets a list of entries that match word blacklist
		/// </summary>
		/// <returns></returns>
		/// <param name="id">From the last entry id checked</param>
		public static List<Entry> GetSuspected(int? id, bool descendingOrder)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.GetSuspected(id, descendingOrder);
		}

		/// <summary>
		/// Gets latest entries by Agency
		/// </summary>
		/// <returns></returns>
		public static List<Entry> GetByAgency(int agencyId)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.GetByAgency(agencyId);
		}

		/// <summary>
		/// Get the most relevant entries.
		/// The max number is defined by the database.
		/// </summary>
		/// <returns></returns>
		public static EntriesQueries.EntriesListDataTable GetRelevant()
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetRelevant();
		}

		/// <summary>
		/// Gets the entries of an specific date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static EntriesQueries.EntriesListDataTable GetByDate(DateTime date)
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetByDate(date);
		}

		/// <summary>
		/// Gets the entries of an specific date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static EntriesQueries.EntriesListDataTable GetByIdList(string list)
		{
			EntriesListTableAdapter ta = new EntriesListTableAdapter();
			return ta.GetByIdList(list);
		}

		/// <summary>
		/// Determines if an entry from a companyn and with an specific external Guid already exist
		/// </summary>
		public static bool ExistEntry(int companyId, string externalGuid)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.Exist(externalGuid, companyId);
		}

		/// <summary>
		/// Deletes an entry from the application
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(int id)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			da.Delete(id);
		}

		/// <summary>
		/// deletes all entries submitted by a user (and the companies submitted that are now empty), returns the entries deleted
		/// </summary>
		/// <param name="id"></param>
		public static int DeleteByUser(int userId)
		{
			var da = new EntriesDataAccess();
			return da.DeleteByUser(userId);
		}

		/// <summary>
		/// Gets latest entries submitted from an IP
		/// </summary>
		public static List<Entry> GetByIp(string ip)
		{
			EntriesDataAccess da = new EntriesDataAccess();
			return da.GetByIp(ip);
		}

		#region Check and save rss
		/// <summary>
		/// Adds the valid
		/// </summary>
		/// <param name="companyId">id of the company of the entry</param>
		/// <param name="ownerId">Id of the user owner of the new entry</param>
		/// <returns></returns>
		public static int AddFeedEntries(List<FeedEntry> entriesList, int companyId, string companyName, string defaultTeaser, int? ownerId, Encoding encoding)
		{
			var totalSaved = 0;
			foreach (var entry in entriesList)
			{
				totalSaved += AddFeedEntry(entry, companyId, companyName, defaultTeaser, ownerId, encoding);
			}
			return totalSaved;
		}

		/// <summary>
		/// Adds a new entry, if valid.
		/// </summary>
		/// <param name="feedEntry"></param>
		/// <param name="companyId">id of the company of the entry</param>
		/// <param name="companyName"></param>
		/// <param name="defaultTeaser"></param>
		/// <param name="ownerId">Id of the user owner of the new entry</param>
		/// <returns></returns>
		public static int AddFeedEntry(FeedEntry feedEntry, int companyId, string companyName, string defaultTeaser, int? ownerId, Encoding encoding)
		{
			if (feedEntry.Title == null || feedEntry.Title.Length < 4)
			{
				return 0;
			}
			#region Check encoding
			if (!String.IsNullOrEmpty(feedEntry.Description))
			{
				if (encoding.GetBytes(feedEntry.Description).Length != feedEntry.Description.Length)
				{
					return 0;
				}
			}
			#endregion

			if (EntriesService.ExistEntry(companyId, feedEntry.Guid))
			{
				return 0;
			}
			if (feedEntry.OriginalGuid != null)
			{
				//Third party services like feedburner uses an original guid.
				if (EntriesService.ExistEntry(companyId, feedEntry.OriginalGuid))
				{
					return 0;
				}
				feedEntry.Guid = feedEntry.OriginalGuid;
			}


			Entry entry = new Entry();
			entry.CompanyId = companyId;
			entry.CompanyName = companyName;
			entry.UserId = null;
			entry.EntryTitle = TextUtils.Summarize(feedEntry.Title, 256, "");
			entry.ExternalGuid = feedEntry.Guid;
			entry.Source = feedEntry.Guid;
			entry.Tag = UrlUtils.ToUrlTag(entry.EntryTitle, 128);
			entry.Content = feedEntry.Content;
			entry.Date = feedEntry.Date;
			entry.UserId = ownerId;

			if (!TextUtils.IsNullOrEmpty(feedEntry.Description))
			{
				entry.Teaser = TextUtils.SummarizeHtml(feedEntry.Description, 509, "...");
				#region Sometimes html is embed in the description element
				if (entry.Content == null && TextUtils.IsHtmlFragment(feedEntry.Description))
				{
					entry.Content = feedEntry.Description;
				}
				#endregion
			}
			if (TextUtils.IsNullOrEmpty(entry.Teaser))
			{
				entry.Teaser = String.Format(defaultTeaser, companyName, entry.EntryTitle);
			}

			#region Date (sometimes the date comes wrong)
			if (DateTime.Now.Subtract(entry.Date) > TimeSpan.FromDays(365 * 5))
			{
				entry.Date = DateTime.Now;
			}
			#endregion

			EntriesService.AddEntry(entry, "127.0.0.1");

			return 1;
		}
		#endregion
	}
}
