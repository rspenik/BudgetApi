using BudgetApi.Models.Enums;

namespace BudgetApi.Models.Entities.Account
{
    public class CreditAccount : Account
    {
        public required decimal CreditLimit { get; set; }
        public required decimal InterestRate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public NetworkType Network { get; set; }
    }
}
