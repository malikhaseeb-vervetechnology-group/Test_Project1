using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Project1.Models;
using Test_Project1.Persistence;
using Xamarin.Forms;

namespace Test_Project1.Services
{
    public class HomeViewModelService : IHomeViewModelService
    {
        private readonly SQLiteAsyncConnection _connection;
        public HomeViewModelService()
        {
            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();
        }
        public async Task<int> SignOut()
        {
            var usersInDb = await GetTotalUserAccounts();

            return await _connection.DeleteAsync<User>(usersInDb[0].Email);
        }

        public async Task<List<User>> GetTotalUserAccounts()
        {
            return (await _connection.Table<User>()
                .OrderByDescending(u => u.LoggedInTime)
                .ToListAsync());
        }

        public async Task<bool> IsUserAlreadyExists(User user)
        {
            await _connection.CreateTableAsync<User>();

            var userInDb = await _connection.Table<User>()
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            return userInDb != null;
        }

        public async Task SaveNewUserInDb(User user)
        {
            await _connection.CreateTableAsync<User>();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _connection.InsertAsync(user);
        }

        public async Task UpdateUserLoggedInTime(User user)
        {
            await _connection.CreateTableAsync<User>();


            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _connection.DeleteAsync<User>(user.Email);

            user.LoggedInTime = DateTime.Now;
            await _connection.InsertAsync(user);
        }
    }
}