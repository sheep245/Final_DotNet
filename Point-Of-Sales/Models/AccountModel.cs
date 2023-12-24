namespace Point_Of_Sales.Models
{
    public class AccountModel
    {
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required int RetailStoreId { get; set; }
    }
}
