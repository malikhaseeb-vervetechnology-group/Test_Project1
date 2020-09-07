using Test_Project1.Models;

namespace Test_Project1.Services
{
    public interface IUserService
    {
        User LoginUser(string email);
    }
}