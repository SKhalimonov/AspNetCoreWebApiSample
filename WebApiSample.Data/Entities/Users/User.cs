using System;
using WebApiSample.Data.Core.Base;

namespace WebApiSample.Data.Entities.Users
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime Registered { get; set; }

        public string Username { get; set; }

        // Hash of the password
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        public UserToken Token { get; set; }

        public bool IsAdmin { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
