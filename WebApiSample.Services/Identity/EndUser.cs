using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Linq;
using WebApiSample.Core.Identity;

namespace WebApiSample.Services.Identity
{
    public class EndUser : IEndUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public EndUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int Id => int.Parse(GetUserClaim(JwtClaimTypes.Subject));

        public string Username => GetUserClaim(JwtClaimTypes.Name);

        public string FirstName => GetUserClaim(JwtClaimTypes.GivenName);

        public string LastName => GetUserClaim(JwtClaimTypes.FamilyName);

        public string Email => GetUserClaim(JwtClaimTypes.Email);

        public string PhoneNumber => GetUserClaim(JwtClaimTypes.PhoneNumber);

        public int Role => int.Parse(GetUserClaim(JwtClaimTypes.Role));

        private string GetUserClaim(string claimType)
        {
            return _contextAccessor.HttpContext.User.Claims.Single(c => c.Type == claimType).Value;
        }
    }
}
