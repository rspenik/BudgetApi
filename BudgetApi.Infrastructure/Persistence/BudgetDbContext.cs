using BudgetApi.Models.Entities.Account;
using BudgetApi.Models.Entities.Transaction;
using BudgetApi.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Infrastructure.Persistence
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {
        }

        public DbSet<BudgetUser> BudgetUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TBH inheritance configuration for Account
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<BankAccount>("Bank")
                .HasValue<CreditAccount>("Credit")
                .HasValue<LoanAccount>("Loan");
        }
    }
}
