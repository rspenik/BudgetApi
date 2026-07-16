using BudgetApi.Core.DTOs.Account;
using BudgetApi.Models.Entities.Account;

namespace BudgetApi.Core.Mapping
{
    public static class AccountMapper
    {
        public static AccountDto ToDto(Account account)
        {
            return account switch
            {
                BankAccount bank => new BankAccountDto
                {
                    Id = bank.Id,
                    Name = bank.Name,
                    Balance = bank.Balance,
                    BudgetUserId = bank.BudgetUserId,
                    AccountNumber = bank.AccountNumber,
                    RoutingNumber = bank.RoutingNumber
                },
                CreditAccount credit => new CreditAccountDto
                {
                    Id = credit.Id,
                    Name = credit.Name,
                    Balance = credit.Balance,
                    BudgetUserId = credit.BudgetUserId,
                    CreditLimit = credit.CreditLimit,
                    InterestRate = credit.InterestRate,
                    NextPaymentDate = credit.NextPaymentDate,
                    Network = credit.Network
                },
                LoanAccount loan => new LoanAccountDto
                {
                    Id = loan.Id,
                    Name = loan.Name,
                    Balance = loan.Balance,
                    BudgetUserId = loan.BudgetUserId,
                    OriginalPrincipal = loan.OriginalPrincipal,
                    InterestRate = loan.InterestRate,
                    MonthlyPayment = loan.MonthlyPayment,
                    OriginationDate = loan.OriginationDate,
                    MaturityDate = loan.MaturityDate,
                    NextPaymentDate = loan.NextPaymentDate
                },
                _ => throw new NotSupportedException($"Unsupported account type: {account.GetType().Name}")
            };
        }

        public static Account ToEntity(CreateAccountDto dto)
        {
            return dto switch
            {
                CreateBankAccountDto bank => new BankAccount
                {
                    Name = bank.Name,
                    Balance = bank.Balance,
                    BudgetUserId = bank.BudgetUserId,
                    AccountNumber = bank.AccountNumber,
                    RoutingNumber = bank.RoutingNumber
                },
                CreateCreditAccountDto credit => new CreditAccount
                {
                    Name = credit.Name,
                    Balance = credit.Balance,
                    BudgetUserId = credit.BudgetUserId,
                    CreditLimit = credit.CreditLimit,
                    InterestRate = credit.InterestRate,
                    NextPaymentDate = credit.NextPaymentDate,
                    Network = credit.Network
                },
                CreateLoanAccountDto loan => new LoanAccount
                {
                    Name = loan.Name,
                    Balance = loan.Balance,
                    BudgetUserId = loan.BudgetUserId,
                    OriginalPrincipal = loan.OriginalPrincipal,
                    InterestRate = loan.InterestRate,
                    MonthlyPayment = loan.MonthlyPayment,
                    OriginationDate = loan.OriginationDate,
                    MaturityDate = loan.MaturityDate,
                    NextPaymentDate = loan.NextPaymentDate
                },
                _ => throw new NotSupportedException($"Unsupported account type: {dto.GetType().Name}")
            };
        }

        public static void ApplyUpdate(Account account, AccountDto dto)
        {
            account.Name = dto.Name;
            account.Balance = dto.Balance;
            account.BudgetUserId = dto.BudgetUserId;

            switch (account, dto)
            {
                case (BankAccount bank, BankAccountDto bankDto):
                    bank.AccountNumber = bankDto.AccountNumber;
                    bank.RoutingNumber = bankDto.RoutingNumber;
                    break;
                case (CreditAccount credit, CreditAccountDto creditDto):
                    credit.CreditLimit = creditDto.CreditLimit;
                    credit.InterestRate = creditDto.InterestRate;
                    credit.NextPaymentDate = creditDto.NextPaymentDate;
                    credit.Network = creditDto.Network;
                    break;
                case (LoanAccount loan, LoanAccountDto loanDto):
                    loan.OriginalPrincipal = loanDto.OriginalPrincipal;
                    loan.InterestRate = loanDto.InterestRate;
                    loan.MonthlyPayment = loanDto.MonthlyPayment;
                    loan.OriginationDate = loanDto.OriginationDate;
                    loan.MaturityDate = loanDto.MaturityDate;
                    loan.NextPaymentDate = loanDto.NextPaymentDate;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Cannot apply {dto.GetType().Name} to {account.GetType().Name}: account type mismatch.");
            }
        }
    }
}
