using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Test_Project1.Models;
using Test_Project1.Services;
using Test_Project1.ViewModels;

namespace Test_Project1.UnitTests
{
    [TestFixture]
    public class HomeTests
    {
        [Test]
        public async Task SignSaveOrUpdateUserIntoDb_WhenCalled_ReturnTrueIfUserExists()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            await viewModel.SaveOrUpdateUserIntoDb();

            homeViewModelService.Verify(hv => hv.IsUserAlreadyExists(user.Object));
        }
    }
}