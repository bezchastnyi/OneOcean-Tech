using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VesselNavigationAPI.Controllers;
using Xunit;

namespace VesselNavigationAPI.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<IConfiguration> _configuration;

        public HomeControllerTests()
        {
            this._loggerMock = new Mock<ILogger<HomeController>>();
            this._configuration = new Mock<IConfiguration>();
        }

        [Fact]
        public void HomeController_NullArgumentsPassed_ExceptionThrown()
        {
            //Act & Assert
            Assert.Throws<ArgumentNullException>("logger",
                () => new HomeController(null, null));

            Assert.Throws<ArgumentNullException>("configuration",
                () => new HomeController(Mock.Of<ILogger<HomeController>>(), null));

            _ = new HomeController(this._loggerMock.Object, this._configuration.Object);
        }
    }
}
