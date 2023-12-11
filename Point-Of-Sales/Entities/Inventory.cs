using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string InventoryID { get; set; }

        public ICollection<Product>? Products { get; set; }

        public int Number { get; set; }

        [ForeignKey("RetailStore")]
        public int RetailStoreId { get; set; }
        public required RetailStore RetailStore { get; set; }
        
    }
}
