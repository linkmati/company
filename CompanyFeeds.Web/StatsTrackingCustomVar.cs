using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyFeeds.Web
{
	public class StatsTrackingCustomVar
	{
		public StatsTrackingCustomVar()
		{
			this.Scope = 3;
		}

		public StatsTrackingCustomVar(int index, string name, string value) : this()
		{
			this.Index = index;
			this.Name = name;
			this.Value = value;
		}

		public StatsTrackingCustomVar(int index, string name, string value, int scope) : this(index, name, value)
		{
			this.Scope = scope;
		}

		public int Index
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		/// <summary>
		/// The scope for the custom variable. Optional. The scope defines the level of user engagement with your site. It is a number whose possible values are 1 (visitor-level), 2 (session-level), or 3 (page-level) / default
		/// </summary>
		public int Scope
		{
			get;
			set;
		}
	}
}
