using SQLite;
using System;

namespace Test_Project1.Models
{
    public class User
    {
        public string Name { get; set; }

        [PrimaryKey]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LoggedInTime { get; set; } = DateTime.Now;
    }
}
