using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using CompanyFeeds.DataAccess;

namespace CompanyFeeds.DataAccess
{
	internal class UsersDataAccess : BaseDataAccess
	{
		internal void AddUser(User user)
		{
			user.Guid = Guid.NewGuid();

			SqlCommand comm = new SqlCommand("SPUsersInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserEmail", SqlDbType.VarChar, user.Email);
			comm.Parameters.Add("@UserPassword", SqlDbType.VarChar, user.Password);
			comm.Parameters.Add("@UserName", SqlDbType.VarChar, user.Name);
			comm.Parameters.Add("@UserGenderMale", SqlDbType.Bit, (int?)user.Gender);
			comm.Parameters.Add("@UserBirthdate", SqlDbType.DateTime, user.Birthday);
			comm.Parameters.Add("@CountryCode", SqlDbType.Char, user.CountryCode);
			comm.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier, user.Guid);
			comm.Parameters.Add("@AgencyId", SqlDbType.Int, user.AgencyId);
			comm.Parameters.Add("@AgencyValidated", SqlDbType.Bit, user.AgencyValidated);

			SqlParameter idParameter = comm.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
			idParameter.Direction = ParameterDirection.Output;

			this.SafeExecuteNonQuery(comm);
			if (idParameter.Value == null)
			{
				throw new DataException("No value for the output parameter: " + idParameter.ParameterName);
			}
			user.Id = Convert.ToInt32(idParameter.Value);
		}

		internal User Get(int id)
		{
			User user = null;
			SqlCommand comm = new SqlCommand("SPUsersGet", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object) id);

			DataTable dt = this.GetTable(comm);
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				user = new User();
				//Fill in fields
				user.Id = id;
				user.Email = dr["UserEmail"].ToString();
				user.Password = dr["UserPassword"].ToString();
				user.Name = dr["UserName"].ToString();
				user.Birthday = GetNullableValue<DateTime?>(dr, "UserBirthdate");
				user.CountryCode = dr["CountryCode"].ToString();
				user.AgencyPremiumDate = dr.GetNullableStruct<DateTime>("AgencyPremiumDate");
                //Added 2016
			    user.Guid = dr.Get<Guid>("UserGuid");
				bool? genderIsMale = GetNullableValue<bool?>(dr, "UserGenderMale");
				if (genderIsMale != null)
				{
					user.Gender = genderIsMale==true ? Gender.Male : Gender.Female;
				}
			}
			return user;
		}

		internal void Update(User user)
		{
			SqlCommand comm = new SqlCommand("SPUsersUpdate", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object) user.Id);
			comm.Parameters.Add("@UserEmail", SqlDbType.VarChar, user.Email);
			comm.Parameters.Add("@UserPassword", SqlDbType.VarChar, user.Password);
			comm.Parameters.Add("@UserName", SqlDbType.VarChar, user.Name);
			comm.Parameters.Add("@UserGenderMale", SqlDbType.Bit, (int?)user.Gender);
			comm.Parameters.Add("@UserBirthdate", SqlDbType.DateTime, user.Birthday);
			comm.Parameters.Add("@CountryCode", SqlDbType.Char, user.CountryCode);

			this.SafeExecuteNonQuery(comm);
		}

		internal bool ValidateEmail(int userId, Guid guid)
		{
			SqlCommand comm = new SqlCommand("SPUsersUpdateEmailValid", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);
			comm.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier, guid);

			DataTable dt = this.GetTable(comm);
			return (dt.Rows.Count > 0);
		}

		/// <summary>
		/// Gets the users with email not validated
		/// </summary>
		/// <returns></returns>
		internal List<User> GetByEmailInactive()
		{
			List<User> list = new List<User>();
			SqlCommand comm = new SqlCommand("SPUsersGetByEmailInactive", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;


			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				User user = new User();
				//Fill in fields
				user.Id = Convert.ToInt32(dr["UserId"]);
				user.Email = dr["UserEmail"].ToString();
				user.Name = dr["UserName"].ToString();
				user.CountryCode = dr["CountryCode"].ToString();
				user.AgencyId = GetNullableValue<int?>(dr["AgencyId"]);
				user.CountryCode = dr["CountryCode"].ToString();
				user.Guid = (Guid) dr["UserGuid"];

				list.Add(user);
			}
			return list;
		}

		internal List<string> GetIpsUsed(int id)
		{
			var list = new List<string>();
			SqlCommand comm = new SqlCommand("SPUsersGetIps", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)id);


			DataTable dt = this.GetTable(comm);
			foreach (DataRow dr in dt.Rows)
			{
				list.Add(dr.GetString("EditIp"));
			}
			return list;
		}

		/// <summary>
		/// Links a user to a agency
		/// </summary>
		internal void SetAgency(int userId, int agencyId, bool agencyValidated)
		{
			var comm = new SqlCommand("SPUsersSetAgency", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);
			comm.Parameters.Add("@AgencyId", SqlDbType.Int, (object)agencyId);
			comm.Parameters.Add("@AgencyValidated", SqlDbType.Bit, agencyValidated);

			this.SafeExecuteNonQuery(comm);
		}

		internal void Delete(int id)
		{
			var comm = new SqlCommand("SPUsersDelete", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)id);

			this.SafeExecuteNonQuery(comm);
		}
	}
}
