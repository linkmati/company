using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CompanyFeeds.DataAccess
{
	internal abstract class BaseDataAccess
	{
		protected string ConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["CompanyFeeds.DataAccess.Properties.Settings.CompanyFeedsConnectionString"].ConnectionString;
			}
		}

		protected SqlConnection GetConnection()
		{
			return new SqlConnection(ConnectionString);
		}

		protected int SafeExecuteNonQuery(SqlCommand comm)
		{
			int rowsAffected = 0;
			try
			{
				comm.Connection.Open();
				rowsAffected = comm.ExecuteNonQuery();
			}
			finally
			{
				comm.Connection.Close();
			}
			return rowsAffected;
		}

		protected DataTable GetTable(SqlCommand comm)
		{
			DataTable dt = new DataTable();
			new SqlDataAdapter(comm).Fill(dt);
			return dt;
		}

		/// <summary>
		/// Gets a datatable filled with the first result of executing the command.
		/// </summary>
		protected DataRow GetFirstRow(SqlCommand command)
		{
			DataRow dr = null;
			DataTable dt = GetTable(command);
			if (dt.Rows.Count > 0)
			{
				dr = dt.Rows[0];
			}
			return dr;
		}

		protected T GetNullableValue<T>(object value)
		{
			if (value == DBNull.Value)
			{
				return default(T);
			}
			return (T) value;
		}

		protected T GetNullableValue<T>(DataRow dr, string columnName)
		{
			return GetNullableValue<T>(dr[columnName]);
		}
	}
}
