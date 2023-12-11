using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class PurchaseHistory
    {
        public int Id { get; set; }
        public required string pID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public  required Employee Employee { get; set; }

        public double Total_Amount { get; set; }
        public double Received_Money { get; set; }
        public double Paid_Back { get; set; }
        public DateTime Date_Of_Purchase { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
