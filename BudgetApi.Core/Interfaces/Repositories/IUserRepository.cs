using BudgetApi.Models.Entities.User;

namespace BudgetApi.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<BudgetUser?> GetByIdAsync(int id);
        Task<BudgetUser?> GetByUsernameAsync(string username);
        Task<IEnumerable<BudgetUser>> GetAllAsync();
        Task<BudgetUser> CreateAsync(BudgetUser user);
        Task UpdateAsync(BudgetUser user);
        Task DeleteAsync(int id);
    }
}
