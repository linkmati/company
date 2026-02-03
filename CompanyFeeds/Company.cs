using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompanyFeeds.Validation;

namespace CompanyFeeds
{
	public class Company : Entity
	{
		public int Id
		{
			get;
			set;
		}

		[RequireField]
		public string Name
		{
			get;
			set;
		}

		[RequireField]
		[Length(2, 50)]
		[RegexFormat(@"^[a-z-]+$")]
		public string Tag
		{
			get;
			set;
		}

		[RequireField]
		[RegexFormat(@"^https?://([a-zA-Z0-9_-]+\.[a-zA-Z0-9_-]+)+(/*[A-Za-z0-9/\-_&:?\+=//.%]*)*$")]
		[Length(128)]
		public string Url
		{
			get;
			set;
		}

		[Length(256)]
		[RegexFormat(@"^https?://([a-zA-Z0-9_-]+\.[a-zA-Z0-9_-]+)+(/*[A-Za-z0-9/\-_&:?\+=//.%]*)*$")]
		public string FeedUrl
		{
			get;
			set;
		}

		[Length(3,4)]
		[RegexFormat(@"^jpg|gif|png|jpeg|JPEG|JPG|GIF|PNG$")]
		public string Logo
		{
			get;
			set;
		}

		private string _description;
		[RequireField]
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

		[RequireField]
		public int CategoryId
		{
			get;
			set;
		}

		/// <summary>
		/// Identifier of the user, owner of the company. 
		/// Used to determine the user sender of a press release through feeds. 
		/// Also, if set, only the owner can edit its detail.
		/// </summary>
		public int? Owner
		{
			get;
			set;
		}
	}
}
