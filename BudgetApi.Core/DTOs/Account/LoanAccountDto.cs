namespace BudgetApi.Core.DTOs.Account
{
    public class LoanAccountDto : AccountDto
    {
        public required decimal OriginalPrincipal { get; set; }
        public required decimal InterestRate { get; set; }
        public required decimal MonthlyPayment { get; set; }
        public required DateTime OriginationDate { get; set; }
        public required DateTime MaturityDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
    }
}
