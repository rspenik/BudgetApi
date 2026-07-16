namespace BudgetApi.Core.DTOs.Account
{
    public class BankAccountDto : AccountDto
    {
        public required string AccountNumber { get; set; }
        public required string RoutingNumber { get; set; }
    }
}
