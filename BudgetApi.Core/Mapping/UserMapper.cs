using BudgetApi.Core.DTOs.User;
using BudgetApi.Models.Entities.User;

namespace BudgetApi.Core.Mapping
{
    public static class UserMapper
    {
        public static UserDto ToDto(BudgetUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static BudgetUser ToEntity(CreateUserDto dto, string passwordHash)
        {
            return new BudgetUser
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public static void ApplyUpdate(BudgetUser user, UserDto dto)
        {
            user.Username = dto.Username;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.EmailAddress = dto.EmailAddress;
            user.PhoneNumber = dto.PhoneNumber;
        }
    }
}
