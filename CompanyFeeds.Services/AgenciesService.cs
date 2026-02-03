using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess;
using CompanyFeeds.Validation;
using CompanyFeeds.DataAccess.Queries.AgenciesQueriesTableAdapters;

namespace CompanyFeeds.Services
{
	public static class AgenciesService
	{
		public static void Add(Agency agency)
		{
			agency.ValidateFields();
			if (!IsTagAvailable(agency.Id, agency.Tag))
			{
				throw new ValidationException(new List<ValidationError>() { new ValidationError("Tag", ValidationErrorType.DuplicateNotAllowed) });
			}

			AgenciesDataAccess da = new AgenciesDataAccess();
			da.Add(agency); 
		}

		/// <summary>
		/// Determines if a agency tag is not been used by another agency.
		/// </summary>
		/// <param name="id">current id, if new use 0</param>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static bool IsTagAvailable(int id, string tag)
		{
			return AgencyNamesTableAdapter.IsTagAvailable(tag, id);
		}

		public static void Update(Agency agency)
		{
			agency.ValidateFields();

			AgenciesDataAccess da = new AgenciesDataAccess();
			da.Update(agency);
		}

		public static Agency Get(string tag)
		{
			var da = new AgenciesDataAccess();
			return da.Get(tag);
		}
	}
}
