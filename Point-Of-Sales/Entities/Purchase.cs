using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public required string purchaseId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual required Customer Customer { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual required Employee Employee { get; set; }

        public double Total_Amount { get; set; }
        public double Received_Money { get; set; }
        public double Paid_Back { get; set; }
        public DateTime Date_Of_Purchase { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
