using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiSample.Controllers.Base;
using WebApiSample.Core.Services;
using WebApiSample.Models.Auth;
using WebApiSample.Models.Users;
using WebApiSample.Services.Exceptions;

namespace WebApiSample.Controllers.Auth
{
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(
            ILoggerFactory loggerFactory,
            IUserService userService,
            ITokenService tokenService,
            IMapper mapper) : base(loggerFactory)
        {
            _mapper = mapper;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody]LoginModel credintials)
        {
            return await ExecuteSafely(() =>
            {
                var user = _userService.Authenticate(credintials.Username, credintials.Password);
                var oauthToken = _tokenService.CreateOAuthToken(user);

                return new
                {
                    access_token = oauthToken.AccessToken,
                    expires_in = oauthToken.ExpiresIn,
                    refresh_token = oauthToken.RefreshToken
                };
            });
        }

        [HttpPost("refresh")]
        public async Task<object> RefreshToken([FromBody]RefreshTokenModel refreshTokenModel)
        {
            var refreshToken = refreshTokenModel.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("refreshToken is not set.");
            }

            return await ExecuteSafely(() =>
            {
                var token = _tokenService.GetTokenByRefreshToken(refreshToken);
                if (token == null)
                {
                    throw new RefreshTokenExpiredException();
                }

                var oauthToken = _tokenService.CreateOAuthToken(token.User);
                return new
                {
                    access_token = oauthToken.AccessToken,
                    expires_in = oauthToken.ExpiresIn,
                    refresh_token = oauthToken.RefreshToken
                };
            });
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<object> GetCurrentUser()
        {
            return await ExecuteSafely(() =>
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = int.Parse(claimsIdentity.Name);

                var user = _mapper.Map<UserProfileModel>(_userService.GetUser(userId));

                return user;
            });
        }
    }
}
