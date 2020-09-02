using System.Threading.Tasks;
using Xamarin.Forms;

namespace Test_Project1.ViewModels
{
    public interface IPageService
    {
        Task PopToRootAsync();
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
        Task PushAsync(Page page);
        Task DisplayAlert(string title, string message, string cancel);
    }
}