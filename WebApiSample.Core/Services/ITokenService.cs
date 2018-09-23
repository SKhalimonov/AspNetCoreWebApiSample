using WebApiSample.Core.Identity;
using WebApiSample.Data.Entities.Users;

namespace WebApiSample.Core.Services
{
    public interface ITokenService
    {
        void AddUserToken(UserToken userToken);

        void AddUserToken(User user, string refreshToken, string accessToken);

        void DeleteTokenByUserId(int userId);

        UserToken GetTokenByRefreshToken(string refreshToken);

        OAuthToken CreateOAuthToken(User user);
    }
}
