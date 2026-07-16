using BudgetApi.Models.Entities.Account;

namespace BudgetApi.Core.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> CreateAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
    }
}
