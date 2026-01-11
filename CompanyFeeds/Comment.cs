using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Validation;

namespace CompanyFeeds
{
	public class Comment : Entity
	{
		public Comment()
		{

		}

		public Comment(int id, string value, User user, bool notify)
		{
			Id = id;
			Value = value;
			User = user;
			Notify = notify;
		}

		public int Id
		{
			get;
			set;
		}

		[RequireField]
		public string Value
		{
			get;
			set;
		}

		[RequireField]
		public User User
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if the user wants to be notified for that comment.
		/// </summary>
		public bool Notify
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public Entry Entry
		{
			get;
			set;
		}
	}
}
