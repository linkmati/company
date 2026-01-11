using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CompanyFeeds.DataAccess
{
	public static class AdoExtensions
	{
		public static SqlParameter Add(this SqlParameterCollection collection, string parameterName, SqlDbType type, object value)
		{
			SqlParameter param = collection.Add(new SqlParameter(parameterName, type));
			if (value == null)
			{
				param.Value = DBNull.Value;
			}
			else if (value is string && Convert.ToString(value) == "")
			{
				param.Value = DBNull.Value;
			}
			else
			{
				param.Value = value;
			}

			return param;
		}

		public static string GetNullableString(this DataRow dr, string columnName)
		{
			object value = dr[columnName];
			if (value == DBNull.Value)
			{
				return null;
			}
			return value.ToString();
		}

		public static T? GetNullableStruct<T>(this DataRow dr, string columnName) where T : struct, IConvertible
		{
			var value = dr[columnName];
			if (value == DBNull.Value)
			{
				return (T?)null;
			}
			return (T)value;
		}

		public static string GetString(this DataRow dr, string columnName)
		{
			return dr.GetNullable<string>(columnName);
		}

		public static T GetNullable<T>(this DataRow dr, string columnName)
		{
			object value = dr[columnName];
			if (value == DBNull.Value)
			{
				return default(T);
			}
			return Get<T>(dr, columnName);
		}

		/// <summary>
		/// Gets the date in UTC Kind
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public static DateTime GetDate(this DataRow dr, string columnName)
		{
			DateTime date = (DateTime)dr[columnName];
			return DateTime.SpecifyKind(date, DateTimeKind.Utc);
		}

		/// <summary>
		/// Gets a boolean value from a bit value (1, 0)
		/// </summary>
		/// <returns></returns>
		public static bool GetBit(this DataRow dr, string columnName)
		{
			return dr[columnName].ToString() == "1";
		}

		public static T Get<T>(this DataRow dr, string columnName)
		{
			try
			{
				if (dr[columnName] == DBNull.Value)
				{
					throw new NoNullAllowedException("Column " + columnName + " has a null value.");
				}
				Type type = typeof(T);
				if (type == typeof(DateTime))
				{
					throw new ArgumentException("Date time not supported.");
				}
				else if (type == typeof(Guid))
				{
					return (T)(object)new Guid(dr[columnName].ToString());
				}
				else if (type.IsEnum)
				{
					return (T)Enum.Parse(type, dr[columnName].ToString());
				}

				return (T)dr[columnName];
			}
			catch (InvalidCastException ex)
			{
				throw new InvalidCastException("Specified cast is not valid, field: " + columnName + ", Type: " + typeof(T).FullName, ex);
			}
		}
	}
}
