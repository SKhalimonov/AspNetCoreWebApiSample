namespace WebApiSample.Services.Exceptions
{
    public class UsernameNotUniqueException : ValidationException
    {
        public UsernameNotUniqueException() : base("username", "This username already exists.")
        {
        }
    }
}
