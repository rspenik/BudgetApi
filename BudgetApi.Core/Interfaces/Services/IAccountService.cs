using BudgetApi.Core.DTOs.Account;

namespace BudgetApi.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountDto?> GetByIdAsync(int id);
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task<AccountDto> CreateAsync(CreateAccountDto dto);
        Task<bool> UpdateAsync(AccountDto dto);
        Task DeleteAsync(int id);
    }
}
