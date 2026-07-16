using BudgetApi.Core.DTOs.Auth;

namespace BudgetApi.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto?> LoginAsync(LoginDto dto);
    }
}
