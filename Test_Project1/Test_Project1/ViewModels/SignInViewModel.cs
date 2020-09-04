using System.Threading.Tasks;
using System.Windows.Input;
using Test_Project1.Services;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class SignInViewModel
    {
        private readonly IPageService _pageService;
        public string EmailEntry { get; set; }
        public string PasswordEntry { get; set; }

        private readonly UserService _userService = new UserService();
        public ICommand LoginCommand { get; set; }

        public SignInViewModel(IPageService pageService)
        {
            _pageService = pageService;
            LoginCommand = new Command(async () => await LogIn());
        }

        private async Task LogIn()
        {
            var user = _userService.LoginUser(EmailEntry);

            if (user == null)
            {
                await _pageService.DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            if (user.Password != PasswordEntry)
            {
                await _pageService.DisplayAlert("Alert", "Invalid Credentials", "Ok");
                return;
            }

            PasswordEntry = "";
            await _pageService.PushAsync(new HomePage(user));

        }
    }
}