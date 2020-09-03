using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Project1.Models;
using Test_Project1.Services;

namespace Test_Project1.UnitTests.FakeServices
{
    public class FakeHomeViewModelService : IHomeViewModelService
    {
        private readonly List<User> _user = new List<User>();

        public async Task SaveNewUserInDb(User user)
        {
            _user.Add(user);

            return;
        }

        public async Task<int> SignOut()
        {
            var response = _user.Remove(_user[0]);

            return response ? 1 : 0;
        }

        public async Task UpdateUserLoggedInTime(User user)
        {
            _user.Remove(user);

            user.LoggedInTime = DateTime.Now;
            _user.Add(user);
        }

        public async Task<List<User>> GetTotalUserAccounts()
        {
            return _user;
        }

        public async Task<bool> IsUserAlreadyExists(User user)
        {
            return _user.Contains(user);
        }
    }
}