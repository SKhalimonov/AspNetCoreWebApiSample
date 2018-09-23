using System;

namespace WebApiSample.Services.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string fieldName, string message) : base(message)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}
