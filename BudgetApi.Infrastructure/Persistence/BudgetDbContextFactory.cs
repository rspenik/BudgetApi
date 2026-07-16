using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetApi.Infrastructure.Persistence
{
    public class BudgetDbContextFactory : IDesignTimeDbContextFactory<BudgetDbContext>
    {
        public BudgetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BudgetDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BudgetApi;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new BudgetDbContext(optionsBuilder.Options);
        }
    }
}
