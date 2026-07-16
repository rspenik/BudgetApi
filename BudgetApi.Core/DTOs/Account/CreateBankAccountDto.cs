namespace BudgetApi.Core.DTOs.Account
{
    public class CreateBankAccountDto : CreateAccountDto
    {
        public required string AccountNumber { get; set; }
        public required string RoutingNumber { get; set; }
    }
}
