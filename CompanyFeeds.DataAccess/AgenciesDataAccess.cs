using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CompanyFeeds.DataAccess
{
	internal class AgenciesDataAccess : BaseDataAccess
	{
		internal Agency Get(string tag)
		{
			Agency agency = null;
			SqlCommand comm = new SqlCommand("SPAgenciesGet", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@AgencyTag", SqlDbType.VarChar, tag);

			DataTable dt = this.GetTable(comm);

			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				agency = ParseAgencyRow(dr);
			}

			return agency;
		}

		/// <summary>
		/// Gets an agency by a user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		internal Agency GetByUser(int userId)
		{
			Agency agency = null;
			SqlCommand comm = new SqlCommand("SPAgenciesGetByUser", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object) userId);

			DataTable dt = this.GetTable(comm);

			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				agency = ParseAgencyRow(dr);
			}

			return agency;
		}

		private Agency ParseAgencyRow(DataRow dr)
		{
			var agency = new Agency();
			agency.Name = dr.GetString("AgencyName");
			agency.Tag = dr.GetString("AgencyTag");
			agency.Id = dr.Get<int>("AgencyId");
			agency.Url = dr.GetString("AgencyUrl");
			agency.Description = dr.GetString("AgencyDescription");
			agency.Logo = dr.GetString("AgencyLogo");
			agency.Email = dr.GetString("AgencyEmail");
			agency.Phone = dr.GetString("AgencyPhone");
			agency.HideAuthor = dr.Get<bool>("AgencyHideAuthor");
			return agency;
		}

		internal void Add(Agency agency)
		{
			var comm = new SqlCommand("SPAgenciesInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@AgencyTag", SqlDbType.VarChar, agency.Tag);
			comm.Parameters.Add("@AgencyName", SqlDbType.VarChar, agency.Name);
			comm.Parameters.Add("@AgencyUrl", SqlDbType.VarChar, agency.Url);
			comm.Parameters.Add("@AgencyDescription", SqlDbType.VarChar, agency.Description);
			comm.Parameters.Add("@AgencyPhone", SqlDbType.VarChar, agency.Phone);
			comm.Parameters.Add("@AgencyEmail", SqlDbType.VarChar, agency.Email);
			comm.Parameters.Add("@AgencyLogo", SqlDbType.VarChar, agency.Logo);
			comm.Parameters.Add("@AgencyHideAuthor", SqlDbType.Bit, agency.HideAuthor);

			SqlParameter idParameter = comm.Parameters.Add(new SqlParameter("@AgencyId", SqlDbType.Int));
			idParameter.Direction = ParameterDirection.Output;

			this.SafeExecuteNonQuery(comm);
			if (idParameter.Value == null)
			{
				throw new DataException("No value for the output parameter: " + idParameter.ParameterName);
			}
			agency.Id = Convert.ToInt32(idParameter.Value);
		}

		internal void Update(Agency agency)
		{
			SqlCommand comm = new SqlCommand("SPAgenciesUpdate", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@AgencyTag", SqlDbType.VarChar, agency.Tag);
			comm.Parameters.Add("@AgencyName", SqlDbType.VarChar, agency.Name);
			comm.Parameters.Add("@AgencyUrl", SqlDbType.VarChar, agency.Url);
			comm.Parameters.Add("@AgencyDescription", SqlDbType.VarChar, agency.Description);
			comm.Parameters.Add("@AgencyPhone", SqlDbType.VarChar, agency.Phone);
			comm.Parameters.Add("@AgencyEmail", SqlDbType.VarChar, agency.Email);
			comm.Parameters.Add("@AgencyLogo", SqlDbType.VarChar, agency.Logo);
			comm.Parameters.Add("@AgencyHideAuthor", SqlDbType.Bit, agency.HideAuthor);


			this.SafeExecuteNonQuery(comm);
		}

		internal void SetPayment(int agencyId, PaymentStatus status, string payerEmail, string subscriptionId, DateTime? endDate)
		{
			var comm = new SqlCommand("SPAgenciesSetPayment", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@AgencyId", SqlDbType.Int, (object)agencyId);
			comm.Parameters.Add("@AgencyPaymentStatus", SqlDbType.SmallInt, (object)(short)status);
			comm.Parameters.Add("@AgencyPayerEmail", SqlDbType.VarChar, payerEmail);
			comm.Parameters.Add("@AgencySubscriptionId", SqlDbType.VarChar, subscriptionId);
			comm.Parameters.Add("@AgencyPremiumDate", SqlDbType.DateTime, endDate);

			var rowsAffected = SafeExecuteNonQuery(comm);
			if (rowsAffected == 0)
			{
				throw new DataException("No agency found (id:" + agencyId + ") to set the payment.");
			}
		}
	}
}
