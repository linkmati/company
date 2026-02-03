using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using CompanyFeeds.Configuration;
using CompanyFeeds.DataAccess.Queries;
using System.Collections;
using System.Web;

namespace CompanyFeeds.Web.State
{
	public class CacheWrapper
	{
		public Cache Cache
		{
			get;
			set;
		}

		/// <summary>
		/// Duration in seconds of the home entries in the cache.
		/// </summary>
		public int HomeEntriesDuration
		{
			get;
			set;
		}

		/// <summary>
		/// Duration in seconds of the IpCompanyVisits dictionary.
		/// </summary>
		public int IpCompanyVisitsDuration
		{
			get;
			set;
		}

		public CacheWrapper(Cache cache)
		{
			Cache = cache;
			HomeEntriesDuration = 1200;
			IpCompanyVisitsDuration = 1200;
		}

		public CacheWrapper(HttpContextBase httpContext)
			: this(httpContext.Cache)
		{

		}

		public T GetItem<T>(string key)
		{
			return (T)Cache[key];
		}

		public T GetItem<T>(string key, bool create) where T : new()
		{
			
			T value = (T)Cache[key];
			if (create && value == null)
			{
				value = new T();
				Cache[key] = value;
			}
			return value;
		}

		private void SetItem<T>(string key, T value)
		{
			SetItem<T>(key, value, 0);
		}

		/// <summary>
		/// Sets an item in the cache
		/// </summary>
		/// <param name="duration">duration in seconds of the item in cache</param>
		private void SetItem<T>(string key, T value, int duration)
		{
			if (duration == 0)
			{
				if (value != null)
				{
					Cache[key] = value;
				}
				else
				{
					Cache.Remove(key);
				}
			}
			else
			{
				Cache.Add(key, value, null, DateTime.Now.Add(TimeSpan.FromSeconds(duration)), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
			}

		}

		public List<string> NotCachedUrls
		{
			get
			{
				return GetItem<List<string>>("NotCachedUrls", true);
			}
			set
			{
				SetItem<List<string>>("NotCachedUrls", value);
			}
		}

		public SiteConfiguration SiteConfiguration
		{
			get
			{
				return GetItem<SiteConfiguration>("SiteConfiguration");
			}
			set
			{
				SetItem<SiteConfiguration>("SiteConfiguration", value);
			}
		}

		public RouteMappingConfiguration RouteMappingConfiguration
		{
			get
			{
				return GetItem<RouteMappingConfiguration>("RouteMappingConfiguration");
			}
			set
			{
				SetItem<RouteMappingConfiguration>("RouteMappingConfiguration", value);
			}
		}

		public CategoriesQueries.CategoriesDataTable Categories
		{
			get
			{
				return GetItem<CategoriesQueries.CategoriesDataTable>("Categories");
			}
			set
			{
				SetItem<CategoriesQueries.CategoriesDataTable>("Categories", value);
			}
		}

		public Dictionary<string, Dictionary<string, int>> IpFilters
		{
			get
			{
				return GetItem<Dictionary<string, Dictionary<string, int>>>("IpFilters", true);
			}
			set
			{
				SetItem<Dictionary<string, Dictionary<string, int>>>("IpFilters", value);
			}
		}

		public SortedList<string, KeyValuePair<int, string>> CompaniesSorted
		{
			get
			{
				return GetItem<SortedList<string, KeyValuePair<int, string>>>("CompaniesSorted");
			}
			set
			{
				SetItem<SortedList<string, KeyValuePair<int, string>>>("CompaniesSorted", value);
			}
		}

		public EntriesQueries.EntriesListDataTable HomeEntries
		{
			get
			{
				return GetItem<EntriesQueries.EntriesListDataTable>("HomeEntries");
			}
			set
			{
				SetItem<EntriesQueries.EntriesListDataTable>("HomeEntries", value, HomeEntriesDuration);
			}
		}


		public HashSet<string> HostBlackList
		{
			get
			{
				return GetItem<HashSet<string>>("HostBlackList");
			}
			set
			{
                SetItem("HostBlackList", value);
			}
		}

//		public Dictionary<string, List<int>> IpCompanyVisits
//		{
//			get
//			{
//				Dictionary<string, List<int>> result = GetItem<Dictionary<string, List<int>>>("IpCompanyVisits");
//				if (result == null)
//				{
//					SetItem<Dictionary<string, List<int>>>("IpCompanyVisits", new Dictionary<string,List<int>>(), IpCompanyVisitsDuration);
//					result = GetItem<Dictionary<string, List<int>>>("IpCompanyVisits");
//				}
//				return result;
//			}
//			set
//			{
//				SetItem<Dictionary<string, List<int>>>("IpCompanyVisits", value, IpCompanyVisitsDuration);
//			}
//		}

		/// <summary>
		/// Gets an instanciated dictionary of string keys (for ip+action) and int value (for count)
		/// </summary>
		protected Dictionary<string, int> UserActionCount
		{
			get
			{
				return GetItem<Dictionary<string, int>>("UserActionCount", true);
			}
		}

		/// <summary>
		/// Removes all the items from the cache
		/// </summary>
		public void Clear()
		{
			List<string> keys = new List<string>(Cache.Count);
			foreach (DictionaryEntry entry in Cache)
			{
				keys.Add(entry.Key.ToString());
			}

			foreach (string key in keys)
			{
				Cache.Remove(key);
			}
		}

		/// <summary>
		/// Sets the user count for the action +1
		/// </summary>
		/// <param name="actionKey">key identifying the action</param>
		public void SetUserActionCount(string ip, string actionKey)
		{
			var userCounts = UserActionCount;
			string userActionKey = ip + actionKey;

			userCounts[userActionKey] = GetUserActionCount(ip, actionKey) + 1;
		}

		/// <summary>
		/// Gets the amount of times the user successfully executed an action
		/// </summary>
		/// <param name="actionKey">key identifying the action</param>
		public int GetUserActionCount(string ip, string actionKey)
		{
			var userCounts = UserActionCount;
			string userActionKey = ip + actionKey;
			if (!userCounts.ContainsKey(userActionKey))
			{
				userCounts.Add(userActionKey, 0);
			}
			return userCounts[userActionKey];
		}
	}
}
