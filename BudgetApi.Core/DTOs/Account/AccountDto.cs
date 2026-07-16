using System.Text.Json.Serialization;

namespace BudgetApi.Core.DTOs.Account
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "accountType")]
    [JsonDerivedType(typeof(BankAccountDto), "Bank")]
    [JsonDerivedType(typeof(CreditAccountDto), "Credit")]
    [JsonDerivedType(typeof(LoanAccountDto), "Loan")]
    public abstract class AccountDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Balance { get; set; }
        public required int BudgetUserId { get; set; }
    }
}
