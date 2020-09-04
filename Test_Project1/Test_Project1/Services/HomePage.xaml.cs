using SQLite;
using System;
using Test_Project1.Models;
using Test_Project1.Pages;
using Test_Project1.Persistence;
using Test_Project1.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Test_Project1.Services
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        private User _user;
        private readonly SQLiteAsyncConnection _connection;
        public HomePage(User user)
        {
            InitializeComponent();

            _user = user ?? throw new ArgumentNullException(nameof(user));

            ViewModel = new HomeViewModel(_user, new PageService(), new HomeViewModelService());

            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();
        }

        protected override void OnAppearing()
        {
            ViewModel = new HomeViewModel(_user, new PageService(), new HomeViewModelService());
            //ViewModel?.SaveOrUpdateUserIntoDb();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void ManageAccounts_OnClicked(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<ManageAccounts, User>(this, Events.NewAccountSelected,
                (source, user) =>
                {
                    _connection.DeleteAsync<User>(user.Email);

                    _user = user;
                    _user.LoggedInTime = DateTime.Now;
                    _connection.InsertAsync(_user);
                });

            Navigation.PushModalAsync(new ManageAccounts());
        }

        private void BusinessListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectBusinessListCommand.Execute(e.SelectedItem);
        }

        private HomeViewModel ViewModel
        {
            get => BindingContext as HomeViewModel;
            set => BindingContext = value;
        }
    }
}