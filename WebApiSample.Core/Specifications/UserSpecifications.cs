using WebApiSample.Data.Core.Specification;
using WebApiSample.Data.Entities.Users;

namespace WebApiSample.Core.Specifications
{
    public class UserSpecifications
    {
        public static ISpecification<User> Active()
        {
            return new SingleSpecification<User>(user => user.IsActive);
        }

        public static ISpecification<User> Username(string username)
        {
            return new SingleSpecification<User>(user => user.Username == username);
        }

        public static ISpecification<User> FirstNameContains(string firstName)
        {
            return new SingleSpecification<User>(user => user.FirstName.Contains(firstName));
        }

        public static ISpecification<User> LastNameContains(string lastName)
        {
            return new SingleSpecification<User>(user => user.LastName.Contains(lastName));
        }

        public static ISpecification<User> Email(string email)
        {
            return new SingleSpecification<User>(user => user.Email == email);
        }
    }
}
