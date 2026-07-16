using BudgetApi.Models.Enums;

namespace BudgetApi.Core.DTOs.Account
{
    public class CreateCreditAccountDto : CreateAccountDto
    {
        public required decimal CreditLimit { get; set; }
        public required decimal InterestRate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public NetworkType Network { get; set; }
    }
}
