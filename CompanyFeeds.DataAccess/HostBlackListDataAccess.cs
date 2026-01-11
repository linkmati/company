using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CompanyFeeds.DataAccess;

namespace CompanyFeeds.DataAccess
{
	internal class HostBlackListDataAccess : BaseDataAccess
	{
		internal List<string> GetAll()
		{
			var hosts = new List<string>();
			SqlCommand comm = new SqlCommand("SPHostBlackListGetAll", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;

			DataTable dt = this.GetTable(comm);

			foreach (DataRow dr in dt.Rows)
			{
				hosts.Add(dr.GetString("HostName"));
			}

			return hosts;
		}

		internal void Add(List<string> hosts)
		{
			foreach (string hostName in hosts)
			{
				SqlCommand comm = new SqlCommand("SPHostBlackListInsert", GetConnection());
				comm.CommandType = CommandType.StoredProcedure;
				comm.Parameters.Add("@HostName", SqlDbType.VarChar, hostName);

				this.SafeExecuteNonQuery(comm);
			}
		}
	}
}
