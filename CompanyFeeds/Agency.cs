using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Validation;

namespace CompanyFeeds
{
	public class Agency : Entity
	{
		public int Id
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 50)]
		public string Tag
		{
			get;
			set;
		}

		[RequireField]
		[Length(4, 50)]
		public string Name
		{
			get;
			set;
		}

		private string _description;
		[RequireField]
		[Length(4, Int32.MaxValue)]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = TextUtils.CleanHtml(value);
			}
		}

		[Length(128)]
		[RegexFormat(@"^https?://([a-zA-Z0-9_-]+\.[a-zA-Z0-9_-]+)+(/*[A-Za-z0-9/\-_&:?\+=//.%]*)*$")]
		public string Url
		{
			get;
			set;
		}

		[EmailFormat]
		public string Email
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		[Length(3, 4)]
		[RegexFormat(@"^jpg|gif|png|jpeg|JPEG|JPG|GIF|PNG$")]
		public string Logo
		{
			get;
			set;
		}


		/// <summary>
		/// Determines if the agency name is not shown in the press release detail
		/// </summary>
		public bool HideAuthor { get; set; }
	}
}
