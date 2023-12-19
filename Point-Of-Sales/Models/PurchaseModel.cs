using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Models
{
    public class PurchaseModel
    {
        public int EmployeeId { get; set; } = 1;
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public List<PurchaseDetailModel> Products { get; set; }
    }
}
