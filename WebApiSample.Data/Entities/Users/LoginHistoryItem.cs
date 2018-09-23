using System;
using WebApiSample.Data.Core.Base;

namespace WebApiSample.Data.Entities.Users
{
    public class LoginHistoryItem : BaseEntity
    {
        public DateTime Occurred { get; set; }

        public string RemoteIp { get; set; }

        public string UserAgent { get; set; }

        public User User { get; set; }
    }
}
