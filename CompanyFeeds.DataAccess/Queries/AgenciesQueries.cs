namespace CompanyFeeds.DataAccess.Queries {
    
    
    public partial class AgenciesQueries {
    }
}

namespace CompanyFeeds.DataAccess.Queries.AgenciesQueriesTableAdapters
{
    internal partial class AgencyNamesTableAdapter
	{
		internal static bool IsTagAvailable(string tag, int id)
		{
			AgencyNamesTableAdapter ta = new AgencyNamesTableAdapter();
			return (ta.GetCheckDuplicate(id, tag).Count == 0);
		}
    }
}
