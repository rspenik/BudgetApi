using BudgetApi.Core.DTOs.Account;
using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Core.Services;
using BudgetApi.Models.Entities.Account;
using Moq;

namespace BudgetApi.Core.Tests.Services
{
    public class AccountServiceTests
    {
        private static BankAccount CreateBankAccount(int id = 1) => new()
        {
            Id = id,
            Name = "Checking",
            Balance = 100m,
            BudgetUserId = 42,
            AccountNumber = "123456789",
            RoutingNumber = "987654321"
        };

        [Fact]
        public async Task GetByIdAsync_ExistingAccount_ReturnsDto()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateBankAccount());
            var service = new AccountService(repository.Object);

            var dto = await service.GetByIdAsync(1);

            Assert.NotNull(dto);
            Assert.IsType<BankAccountDto>(dto);
        }

        [Fact]
        public async Task GetByIdAsync_MissingAccount_ReturnsNull()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);
            var service = new AccountService(repository.Object);

            var dto = await service.GetByIdAsync(99);

            Assert.Null(dto);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetAllAsync()).ReturnsAsync([CreateBankAccount(1), CreateBankAccount(2)]);
            var service = new AccountService(repository.Object);

            var dtos = await service.GetAllAsync();

            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task CreateAsync_BankAccountDto_PersistsBankAccount()
        {
            Account? persisted = null;
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<Account>()))
                .Callback<Account>(a => persisted = a)
                .ReturnsAsync((Account a) => a);
            var service = new AccountService(repository.Object);
            var dto = new CreateBankAccountDto
            {
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };

            var result = await service.CreateAsync(dto);

            Assert.IsType<BankAccount>(persisted);
            Assert.IsType<BankAccountDto>(result);
        }

        [Fact]
        public async Task UpdateAsync_ExistingAccount_UpdatesAndReturnsTrue()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateBankAccount());
            var service = new AccountService(repository.Object);
            var dto = new BankAccountDto
            {
                Id = 1,
                Name = "Checking Updated",
                Balance = 200m,
                BudgetUserId = 42,
                AccountNumber = "111111111",
                RoutingNumber = "222222222"
            };

            var result = await service.UpdateAsync(dto);

            Assert.True(result);
            repository.Verify(r => r.UpdateAsync(It.Is<Account>(a => a.Name == "Checking Updated")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_MissingAccount_ReturnsFalse()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);
            var service = new AccountService(repository.Object);
            var dto = new BankAccountDto
            {
                Id = 99,
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };

            var result = await service.UpdateAsync(dto);

            Assert.False(result);
            repository.Verify(r => r.UpdateAsync(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DelegatesToRepository()
        {
            var repository = new Mock<IAccountRepository>();
            var service = new AccountService(repository.Object);

            await service.DeleteAsync(1);

            repository.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
