using BudgetApi.Core.DTOs.User;

namespace BudgetApi.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(UserDto dto);
        Task DeleteAsync(int id);
    }
}
