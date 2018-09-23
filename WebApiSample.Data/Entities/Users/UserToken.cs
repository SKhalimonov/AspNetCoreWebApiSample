using System;
using WebApiSample.Data.Core.Base;

namespace WebApiSample.Data.Entities.Users
{
    public class UserToken : BaseEntity
    {
        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
