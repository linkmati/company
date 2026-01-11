using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CompanyFeeds.DataAccess;
using CompanyFeeds.DataAccess.Queries;
using CompanyFeeds.DataAccess.Queries.UsersQueriesTableAdapters;
using CompanyFeeds.Validation;

namespace CompanyFeeds.Services
{
	public static class UsersService
	{
		/// <summary>
		/// Add a new user
		/// </summary>
		/// <exception cref="ValidationException">Throws this exception when the fields are not properly set in order to save.</exception>  
		public static void AddUser(User user)
		{
			//Check for multiple emails.
			if (!IsEmailUnique(user))
			{
				throw new ValidationException(new List<ValidationError> { new ValidationError("Email", ValidationErrorType.DuplicateNotAllowed) });
			}
			
			//Validate
			user.ValidateFields();

			//Save the user in the database
			UsersDataAccess data = new UsersDataAccess();
			data.AddUser(user);
		}

		/// <summary>
		/// Determines if the email is unique
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool IsEmailUnique(User user)
		{
			UsersCheckTableAdapter ta = new UsersCheckTableAdapter();
			return ta.CheckEmail(user.Id, user.Email).Count == 0;
		}

		public static UsersQueries.UsersLoginRow GetLogin(int id, Guid guid)
		{
			UsersQueries.UsersLoginRow dr = null;

			UsersLoginTableAdapter ta = new UsersLoginTableAdapter();
			UsersQueries.UsersLoginDataTable dt = ta.GetByGuid(id, guid);
			if (dt.Count > 0)
			{
				dr = dt[0];
			}
			return dr;
		}

		public static UsersQueries.UsersLoginRow GetLogin(string email, string password)
		{
			UsersQueries.UsersLoginRow dr = null;

			UsersLoginTableAdapter ta = new UsersLoginTableAdapter();
			UsersQueries.UsersLoginDataTable dt = ta.GetByLogin(email, password);
			if (dt.Count > 0)
			{
				dr = dt[0];
			}
			return dr;
		}

		public static User Get(int id)
		{
			UsersDataAccess data = new UsersDataAccess();
			return data.Get(id);
		}

		public static void Update(User user)
		{
			user.ValidateFields();
			if (!IsEmailUnique(user))
			{
				throw new ValidationException(new List<ValidationError>{new ValidationError("Email", ValidationErrorType.DuplicateNotAllowed)});
			}
			UsersDataAccess data = new UsersDataAccess();
			data.Update(user);
		}

		public static bool ValidateEmail(int userId, Guid guid)
		{
			UsersDataAccess da = new UsersDataAccess();
			return da.ValidateEmail(userId, guid);
		}

		public static UsersQueries.UsersDetailRow GetDetail(int id)
		{
			UsersDetailTableAdapter ta = new UsersDetailTableAdapter();
			UsersQueries.UsersDetailDataTable dt = ta.Get(id);
			UsersQueries.UsersDetailRow user = null;
			if (dt.Count > 0)
			{
				user = dt[0];
			}
			return user;
		}

		/// <summary>
		/// Gets the users with email not validated
		/// </summary>
		/// <returns></returns>
		public static List<User> GetByEmailInactive()
		{
			UsersDataAccess da = new UsersDataAccess();
			return da.GetByEmailInactive();
		}

		/// <summary>
		/// Gets a list of ip used to submit content
		/// </summary>
		public static List<string> GetIpsUsed(int id)
		{
			var da = new UsersDataAccess();
			return da.GetIpsUsed(id);
		}

		/// <summary>
		/// Sets the payment data as "Completed or Pending"
		/// </summary>
		public static void SetPayment(int userId, PaymentStatus status, string payerEmail, string subscriptionId, bool isYearly)
		{
			var endDate = DateTime.Now.AddMonths(1);
			if (isYearly)
			{
				endDate = DateTime.Now.AddYears(1);
			}
			if (status == PaymentStatus.Pending)
			{
				//the payment is being processed
				endDate = DateTime.Now.AddDays(2);
			}

			var agency = GetOrCreateAgency(userId);
			var da = new AgenciesDataAccess();
			da.SetPayment(agency.Id, status, payerEmail, subscriptionId, endDate);
		}

		/// <summary>
		/// Gets the agency which the users belongs to. If does not exist, creates a new agency.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		private static Agency GetOrCreateAgency(int userId)
		{
			var agencyDa = new AgenciesDataAccess();
			var agency = agencyDa.GetByUser(userId);
			if (agency == null)
			{
				var userDa = new UsersDataAccess();
				var user = userDa.Get(userId);
				if (user == null)
				{
					throw new NullReferenceException("The user does not exist. id: " + userId);
				}
				agency = new Agency();
				agency.Name = user.Name;
				agency.Description = " ";
				agency.Tag = "agency-user-" + user.Id;
				agency.HideAuthor = true;
				agencyDa.Add(agency);

				//Link the user to the agency
				userDa.SetAgency(userId, agency.Id, true);
			}
			return agency;
		}

		internal static void RollbackPayment(int userId)
		{
			var da = new AgenciesDataAccess();
			var agency = da.GetByUser(userId);
			if (agency != null)
			{
				da.SetPayment(agency.Id, PaymentStatus.Failed, null, null, null);
			}
		}

		public static void Delete(int id)
		{
			var da = new UsersDataAccess();
			da.Delete(id);
		}
	}
}
