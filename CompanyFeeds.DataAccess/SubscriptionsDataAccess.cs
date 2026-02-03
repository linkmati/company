using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CompanyFeeds.DataAccess
{
	internal class SubscriptionsDataAccess : BaseDataAccess
	{
		public void Add(int userId, int companyId)
		{
			SqlCommand comm = new SqlCommand("SPUsersSubscriptionsInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)companyId);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			this.SafeExecuteNonQuery(comm);
		}

		public void Remove(int userId, int companyId)
		{
			SqlCommand comm = new SqlCommand("SPUsersSubscriptionsDelete", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)companyId);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			this.SafeExecuteNonQuery(comm);
		}
	}
}
