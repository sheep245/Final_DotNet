using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Models
{
    public class PurchaseModel
    {
        public int EmployeeId { get;set; } 
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public List<PurchaseDetail> Products { get; set; }
    }
}
