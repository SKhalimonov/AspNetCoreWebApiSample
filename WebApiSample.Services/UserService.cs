using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApiSample.Core.Configuration;
using WebApiSample.Core.Extensions;
using WebApiSample.Core.Services;
using WebApiSample.Core.Specifications;
using WebApiSample.Data.Core.Repositories;
using WebApiSample.Data.Entities.Users;
using WebApiSample.Infrastructure.Utils;
using WebApiSample.Services.Exceptions;

namespace WebApiSample.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityConfig _config;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<LoginHistoryItem> _loginHistoryItemRepository;

        public UserService(
            IdentityConfig config,
            IRepository<LoginHistoryItem> loginHistoryItemRepository,
            IRepository<User> userRepository)
        {
            _config = config;
            _loginHistoryItemRepository = loginHistoryItemRepository;
            _userRepository = userRepository;
        }

        public IQueryable<User> GetUsers(out int total, int? offset = null, int? limit = null, bool? active = null, string fullname = null)
        {
            IQueryable<User> query = _userRepository;

            if (!string.IsNullOrWhiteSpace(fullname))
            {
                query = query.GetFinder()
                    .All(UserSpecifications.FirstNameContains(fullname).Or(UserSpecifications.LastNameContains(fullname)))
                    .AsQueryable();
            }

            if (active.HasValue)
            {
                query = query.Where(x => x.IsActive == active.Value);
            }

            total = query.Count();

            var users = query.Paging(offset, limit)
                .OrderBy(x => x.FirstName + x.LastName).AsQueryable();

            return users;
        }

        public IQueryable<User> GetUsers(string fullname)
        {
            IQueryable<User> query = _userRepository;

            if (!string.IsNullOrWhiteSpace(fullname))
            {
                query = query.GetFinder()
                    .All(UserSpecifications.FirstNameContains(fullname).Or(UserSpecifications.LastNameContains(fullname)))
                    .AsQueryable();
            }

            var users = query
                .OrderBy(x => x.FirstName + x.LastName).AsQueryable();

            return users;
        }

        public void SaveUser(User user)
        {
            bool isNew = user.IsNew;

            ValidateUser(user);
            if (isNew)
            {
                user.Registered = DateTime.Now;
            }

            SaveToDb(user);

            // TODO: Implement email approving account
        }

        public bool EmailUnique(int? userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return true;
            }

            return !_userRepository.Any(u => u.Email == email && u.Id != userId.GetValueOrDefault());
        }

        public bool UsernameUnique(int? userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return true;
            }

            return !_userRepository.Any(u => u.Username == username && u.Id != userId.GetValueOrDefault());
        }

        public User GetUser(int userId)
        {
            var user = _userRepository.GetFinder().ById(userId);
            return user;
        }

        public IQueryable<LoginHistoryItem> GetLoginHistory(out int total, int? offset = null, int? limit = null)
        {
            IQueryable<LoginHistoryItem> query = _loginHistoryItemRepository;
            total = query.Count();

            var items = query.Paging(offset, limit)
                .Include(item => item.User)
                .OrderBy(x => x.Occurred).AsQueryable();

            return items;
        }

        public void Delete(int id)
        {
            User user = GetUser(id);
            _userRepository.Delete(user);
            _userRepository.Commit();
        }

        public void Activate(int id)
        {
            User user = GetUser(id);
            user.Activate();
            SaveToDb(user);
        }

        public void Deactivate(int id)
        {
            User user = GetUser(id);
            user.Deactivate();
            SaveToDb(user);
        }

        public User Authenticate(string username, string password)
        {
            var user = GetUserByUsername(username);
            var passwordHash = PasswordCreator.GetHash(password, user.PasswordSalt);

            if (user.PasswordHash != passwordHash)
            {
                throw new IncorrectPasswordException();
            }

            return user;
        }

        private User GetUserByEmail(string email)
        {
            User user = _userRepository.Find.One(UserSpecifications.Email(email));

            if (user == null)
            {
                throw new UserNotFoundByEmailException();
            }

            return user;
        }

        private User GetUserByUsername(string username)
        {
            User user = _userRepository.Find.One(UserSpecifications.Username(username));

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        private void SaveToDb(User user)
        {
            _userRepository.Save(user);
            _userRepository.Commit();
        }

        private void ValidateUser(User user)
        {
            if (!EmailUnique(user.Id, user.Email))
            {
                throw new EmailNotUniqueException();
            }

            if (!UsernameUnique(user.Id, user.Username))
            {
                throw new UsernameNotUniqueException();
            }
        }
    }
}
