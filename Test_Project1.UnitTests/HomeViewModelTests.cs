using Moq;
using NUnit.Framework;
using Test_Project1.Models;
using Test_Project1.Pages.Syncfusion_Charts;
using Test_Project1.Services;
using Test_Project1.ViewModels;

namespace Test_Project1.UnitTests
{
    [TestFixture]
    public class HomeViewModelTests
    {
        private HomeViewModel _homeViewModel;
        private Mock<PageService> _pageServiceMock;
        private Mock<User> _userMock;

        [SetUp]
        public void SetUp()
        {
            _userMock = new Mock<User>();
            _pageServiceMock = new Mock<PageService>();
            _homeViewModel = new HomeViewModel(_userMock.Object, _pageServiceMock.Object, new HomeViewModelService());
        }

        [Test]
        public void BusinessSelected_WhenCalledAndTapOnPieChart_ShouldNavigateTheUserToPieChartPage()
        {
            var business = new Business();
            _homeViewModel.Businesses.Add(business);

            _homeViewModel.SelectBusinessListCommand.Execute(business);


            _pageServiceMock.Verify(p => p.PushAsync(It.IsAny<PieChartPage>()));
        }

        [Test]
        public void Businesses_WhenCalled_ShouldPopulateWithBusinessList()
        {
            _homeViewModel.Businesses.Add(new Business());
            _homeViewModel.Businesses.Add(new Business());

            Assert.That(_homeViewModel.Businesses.Count, Is.EqualTo(2));
        }
    }
}
