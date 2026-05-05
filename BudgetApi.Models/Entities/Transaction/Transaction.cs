using BudgetApi.Models.Enums;

namespace BudgetApi.Models.Entities.Transaction
{
    public class Transaction
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public required TransactionDirection Direction { get; set; }
        public required DateTime Date { get; set; }
        public required string Description { get; set; }
        public required int AccountId { get; set; }
        public int? CategoryId { get; set; }
    }
}
