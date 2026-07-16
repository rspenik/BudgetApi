using BudgetApi.Infrastructure.Persistence;
using BudgetApi.Infrastructure.Persistence.Repositories;
using BudgetApi.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private static BudgetDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BudgetDbContext(options);
        }

        private static BudgetUser CreateUser() => new()
        {
            Username = "jdoe",
            PasswordHash = "hashed-password",
            FirstName = "Jane",
            LastName = "Doe",
            EmailAddress = "jane@example.com",
            PhoneNumber = "555-0100"
        };

        [Fact]
        public async Task CreateAsync_AddsUserAndReturnsItWithGeneratedId()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);

            var created = await repository.CreateAsync(CreateUser());

            Assert.True(created.Id > 0);
            Assert.Equal(1, await context.BudgetUsers.CountAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);
            var created = await repository.CreateAsync(CreateUser());

            var found = await repository.GetByIdAsync(created.Id);

            Assert.NotNull(found);
            Assert.Equal("jdoe", found!.Username);
        }

        [Fact]
        public async Task GetByIdAsync_MissingUser_ReturnsNull()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);

            var found = await repository.GetByIdAsync(999);

            Assert.Null(found);
        }

        [Fact]
        public async Task GetByUsernameAsync_ExistingUsername_ReturnsUser()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);
            await repository.CreateAsync(CreateUser());

            var found = await repository.GetByUsernameAsync("jdoe");

            Assert.NotNull(found);
            Assert.Equal("jdoe", found!.Username);
        }

        [Fact]
        public async Task GetByUsernameAsync_UnknownUsername_ReturnsNull()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);

            var found = await repository.GetByUsernameAsync("ghost");

            Assert.Null(found);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);
            await repository.CreateAsync(CreateUser());
            await repository.CreateAsync(CreateUser());

            var all = await repository.GetAllAsync();

            Assert.Equal(2, all.Count());
        }

        [Fact]
        public async Task UpdateAsync_PersistsChanges()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);
            var created = await repository.CreateAsync(CreateUser());

            created.FirstName = "Janet";
            await repository.UpdateAsync(created);

            var reloaded = await repository.GetByIdAsync(created.Id);
            Assert.Equal("Janet", reloaded!.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUser()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);
            var created = await repository.CreateAsync(CreateUser());

            await repository.DeleteAsync(created.Id);

            Assert.Null(await repository.GetByIdAsync(created.Id));
        }

        [Fact]
        public async Task DeleteAsync_MissingUser_DoesNotThrow()
        {
            await using var context = CreateContext();
            var repository = new UserRepository(context);

            var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(999));

            Assert.Null(exception);
        }
    }
}
