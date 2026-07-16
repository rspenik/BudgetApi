using BudgetApi.Core.DTOs.Account;
using BudgetApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            return account is null ? NotFound() : Ok(account);
        }

        [HttpPost("bank-account")]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccountDto dto)
        {
            var created = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("credit-account")]
        public async Task<IActionResult> CreateCreditAccount(CreateCreditAccountDto dto)
        {
            var created = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("loan-account")]
        public async Task<IActionResult> CreateLoanAccount(CreateLoanAccountDto dto)
        {
            var created = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var updated = await _accountService.UpdateAsync(dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _accountService.DeleteAsync(id);
            return NoContent();
        }
    }
}
