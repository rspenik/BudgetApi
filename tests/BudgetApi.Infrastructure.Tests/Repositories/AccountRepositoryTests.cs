using BudgetApi.Infrastructure.Persistence;
using BudgetApi.Infrastructure.Persistence.Repositories;
using BudgetApi.Models.Entities.Account;
using BudgetApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Infrastructure.Tests.Repositories
{
    public class AccountRepositoryTests
    {
        private static BudgetDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BudgetDbContext(options);
        }

        private static BankAccount CreateBankAccount() => new()
        {
            Name = "Checking",
            Balance = 100m,
            BudgetUserId = 42,
            AccountNumber = "123456789",
            RoutingNumber = "987654321"
        };

        private static CreditAccount CreateCreditAccount() => new()
        {
            Name = "Rewards Card",
            Balance = -250m,
            BudgetUserId = 42,
            CreditLimit = 5000m,
            InterestRate = 19.99m,
            Network = NetworkType.Visa
        };

        private static LoanAccount CreateLoanAccount() => new()
        {
            Name = "Auto Loan",
            Balance = 15000m,
            BudgetUserId = 42,
            OriginalPrincipal = 20000m,
            InterestRate = 4.5m,
            MonthlyPayment = 375m,
            OriginationDate = new DateTime(2024, 1, 1),
            MaturityDate = new DateTime(2029, 1, 1)
        };

        [Fact]
        public async Task CreateAsync_BankAccount_PersistsAndReturnsItWithGeneratedId()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);

            var created = await repository.CreateAsync(CreateBankAccount());

            Assert.True(created.Id > 0);
            Assert.IsType<BankAccount>(created);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectRuntimeTypePerAccount()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);
            var bank = await repository.CreateAsync(CreateBankAccount());
            var credit = await repository.CreateAsync(CreateCreditAccount());
            var loan = await repository.CreateAsync(CreateLoanAccount());

            Assert.IsType<BankAccount>(await repository.GetByIdAsync(bank.Id));
            Assert.IsType<CreditAccount>(await repository.GetByIdAsync(credit.Id));
            Assert.IsType<LoanAccount>(await repository.GetByIdAsync(loan.Id));
        }

        [Fact]
        public async Task GetByIdAsync_MissingAccount_ReturnsNull()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);

            var found = await repository.GetByIdAsync(999);

            Assert.Null(found);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAccountsOfAllTypes()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);
            await repository.CreateAsync(CreateBankAccount());
            await repository.CreateAsync(CreateCreditAccount());
            await repository.CreateAsync(CreateLoanAccount());

            var all = (await repository.GetAllAsync()).ToList();

            Assert.Equal(3, all.Count);
            Assert.Contains(all, a => a is BankAccount);
            Assert.Contains(all, a => a is CreditAccount);
            Assert.Contains(all, a => a is LoanAccount);
        }

        [Fact]
        public async Task UpdateAsync_PersistsChanges()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);
            var created = await repository.CreateAsync(CreateBankAccount());

            created.Balance = 500m;
            await repository.UpdateAsync(created);

            var reloaded = await repository.GetByIdAsync(created.Id);
            Assert.Equal(500m, reloaded!.Balance);
        }

        [Fact]
        public async Task DeleteAsync_RemovesAccount()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);
            var created = await repository.CreateAsync(CreateBankAccount());

            await repository.DeleteAsync(created.Id);

            Assert.Null(await repository.GetByIdAsync(created.Id));
        }

        [Fact]
        public async Task DeleteAsync_MissingAccount_DoesNotThrow()
        {
            await using var context = CreateContext();
            var repository = new AccountRepository(context);

            var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(999));

            Assert.Null(exception);
        }
    }
}
