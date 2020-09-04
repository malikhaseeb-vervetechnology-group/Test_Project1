using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Test_Project1.Models;
using Test_Project1.Pages.Syncfusion_Charts;
using Test_Project1.Services;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;
        private readonly IHomeViewModelService _homeViewModelService;

        private Business _selectedBusiness;

        private readonly BusinessService _businessService = new BusinessService();



        public ICommand SignOutCommand { get; }
        public ICommand SelectBusinessListCommand { get; }

        public User User { get; set; } // I have to modify set to private 
        public Business SelectedBusiness
        {
            get => _selectedBusiness;
            set => SetValue(ref _selectedBusiness, value);
        }
        public ObservableCollection<Business> Businesses { get; private set; }
            = new ObservableCollection<Business>();

        public HomeViewModel(
            User user,
            IPageService pageService, IHomeViewModelService homeViewModelService)
        {
            _pageService = pageService;
            _homeViewModelService = homeViewModelService;
            SignOutCommand = new Command(async () => await SignOut());
            SelectBusinessListCommand = new Command<Business>(async vm => await BusinessSelected(vm));

            User = user;
            InitializingBusinessListView(user.Email);
            SaveOrUpdateUserIntoDb();
        }

        public async Task BusinessSelected(Business business) // Make this method to private
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
        }

        private void InitializingBusinessListView(string email)
        {
            Businesses = new ObservableCollection<Business>(_businessService.GetUserBusinesses(email));
        }

        public async Task SaveOrUpdateUserIntoDb()
        {
            var isExists = await _homeViewModelService.IsUserAlreadyExists(User);

            if (isExists)
                await _homeViewModelService.UpdateUserLoggedInTime(User);
            else
                await _homeViewModelService.SaveNewUserInDb(User);
        }
    }
}