using BudgetApi.Core.DTOs.Account;
using BudgetApi.Core.Mapping;
using BudgetApi.Models.Entities.Account;
using BudgetApi.Models.Enums;

namespace BudgetApi.Core.Tests.Mapping
{
    public class AccountMapperTests
    {
        [Fact]
        public void ToDto_BankAccount_MapsToBankAccountDto()
        {
            var account = new BankAccount
            {
                Id = 1,
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };

            var dto = Assert.IsType<BankAccountDto>(AccountMapper.ToDto(account));

            Assert.Equal(account.Id, dto.Id);
            Assert.Equal(account.Name, dto.Name);
            Assert.Equal(account.Balance, dto.Balance);
            Assert.Equal(account.BudgetUserId, dto.BudgetUserId);
            Assert.Equal(account.AccountNumber, dto.AccountNumber);
            Assert.Equal(account.RoutingNumber, dto.RoutingNumber);
        }

        [Fact]
        public void ToDto_CreditAccount_MapsToCreditAccountDto()
        {
            var account = new CreditAccount
            {
                Id = 2,
                Name = "Rewards Card",
                Balance = -250m,
                BudgetUserId = 42,
                CreditLimit = 5000m,
                InterestRate = 19.99m,
                NextPaymentDate = new DateTime(2026, 8, 1),
                Network = NetworkType.Visa
            };

            var dto = Assert.IsType<CreditAccountDto>(AccountMapper.ToDto(account));

            Assert.Equal(account.Id, dto.Id);
            Assert.Equal(account.CreditLimit, dto.CreditLimit);
            Assert.Equal(account.InterestRate, dto.InterestRate);
            Assert.Equal(account.NextPaymentDate, dto.NextPaymentDate);
            Assert.Equal(account.Network, dto.Network);
        }

        [Fact]
        public void ToDto_LoanAccount_MapsToLoanAccountDto()
        {
            var account = new LoanAccount
            {
                Id = 3,
                Name = "Auto Loan",
                Balance = 15000m,
                BudgetUserId = 42,
                OriginalPrincipal = 20000m,
                InterestRate = 4.5m,
                MonthlyPayment = 375m,
                OriginationDate = new DateTime(2024, 1, 1),
                MaturityDate = new DateTime(2029, 1, 1),
                NextPaymentDate = new DateTime(2026, 8, 1)
            };

            var dto = Assert.IsType<LoanAccountDto>(AccountMapper.ToDto(account));

            Assert.Equal(account.Id, dto.Id);
            Assert.Equal(account.OriginalPrincipal, dto.OriginalPrincipal);
            Assert.Equal(account.InterestRate, dto.InterestRate);
            Assert.Equal(account.MonthlyPayment, dto.MonthlyPayment);
            Assert.Equal(account.OriginationDate, dto.OriginationDate);
            Assert.Equal(account.MaturityDate, dto.MaturityDate);
            Assert.Equal(account.NextPaymentDate, dto.NextPaymentDate);
        }

        [Fact]
        public void ToEntity_CreateBankAccountDto_MapsToBankAccount()
        {
            var dto = new CreateBankAccountDto
            {
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };

            var account = Assert.IsType<BankAccount>(AccountMapper.ToEntity(dto));

            Assert.Equal(dto.Name, account.Name);
            Assert.Equal(dto.Balance, account.Balance);
            Assert.Equal(dto.BudgetUserId, account.BudgetUserId);
            Assert.Equal(dto.AccountNumber, account.AccountNumber);
            Assert.Equal(dto.RoutingNumber, account.RoutingNumber);
        }

        [Fact]
        public void ToEntity_CreateCreditAccountDto_MapsToCreditAccount()
        {
            var dto = new CreateCreditAccountDto
            {
                Name = "Rewards Card",
                Balance = -250m,
                BudgetUserId = 42,
                CreditLimit = 5000m,
                InterestRate = 19.99m,
                NextPaymentDate = new DateTime(2026, 8, 1),
                Network = NetworkType.MasterCard
            };

            var account = Assert.IsType<CreditAccount>(AccountMapper.ToEntity(dto));

            Assert.Equal(dto.CreditLimit, account.CreditLimit);
            Assert.Equal(dto.InterestRate, account.InterestRate);
            Assert.Equal(dto.NextPaymentDate, account.NextPaymentDate);
            Assert.Equal(dto.Network, account.Network);
        }

        [Fact]
        public void ToEntity_CreateLoanAccountDto_MapsToLoanAccount()
        {
            var dto = new CreateLoanAccountDto
            {
                Name = "Auto Loan",
                Balance = 15000m,
                BudgetUserId = 42,
                OriginalPrincipal = 20000m,
                InterestRate = 4.5m,
                MonthlyPayment = 375m,
                OriginationDate = new DateTime(2024, 1, 1),
                MaturityDate = new DateTime(2029, 1, 1),
                NextPaymentDate = new DateTime(2026, 8, 1)
            };

            var account = Assert.IsType<LoanAccount>(AccountMapper.ToEntity(dto));

            Assert.Equal(dto.OriginalPrincipal, account.OriginalPrincipal);
            Assert.Equal(dto.InterestRate, account.InterestRate);
            Assert.Equal(dto.MonthlyPayment, account.MonthlyPayment);
            Assert.Equal(dto.OriginationDate, account.OriginationDate);
            Assert.Equal(dto.MaturityDate, account.MaturityDate);
            Assert.Equal(dto.NextPaymentDate, account.NextPaymentDate);
        }

        [Fact]
        public void ApplyUpdate_MatchingBankAccountTypes_UpdatesFields()
        {
            var account = new BankAccount
            {
                Id = 1,
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };
            var dto = new BankAccountDto
            {
                Id = 1,
                Name = "Checking Updated",
                Balance = 200m,
                BudgetUserId = 42,
                AccountNumber = "111111111",
                RoutingNumber = "222222222"
            };

            AccountMapper.ApplyUpdate(account, dto);

            Assert.Equal(dto.Name, account.Name);
            Assert.Equal(dto.Balance, account.Balance);
            Assert.Equal(dto.AccountNumber, account.AccountNumber);
            Assert.Equal(dto.RoutingNumber, account.RoutingNumber);
        }

        [Fact]
        public void ApplyUpdate_MismatchedAccountTypes_ThrowsInvalidOperationException()
        {
            var account = new BankAccount
            {
                Id = 1,
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };
            var dto = new CreditAccountDto
            {
                Id = 1,
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                CreditLimit = 5000m,
                InterestRate = 19.99m
            };

            Assert.Throws<InvalidOperationException>(() => AccountMapper.ApplyUpdate(account, dto));
        }
    }
}
