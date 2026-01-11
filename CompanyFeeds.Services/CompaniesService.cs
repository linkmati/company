using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.CompaniesQueriesTableAdapters;
using CompanyFeeds.Validation;
using CompanyFeeds.Configuration;

namespace CompanyFeeds.Services
{
	public class CompaniesService
	{
		/// <summary>
		/// Get all companies basic info ordered alphabetically
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public static CompaniesQueries.CompanyNamesDataTable GetCompanies()
		{
			CompanyNamesTableAdapter ta = new CompanyNamesTableAdapter();
			return ta.GetData();
		}

		[Obsolete]
		public static Company GetCompany(string name)
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			Company company = da.GetByName(name);
			return company;
		}

		public static Company Get(string tag)
		{
			var da = new CompaniesDataAccess();
			return da.Get(tag);
		}

		public static Company Get(int id)
		{
			var da = new CompaniesDataAccess();
			return da.Get(id);
		}

		/// <summary>
		/// Gets a company detail and its entries (paged)
		/// </summary>
		/// <returns>Returns the company containing the entries. Returns null if the company is not found or if the page is greater than the amount of pages</returns>
		public static CompaniesQueries.CompaniesDetailRow GetDetailByTag(string tag, int page, SiteConfiguration config)
		{
			CompaniesQueries.CompaniesDetailRow companyDetail = null;
			CompaniesDetailTableAdapter ta = new CompaniesDetailTableAdapter();
			CompaniesQueries.CompaniesDetailDataTable dt = ta.GetByTag(tag);
			if (dt.Count > 0)
			{
				companyDetail = dt[0];
				companyDetail.Entries = EntriesService.GetByCompany(companyDetail.CompanyId, page, config);
				if (companyDetail.Entries == null)
				{
					//the page is greater than total amount of pages.
					companyDetail = null;
				}
			}
			return companyDetail;
		}

		/// <summary>
		/// Determines if a company tag is not been used by another company.
		/// </summary>
		/// <param name="id">current id, if new use 0</param>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static bool IsTagAvailable(int id, string tag)
		{
			return CompanyNamesTableAdapter.IsTagAvailable(tag, id);
			
		}

		/// <summary>
		/// Saves the new company in the database.
		/// </summary>
		/// <exception cref="ValidationException">Throws this exception when the fields are not properly set in order to save.</exception>  
		public static void Add(Company company, string ipAddress, int userId)
		{
			company.ValidateFields();
			if (!IsTagAvailable(company.Id, company.Tag))
			{
				throw new ValidationException(new List<ValidationError>() { new ValidationError("Tag", ValidationErrorType.DuplicateNotAllowed) });
			}

			CompaniesDataAccess da = new CompaniesDataAccess();
			da.Add(company, ipAddress, userId);
		}

		/// <summary>
		/// Updates a company in the Db. Uses the companyTag for key.
		/// </summary>
		public static void Update(Company company, string ipAddress, int userId)
		{
			company.ValidateFields();

			CompaniesDataAccess da = new CompaniesDataAccess();
			da.Update(company, ipAddress, userId);
		}

		public static List<Company> GetCompaniesFeeds()
		{
			var da = new CompaniesDataAccess();
			return da.GetWithFeeds();
		}

		public static void AddVisit(int companyId)
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			da.AddVisit(companyId);
		}

		public static void SetNoVisits(int companyId)
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			da.SetNoVisits(companyId);
		}

		public static SortedList<string, KeyValuePair<int, string>> GetCompaniesSorted()
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			return da.GetCompaniesSorted();
		}

		/// <summary>
		/// Gets a list of most relevant companies alpha' sorted
		/// </summary>
		/// <returns></returns>
		public static List<Company> GetTopCompanies()
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			return da.GetTopCompanies();
		}

		/// <summary>
		/// Removes the company and all of its entries
		/// </summary>
		public static void Delete(int id)
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			da.Delete(id);
		}

		public static List<Company> GetByIp(string ip)
		{
			CompaniesDataAccess da = new CompaniesDataAccess();
			return da.GetByIp(ip);
		}

		/// <summary>
		/// Determines if a company is closed for adding new entries or edit the detail, for a given userId
		/// </summary>
		/// <param name="id">company id</param>
		public static bool IsClosed(int id, int userId)
		{
			var da = new CompaniesDataAccess();
			return da.IsClosed(id, null, userId);
		}

		/// <summary>
		/// Determines if a company is closed for adding new entries or edit the detail, for a given userId
		/// </summary>
		/// <param name="tag">company tag</param>
		public static bool IsClosed(string tag, int userId)
		{
			var da = new CompaniesDataAccess();
			return da.IsClosed(null, tag, userId);
		}
	}
}
