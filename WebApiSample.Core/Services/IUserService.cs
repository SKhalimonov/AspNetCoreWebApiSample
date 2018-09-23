using System.Linq;
using WebApiSample.Data.Entities.Users;

namespace WebApiSample.Core.Services
{
    public interface IUserService
    {
        IQueryable<User> GetUsers(out int total, int? offset = null, int? limit = null, bool? active = null, string fullname = null);

        IQueryable<User> GetUsers(string fullname);

        IQueryable<LoginHistoryItem> GetLoginHistory(out int total, int? offset = null, int? limit = null);

        User GetUser(int userId);

        User Authenticate(string username, string password);

        void Delete(int id);

        void Activate(int id);

        void Deactivate(int id);

        void SaveUser(User user);

        bool EmailUnique(int? userId, string email);

        bool UsernameUnique(int? userId, string username);
    }
}
