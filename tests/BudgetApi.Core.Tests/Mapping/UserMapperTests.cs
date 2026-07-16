using BudgetApi.Core.DTOs.User;
using BudgetApi.Core.Mapping;
using BudgetApi.Models.Entities.User;

namespace BudgetApi.Core.Tests.Mapping
{
    public class UserMapperTests
    {
        private static BudgetUser CreateUser() => new()
        {
            Id = 1,
            Username = "jdoe",
            PasswordHash = "hashed-password",
            FirstName = "Jane",
            LastName = "Doe",
            EmailAddress = "jane@example.com",
            PhoneNumber = "555-0100"
        };

        [Fact]
        public void ToDto_MapsAllFields_ExcludingPasswordHash()
        {
            var user = CreateUser();

            var dto = UserMapper.ToDto(user);

            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Username, dto.Username);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.EmailAddress, dto.EmailAddress);
            Assert.Equal(user.PhoneNumber, dto.PhoneNumber);
        }

        [Fact]
        public void ToEntity_MapsAllFieldsAndUsesProvidedPasswordHash()
        {
            var dto = new CreateUserDto
            {
                Username = "jdoe",
                Password = "plain-text-password",
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane@example.com",
                PhoneNumber = "555-0100"
            };

            var user = UserMapper.ToEntity(dto, "hashed-password");

            Assert.Equal(dto.Username, user.Username);
            Assert.Equal("hashed-password", user.PasswordHash);
            Assert.Equal(dto.FirstName, user.FirstName);
            Assert.Equal(dto.LastName, user.LastName);
            Assert.Equal(dto.EmailAddress, user.EmailAddress);
            Assert.Equal(dto.PhoneNumber, user.PhoneNumber);
        }

        [Fact]
        public void ApplyUpdate_UpdatesMutableFields_LeavesPasswordHashUnchanged()
        {
            var user = CreateUser();
            var dto = new UserDto
            {
                Id = user.Id,
                Username = "jdoe2",
                FirstName = "Janet",
                LastName = "Doey",
                EmailAddress = "janet@example.com",
                PhoneNumber = "555-0199"
            };

            UserMapper.ApplyUpdate(user, dto);

            Assert.Equal(dto.Username, user.Username);
            Assert.Equal(dto.FirstName, user.FirstName);
            Assert.Equal(dto.LastName, user.LastName);
            Assert.Equal(dto.EmailAddress, user.EmailAddress);
            Assert.Equal(dto.PhoneNumber, user.PhoneNumber);
            Assert.Equal("hashed-password", user.PasswordHash);
        }
    }
}
