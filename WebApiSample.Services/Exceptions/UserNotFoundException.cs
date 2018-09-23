namespace WebApiSample.Services.Exceptions
{
    public class UserNotFoundException : BusinessException
    {
        public UserNotFoundException() : base("User not found.")
        {
        }
    }
}
