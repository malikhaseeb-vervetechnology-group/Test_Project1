using System.Collections.Generic;
using System.Linq;
using Test_Project1.Models;

namespace Test_Project1.Services
{
    public class UserService
    {
        private readonly List<User> _users = new List<User>()
        {
            new User { Name = "Malik Haseeb", Email = "you@malik.co", Password = "haseeb"},
            new User { Name = "Malik Haseeb", Email = "you@malik.inc", Password = "haseeb"},
        };

        public User LoginUser(string email)
        {
            return _users.SingleOrDefault(u => u.Email == email);
        }
    }
}