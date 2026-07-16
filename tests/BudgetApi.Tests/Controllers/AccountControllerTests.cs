using BudgetApi.Controllers;
using BudgetApi.Core.DTOs.Account;
using BudgetApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetApi.Tests.Controllers
{
    public class AccountControllerTests
    {
        private static BankAccountDto CreateBankAccountDto(int id = 1) => new()
        {
            Id = id,
            Name = "Checking",
            Balance = 100m,
            BudgetUserId = 42,
            AccountNumber = "123456789",
            RoutingNumber = "987654321"
        };

        [Fact]
        public async Task GetAll_ReturnsOkWithAccounts()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.GetAllAsync()).ReturnsAsync([CreateBankAccountDto(1), CreateBankAccountDto(2)]);
            var controller = new AccountController(service.Object);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var accounts = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(ok.Value);
            Assert.Equal(2, accounts.Count());
        }

        [Fact]
        public async Task GetById_ExistingAccount_ReturnsOkWithAccount()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(CreateBankAccountDto());
            var controller = new AccountController(service.Object);

            var result = await controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<BankAccountDto>(ok.Value);
        }

        [Fact]
        public async Task GetById_MissingAccount_ReturnsNotFound()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((AccountDto?)null);
            var controller = new AccountController(service.Object);

            var result = await controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBankAccount_ReturnsCreatedAtActionWithAccount()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.CreateAsync(It.IsAny<CreateAccountDto>())).ReturnsAsync(CreateBankAccountDto());
            var controller = new AccountController(service.Object);
            var dto = new CreateBankAccountDto
            {
                Name = "Checking",
                Balance = 100m,
                BudgetUserId = 42,
                AccountNumber = "123456789",
                RoutingNumber = "987654321"
            };

            var result = await controller.CreateBankAccount(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AccountController.GetById), created.ActionName);
        }

        [Fact]
        public async Task CreateCreditAccount_ReturnsCreatedAtActionWithAccount()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.CreateAsync(It.IsAny<CreateAccountDto>())).ReturnsAsync(new CreditAccountDto
            {
                Id = 1,
                Name = "Rewards Card",
                Balance = -250m,
                BudgetUserId = 42,
                CreditLimit = 5000m,
                InterestRate = 19.99m
            });
            var controller = new AccountController(service.Object);
            var dto = new CreateCreditAccountDto
            {
                Name = "Rewards Card",
                Balance = -250m,
                BudgetUserId = 42,
                CreditLimit = 5000m,
                InterestRate = 19.99m
            };

            var result = await controller.CreateCreditAccount(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<CreditAccountDto>(created.Value);
        }

        [Fact]
        public async Task CreateLoanAccount_ReturnsCreatedAtActionWithAccount()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.CreateAsync(It.IsAny<CreateAccountDto>())).ReturnsAsync(new LoanAccountDto
            {
                Id = 1,
                Name = "Auto Loan",
                Balance = 15000m,
                BudgetUserId = 42,
                OriginalPrincipal = 20000m,
                InterestRate = 4.5m,
                MonthlyPayment = 375m,
                OriginationDate = new DateTime(2024, 1, 1),
                MaturityDate = new DateTime(2029, 1, 1)
            });
            var controller = new AccountController(service.Object);
            var dto = new CreateLoanAccountDto
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

            var result = await controller.CreateLoanAccount(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<LoanAccountDto>(created.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var service = new Mock<IAccountService>();
            var controller = new AccountController(service.Object);

            var result = await controller.Update(1, CreateBankAccountDto(2));

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ExistingAccount_ReturnsNoContent()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.UpdateAsync(It.IsAny<AccountDto>())).ReturnsAsync(true);
            var controller = new AccountController(service.Object);

            var result = await controller.Update(1, CreateBankAccountDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_MissingAccount_ReturnsNotFound()
        {
            var service = new Mock<IAccountService>();
            service.Setup(s => s.UpdateAsync(It.IsAny<AccountDto>())).ReturnsAsync(false);
            var controller = new AccountController(service.Object);

            var result = await controller.Update(1, CreateBankAccountDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var service = new Mock<IAccountService>();
            var controller = new AccountController(service.Object);

            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            service.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}
