using System.Threading.Tasks;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public class PageService : IPageService
    {
        public async Task PopToRootAsync()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}