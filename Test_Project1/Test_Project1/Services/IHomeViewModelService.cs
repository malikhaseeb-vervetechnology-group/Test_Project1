using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Project1.Models;

namespace Test_Project1.Services
{
    public interface IHomeViewModelService
    {
        Task SaveNewUserInDb(User user);
        Task<int> SignOut();
        Task UpdateUserLoggedInTime(User user);
        Task<List<User>> GetTotalUserAccounts();
        Task<bool> IsUserAlreadyExists(User user);
    }
}