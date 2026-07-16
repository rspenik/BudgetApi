namespace BudgetApi.Models.Entities.User
{
    public class BudgetUser
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
