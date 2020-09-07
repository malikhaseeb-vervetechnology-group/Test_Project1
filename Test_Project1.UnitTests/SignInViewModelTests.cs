using FluentAssertions;
using Moq;
using NUnit.Framework;
using Test_Project1.Models;
using Test_Project1.Services;
using Test_Project1.ViewModels;

namespace Test_Project1.UnitTests
{
    [TestFixture]
    public class SignInViewModelTests
    {
        private Mock<IPageService> _pageService;
        private Mock<IUserService> _userService;
        private SignInViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _pageService = new Mock<IPageService>();
            _userService = new Mock<IUserService>();
            _viewModel = new SignInViewModel(_pageService.Object, _userService.Object);
        }

        [Test]
        public void LogIn_EmailIsValid_LoginUserMethodShouldReturnUser()
        {
            _viewModel.EmailEntry = "you@malik.co";

            _viewModel.LoginCommand.Execute(null);

            _userService.Verify(u => u.LoginUser(It.IsAny<string>()), Times.AtLeastOnce);
            _userService.VerifyNoOtherCalls();
        }

        [Test]
        public void LogIn_InvalidEmail_DisplayInvalidCredentials()
        {
            _viewModel.EmailEntry = "you@malik.co";

            _userService.Setup(u =>
                u.LoginUser(It.IsAny<string>())).Returns((User)null)
                .Verifiable();

            _viewModel.LoginCommand.Execute(null);

            _userService.Verify();
            _pageService.Verify(p => p.DisplayAlert(
                It.IsAny<string>(),
                "Invalid Credentials",
                It.IsAny<string>()), Times.AtLeastOnce);

            _pageService.VerifyNoOtherCalls();
        }

        [Test]
        public void LogIn_InValidPassword_DisplayInvalidCredentials()
        {
            _viewModel.EmailEntry = "you@malik.co";
            _viewModel.PasswordEntry = "wrong password";

            _userService.Setup(u => u.LoginUser(It.IsAny<string>())).Returns(new User
            {
                Email = _viewModel.EmailEntry,
                Password = "haseeb"
            }).Verifiable();

            _viewModel.LoginCommand.Execute(null);

            _userService.Verify();
            _pageService.Verify(p => p.DisplayAlert(
                It.IsAny<string>(),
                "Invalid Credentials",
                It.IsAny<string>()), Times.AtLeastOnce);
            _pageService.VerifyNoOtherCalls();
        }


        [Test]
        public void LogIn_ValidCredentials_NavigateToHomePage()
        {
            _viewModel.EmailEntry = "you@malik.co";
            _viewModel.PasswordEntry = "haseeb";

            var user = new User
            {
                Email = _viewModel.EmailEntry,
                Password = _viewModel.PasswordEntry
            };

            _userService.Setup(u => u.LoginUser(It.IsAny<string>())).Returns(user).Verifiable();

            _viewModel.LoginCommand.Execute(null);

            _userService.Verify();
            _pageService.Verify(p => p.PushAsync(It.IsAny<HomePage>()));
            _pageService.VerifyNoOtherCalls();

            _viewModel.PasswordEntry.Should().BeEmpty();
        }
    }
}