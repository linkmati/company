using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Web.State;
using CompanyFeeds.Services;
using System.Web;

namespace CompanyFeeds.Web.Helpers
{
	public static class CompaniesHelper
	{
		public static bool RegisterVisit(int companyId, CacheWrapper cache, string ip)
		{
			//Check if the ip is not in the ip/company collection
		    return false;
//			if (!cache.IpCompanyVisits.ContainsKey(ip))
//			{
//				cache.IpCompanyVisits[ip] = new List<int>();
//			}
//			if (!cache.IpCompanyVisits[ip].Contains(companyId))
//			{
//				cache.IpCompanyVisits[ip].Add(companyId);
//				//Save through service.
//				CompaniesService.AddVisit(companyId);
//
//				return true;
//			}
//			else
//			{
//				return false;
//			}
		}

		/// <summary>
		/// Gets the full list of companies sorted by name in lowercase (cached).
		/// </summary>
		private static SortedList<string, KeyValuePair<int, string>> CompaniesSorted
		{
			get
			{
				var cache = new CacheWrapper(HttpRuntime.Cache);
				if (cache.CompaniesSorted == null)
				{
					cache.CompaniesSorted = CompaniesService.GetCompaniesSorted();
				}
				return cache.CompaniesSorted;
			}
		}

		/// <summary>
		/// Clears the list of cached companies
		/// </summary>
		public static void RefreshFullList()
		{
			var cache = new CacheWrapper(HttpRuntime.Cache);
			//clear
			cache.CompaniesSorted = null;

			//force get
			var list = cache.CompaniesSorted;
		}

		/// <summary>
		/// Searches companies starting with the value passed in the params.
		/// </summary>
		public static List<KeyValuePair<int, string>> SearchByName(string nameStartsWith)
		{
			#region Check args
			if (nameStartsWith == null)
			{
				throw new ArgumentNullException("nameStartsWith");
			}
			if (nameStartsWith.Trim() == "")
			{
				throw new ArgumentException("Company name cannot be empty");
			} 
			#endregion

			var fullList = CompaniesSorted;

			var values = fullList.Where(x => x.Key.StartsWith(nameStartsWith.Trim().ToLower())).ToDictionary(x => x.Key, x => x.Value).Values.ToList();

			return values;
		}

		/// <summary>
		/// Gets a company by the name (cached). 0 if not exists.
		/// </summary>
		/// <returns></returns>
		public static int GetCompanyId(string name)
		{
			#region Check args
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Trim() == "")
			{
				throw new ArgumentException("Company name cannot be empty");
			} 
			#endregion

			int companyId = 0;
			var fullList = CompaniesSorted;
			if (fullList.ContainsKey(name.Trim().ToLower()))
			{
				companyId = fullList[name.Trim().ToLower()].Key;
			}
			return companyId;
		}

		/// <summary>
		/// Determines if a company is closed for adding new entries or edit the detail
		/// </summary>
		public static bool IsClosed(int id, int userId, bool isAdmin)
		{
			var isClosed = false;
			if ((!isAdmin) && id > 0)
			{
				isClosed = CompaniesService.IsClosed(id, userId);
			}
			return isClosed;
		}

		/// <summary>
		/// Determines if a company is closed for adding new entries or edit the detail
		/// </summary>
		public static bool IsClosed(string tag, int userId, bool isAdmin)
		{
			bool isClosed = false;
			if ((!isAdmin) && (!String.IsNullOrEmpty(tag)))
			{
				isClosed = CompaniesService.IsClosed(tag, userId);
			}

			return isClosed;
		}
	}
}
