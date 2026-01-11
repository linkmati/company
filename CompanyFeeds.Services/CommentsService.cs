using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.DataAccess;

namespace CompanyFeeds.Services
{
	public static class CommentsService
	{
		public static List<Comment> GetComments(int entryId)
		{
			CommentsDataAccess da = new CommentsDataAccess();
			return da.GetByEntry(entryId);
		}

		public static List<User> GetUsersToNotify(int entryId)
		{
			CommentsDataAccess da = new CommentsDataAccess();
			return da.GetUsersToNotify(entryId);
		}

		public static void Add(Comment comment, int entryId, string ip)
		{
			comment.ValidateFields();

			CommentsDataAccess da = new CommentsDataAccess();
			da.Add(comment, entryId, ip);
		}

		/// <summary>
		/// Unsubscribes a commenter for future notifications, using email or the user id (for registered users).
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="email"></param>
		/// <param name="userId">id of the registered user, if not a registered user fill 0</param>
		public static void Unsubscribe(int entryId, string email, int userId)
		{
			CommentsDataAccess da = new CommentsDataAccess();
			if (userId > 0)
			{
				da.Unsubscribe(entryId, userId);
			}
			else
			{
				da.Unsubscribe(entryId, email);
			}
		}

		/// <summary>
		/// Gets the latest comments
		/// </summary>
		/// <returns></returns>
		public static List<Comment> GetLatest()
		{
			CommentsDataAccess da = new CommentsDataAccess();
			return da.GetLatest();
		}
	}
}
