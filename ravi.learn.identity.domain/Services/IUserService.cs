﻿using ravi.learn.identity.domain.Entities;
using System.Threading.Tasks;

namespace ravi.learn.identity.domain.Services
{
    public interface IUserService
    {
        //Task<bool> ValidateCredentials(string userName, string password, out User user);
        //Task<bool> AddUser(string userName, string password);

        Task<User> GetUserById(string id);
        Task<User> AddUser(string id, string displayName, string email);
    }

   

}
