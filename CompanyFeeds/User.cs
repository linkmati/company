using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Validation;
using System.Collections;

namespace CompanyFeeds
{
	public class User : Entity
	{
		public User()
		{

		}

		public User(int id, string name, string email)
		{
			this.Id = id;
			this.Name = name;
			this.Email = email;
		}

		public int Id
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 30)]
		public string Name
		{
			get;
			set;
		}

		[RequireField]
		[EmailFormat]
		public string Email
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 30)]
		public string Password
		{
			get;
			set;
		}

		public Gender? Gender
		{
			get;
			set;
		}

		[Birthday()]
		public DateTime? Birthday
		{
			get;
			set;
		}

		/// <summary>
		/// ISO 2 char Code
		/// </summary>
		[RequireField]
		[Length(2, 2)]
		public string CountryCode
		{
			get;
			set;
		}

		[RequireField]
		public Guid Guid
		{
			get;
			set;
		}

		public int? AgencyId
		{
			get;
			set;
		}

		public bool? AgencyValidated
		{
			get;
			set;
		}

		public DateTime? AgencyPremiumDate
		{
			get;
			set;
		}

		public bool IsEmailValidated
		{
			get;
			set;
		}
	}
}
