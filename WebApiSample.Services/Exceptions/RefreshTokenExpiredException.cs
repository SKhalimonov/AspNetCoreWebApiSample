namespace WebApiSample.Services.Exceptions
{
    public class RefreshTokenExpiredException : BusinessException
    {
        public RefreshTokenExpiredException() : base("Refresh token is expired.")
        {
        }
    }
}
