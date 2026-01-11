using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using CompanyFeeds.DataAccess;

namespace CompanyFeeds.DataAccess
{
	internal class CompaniesDataAccess : BaseDataAccess
	{
		internal Company GetByName(string companyName)
		{
			Company company = null;
			SqlCommand comm = new SqlCommand("SPCompaniesGetByName", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyName", SqlDbType.VarChar, companyName);

			DataTable dt = this.GetTable(comm);

			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				company = new Company();
				company.Name = dr["CompanyName"].ToString();
				company.Id = Convert.ToInt32(dr["CompanyId"].ToString());
				company.Tag = dr["CompanyTag"].ToString();
			}

			return company;
		}

		internal Company Get(string tag)
		{
			Company company = null;
			SqlCommand comm = new SqlCommand("SPCompaniesGet", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyTag", SqlDbType.VarChar, tag);

			DataTable dt = this.GetTable(comm);

			if (dt.Rows.Count > 0)
			{
				company = ParseCompany(dt.Rows[0]);
			}

			return company;
		}

		internal Company Get(int id)
		{
			Company company = null;
			SqlCommand comm = new SqlCommand("SPCompaniesGetById", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object) id);

			DataTable dt = this.GetTable(comm);

			if (dt.Rows.Count > 0)
			{
				company = ParseCompany(dt.Rows[0]);
			}

			return company;
		}

		protected Company ParseCompany(DataRow dr)
		{
			var company = new Company();
			company.Name = dr["CompanyName"].ToString();
			//company.Tag = dr["CompanyTag"].ToString();
			company.Id = Convert.ToInt32(dr["CompanyId"]);
			company.Url = dr["CompanyUrl"].ToString();
			company.Description = dr["CompanyDescription"].ToString();
			company.CategoryId = Convert.ToInt32(dr["CategoryId"]);
			company.FeedUrl = Convert.ToString(dr["CompanyFeedUrl"]);
			company.Logo = Convert.ToString(dr["CompanyLogo"]);
			company.Owner = dr.GetNullableStruct<int>("OwnerUserId");
			return company;
		}

		internal void Add(Company company, string ipAddress, int userId)
		{
			SqlCommand comm = new SqlCommand("SPCompaniesInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyTag", SqlDbType.VarChar, company.Tag);
			comm.Parameters.Add("@CompanyName", SqlDbType.VarChar, company.Name);
			comm.Parameters.Add("@CompanyUrl", SqlDbType.VarChar, company.Url);
			comm.Parameters.Add("@CompanyDescription", SqlDbType.VarChar, company.Description);
			comm.Parameters.Add("@CategoryId", SqlDbType.Int, (object)company.CategoryId);

			comm.Parameters.Add("@CompanyFeedUrl", SqlDbType.VarChar, company.FeedUrl);
			comm.Parameters.Add("@CompanyLogo", SqlDbType.VarChar, company.Logo);
			comm.Parameters.Add("@EditIp", SqlDbType.VarChar, ipAddress);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			SqlParameter idParameter = comm.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int));
			idParameter.Direction = ParameterDirection.Output;

