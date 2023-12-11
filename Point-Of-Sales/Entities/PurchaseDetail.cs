using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }
        public string pdetail_ID { get; set; }

        [ForeignKey("PurchaseHistory")]
        public int PurchaseHistoryId { get; set; }
        public required PurchaseHistory PurchaseHistory { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
