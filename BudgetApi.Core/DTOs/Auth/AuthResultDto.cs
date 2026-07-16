namespace BudgetApi.Core.DTOs.Auth
{
    public class AuthResultDto
    {
        public required string Token { get; set; }
        public required DateTime ExpiresAtUtc { get; set; }
        public required int UserId { get; set; }
        public required string Username { get; set; }
    }
}