			this.SafeExecuteNonQuery(comm);
			if (idParameter.Value == null)
			{
				throw new DataException("No value for the output parameter: " + idParameter.ParameterName);
			}
			company.Id = Convert.ToInt32(idParameter.Value);
		}

		internal void Update(Company company, string ipAddress, int userId)
		{
			SqlCommand comm = new SqlCommand("SPCompaniesUpdate", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyTag", SqlDbType.VarChar, company.Tag);
			comm.Parameters.Add("@CompanyName", SqlDbType.VarChar, company.Name);
			comm.Parameters.Add("@CompanyUrl", SqlDbType.VarChar, company.Url);
			comm.Parameters.Add("@CompanyDescription", SqlDbType.VarChar, company.Description);
			comm.Parameters.Add("@CategoryId", SqlDbType.Int, (object)company.CategoryId);
			comm.Parameters.Add("@CompanyFeedUrl", SqlDbType.VarChar, company.FeedUrl);
			comm.Parameters.Add("@CompanyLogo", SqlDbType.VarChar, company.Logo);
			comm.Parameters.Add("@EditIp", SqlDbType.VarChar, ipAddress);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			this.SafeExecuteNonQuery(comm);
		}

		internal void AddVisit(int companyId)
		{
			SqlCommand comm = new SqlCommand("SPCompaniesAddVisit", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)companyId);

			this.SafeExecuteNonQuery(comm);
		}

		internal void SetNoVisits(int companyId)
		{
			SqlCommand comm = new SqlCommand("SPCompaniesSetNoVisits", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)companyId);

			this.SafeExecuteNonQuery(comm);
		}

		internal SortedList<string, KeyValuePair<int, string>> GetCompaniesSorted()
		{
			SqlCommand comm = new SqlCommand("SPCompaniesGetAll", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			var dt = this.GetTable(comm);

			var list = new SortedList<string, KeyValuePair<int, string>>();
			foreach (DataRow dr in dt.Rows)
			{
				var companyName = dr["CompanyName"].ToString();
				if (!list.ContainsKey(companyName.ToLower()))
				{
					list.Add(companyName.ToLower(), new KeyValuePair<int, string>(Convert.ToInt32(dr["CompanyId"]), companyName));
				}
			}
			return list;
		}

		internal List<Company> GetTopCompanies()
		{
			List<Company> companies = new List<Company>();
			SqlCommand comm = new SqlCommand("SPCompaniesGetRelevant", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;

			DataTable dt = this.GetTable(comm);

			foreach (DataRow dr in dt.Rows)
			{
				var company = new Company();
				company.Name = dr["CompanyName"].ToString();
				company.Tag = dr["CompanyTag"].ToString();
				company.Id = Convert.ToInt32(dr["CompanyId"]);
				companies.Add(company);
			}
			return companies;
		}

		internal List<Company> GetByIp(string ip)
		{
			List<Company> companies = new List<Company>();
			SqlCommand comm = new SqlCommand("SPCompaniesGetByIp", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@Ip", SqlDbType.VarChar, ip);

			DataTable dt = this.GetTable(comm);

			foreach (DataRow dr in dt.Rows)
			{
				var company = new Company();
				company.Name = dr["CompanyName"].ToString();
				company.Tag = dr["CompanyTag"].ToString();
				company.Id = Convert.ToInt32(dr["CompanyId"]);
				companies.Add(company);
			}
			return companies;
		}

		internal void Delete(int id)
		{
			SqlCommand comm = new SqlCommand("SPCompaniesDelete", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)id);

			this.SafeExecuteNonQuery(comm);
		}

		internal List<Company> GetWithFeeds()
		{
			var companies = new List<Company>();
			var comm = new SqlCommand("SPCompaniesGetWithFeeds", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;

			var dt = this.GetTable(comm);

			foreach (DataRow dr in dt.Rows)
			{
				var company = new Company();
				company.Id = dr.Get<int>("CompanyId");
				company.Name = dr.GetString("CompanyName");
				company.Tag = dr.GetString("CompanyTag");
				company.FeedUrl = dr.GetString("CompanyFeedUrl");
				company.Owner = dr.GetNullableStruct<int>("OwnerUserId");
				companies.Add(company);
			}
			return companies;
		}

		internal bool IsClosed(int? id, string tag, int userId)
		{
			bool isClosed = false;
			var comm = new SqlCommand("SPCompaniesIsClosed", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, id);
			comm.Parameters.Add("@CompanyTag", SqlDbType.VarChar, tag);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			var dr = GetFirstRow(comm);
			if (dr != null)
			{
				isClosed = dr.GetBit("IsClosed");
			}
			return isClosed;
		}
	}
}
