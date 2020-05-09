using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Infrastructure.Data.Exceptions
{
    public class GloballException : Exception
    {
        public GloballException()
        {

        }

        public GloballException(string message) : base(message)
        {
        }
    }

    public class GloballValidationException : GloballException
    {
        public override string Message => ValidationErrors.Any()
            ? ValidationErrors.Aggregate("", (current, error) => current + error + ", ")
            : string.Empty;

        public List<string> ValidationErrors { get; }

        public GloballValidationException(List<string> errors)
        {
            ValidationErrors = errors;
        }
    }
}
