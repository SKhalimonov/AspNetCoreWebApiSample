namespace WebApiSample.Services.Exceptions
{
    public class EmailNotUniqueException : ValidationException
    {
        public EmailNotUniqueException() : base("email", "This email already exists.")
        {
        }
    }
}
