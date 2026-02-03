using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace CompanyFeeds.Web.State
{
	public class UserState
	{
		public UserState()
		{
		}

		public UserState(int id)
		{
			Id = id;
		}

		public UserState(int id, Guid guid, string name, string email, string agencyTag, bool isEmailActive, int updates, bool isPremium)
		{
			Id = id;
			Guid = guid;
			Name = name;
			Email = email;
			AgencyTag = agencyTag;
			IsEmailActive = isEmailActive;
			IsPremium = isPremium;
			SetRole(updates);
		}

		public int Id
		{
			get;
			set;
		}

		public Guid Guid
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string AgencyTag
		{
			get;
			set;
		}

		public bool IsEmailActive
		{
			get;
			set;
		}

		public UserRole Role
		{
			get;
			private set;
		}

		/// <summary>
		/// Determines if the user has a premium account (through the agency)
		/// </summary>
		public bool IsPremium 
		{ 
			get; 
			set; 
		}

		private void SetRole(int updates)
		{
			if (updates == -1)
			{
				Role = UserRole.Admin;
			}
			else if (updates < 50)
			{
				Role = UserRole.Normal;
			}
			else
			{
				Role = UserRole.Power;
			}
		}
	}

	public enum UserRole
	{
		Admin = -1,
		Normal = 0,
		Power = 1
	}
}
