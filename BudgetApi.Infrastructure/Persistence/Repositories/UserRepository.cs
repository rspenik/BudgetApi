using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BudgetDbContext _context;

        public UserRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<BudgetUser?> GetByIdAsync(int id)
        {
            return await _context.BudgetUsers.FindAsync(id);
        }

        public async Task<BudgetUser?> GetByUsernameAsync(string username)
        {
            return await _context.BudgetUsers.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<BudgetUser>> GetAllAsync()
        {
            return await _context.BudgetUsers.ToListAsync();
        }

        public async Task<BudgetUser> CreateAsync(BudgetUser user)
        {
            _context.BudgetUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(BudgetUser user)
        {
            _context.BudgetUsers.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.BudgetUsers.FindAsync(id);
            if (user is null)
            {
                return;
            }

            _context.BudgetUsers.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
