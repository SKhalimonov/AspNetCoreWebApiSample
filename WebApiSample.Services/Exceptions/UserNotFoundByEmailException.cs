namespace WebApiSample.Services.Exceptions
{
    public class UserNotFoundByEmailException : ValidationException
    {
        public UserNotFoundByEmailException() : base("email", "User with this email not found")
        {
        }
    }
}
