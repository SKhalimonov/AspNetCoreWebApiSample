using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiSample.Data.Context;
using WebApiSample.Data.Entities.Users;
using WebApiSample.Infrastructure.Utils;

namespace WebApiSample.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Users.Any())
            {
                return;
            }

        }

        private static void AddUsers(ApplicationDbContext context)
        {
            var passwordSalt = PasswordCreator.GetSalt();
            var passwordHash = PasswordCreator.GetHash("admin", passwordSalt);

            var admin = new User
            {
                FirstName = "Admin First Name",
                LastName = "Admin Last Name",
                Email = "admin@test.com",
                IsActive = true,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                Username = "admin"
            };

            context.Users.Add(admin);

            context.SaveChanges();
        }
    }
}
