using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;

namespace CompanyFeeds.Web.State
{
	public class SessionWrapper
	{
		public HttpSessionStateBase Session
		{
			get;
			set;
		}

		public T GetItem<T>(string key)
		{
			return (T)Session[key];
		}

		public T GetItem<T>(string key, bool create) where T : new()
		{
			
			if (Session[key] == null)
			{
				if (create)
				{
					T value = new T();
					Session[key] = value;
					return value;
				}
				else
				{
					return (T)Session[key];
				}
			}
			else
			{
				return (T)Session[key];
			}
		}

		public void SetItem<T>(string key, T value)
		{
			Session[key] = value;
		}

		public SessionWrapper(HttpSessionStateBase session)
		{
			Session = session;
		}

		/// <summary>
		/// Current logged user. If the user is not logged in, its null.
		/// </summary>
		public UserState User
		{
			get
			{
				//return new UserState(1);
				return GetItem<UserState>("User");
			}
			set
			{
				SetItem<UserState>("User", value);
			}
		}

		/// <summary>
		/// Last entry temporarily saved in session in order to save the company first.
		/// </summary>
		public Entry Entry
		{
			get
			{
				return GetItem<Entry>("Entry");
			}
			set
			{
				SetItem<Entry>("Entry", value);
			}
		}

		/// <summary>
		/// Id of the newly created agency, in order to determine if the user is admin of the agency.
		/// </summary>
		public int? NewlyCreatedAgencyId
		{
			get
			{
				return GetItem<int?>("NewlyCreatedAgencyId", true);
			}
			set
			{
				SetItem<int?>("NewlyCreatedAgencyId", value);
			}
		}

		/// <summary>
		/// Determines if the cookie from the client has been read
		/// </summary>
		public bool CookieRetrieved
		{
			get
			{
				return GetItem<bool>("CookieRetrieved", true);
			}
			set
			{
				SetItem<bool>("CookieRetrieved", value);
			}
		}

		/// <summary>
		/// Determines if the entry was recently submitted.
		/// </summary>
		public bool IsNewEntry
		{
			get
			{
				return GetItem<bool>("IsNewEntry", true);
			}
			set
			{
				SetItem<bool>("IsNewEntry", value);
			}
		}

		/// <summary>
		/// Determines if the company was recently submitted.
		/// </summary>
		public bool IsNewCompany
		{
			get
			{
				return GetItem<bool>("IsNewCompany", true);
			}
			set
			{
				SetItem<bool>("IsNewCompany", value);
			}
		}
	}
}
