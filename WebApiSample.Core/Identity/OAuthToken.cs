namespace WebApiSample.Core.Identity
{
    public class OAuthToken
    {
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
}
