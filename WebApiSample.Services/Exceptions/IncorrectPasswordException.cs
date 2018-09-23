namespace WebApiSample.Services.Exceptions
{
    public class IncorrectPasswordException : ValidationException
    {
        public IncorrectPasswordException() : base("password", "Password is incorrect.")
        {
        }
    }
}
