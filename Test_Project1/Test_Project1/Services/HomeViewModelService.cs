using SQLite;
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
    }
}