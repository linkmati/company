using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CompanyFeeds.DataAccess
{
	internal class CommentsDataAccess : BaseDataAccess
	{

		internal List<Comment> GetByEntry(int entryId)
		{
			SqlCommand comm = new SqlCommand("SPEntriesCommentsGetByEntry", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);

			DataTable dt = this.GetTable(comm);

			List<Comment> comments = AdaptComments(dt, false);

			return comments;
		}

		/// <summary>
		/// Adapts the comments datatable to a list of comments
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="adaptEntry">Determines if the entry of the comment should be parsed/adapted</param>
		/// <returns></returns>
		public List<Comment> AdaptComments(DataTable dt, bool adaptEntry)
		{
			List<Comment> comments = new List<Comment>();
			foreach (DataRow dr in dt.Rows)
			{
				Comment c = new Comment();
				c.Id = Convert.ToInt32(dr["EntryCommentId"]);
				c.Value = Convert.ToString(dr["CommentValue"]);
				c.Notify = Convert.ToBoolean(dr["NotifyReplies"]);
				c.Date = Convert.ToDateTime(dr["CommentDate"]);
				if (dr["UserId"] != DBNull.Value)
				{
					c.User = new User();
					c.User.Id = Convert.ToInt32(dr["UserId"]);
					c.User.Name = Convert.ToString(dr["UserName"]);
					c.User.Email = Convert.ToString(dr["UserEmail"]);
				}
				else
				{
					c.User = new User();
					c.User.Name = Convert.ToString(dr["Name"]);
					c.User.Email = Convert.ToString(dr["Email"]);
				}
				if (adaptEntry)
				{
					c.Entry = new Entry();
					c.Entry.Id = Convert.ToInt32(dr["EntryId"]);
				}
				comments.Add(c);
			}

			return comments;
		}

		internal List<Comment> GetLatest()
		{
			SqlCommand comm = new SqlCommand("SPEntriesCommentsGetLatest", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;

			DataTable dt = this.GetTable(comm);

			List<Comment> comments = AdaptComments(dt, true);

			return comments;
		}

		internal void Add(Comment comment, int entryId, string ip)
		{
			SqlCommand comm = new SqlCommand("SPEntriesCommentsInsert", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);
			if (comment.User.Id > 0)
			{
				comm.Parameters.Add("@UserId", SqlDbType.Int, (object)comment.User.Id);
				comm.Parameters.Add("@Name", SqlDbType.VarChar, null);
				comm.Parameters.Add("@Email", SqlDbType.VarChar, null);
			}
			else
			{
				comm.Parameters.Add("@UserId", SqlDbType.Int, null);
				comm.Parameters.Add("@Name", SqlDbType.VarChar, comment.User.Name);
				comm.Parameters.Add("@Email", SqlDbType.VarChar, comment.User.Email);
			}
			comm.Parameters.Add("@CommentValue", SqlDbType.VarChar, (object)comment.Value);
			comm.Parameters.Add("@NotifyReplies", SqlDbType.Bit, (object)comment.Notify);
			comm.Parameters.Add("@EditIp", SqlDbType.VarChar, (object)ip);

			this.SafeExecuteNonQuery(comm); 
		}

		internal List<User> GetUsersToNotify(int entryId)
		{
			List<User> users = new List<User>();
			SqlCommand comm = new SqlCommand("SPEntriesCommentsGetNotify", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);

			DataTable dt = this.GetTable(comm);

			foreach (DataRow dr in dt.Rows)
			{
				User u = new User();
				if (dr["UserId"] != DBNull.Value)
				{
					u.Id = Convert.ToInt32(dr["UserId"]);
					u.Name = Convert.ToString(dr["UserName"]);
					u.Email = Convert.ToString(dr["UserEmail"]);
				}
				else
				{
					u.Name = Convert.ToString(dr["Name"]);
					u.Email = Convert.ToString(dr["Email"]);
				}
				users.Add(u);
			}

			return users;
		}

		internal void Unsubscribe(int entryId, int userId)
		{
			SqlCommand comm = new SqlCommand("SPEntriesCommentsUnsubscribeByUser", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);
			comm.Parameters.Add("@UserId", SqlDbType.Int, (object)userId);
			this.SafeExecuteNonQuery(comm);
		}

		internal void Unsubscribe(int entryId, string email)
		{
			SqlCommand comm = new SqlCommand("SPEntriesCommentsUnsubscribeByEmail", GetConnection());
			comm.CommandType = CommandType.StoredProcedure;
			comm.Parameters.Add("@EntryId", SqlDbType.Int, (object)entryId);
			comm.Parameters.Add("@Email", SqlDbType.VarChar, email);
			this.SafeExecuteNonQuery(comm);
		}
	}
}
