using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ravi.learn.identity.mvc.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetUserProfileAsync(string userId);
    }

    public class UserProfile
    {
        public UserProfile(string firstName, string lastName, string[] roles)
        {
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string[] Roles { get; }
    }
}
