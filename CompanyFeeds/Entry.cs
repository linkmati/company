using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Validation;

namespace CompanyFeeds
{
	public class Entry : Entity
	{
		public Entry()
		{
			Date = DateTime.MinValue;
		}

		public int Id
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 128)]
		[RegexFormat(@"^[a-z-]+$")]
		public string Tag
		{
			get;
			set;
		}

		[RequireField]
		public string CompanyName
		{
			get;
			set;
		}

		public string CompanyTag
		{
			get;
			set;
		}

		[RequireField]
		public int CompanyId
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 256)]
		[RegexFormat(@"[a-z-]")]
		public string EntryTitle
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 512)]
		public string Teaser
		{
			get;
			set;
		}

		/// <summary>
		/// Id of the user who submitted the entry
		/// </summary>
		public int? UserId
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		[Length(1024)]
		public string ExternalGuid
		{
			get;
			set;
		}

		[Length(1024)]
		public string Source
		{
			get;
			set;
		}

		public string ContactInfo
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		/// <summary>
		/// Determines if an entry allow comments.
		/// If null -> allow comments remains unchanged.
		/// </summary>
		public bool? AllowComments
		{
			get;
			set;
		}

		public int? Owner
		{
			get;
			set;
		}

		public Company Company
		{
			get;
			set;
		}

		public User User
		{
			get;
			set;
		}

        /// <summary>
        /// Determines if the user that posted this content is premium.
        /// </summary>
        public bool? IsPremium { get; set; }

		public override void ValidateFields()
		{
			List<ValidationError> errors = base.ValidateFields(false);
			if (ExternalGuid == null)
			{
				if (String.IsNullOrEmpty(this.Content))
				{
					errors.Add(new ValidationError("Content", ValidationErrorType.NullOrEmpty));
				}
			}

			if (errors.Count > 0)
			{
				throw new ValidationException(errors);
			}
		}
	}
}
