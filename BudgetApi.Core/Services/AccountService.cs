using BudgetApi.Core.DTOs.Account;
using BudgetApi.Core.Interfaces.Repositories;
using BudgetApi.Core.Interfaces.Services;
using BudgetApi.Core.Mapping;

namespace BudgetApi.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto?> GetByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account is null ? null : AccountMapper.ToDto(account);
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(AccountMapper.ToDto);
        }

        public async Task<AccountDto> CreateAsync(CreateAccountDto dto)
        {
            var account = AccountMapper.ToEntity(dto);
            var created = await _accountRepository.CreateAsync(account);
            return AccountMapper.ToDto(created);
        }

        public async Task<bool> UpdateAsync(AccountDto dto)
        {
            var account = await _accountRepository.GetByIdAsync(dto.Id);
            if (account is null)
            {
                return false;
            }

            AccountMapper.ApplyUpdate(account, dto);
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
