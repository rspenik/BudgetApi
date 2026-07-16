using BudgetApi.Core.DTOs.User;
using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Core.Interfaces.Services;
using BudgetApi.Core.Mapping;

namespace BudgetApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : UserMapper.ToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(UserMapper.ToDto);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = UserMapper.ToEntity(dto, passwordHash);
            var created = await _userRepository.CreateAsync(user);
            return UserMapper.ToDto(created);
        }

        public async Task<bool> UpdateAsync(UserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id);
            if (user is null)
            {
                return false;
            }

            UserMapper.ApplyUpdate(user, dto);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
