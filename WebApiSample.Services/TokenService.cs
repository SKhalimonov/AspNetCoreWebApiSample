using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiSample.Core.Configuration;
using WebApiSample.Core.Extensions;
using WebApiSample.Core.Identity;
using WebApiSample.Core.Services;
using WebApiSample.Core.Specifications;
using WebApiSample.Data.Core.Repositories;
using WebApiSample.Data.Entities.Users;
using WebApiSample.Infrastructure.Utils;

namespace WebApiSample.Services
{
    public class TokenService : ITokenService
    {
        private readonly IdentityConfig _config;
        private readonly IRepository<UserToken> _tokenRepository;

        public TokenService(
            IdentityConfig config,
            IRepository<UserToken> tokenRepository)
        {
            _config = config;
            _tokenRepository = tokenRepository;
        }

        public void AddUserToken(UserToken userToken)
        {
            DeleteTokenByUserId(userToken.User.Id);
            _tokenRepository.Save(userToken);
            _tokenRepository.Commit();
        }

        public void AddUserToken(User user, string refreshToken, string accessToken)
        {
            var now = DateTimeOffset.UtcNow;
            var token = new UserToken
            {
                User = user,
                RefreshTokenIdHash = Hasher.GetSha256Hash(refreshToken),
                AccessTokenHash = Hasher.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(_config.RefreshTokenExpiration),
                AccessTokenExpiresDateTime = now.AddMinutes(_config.TokenExpiration)
            };
            AddUserToken(token);
        }

        public void DeleteTokenByUserId(int userId)
        {
            var userToken = _tokenRepository
                                .Include(x => x.User)
                                .GetFinder()
                                .One(UserTokenSpecifications.User(userId));
            if (userToken == null)
            {
                return;
            }

            _tokenRepository.Delete(userToken);
            _tokenRepository.Commit();
        }

        public UserToken GetTokenByRefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }

            var refreshTokenIdHash = Hasher.GetSha256Hash(refreshToken);
            return _tokenRepository
                .Include(x => x.User)
                .GetFinder()
                .One(UserTokenSpecifications.RefreshTokenIdHash(refreshTokenIdHash));
        }

        public OAuthToken CreateOAuthToken(User user)
        {
            var accessToken = CreateAccessToken(user);
            var refreshToken = Guid.NewGuid().ToString().Replace("-", string.Empty);
            AddUserToken(user, refreshToken, accessToken);

            return new OAuthToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _config.TokenExpiration
            };
        }

        private string CreateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.IsAdmin.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_config.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
