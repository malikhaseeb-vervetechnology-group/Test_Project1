using Test_Project1.Services;
using Test_Project1.ViewModels;
using Xamarin.Forms.Xaml;

namespace Test_Project1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            ViewModel = new SignInViewModel(new PageService(), new UserService());

        }

        private SignInViewModel ViewModel
        {
            get => BindingContext as SignInViewModel;
            set => BindingContext = value;
        }
    }
}