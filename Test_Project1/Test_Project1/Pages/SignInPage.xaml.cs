using System;
using Test_Project1.Services;
using Xamarin.Forms.Xaml;

namespace Test_Project1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage
    {
        private readonly UserService _userService = new UserService();
        public SignInPage()
        {
            InitializeComponent();

        }

        private async void Login_OnClicked(object sender, EventArgs e)
        {
            var user = _userService.LoginUser(EmailEntry.Text);

            if (user == null)
            {
                await DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            if (user.Password != PasswordEntry.Text)
            {
                await DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            PasswordEntry.Text = "";
            await Navigation.PushAsync(new HomePage(user));
        }
    }
}