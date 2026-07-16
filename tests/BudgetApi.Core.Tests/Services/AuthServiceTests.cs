using System.IdentityModel.Tokens.Jwt;
using BudgetApi.Core.Configuration;
using BudgetApi.Core.DTOs.Auth;
using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Core.Services;
using BudgetApi.Models.Entities.User;
using Microsoft.Extensions.Options;
using Moq;

namespace BudgetApi.Core.Tests.Services
{
    public class AuthServiceTests
    {
        private static readonly JwtSettings TestSettings = new()
        {
            Issuer = "BudgetApi.Tests",
            Audience = "BudgetApi.Tests",
            SigningKey = "a-sufficiently-long-test-signing-key-for-hmacsha256",
            ExpiryMinutes = 60
        };

        private static BudgetUser CreateUser(string password) => new()
        {
            Id = 1,
            Username = "jdoe",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FirstName = "Jane",
            LastName = "Doe",
            EmailAddress = "jane@example.com",
            PhoneNumber = "555-0100"
        };

        [Fact]
        public async Task LoginAsync_UnknownUsername_ReturnsNull()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((BudgetUser?)null);
            var service = new AuthService(repository.Object, Options.Create(TestSettings));

            var result = await service.LoginAsync(new LoginDto { Username = "ghost", Password = "whatever" });

            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByUsernameAsync("jdoe")).ReturnsAsync(CreateUser("correct-password"));
            var service = new AuthService(repository.Object, Options.Create(TestSettings));

            var result = await service.LoginAsync(new LoginDto { Username = "jdoe", Password = "wrong-password" });

            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokenWithExpectedClaims()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByUsernameAsync("jdoe")).ReturnsAsync(CreateUser("correct-password"));
            var service = new AuthService(repository.Object, Options.Create(TestSettings));

            var result = await service.LoginAsync(new LoginDto { Username = "jdoe", Password = "correct-password" });

            Assert.NotNull(result);
            Assert.Equal(1, result!.UserId);
            Assert.Equal("jdoe", result.Username);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
            Assert.Equal("1", jwt.Claims.Single(c => c.Type == "nameid" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            Assert.True(jwt.ValidTo > DateTime.UtcNow.AddMinutes(58) && jwt.ValidTo <= DateTime.UtcNow.AddMinutes(60));
        }
    }
}
