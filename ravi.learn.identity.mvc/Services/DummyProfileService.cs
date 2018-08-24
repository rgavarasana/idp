using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ravi.learn.identity.mvc.Services
{
    public class DummyProfileService : IProfileService
    {
        private readonly IDictionary<string, UserProfile> _profiles;

        public DummyProfileService(IDictionary<string, UserProfile> profiles)
        {
            this._profiles = profiles;
        }
        public Task<UserProfile> GetUserProfileAsync(string userId)
        {
            if (_profiles.ContainsKey(userId))
            {
                return Task.FromResult(_profiles[userId]);
            }
            return Task.FromResult<UserProfile>(null);
        }
    }
}
