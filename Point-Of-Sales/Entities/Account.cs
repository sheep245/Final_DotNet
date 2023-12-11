using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public required Employee Employee { get; set; }

        public required string Username { get; set; }
        public required string Pwd { get; set; }
        public required string Role { get; set; }
    }
}
