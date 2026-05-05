using BudgetApi.Models.Enums;

namespace BudgetApi.Models.Entities.Account
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Balance { get; set; }
        public required int BudgetUserId { get; set; }
    }
}
