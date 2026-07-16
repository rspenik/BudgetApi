using BudgetApi.Core.DTOs.User;
using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Core.Services;
using BudgetApi.Models.Entities.User;
using Moq;

namespace BudgetApi.Core.Tests.Services
{
    public class UserServiceTests
    {
        private static BudgetUser CreateUser(int id = 1) => new()
        {
            Id = id,
            Username = "jdoe",
            PasswordHash = "existing-hash",
            FirstName = "Jane",
            LastName = "Doe",
            EmailAddress = "jane@example.com",
            PhoneNumber = "555-0100"
        };

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ReturnsDto()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateUser());
            var service = new UserService(repository.Object);

            var dto = await service.GetByIdAsync(1);

            Assert.NotNull(dto);
            Assert.Equal("jdoe", dto!.Username);
        }

        [Fact]
        public async Task GetByIdAsync_MissingUser_ReturnsNull()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BudgetUser?)null);
            var service = new UserService(repository.Object);

            var dto = await service.GetByIdAsync(99);

            Assert.Null(dto);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetAllAsync()).ReturnsAsync([CreateUser(1), CreateUser(2)]);
            var service = new UserService(repository.Object);

            var dtos = await service.GetAllAsync();

            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task CreateAsync_HashesPasswordBeforePersisting()
        {
            BudgetUser? persisted = null;
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<BudgetUser>()))
                .Callback<BudgetUser>(u => persisted = u)
                .ReturnsAsync((BudgetUser u) => u);
            var service = new UserService(repository.Object);
            var dto = new CreateUserDto
            {
                Username = "jdoe",
                Password = "plain-text-password",
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane@example.com",
                PhoneNumber = "555-0100"
            };

            await service.CreateAsync(dto);

            Assert.NotNull(persisted);
            Assert.NotEqual("plain-text-password", persisted!.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify("plain-text-password", persisted.PasswordHash));
        }

        [Fact]
        public async Task UpdateAsync_ExistingUser_UpdatesAndReturnsTrue()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateUser());
            var service = new UserService(repository.Object);
            var dto = new UserDto
            {
                Id = 1,
                Username = "jdoe2",
                FirstName = "Janet",
                LastName = "Doey",
                EmailAddress = "janet@example.com",
                PhoneNumber = "555-0199"
            };

            var result = await service.UpdateAsync(dto);

            Assert.True(result);
            repository.Verify(r => r.UpdateAsync(It.Is<BudgetUser>(u => u.Username == "jdoe2")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_MissingUser_ReturnsFalse()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BudgetUser?)null);
            var service = new UserService(repository.Object);
            var dto = new UserDto
            {
                Id = 99,
                Username = "jdoe",
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane@example.com",
                PhoneNumber = "555-0100"
            };

            var result = await service.UpdateAsync(dto);

            Assert.False(result);
            repository.Verify(r => r.UpdateAsync(It.IsAny<BudgetUser>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DelegatesToRepository()
        {
            var repository = new Mock<IUserRepository>();
            var service = new UserService(repository.Object);

            await service.DeleteAsync(1);

            repository.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
