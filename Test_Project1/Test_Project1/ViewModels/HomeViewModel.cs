using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Test_Project1.Models;
using Test_Project1.Pages.Syncfusion_Charts;
using Test_Project1.Persistence;
using Test_Project1.Services;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;
        private readonly HomeViewModelService _homeViewModelService;

        private Business _selectedBusiness;
        private readonly SQLiteAsyncConnection _connection;

        private readonly BusinessService _businessService = new BusinessService();



        public ICommand SignOutCommand { get; }
        public ICommand SelectBusinessListCommand { get; }

        public User User { get; private set; }
        public Business SelectedBusiness
        {
            get => _selectedBusiness;
            set => SetValue(ref _selectedBusiness, value);
        }
        public ObservableCollection<Business> Businesses { get; private set; }
            = new ObservableCollection<Business>();

        public HomeViewModel(User user, IPageService pageService, HomeViewModelService homeViewModelService)
        {
            _pageService = pageService;
            _homeViewModelService = homeViewModelService;
            SignOutCommand = new Command(async () => await SignOut());
            SelectBusinessListCommand = new Command<Business>(async vm => await BusinessSelected(vm));

            User = user;
            InitializingBusinessListView(user.Email);
            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();

        }

        private async Task BusinessSelected(Business business)
        {
            if (business == null)
                return;

            var response =
                await _pageService.DisplayActionSheet("Charts", "Ok", null, Charts.PieChart,
                    Charts.WaterfallChart);

            if (response == Charts.PieChart)
                await _pageService.PushAsync(new PieChartPage());

            if (response == Charts.WaterfallChart)
                await _pageService.PushAsync(new WaterfallChartPage());

            SelectedBusiness = null;

        }

        private async Task SignOut()
        {
            var response = await _homeViewModelService.SignOut();

            if (response == 1)
            {
                var accounts = await _homeViewModelService.GetTotalUserAccounts();

                if (accounts.Count == 0)
                    await _pageService.PopToRootAsync();
                else
                {
                    User = accounts[0];
                    InitializingBusinessListView(accounts[0].Email);
                }
            }
            else
                await _pageService.DisplayAlert("Alert", "Something went wrong", "Ok");

            //var usersInDb = (await _connection.Table<User>()
            //    .OrderByDescending(u => u.LoggedInTime)
            //    .ToListAsync());

            //var response = await _connection.DeleteAsync<User>(usersInDb[0].Email);

            //if (response == 1)
            //{
            //    if (usersInDb.Count <= 1)
            //        await _pageService.PopToRootAsync();

            //    else
            //    {
            //        User = usersInDb[1];
            //        InitializingBusinessListView(usersInDb[1].Email);
            //    }
            //}
            //else
            //    await _pageService.DisplayAlert("Alert", "Something went wrong", "Ok");
        }

        private void InitializingBusinessListView(string email)
        {
            Businesses = new ObservableCollection<Business>(_businessService.GetUserBusinesses(email));
        }

        public async Task SaveOrUpdateUserIntoDb()
        {
            await _connection.CreateTableAsync<User>();

            var userInDb = await _connection.Table<User>()
                .FirstOrDefaultAsync(u => u.Email == User.Email);

            if (userInDb == null)
                await _connection.InsertAsync(User);

            if (userInDb != null)
            {
                await _connection.DeleteAsync<User>(userInDb.Email);

                User.LoggedInTime = DateTime.Now;
                await _connection.InsertAsync(User);
            }
        }
    }
}