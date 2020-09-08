using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Test_Project1.Models;
using Test_Project1.Services;
using Test_Project1.ViewModels;

namespace Test_Project1.UnitTests
{
    [TestFixture]
    public class HomeViewModelTests
    {
        private HomeViewModel _viewModel;
        private Mock<IPageService> _pageServiceMock;
        private Mock<User> _userMock;
        private Mock<IHomeViewModelService> _homeViewModelService;

        [SetUp]
        public void SetUp()
        {
            _userMock = new Mock<User>();
            _homeViewModelService = new Mock<IHomeViewModelService>();
            _pageServiceMock = new Mock<IPageService>();
            _viewModel = new HomeViewModel(_userMock.Object, _pageServiceMock.Object, _homeViewModelService.Object);
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
            _viewModel.Businesses.Add(new Business());
            _viewModel.Businesses.Add(new Business());

            Assert.That(_viewModel.Businesses.Count, Is.EqualTo(2));
        }

        [Test]
        public void SignOut_WhenCalled_ShouldCallSignOutMethod()
        {
            _viewModel.SignOutCommand.Execute(null);

            _homeViewModelService.Verify(s => s.SignOut(), Times.AtLeastOnce);
        }

        [Test]
        public void SignOut_SuccessfullyLogout_ShouldCallGetTotalUserAccounts()
        {
            _homeViewModelService.Setup(s => s.SignOut()).ReturnsAsync(1).Verifiable();

            _viewModel.SignOutCommand.Execute(null);

            _homeViewModelService.Verify(s => s.GetTotalUserAccounts(), Times.AtLeastOnce);
        }

        [Test]
        public void SignOut_IfUserHasOnly1AccountLoggedIn_ShouldCallPopToRootAsync()
        {
            _homeViewModelService.Setup(s => s.SignOut()).ReturnsAsync(1);
            _homeViewModelService.Setup(s => s.GetTotalUserAccounts())
                .ReturnsAsync(new List<User>());

            _viewModel.SignOutCommand.Execute(null);

            _pageServiceMock.Verify(p => p.PopToRootAsync(), Times.AtLeastOnce);
        }

        [Test]
        public void SignOut_UserIsNotSuccessfullySignedOut_DisplayAlertMessage()
        {
            _homeViewModelService.Setup(s => s.SignOut()).ReturnsAsync(0);

            _viewModel.SignOutCommand.Execute(null);

            _pageServiceMock.Verify(p => p.DisplayAlert(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.AtLeastOnce);
        }

        [Test]
        public void SaveOrUpdateUserIntoDb_WhenCalled_VerifyIsUserAlreadyExistsMethodCalled()
        {
            _homeViewModelService.Setup(s => s.IsUserAlreadyExists(It.IsAny<User>()))
                .ReturnsAsync(true);

            _viewModel?.SaveOrUpdateUserIntoDb();

            _homeViewModelService.Verify(s => s.IsUserAlreadyExists(It.IsAny<User>()), Times.AtLeastOnce);
        }

        [Test]
        public void SaveOrUpdateUserIntoDb_UserExists_ShouldCallUpdateUserLoggedInTime()
        {
            _homeViewModelService.Setup(s => s.IsUserAlreadyExists(It.IsAny<User>()))
                .ReturnsAsync(true);

            _viewModel?.SaveOrUpdateUserIntoDb();

            _homeViewModelService.Verify(s => s.UpdateUserLoggedInTime(It.IsAny<User>()), Times.AtLeastOnce);
        }

        [Test]
        public void SaveOrUpdateUserIntoDb_UserNotExists_ShouldCallSaveNewUserInDb()
        {
            _homeViewModelService.Setup(s => s.IsUserAlreadyExists(It.IsAny<User>()))
                .ReturnsAsync(false).Verifiable();

            _viewModel?.SaveOrUpdateUserIntoDb();

            _homeViewModelService.Verify();
            _homeViewModelService.Verify(s => s.SaveNewUserInDb(It.IsAny<User>()), Times.AtLeastOnce);
        }
    }
}
