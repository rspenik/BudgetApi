using System.Text.Json.Serialization;

namespace BudgetApi.Core.DTOs.Account
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "accountType")]
    [JsonDerivedType(typeof(CreateBankAccountDto), "Bank")]
    [JsonDerivedType(typeof(CreateCreditAccountDto), "Credit")]
    [JsonDerivedType(typeof(CreateLoanAccountDto), "Loan")]
    public abstract class CreateAccountDto
    {
        public required string Name { get; set; }
        public required decimal Balance { get; set; }
        public required int BudgetUserId { get; set; }
    }
}
