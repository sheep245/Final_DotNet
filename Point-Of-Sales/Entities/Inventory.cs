using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string InventoryID { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual required Product? Product { get; set; }
        public int Number { get; set; } = 0;

        [ForeignKey("RetailStore")]
        public int RetailStoreId { get; set; }
        public virtual required RetailStore? RetailStore { get; set; }
        
    }
}
