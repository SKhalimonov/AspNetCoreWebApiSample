namespace WebApiSample.Core.Identity
{
    public interface IEndUser
    {
        int Id { get; }

        string Username { get; }

        string FirstName { get; }

        string LastName { get; }

        string Email { get; }

        string PhoneNumber { get; }

        int Role { get; }
    }
}
