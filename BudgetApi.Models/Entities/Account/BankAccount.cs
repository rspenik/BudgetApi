namespace BudgetApi.Models.Entities.Account
{
    public class BankAccount : Account
    {
        public required string AccountNumber { get; set; }
        public required string RoutingNumber { get; set; }
    }
}
