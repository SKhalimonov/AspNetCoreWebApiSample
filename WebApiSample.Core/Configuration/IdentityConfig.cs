namespace WebApiSample.Core.Configuration
{
    public class IdentityConfig
    {
        public bool RequireHttpsMetadata { get; set; }

        public string Secret { get; set; }

        public int TokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }
    }
}
