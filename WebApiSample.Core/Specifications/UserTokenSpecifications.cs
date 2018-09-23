using System;
using WebApiSample.Data.Core.Specification;
using WebApiSample.Data.Entities.Users;

namespace WebApiSample.Core.Specifications
{
    public class UserTokenSpecifications
    {
        public static ISpecification<UserToken> User(int userId)
        {
            return new SingleSpecification<UserToken>(token => token.User.Id == userId);
        }

        public static ISpecification<UserToken> RefreshTokenIdHash(string refreshTokenIdHash)
        {
            return new SingleSpecification<UserToken>(token =>
                token.RefreshTokenIdHash == refreshTokenIdHash
                && token.RefreshTokenExpiresDateTime > DateTime.UtcNow);
        }

        public static ISpecification<UserToken> RefreshAvailable()
        {
            return new SingleSpecification<UserToken>(token => token.RefreshTokenExpiresDateTime > DateTime.UtcNow);
        }
    }
}
