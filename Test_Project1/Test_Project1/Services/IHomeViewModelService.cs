using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Project1.Models;

namespace Test_Project1.Services
{
    public interface IHomeViewModelService
    {
        Task<int> SignOut();
        Task SaveOrUpdateUserInDb(User user);

        Task<List<User>> GetTotalUserAccounts();
    }
}