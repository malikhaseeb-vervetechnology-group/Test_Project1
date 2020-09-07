using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test_Project1.Models;
using Test_Project1.Services;
using Test_Project1.UnitTests.FakeService;
using Test_Project1.ViewModels;

namespace Test_Project1.UnitTests
{
    [TestFixture]
    public class HomeViewModelTests
    {
        private HomeViewModel _homeViewModel;
        private Mock<PageService> _pageServiceMock;
        private Mock<User> _userMock;
        private IHomeViewModelService _homeViewModelService;

        [SetUp]
        public void SetUp()
        {
            _userMock = new Mock<User>();
            _homeViewModelService = new FakeHomeViewModelService();
            _pageServiceMock = new Mock<PageService>();
            _homeViewModel = new HomeViewModel(_userMock.Object, _pageServiceMock.Object, new FakeHomeViewModelService());
        }

        //[Test]
        //public void BusinessSelected_WhenCalledAndTapOnPieChart_ShouldNavigateTheUserToPieChartPage()
        //{
        //    var business = new Business();
        //    _homeViewModel.Businesses.Add(business);

        //    _homeViewModel.SelectBusinessListCommand.Execute(business);


        //    _pageServiceMock.Verify(p => p.PushAsync(It.IsAny<PieChartPage>()));
        //}

        [Test]
        public void Businesses_WhenCalled_ShouldPopulateWithBusinessList()
        {
            _homeViewModel.Businesses.Add(new Business());
            _homeViewModel.Businesses.Add(new Business());

            Assert.That(_homeViewModel.Businesses.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task SaveOrUpdateUserIntoDb_IfUserExists_UpdateUserLoggedInTime()
        {
            // Act
            var user = new User()
            {
                Name = "Malik Haseeb",
                Email = "malik@gmail.com",
                Password = "haseeb",
                LoggedInTime = DateTime.Now
            };

            await _homeViewModelService.UpdateUserLoggedInTime(user);
            var result = (await _homeViewModelService.GetTotalUserAccounts()).Single(u => u.Email == user.Email);

            Assert.That(result.LoggedInTime.Minute, Is.EqualTo(DateTime.Now.Minute));
        }

        [Test]
        public async Task SaveOrUpdateUserIntoDb_IfUserNotExists_SaveNewUserInDb()
        {
            // Act
            var user = new User()
            {
                Name = "Malik Haseeb",
                Email = "malik@gmail.com",
                Password = "haseeb",
                LoggedInTime = DateTime.Now
            };

            await _homeViewModelService.SaveNewUserInDb(user);
            var result = await _homeViewModelService.GetTotalUserAccounts();

            // Assert
            Assert.That(result, Does.Contain(user));
        }

        [Test]
        public async Task IsUserAlreadyExists_UserExists_ReturnTrue()
        {
            var user = new User()
            {
                Email = "malik@gmail",
                LoggedInTime = DateTime.Now,
                Name = "Malik Haseeb",
                Password = "Haseeb"
            };


            await _homeViewModelService.SaveNewUserInDb(user);
            var result = await _homeViewModelService.IsUserAlreadyExists(user);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task IsUserAlreadyExists_UserNotExists_ReturnFalse()
        {
            var user = new User()
            {
                Email = "malik@gmail",
                LoggedInTime = DateTime.Now,
                Name = "Malik Haseeb",
                Password = "Haseeb"
            };


            var result = await _homeViewModelService.IsUserAlreadyExists(user);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task SignOut_WhenCalled_CurrentlyRunningAccountRemove()
        {
            var user1 = new User { Email = "malk@gmail.com", Name = "Malik" };
            var user2 = new User { Email = "haseeb@gmail.com", Name = "Haseeb" };

            await _homeViewModelService.SaveNewUserInDb(user1);
            await _homeViewModelService.SaveNewUserInDb(user2);

            var response = await _homeViewModelService.SignOut();

            Assert.That(response, Is.EqualTo(1));
        }

        [Test]
        public async Task SignOut_WhenCalled_CurrentlyRunningAccountRemoveAndApplicationAutomaticallyShiftedToNewAccount()
        {
            var user1 = new User { Email = "malk@gmail.com", Name = "Malik" };
            var user2 = new User { Email = "haseeb@gmail.com", Name = "Haseeb" };

            await _homeViewModelService.SaveNewUserInDb(user1);
            await _homeViewModelService.SaveNewUserInDb(user2);

            var response = await _homeViewModelService.SignOut();

            Assert.That(response, Is.EqualTo(1));
        }

        [Test]
        public async Task SaveOrUpdateUserIntoDb_WhenCalled_IsUserAlreadyExists()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            await viewModel.SaveOrUpdateUserIntoDb();

            homeViewModelService.Verify(hm => hm.IsUserAlreadyExists(user.Object));
        }

        [Test]
        public async Task SaveOrUpdateUserIntoDb_WhenCalled_UpdateUserLoggedInTime()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            await viewModel.SaveOrUpdateUserIntoDb();

            homeViewModelService.Verify(hm => hm.UpdateUserLoggedInTime(user.Object));
        }

        [Test]
        public async Task SaveOrUpdateUserIntoDb_WhenCalled_SaveNewUserInDb()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            await viewModel.SaveOrUpdateUserIntoDb();

            homeViewModelService.Verify(hm => hm.SaveNewUserInDb(user.Object));
        }

        [Test]
        public async Task SignOut_WhenCalled_SignOutUser()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            //await viewModel.SignOut();

            homeViewModelService.Setup(hm => hm.SaveNewUserInDb(user.Object));

            homeViewModelService.Verify(hm => hm.SignOut());
        }

        [Test]
        public async Task SignOut_WhenUserSuccessfullySignOut_GetTotalUserAccounts()
        {
            var user = new Mock<User>();
            var pageService = new Mock<IPageService>();
            var homeViewModelService = new Mock<IHomeViewModelService>();

            var viewModel = new HomeViewModel(user.Object, pageService.Object, homeViewModelService.Object);

            //await viewModel.SignOut();

            homeViewModelService.Setup(hm => hm.SaveNewUserInDb(user.Object));

            homeViewModelService.Verify(hm => hm.GetTotalUserAccounts());
        }
    }
}
