using BudgetApi.Controllers;
using BudgetApi.Core.DTOs.Auth;
using BudgetApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            var service = new Mock<IAuthService>();
            service.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(new AuthResultDto
            {
                Token = "fake-token",
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                UserId = 1,
                Username = "jdoe"
            });
            var controller = new AuthController(service.Object);

            var result = await controller.Login(new LoginDto { Username = "jdoe", Password = "correct-password" });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuthResultDto>(ok.Value);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var service = new Mock<IAuthService>();
            service.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync((AuthResultDto?)null);
            var controller = new AuthController(service.Object);

            var result = await controller.Login(new LoginDto { Username = "jdoe", Password = "wrong-password" });

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
