using SQLite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Test_Project1.Models;
using Test_Project1.Persistence;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class ManageAccountsViewModel
    {
        private readonly SQLiteAsyncConnection _connection;

        public ObservableCollection<User> Users { get; private set; }

        public ManageAccountsViewModel()
        {
            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();
            InitializeUserAccountsListView();
        }

        private async Task InitializeUserAccountsListView()
        {
            Users = new ObservableCollection<User>(await _connection.Table<User>()
                .OrderByDescending(u => u.LoggedInTime)
                .ToListAsync());
        }
    }
}