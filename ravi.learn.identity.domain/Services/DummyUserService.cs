using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ravi.learn.identity.domain.Entities;

namespace ravi.learn.identity.domain.Services
{



    public class DummyUserService : IUserService
    {
        private IDictionary<string, User> _users = new Dictionary<string, User>();

        public Task<User> AddUser(string id, string displayName, string email)
        {
            var user = User.Create(id, displayName, email);
            _users.Add(id, user);
            return Task.FromResult(user);
        }

        public Task<User> GetUserById(string id)
        {            
            if (_users.ContainsKey(id))
            {                
                return Task.FromResult(_users[id]);
            }
            return Task.FromResult<User>(null);
        }
    }
    //public class DummyUserService : IUserService
    //{
    //    private IDictionary<string, (string PasswordHash, User user)> _users =
    //            new Dictionary<string, (string PasswordHash, User user)>();



    //    public DummyUserService(IDictionary<string,string> users)
    //    {
    //        foreach (var user in users)
    //        {
    //            _users.Add(user.Key.ToLower(), (BCrypt.Net.BCrypt.HashPassword(user.Value), new User(user.Key)));
    //        }
    //    }

    //    public Task<bool> AddUser(string userName, string password)
    //    {
    //        if (_users.ContainsKey(userName.ToLower()))
    //        {
    //            return Task.FromResult(false);
    //        }
    //        _users.Add(userName.ToLower(), (BCrypt.Net.BCrypt.HashPassword(password), new User(userName)));
    //        return Task.FromResult(true);
    //    }

    //    public Task<bool> ValidateCredentials(string userName, string password, out User user)
    //    {
    //        user = null;
    //        var key = userName.ToLower();
    //        if (_users.ContainsKey(key))
    //        {
    //            var hashedPassword = _users[key].PasswordHash;
    //            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
    //            {
    //                user = _users[key].user;
    //                return Task.FromResult(true);
    //            }
    //        }
    //        return Task.FromResult(false);
    //    }
    //}
}
