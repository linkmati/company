using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CompanyFeeds.FeedClient
{
	//Source: http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=172
	public struct W3CDateTime
	{
		private DateTime dtime;
		private TimeSpan ofs;

		public W3CDateTime(DateTime dt, TimeSpan off)
		{
			ofs = off;
			dtime = dt;
		}

		public DateTime UtcTime
		{
			get
			{
				return dtime;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return dtime + ofs;
			}
		}

		public DateTime LocalDateTime
		{
			get
			{

				return this.UtcTime.ToLocalTime();
			}
		}

		public TimeSpan UtcOffset
		{
			get
			{
				return ofs;
			}
		}

		static public W3CDateTime Parse(string s)
		{
			const string Rfc822DateFormat =
			  @"^((Mon|Tue|Wed|Thu|Fri|Sat|Sun), *)?(?<day>\d\d?) +" +
			  @"(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) +" +
			  @"(?<year>\d\d(\d\d)?) +" +
			  @"(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d))? +" +
			  @"(?<ofs>([+\-]?\d\d\d\d)|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT)$";
			const string W3CDateFormat =
			  @"^(?<year>\d\d\d\d)" +
			  @"(-(?<month>\d\d)(-(?<day>\d\d)(T(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" +
			  @"(?<ofs>(Z|[+\-]\d\d:\d\d)))?)?)?$";

			string combinedFormat = string.Format(
			  @"(?<rfc822>{0})|(?<w3c>{1})", Rfc822DateFormat, W3CDateFormat);

			// try to parse it
			Regex reDate = new Regex(combinedFormat);
			Match m = reDate.Match(s);
			if (!m.Success)
			{
				// Didn't match either expression. Throw an exception.
				throw new FormatException("String is not a valid date time stamp.");
			}
			try
			{
				bool isRfc822 = m.Groups["rfc822"].Success;
				int year = int.Parse(m.Groups["year"].Value);
				// handle 2-digit and 3-digit years
				if (year < 1000)
				{
					if (year < 50)
						year = year + 2000;
					else
						year = year + 1999;
				}

				int month;
				if (isRfc822)
					month = ParseRfc822Month(m.Groups["month"].Value);
				else
					month = (m.Groups["month"].Success) ? int.Parse(m.Groups["month"].Value) : 1;

				int day = m.Groups["day"].Success ? int.Parse(m.Groups["day"].Value) : 1;
				int hour = m.Groups["hour"].Success ? int.Parse(m.Groups["hour"].Value) : 0;
				int min = m.Groups["min"].Success ? int.Parse(m.Groups["min"].Value) : 0;
				int sec = m.Groups["sec"].Success ? int.Parse(m.Groups["sec"].Value) : 0;
				int ms = m.Groups["ms"].Success ? (int)Math.Round((1000 * double.Parse(m.Groups["ms"].Value))) : 0;

				TimeSpan ofs = TimeSpan.Zero;
				if (m.Groups["ofs"].Success)
				{
					if (isRfc822)
						ofs = ParseRfc822Offset(m.Groups["ofs"].Value);
					else
						ofs = ParseW3COffset(m.Groups["ofs"].Value);
				}
				// datetime is stored in UTC
				return new W3CDateTime(new DateTime(year, month, day, hour, min, sec, ms) - ofs, ofs);
			}
			catch (Exception ex)
			{
				throw new FormatException("String is not a valid date time stamp.", ex);
			}
		}

		private static readonly string[] MonthNames = new string[]
    {"Jan", "Feb", "Mar", "Apr", "May", "Jun", 
      "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};

		private static int ParseRfc822Month(string monthName)
		{
			for (int i = 0; i < 12; i++)
			{
				if (monthName == MonthNames[i])
				{
					return i + 1;
				}
			}
			throw new ApplicationException("Invalid month name");
		}

		private static TimeSpan ParseRfc822Offset(string s)
		{
			if (s == string.Empty)
				return TimeSpan.Zero;
			int hours = 0;
			switch (s)
			{
				case "UT":
				case "GMT":
					break;
				case "EDT":
					hours = -4;
					break;
				case "EST":
				case "CDT":
					hours = -5;
					break;
				case "CST":
				case "MDT":
					hours = -6;
					break;
				case "MST":
				case "PDT":
					hours = -7;
					break;
				case "PST":
					hours = -8;
					break;
				default:
					if (s[0] == '+')
					{
						string sfmt = s.Substring(1, 2) + ":" + s.Substring(3, 2);
						return TimeSpan.Parse(sfmt);
					}
					else
						return TimeSpan.Parse(s.Insert(s.Length - 2, ":"));
			}
			return TimeSpan.FromHours(hours);
		}

		private static TimeSpan ParseW3COffset(string s)
		{
			if (s == string.Empty || s == "Z")
				return TimeSpan.Zero;
			else
			{
				if (s[0] == '+')
					return TimeSpan.Parse(s.Substring(1));
				else
					return TimeSpan.Parse(s);
			}
		}


	}
}
