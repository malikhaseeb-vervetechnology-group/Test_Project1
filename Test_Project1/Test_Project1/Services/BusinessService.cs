using System.Collections.Generic;
using System.Linq;
using Test_Project1.Models;

namespace Test_Project1.Services
{
    public class BusinessService
    {
        private List<Business> _businesses = new List<Business>
        {
            new Business { Id = 1, Name = "Business1", UserEmail = "you@malik.co"},
            new Business { Id = 1, Name = "Business1", UserEmail = "you@malik.co"},
            new Business { Id = 1, Name = "Business2", UserEmail = "you@malik.inc"},
            new Business { Id = 1, Name = "Business2", UserEmail = "you@malik.inc"},
        };

        public IEnumerable<Business> GetUserBusinesses(string email)
        {
            return _businesses.Where(b => b.UserEmail == email).ToList();
        }
    }
}