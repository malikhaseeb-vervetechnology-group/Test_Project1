using System.Threading.Tasks;
using System.Windows.Input;
using Test_Project1.Services;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;
        private readonly IUserService _userService;
        private string _passwordEntry;

        public string EmailEntry { get; set; }

        public string PasswordEntry
        {
            get => _passwordEntry;
            set => SetValue(ref _passwordEntry, value);
        }

        public ICommand LoginCommand { get; }

        public SignInViewModel(IPageService pageService, IUserService userService)
        {
            _userService = userService;
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