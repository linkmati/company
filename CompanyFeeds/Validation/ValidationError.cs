using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyFeeds.Validation
{
	public class ValidationError : Exception
	{
		public ValidationError()
		{

		}

		public ValidationError(string fieldName, ValidationErrorType type)
		{
			this.FieldName = fieldName;
			this.Type = type;
		}

		public string FieldName
		{
			get;
			set;
		}

		public ValidationErrorType Type
		{
			get;
			set;
		}
	}
}
