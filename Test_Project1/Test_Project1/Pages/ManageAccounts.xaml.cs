using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.ObjectModel;
using Test_Project1.Models;
using Test_Project1.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Test_Project1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageAccounts
    {
        private readonly SQLiteAsyncConnection _connection;
        private ObservableCollection<User> _users;
        public ManageAccounts()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            _users = new ObservableCollection<User>(await _connection.Table<User>()
                .OrderByDescending(u => u.LoggedInTime)
                .ToListAsync());

            UserAccountsListView.ItemsSource = _users;
        }

        private async void AddAccount_OnClicked(object sender, EventArgs e)
        {
            var loginPopupPage = new LoginPopupPage();
            loginPopupPage.UserSuccessfullyAdded += async (source, user) =>
            {
                _users.Add(user);
                await _connection.InsertAsync(user);
            };

            await PopupNavigation.Instance.PushAsync(loginPopupPage);
        }

        private void BackToHomePage_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        private void UserAccountSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is User user)
                MessagingCenter.Send(this, Events.NewAccountSelected, user);

            Navigation.PopModalAsync();
        }
    }
}