using Point_Of_Sales.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Pwd { get; set; }
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required int RetailStoreId { get; set; }
    }
}
