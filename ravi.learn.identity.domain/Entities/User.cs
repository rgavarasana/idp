using System;
using System.Collections.Generic;
using System.Text;

namespace ravi.learn.identity.domain.Entities
{
    //public class User
    //{
    //    public User(string username)
    //    {
    //        UserName = username;
    //    }

    //    public string UserName { get; private set; }
    //}

    public class User
    {
        private User() { }

        public static User Create(string id, string displayName, string email)
        {
            return new User
            {
                Id = id,
                DisplayName = displayName,
                Email = email

            };
        }

        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
    }
}
