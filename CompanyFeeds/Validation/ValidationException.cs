using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyFeeds.Validation
{
	public class ValidationException : Exception
	{
		public List<ValidationError> ValidationErrors
		{
			get;
			private set;
		}

		public ValidationException(List<ValidationError> errors)
		{
			this.ValidationErrors = errors;
		}
	}
}
