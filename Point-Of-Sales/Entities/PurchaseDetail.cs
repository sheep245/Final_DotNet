using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }
        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }
        public virtual required Purchase Purchase { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual required Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
