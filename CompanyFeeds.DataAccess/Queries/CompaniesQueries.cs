namespace CompanyFeeds.DataAccess.Queries {
    
    
    public partial class CompaniesQueries {
		partial class CompaniesDetailDataTable
		{

		}

		partial class CompaniesDetailRow
		{
			/// <summary>
			/// Gets or sets the list of entries of the company
			/// </summary>
			public EntriesQueries.EntriesListBasicDataTable Entries { get; set; }
		}
	}
}

namespace CompanyFeeds.DataAccess.Queries.CompaniesQueriesTableAdapters
{
    internal partial class CompanyNamesTableAdapter 
	{
		internal static bool IsTagAvailable(string tag, int id)
		{
			CompanyNamesTableAdapter ta = new CompanyNamesTableAdapter();
			return (ta.GetCheckDuplicate(id, tag).Count == 0);
		}
    }
}
