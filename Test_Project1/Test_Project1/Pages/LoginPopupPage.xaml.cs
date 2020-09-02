using System;
using Rg.Plugins.Popup.Services;
using SQLite;
using Test_Project1.Models;
using Test_Project1.Persistence;
using Test_Project1.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Test_Project1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPopupPage
    {
        private readonly SQLiteAsyncConnection _connection;
        public event EventHandler<User> UserSuccessfullyAdded;
        private readonly UserService _userService = new UserService();
        public LoginPopupPage()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISqLiteDb>().GetConnection();
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            var user = _userService.LoginUser(UsernameEntry.Text);

            var userInDb = await _connection.Table<User>().FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userInDb != null)
            {
                await DisplayAlert("Alert", "you are already loggedIN", "Ok");
                await PopupNavigation.Instance.PopAsync();
                return;
            }

            if (user.Email != UsernameEntry.Text)
            {
                await DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            if (user.Password != PasswordEntry.Text)
            {
                await DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            UserSuccessfullyAdded?.Invoke(this, user);

            await PopupNavigation.Instance.PopAsync();
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }
}