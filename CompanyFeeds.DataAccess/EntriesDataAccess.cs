using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using CompanyFeeds.DataAccess;

namespace CompanyFeeds.DataAccess
{
	internal class EntriesDataAccess : BaseDataAccess
	{
		internal void AddEntry(Entry entry, string ipAddress)
		{
			if (entry.Date == DateTime.MinValue)
			{
				entry.Date = DateTime.Now;
			}

			SqlCommand comm = new SqlCommand("SPEntriesInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryTitle", SqlDbType.VarChar, entry.EntryTitle);
			comm.Parameters.Add("@EntryTag", SqlDbType.VarChar, entry.Tag);
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)entry.CompanyId);
			comm.Parameters.Add("@EntryTeaser", SqlDbType.VarChar, entry.Teaser);
			comm.Parameters.Add("@EntryDate", SqlDbType.DateTime, entry.Date);
			comm.Parameters.Add("@EntryContent", SqlDbType.VarChar, entry.Content);
			
			comm.Parameters.Add("@EntryExternalGuid", SqlDbType.VarChar, entry.ExternalGuid);
			comm.Parameters.Add("@EntrySource", SqlDbType.VarChar, entry.Source);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)entry.UserId);
			comm.Parameters.Add("@EntryContactInfo", SqlDbType.VarChar, entry.ContactInfo);
			comm.Parameters.Add("@Ip", SqlDbType.VarChar, ipAddress);
			comm.Parameters.Add("@EntryAllowComments", SqlDbType.Bit, entry.AllowComments);

			SqlParameter idParameter = comm.Parameters.Add(new SqlParameter("@EntryId", SqlDbType.Int));
			idParameter.Direction = ParameterDirection.Output;

			this.SafeExecuteNonQuery(comm);
			if (idParameter.Value == null)
			{
				throw new DataException("No value for the output parameter: " + idParameter.ParameterName);
			}
			if (idParameter.Value != DBNull.Value)
			{
				entry.Id = Convert.ToInt32(idParameter.Value);
			}
		}

		internal void AddFlag(int entryId, int? userId)
		{
			SqlCommand comm = new SqlCommand("SPEntriesFlagsInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			this.SafeExecuteNonQuery(comm); 
		}

		internal Entry Get(int id)
		{
			Entry entry = null;
			SqlCommand comm = new SqlCommand("SPEntriesGet", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object) id);

			DataTable dt = this.GetTable(comm);
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				entry = new Entry();
				entry.Id = id;
				entry.Tag = dr["EntryTag"].ToString();
				entry.CompanyId = Convert.ToInt32(dr["CompanyId"]);
				entry.CompanyName = dr["CompanyName"].ToString();
				entry.CompanyTag = dr["CompanyTag"].ToString();
				entry.EntryTitle = dr["EntryTitle"].ToString();
				entry.Teaser = dr["EntryTeaser"].ToString();
				entry.Content = dr["EntryContent"].ToString();
				entry.ContactInfo = dr["EntryContactInfo"].ToString();
				entry.Source = dr["EntrySource"].ToString();
				entry.AllowComments = Convert.ToBoolean(dr["EntryAllowComments"]);
				if (dr["EntryOwner"] != DBNull.Value)
				{
					entry.Owner = Convert.ToInt32(dr["EntryOwner"]);
				}
			}
			return entry;
		}

		internal List<Entry> GetByUser(int userId)
		{
			List<Entry> list = new List<Entry>();
			SqlCommand comm = new SqlCommand("SPEntriesGetByUser", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);

			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				Entry entry = ParseBasicEntry(dr);

				list.Add(entry);
			}
			return list;
		}

		internal List<Entry> GetByIp(string ip)
		{
			List<Entry> list = new List<Entry>();
			SqlCommand comm = new SqlCommand("SPEntriesGetByIp", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@Ip", SqlDbType.VarChar, ip);

			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				Entry entry = ParseBasicEntry(dr);

				list.Add(entry);
			}
			return list;
		}

		internal List<Entry> GetSuspected()
		{
			return GetSuspected(null, true);
		}

		internal List<Entry> GetSuspected(int? id, bool descendingOrder)
		{
			List<Entry> list = new List<Entry>();
			SqlCommand comm = new SqlCommand("SPEntriesGetSuspected", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			if (id != null)
			{
				comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)id);
			}
			comm.Parameters.Add("@Descending", SqlDbType.Bit, descendingOrder);

			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				Entry entry = ParseBasicEntry(dr);
				entry.Date = Convert.ToDateTime(dr["EntryDate"]);
				entry.Company = new Company()
				{
					Id = dr.Get<int>("CompanyId"),
					Tag = dr.GetString("CompanyTag"),
					Name = dr.GetString("CompanyName"),
					Url = dr.GetString("CompanyUrl")
				};
				if (dr["UserId"] != DBNull.Value)
				{
					//User can be null when it was added from feeds
					entry.User = new User(dr.Get<int>("UserId"), dr.GetString("UserName"), null);
				}
			    entry.IsPremium = dr.Get<int>("IsPremium") == 1;

				list.Add(entry);
			}
			return list;
		}

		protected virtual Entry ParseBasicEntry(DataRow dr)
		{
			Entry entry = new Entry();
			entry.Id = Convert.ToInt32(dr["EntryId"]);
			entry.Tag = dr["EntryTag"].ToString();
			entry.CompanyId = Convert.ToInt32(dr["CompanyId"]);
			entry.CompanyName = dr["CompanyName"].ToString();
			entry.CompanyTag = dr["CompanyTag"].ToString();
			entry.EntryTitle = dr["EntryTitle"].ToString();
			entry.Teaser = dr["EntryTeaser"].ToString();
			return entry;
		}

		internal List<Entry> GetByAgency(int agencyId)
		{
			List<Entry> list = new List<Entry>();
			SqlCommand comm = new SqlCommand("SPEntriesGetByAgency", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@AgencyId", SqlDbType.Int, (object)agencyId);

			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				Entry entry = new Entry();
				entry.Id = Convert.ToInt32(dr["EntryId"]);
				entry.Tag = dr["EntryTag"].ToString();
				entry.CompanyId = Convert.ToInt32(dr["CompanyId"]);
				entry.CompanyName = dr["CompanyName"].ToString();
				entry.CompanyTag = dr["CompanyTag"].ToString();
				entry.EntryTitle = dr["EntryTitle"].ToString();
				entry.Teaser = dr["EntryTeaser"].ToString();

				list.Add(entry);
			}
			return list;
		}

		internal void Update(Entry entry, string ipAddress)
		{
			SqlCommand comm = new SqlCommand("SPEntriesUpdate", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entry.Id);
			comm.Parameters.Add("@EntryTitle", SqlDbType.VarChar, entry.EntryTitle);
			comm.Parameters.Add("@EntryTeaser", SqlDbType.VarChar, entry.Teaser);
			comm.Parameters.Add("@EntryContent", SqlDbType.VarChar, entry.Content);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)entry.UserId);
			comm.Parameters.Add("@EntryContactInfo", SqlDbType.VarChar, entry.ContactInfo);
			comm.Parameters.Add("@Ip", SqlDbType.VarChar, ipAddress);
			comm.Parameters.Add("@EntryAllowComments", SqlDbType.Bit, entry.AllowComments);

			this.SafeExecuteNonQuery(comm);
		}

		/// <summary>
		/// Determines if a entry already exist
		/// </summary>
		/// <param name="externalGuid"></param>
		/// <param name="company"></param>
		/// <returns></returns>
		internal bool Exist(string externalGuid, int companyId)
		{
			SqlCommand comm = new SqlCommand("SPEntriesGetByExternalGuid", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@CompanyId", SqlDbType.Int, (object)companyId);
			comm.Parameters.Add("@Guid", SqlDbType.VarChar, externalGuid);

			DataTable dt = this.GetTable(comm);

			return dt.Rows.Count > 0;
		}

		internal void Delete(int id)
		{
			SqlCommand comm = new SqlCommand("SPEntriesDelete", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)id);

			this.SafeExecuteNonQuery(comm);
		}

		/// <summary>
		/// deletes all entries submitted (and the companies submitted that are now empty) by a user, returns the entries deleted
		/// </summary>
		/// <returns></returns>
		internal int DeleteByUser(int userId)
		{
			SqlCommand comm = new SqlCommand("SPEntriesDeleteByUser", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);
			var entriesCount = comm.Parameters.Add("@EntriesCount", SqlDbType.Int);
			entriesCount.Direction = ParameterDirection.Output;
			var companiesCount = comm.Parameters.Add("@CompaniesCount", SqlDbType.Int);
			companiesCount.Direction = ParameterDirection.Output;

			this.SafeExecuteNonQuery(comm);

			return Convert.ToInt32(entriesCount.Value);
		}
	}
}
