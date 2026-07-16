using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Models.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BudgetDbContext _context;

        public AccountRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
            {
                return;
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
